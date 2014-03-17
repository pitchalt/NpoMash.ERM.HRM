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
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "HrmSalaryTYaskMatrixReductionVC_BringingMatrixInReducAction", Criteria = "isNotReadyToBring", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "AcceptCoercedMatrixAction", Criteria = "isNotReadyToAccept", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "ExportCoercedMatrix", Criteria = "isNotReadyToExport", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    public class HrmSalaryTaskMatrixReduction : HrmSalaryTask {
        public HrmSalaryTaskMatrixReduction(Session session) : base(session) { }

        private HrmMatrix _MinimizeNumberOfDeviationsMatrix;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public HrmMatrix MinimizeNumberOfDeviationsMatrix {
            get { return _MinimizeNumberOfDeviationsMatrix; }
            set { SetPropertyValue<HrmMatrix>("MinimizeNumberOfDeviationsMatrix", ref _MinimizeNumberOfDeviationsMatrix, value); }
        }

        private HrmMatrix _MinimizeMaximumDeviationsMatrix;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public HrmMatrix MinimizeMaximumDeviationsMatrix {
            get { return _MinimizeMaximumDeviationsMatrix; }
            set { SetPropertyValue<HrmMatrix>("MinimizeMaximumDeviationsMatrix", ref _MinimizeMaximumDeviationsMatrix, value); }
        }

        private HrmMatrix _ProportionsMethodMatrix;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public HrmMatrix ProportionsMethodMatrix {
            get { return _ProportionsMethodMatrix; }
            set { SetPropertyValue<HrmMatrix>("ProportionsMethodMatrix", ref _ProportionsMethodMatrix, value); }
        }

        private HrmMatrix _MatrixPlan;
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public HrmMatrix MatrixPlan {
            get { return _MatrixPlan; }
            set { SetPropertyValue<HrmMatrix>("MatrixPlan", ref _MatrixPlan, value); }

        }

        private HrmTimeSheet _TimeSheet;
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public HrmTimeSheet TimeSheet {
            get { return _TimeSheet; }
            set { SetPropertyValue<HrmTimeSheet>("TimeSheet", ref _TimeSheet, value); }
        }

        private HrmPeriodAllocParameter _AllocParameters;
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public HrmPeriodAllocParameter AllocParameters {
            get { return _AllocParameters; }
            set { SetPropertyValue<HrmPeriodAllocParameter>("AllocParameters", ref _AllocParameters, value); }
        }

        [NonPersistent]
        public class DepartmentItem : XPCustomObject {
            public Department Department;
            public Int64 DepartmentPlan;
            public Int64 MinimizeNumberOfDeviationsAlloc;
            public Int64 MinimizeMaximumDeviationsAlloc;
            public Int64 ProportionsMethodAlloc;
            public IList<OrderItem> OrderItems = new List<OrderItem>();
            public DepartmentItem(Session session) : base(session) { }
        }

        [NonPersistent]
        public class OrderItem : XPCustomObject {
            public fmCOrder Order;
            public FmCOrderTypeControl TypeControl;
            public Int64 OrderPlan;
            public Int64 MinimizeNumberOfDeviationsAlloc;
            public Int64 MinimizeMaximumDeviationsAlloc;
            public Int64 ProportionsMethodAlloc;
            public IList<DepartmentItem> DepartmentItems = new List<DepartmentItem>();
            public OrderItem(Session session) : base(session) { }
        }


        private IList<DepartmentItem> _Department;
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public IList<DepartmentItem> Department {
            get {
                if (_Department == null) {
                    _Department = new List<DepartmentItem>();
                    departmentCreate();
                }
                return _Department;
            }
        }

        private IList<OrderItem> _Order;
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public IList<OrderItem> Order {
            get {
                if (_Order == null) {
                    _Order = new List<OrderItem>();
                    orderCreate();
                }
                return _Order;
            }
        }

        public void Refresh(HrmMatrixVariant variant) {
            switch (variant) {
                case HrmMatrixVariant.MINIMIZE_MAXIMUM_DEVIATIONS_VARIANT:
                    if (MinimizeMaximumDeviationsMatrix != null) {
                        LoadMatrixOrder(MinimizeMaximumDeviationsMatrix, null, Order);
                        LoadMatrixDepartment(MinimizeMaximumDeviationsMatrix, null, Department);
                    }
                    break;
                case HrmMatrixVariant.MINIMIZE_NUMBER_OF_DEVIATIONS_VARIANT:
                    if (MinimizeNumberOfDeviationsMatrix != null) {
                        LoadMatrixOrder(MinimizeNumberOfDeviationsMatrix, null, Order);
                        LoadMatrixDepartment(MinimizeNumberOfDeviationsMatrix, null, Department);
                    }
                    break;
                case HrmMatrixVariant.PROPORTIONS_METHOD_VARIANT:
                    if (ProportionsMethodMatrix != null) {
                        LoadMatrixOrder(ProportionsMethodMatrix, null, Order);
                        LoadMatrixDepartment(ProportionsMethodMatrix, null, Department);
                    }
                    break;
            }
        }

        protected void orderCreate() {
            LoadMatrixOrder(MatrixPlan, null, Order);
        }

        protected void departmentCreate() {
            LoadMatrixDepartment(MatrixPlan, null, Department);
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
                        case HrmMatrixTypeMatrix.MATRIX_PLANNED:
                            item.OrderPlan += cell.Time;
                            break;
                        case HrmMatrixTypeMatrix.MATRIX_COERCED:
                            switch (matrix.Variant) {
                                case HrmMatrixVariant.PROPORTIONS_METHOD_VARIANT:
                                    item.ProportionsMethodAlloc += cell.Time;
                                    break;
                                case HrmMatrixVariant.MINIMIZE_MAXIMUM_DEVIATIONS_VARIANT:
                                    item.MinimizeMaximumDeviationsAlloc += cell.Time;
                                    break;
                                case HrmMatrixVariant.MINIMIZE_NUMBER_OF_DEVIATIONS_VARIANT:
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
                        Department = col.Department // Подразделение
                    };
                }

                foreach (HrmMatrixCell cell in col.Cells) {
                    if (row != null && cell.Row != row)
                        continue;
                    switch (matrix.TypeMatrix) {
                        case HrmMatrixTypeMatrix.MATRIX_PLANNED:
                            item.DepartmentPlan += cell.Time;
                            break;
                        case HrmMatrixTypeMatrix.MATRIX_COERCED:
                            switch (matrix.Variant) {
                                case HrmMatrixVariant.PROPORTIONS_METHOD_VARIANT:
                                    item.ProportionsMethodAlloc += cell.Time;
                                    break;
                                case HrmMatrixVariant.MINIMIZE_MAXIMUM_DEVIATIONS_VARIANT:
                                    item.MinimizeMaximumDeviationsAlloc += cell.Time;
                                    break;
                                case HrmMatrixVariant.MINIMIZE_NUMBER_OF_DEVIATIONS_VARIANT:
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

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        [Browsable(false)]
        private bool isNotReadyToBring {
            get {
                if (Period.Status != HrmPeriodStatus.READY_TO_CALCULATE_COERCED_MATRIXS)
                    return true;
                return HrmSalaryTaskMatrixReductionLogic.matrixIsAccepted(this);
            }
        }


        [Browsable(false)]
        private bool isNotReadyToExport { get { return (Period.Status != HrmPeriodStatus.READY_TO_EXPORT_CORCED_MATRIXS || GroupDep != DepartmentGroupDep.DEPARTMENT_KB); } }

        [Browsable(false)]
        private bool isNotReadyToAccept {
            get {
                return (HrmSalaryTaskMatrixReductionLogic.matrixIsAccepted(this) ||
                Period.Status != HrmPeriodStatus.READY_TO_CALCULATE_COERCED_MATRIXS);
            }
        }
    }
}