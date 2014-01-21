using System;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;
//
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.ConditionalAppearance;
//
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.FM.Order;

namespace NpoMash.Erm.Hrm.Salary {

    [Appearance("", AppearanceItemType = "Action", TargetItems = "Delete, New", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [MapInheritance(MapInheritanceType.OwnTable)]
    [Persistent("HrmSalaryTaskMatrixReduction")]
    public class HrmSalaryTaskMatrixReduction : HrmSalaryTask {
        public HrmSalaryTaskMatrixReduction(Session session) : base(session) { }

        private HrmMatrix _MatrixPlan;
        [Appearance("", Enabled = false)]
        public HrmMatrix MatrixPlan {
            get { return _MatrixPlan; }
            set { SetPropertyValue<HrmMatrix>("MatrixPlan", ref _MatrixPlan, value); }

        }
        [Browsable(false)]
        private HrmMatrix _MinimizeNumberOfDeviationsMatrix;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public HrmMatrix MinimizeNumberOfDeviationsMatrix {
            get { return _MinimizeNumberOfDeviationsMatrix; }
            set { SetPropertyValue<HrmMatrix>("MinimizeNumberOfDeviationsMatrix", ref _MinimizeNumberOfDeviationsMatrix, value); }
        }

        [Browsable(false)]
        private HrmMatrix _MinimizeMaximumDeviationsMatrix;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public HrmMatrix MinimizeMaximumDeviationsMatrix {
            get { return _MinimizeMaximumDeviationsMatrix; }
            set { SetPropertyValue<HrmMatrix>("MinimizeMaximumDeviationsMatrix", ref _MinimizeMaximumDeviationsMatrix, value); }
        }

        [Browsable(false)]
        private HrmMatrix _ProportionsMethodMatrix;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public HrmMatrix ProportionsMethodMatrix {
            get { return _ProportionsMethodMatrix; }
            set { SetPropertyValue<HrmMatrix>("ProportionsMethodMatrix", ref _ProportionsMethodMatrix, value); }
        }


        private HrmTimeSheet _TimeSheet;
        [Appearance("", Enabled = false)]
        public HrmTimeSheet TimeSheet {
            get { return _TimeSheet; }
            set { SetPropertyValue<HrmTimeSheet>("TimeSheet", ref _TimeSheet, value); }
        }

        private HrmPeriodAllocParameter _AllocParameters;
        [Appearance("", Enabled = false)]
        public HrmPeriodAllocParameter AllocParameters {
            get { return _AllocParameters; }
            set { SetPropertyValue<HrmPeriodAllocParameter>("AllocParameters", ref _AllocParameters, value); }
        }

        [NonPersistent]
        public class DepartmentItem : XPCustomObject {
            public Department Department;
            public Int32 DepartmentPlan;
            public Int32 DepartmentFact;
            public Int32 MinimizeNumberOfDeviationsAlloc;
            public Int32 MinimizeMaximumDeviationsAlloc;
            public Int32 ProportionsMethodAlloc;
            public IList<OrderItem> OrderItems = new List<OrderItem>();
            public DepartmentItem(Session session) : base(session) { }
        }

        [NonPersistent]
        public class OrderItem : XPCustomObject {
            public fmCOrder Order;
            public fmCOrderTypeCOntrol TypeControl;
            public Int32 OrderPlan;
            public Int32 MinimizeNumberOfDeviationsAlloc;
            public Int32 MinimizeMaximumDeviationsAlloc;
            public Int32 ProportionsMethodAlloc;
            public IList<DepartmentItem> DepartmentItems = new List<DepartmentItem>();
            public OrderItem(Session session) : base(session) { }
        }


        private IList<DepartmentItem> _Department;
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public IList<DepartmentItem> Department {
            get {
                if (_Department == null)
                    _Department = departmentCreate();
                return _Department;
            }
        }

        private IList<OrderItem> _Order;
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public IList<OrderItem> Order {
            get {
                if (_Order == null)
                    _Order = orderCreate();
                return _Order;
            }
        }


        protected IList<OrderItem> orderCreate() {
            IList<OrderItem> orderList = new List<OrderItem>();
            //
            LoadMatrixOrder(MatrixPlan, null, orderList);
            if (ProportionsMethodMatrix != null) 
                LoadMatrixOrder(ProportionsMethodMatrix, null, orderList);
            if (MinimizeNumberOfDeviationsMatrix != null)
                LoadMatrixOrder(MinimizeNumberOfDeviationsMatrix, null, orderList);
            if (MinimizeMaximumDeviationsMatrix != null)
                LoadMatrixOrder(MinimizeMaximumDeviationsMatrix, null, orderList);
            return orderList;
        }


        protected void LoadMatrixOrder(HrmMatrix matrix, HrmMatrixColumn col, IList<OrderItem> items) {
            foreach (HrmMatrixRow row in matrix.Rows) {
                if (col != null && row.Cells.FirstOrDefault(x => x.Column == col) == null)
                    continue;
                OrderItem item = items.FirstOrDefault(x => x.Order == row.Order);
                if (item == null) {
                    item = new OrderItem(this.Session) {
                    Order = row.Order
                    };
                    
                    
                    items.Add(item);
                }
                item.TypeControl = row.Order.TypeControl;
                
                foreach (HrmMatrixCell cell in row.Cells) {
                    if (col != null && cell.Column != col)
                        continue;
                    switch (matrix.TypeMatrix) {
                        case HRM_MATRIX_TYPE_MATRIX.Planned:
                            item.OrderPlan += cell.Time;
                            break;
                        case HRM_MATRIX_TYPE_MATRIX.Coerced:
                            switch (matrix.Variant) {
                                case HRM_MATRIX_VARIANT.ProportionsMethod:
                                    item.ProportionsMethodAlloc += cell.Time;
                                    break;
                                case HRM_MATRIX_VARIANT.MinimizeMaximumDeviations:
                                    item.MinimizeMaximumDeviationsAlloc += cell.Time;
                                    break;
                                case HRM_MATRIX_VARIANT.MinimizeNumberOfDeviations:
                                    item.MinimizeNumberOfDeviationsAlloc += cell.Time;
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                }
                item.DepartmentItems = new List<DepartmentItem>();
                if (col == null)
                    LoadMatrixDepartment(matrix, row, item.DepartmentItems);
            }
            
        }

        protected void LoadMatrixDepartment(HrmMatrix matrix, HrmMatrixRow row, IList<DepartmentItem> items) {
            foreach (HrmMatrixColumn col in matrix.Columns) {
                if (row != null && col.Cells.FirstOrDefault(x => x.Row == row) == null)
                    continue;
                DepartmentItem item = items.FirstOrDefault(x => x.Department == col.Department);
                if (item == null) {
                    item = new DepartmentItem(this.Session) {
                        Department = col.Department // ѕодразделение
                    };
                }
                
                foreach (HrmMatrixCell cell in col.Cells) {
                    if (row != null && cell.Row != row)
                        continue;
                    switch (matrix.TypeMatrix) {
                        case HRM_MATRIX_TYPE_MATRIX.Planned:
                            item.DepartmentPlan += cell.Time;
                            break;
                        case HRM_MATRIX_TYPE_MATRIX.Coerced:
                            switch (matrix.Variant) {
                                case HRM_MATRIX_VARIANT.ProportionsMethod:
                                    item.ProportionsMethodAlloc += cell.Time;
                                    break;
                                case HRM_MATRIX_VARIANT.MinimizeMaximumDeviations:
                                    item.MinimizeMaximumDeviationsAlloc += cell.Time;
                                    break;
                                case HRM_MATRIX_VARIANT.MinimizeNumberOfDeviations:
                                    item.MinimizeNumberOfDeviationsAlloc += cell.Time;
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                }
                items.Add(item);

                item.OrderItems = new List<OrderItem>();
                if (row == null)
                    LoadMatrixOrder(matrix, col, item.OrderItems);
            }
        }

        protected IList<DepartmentItem> departmentCreate() {
            IList<DepartmentItem> departmentList = new List<DepartmentItem>();
            //
            LoadMatrixDepartment(MatrixPlan, null, departmentList);
            if (ProportionsMethodMatrix != null)
                LoadMatrixDepartment(ProportionsMethodMatrix, null, departmentList);
            if (MinimizeNumberOfDeviationsMatrix != null)
                LoadMatrixDepartment(MinimizeNumberOfDeviationsMatrix, null, departmentList);
            if (MinimizeMaximumDeviationsMatrix != null)
                LoadMatrixDepartment(MinimizeMaximumDeviationsMatrix, null, departmentList);

            //заполн€ем факт по подразделению
            foreach (var t in TimeSheet.TimeSheetDeps) {
                for (int i = 0; i < departmentList.Count; i++) {
                    if (t.Department.Code == departmentList[i].Department.Code) {
                        departmentList[i].DepartmentFact = t.MatrixWorkTime;
                    }
                }
            }
            return departmentList;
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }
}

