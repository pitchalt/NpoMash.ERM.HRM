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

    [NonPersistent]
    public class DepartmentItemReduction : DepartmentItem<OrderItemReduction> {
        public DepartmentItemReduction(Session session) : base(session) { }
        public DepartmentItemReduction() { }
        public Decimal MinimizeNumberOfDeviationsAlloc;
        public Decimal MinimizeMaximumDeviationsAlloc;
        public Decimal ProportionsMethodAlloc;
        public Decimal DepartmentPlan;

    }

    [NonPersistent]
    public class OrderItemReduction : OrderItem<DepartmentItemReduction> {
        public OrderItemReduction(Session session) : base(session) { }
        public OrderItemReduction() { }
        public Decimal MinimizeNumberOfDeviationsAlloc;
        public Decimal MinimizeMaximumDeviationsAlloc;
        public Decimal ProportionsMethodAlloc;
        public Decimal OrderPlan;

    }


    [Appearance("", AppearanceItemType = "Action", TargetItems = "Delete, New", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [MapInheritance(MapInheritanceType.OwnTable)]
    [Persistent("HrmSalaryTaskMatrixReduction")]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "HrmSalaryTYaskMatrixReductionVC_BringingMatrixInReducAction", Criteria = "isNotReadyToBring", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "AcceptCoercedMatrixAction", Criteria = "isNotReadyToAccept", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "ExportCoercedMatrix", Criteria = "isNotReadyToExport", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    public class HrmSalaryTaskMatrixReduction :
        HrmSalaryTaskReductionBase<DepartmentItemReduction, OrderItemReduction> {

        public HrmSalaryTaskMatrixReduction(Session session) : base(session) { }


        

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

        protected override void orderCreate() {
            if (MatrixPlan != null)
                LoadMatrixOrder(MatrixPlan, null, Order);
            if (MinimizeNumberOfDeviationsMatrix != null)
                LoadMatrixOrder(MinimizeNumberOfDeviationsMatrix, null, Order);
            if (MinimizeMaximumDeviationsMatrix != null)
                LoadMatrixOrder(MinimizeMaximumDeviationsMatrix, null, Order);
            if (ProportionsMethodMatrix != null)
                LoadMatrixOrder(ProportionsMethodMatrix, null, Order);
        }

        protected override void departmentCreate() {
            if (MatrixPlan != null)
                LoadMatrixDepartment(MatrixPlan, null, Department);
            if (MinimizeNumberOfDeviationsMatrix != null)
                LoadMatrixDepartment(MinimizeNumberOfDeviationsMatrix, null, Department);
            if (MinimizeMaximumDeviationsMatrix != null)
                LoadMatrixDepartment(MinimizeMaximumDeviationsMatrix, null, Department);
            if (ProportionsMethodMatrix != null)
                LoadMatrixDepartment(ProportionsMethodMatrix, null, Department);
        }
        /*
        protected void LoadMatrixOrder(HrmMatrix matrix, HrmMatrixColumn col, IList<OrderItemReduction> items) {
            foreach (HrmMatrixRow row in matrix.Rows) {
                if (col != null && row.Cells.FirstOrDefault(x => x.Column == col) == null)
                    continue;
                OrderItemReduction item = items.FirstOrDefault(x => x.Order == row.Order);
                if (item == null) {
                    item = new OrderItemReduction(this.Session) {
                        Order = row.Order,
                        DepartmentItems = new List<DepartmentItemReduction>(),
                        TypeControl = row.Order.TypeControl
                    };
                    items.Add(item);
                }

                if (col == null)
                    LoadMatrixDepartment(matrix, row, item.DepartmentItems);
            }

        }

        protected void LoadMatrixDepartment(HrmMatrix matrix, HrmMatrixRow row, IList<DepartmentItemReduction> items) {
            foreach (HrmMatrixColumn col in matrix.Columns) {
                if (row != null && col.Cells.FirstOrDefault(x => x.Row == row) == null)
                    continue;
                DepartmentItemReduction item = items.FirstOrDefault(x => x.Department == col.Department);
                if (item == null) {
                    item = new DepartmentItemReduction(this.Session) {
                        Department = col.Department, // Подразделение
                        OrderItems = new List<OrderItemReduction>(),
                        Group = col.Department.GroupDep
                    };
                    items.Add(item);
                }

                if (row == null)
                    LoadMatrixOrder(matrix, col, item.OrderItems);
            }
        }*/

        protected override void LoadMatrixDepartmentLogic(HrmMatrix matrix, HrmMatrixColumn col, HrmMatrixRow row, DepartmentItemReduction item) {
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
        }

        protected override void LoadMatrixOrderLogic(HrmMatrix matrix, HrmMatrixColumn col, HrmMatrixRow row, OrderItemReduction item) {
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
        private bool isNotReadyToExport { 
            get { 
                return (Period.Status != HrmPeriodStatus.READY_TO_EXPORT_CORCED_MATRIXS || 
                        GroupDep != DepartmentGroupDep.DEPARTMENT_KB); 
            } 
        }

        [Browsable(false)]
        private bool isNotReadyToAccept {
            get {
                return (HrmSalaryTaskMatrixReductionLogic.matrixIsAccepted(this) ||
                        Period.Status != HrmPeriodStatus.READY_TO_CALCULATE_COERCED_MATRIXS);
            }
        }

        protected override void InObjectsLoad() {
            if (AllocParameters != null)
                InObjects.Add(AllocParameters);
            if (TimeSheet != null)
                InObjects.Add(TimeSheet);
            if (MatrixPlan != null)
                InObjects.Add(MatrixPlan);
        }
    }
}