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
    public class HrmSalaryTaskMatrixReduction :
        //        HrmSalaryTaskReductionBase<HrmSalaryTaskMatrixReduction.DepartmentItem, HrmSalaryTaskMatrixReduction.OrderItem> {
        HrmSalaryTaskReductionBase {

        public HrmSalaryTaskMatrixReduction(Session session) : base(session) { }

        [NonPersistent]
        public class DepartmentItem1 : DepartmentItemBase {
            public DepartmentItem1(Session session) : base(session) { }
            public DepartmentItem1() { }
            [Browsable(false)]
            public override IList<OrderItemBase> OrderItemBases {
                get { return new ListConverter<OrderItemBase, OrderItem1>(OrderItems); }
            }
            protected IList<OrderItem1> _OrderItems = new List<OrderItem1>();
            public IList<OrderItem1> OrderItems {
                get {
                    return _OrderItems;
                }
            }
            public Decimal MinimizeNumberOfDeviationsAlloc;
            public Decimal MinimizeMaximumDeviationsAlloc;
            public Decimal ProportionsMethodAlloc;
            public Decimal DepartmentPlan;

        }

        [NonPersistent]
        public class OrderItem1 : OrderItemBase {
            public OrderItem1(Session session) : base(session) { }
            public OrderItem1() { }
            [Browsable(false)]
            public override IList<DepartmentItemBase> DepartmentItemBases {
                get { return new ListConverter<DepartmentItemBase, DepartmentItem1>(DepartmentItems); }
            }
            public IList<DepartmentItem1> _DepartmentItems = new List<DepartmentItem1>();
            public IList<DepartmentItem1> DepartmentItems {
                get { return _DepartmentItems; }
            }
            public Decimal MinimizeNumberOfDeviationsAlloc;
            public Decimal MinimizeMaximumDeviationsAlloc;
            public Decimal ProportionsMethodAlloc;
            public Decimal OrderPlan;
        }
        protected IList<DepartmentItem1> _DepartmentItems;
        [NonPersistent]
        public IList<DepartmentItem1> DepartmentItems {
            get {
                if (_DepartmentItems == null) {
                    _DepartmentItems = new List<DepartmentItem1>();
                    departmentCreate();
                }
                return _DepartmentItems;
            }
        }
        [Browsable(false)]
        public override IList<DepartmentItemBase> DepartmentItemBases {
            get {
                return new ListConverter<DepartmentItemBase, DepartmentItem1>(DepartmentItems);
            }
        }

        protected IList<OrderItem1> _OrderItems;
        [NonPersistent]
        public IList<OrderItem1> OrderItems {
            get {
                if (_OrderItems == null) {
                    _OrderItems = new List<OrderItem1>();
                    orderCreate();
                }
                return _OrderItems;
            }
        }
        [Browsable(false)]
        public  override IList<OrderItemBase> OrderItemBases {
            get {
                return new ListConverter<OrderItemBase, OrderItem1>(OrderItems);
            }
        }

        private HrmMatrix _MinimizeNumberOfDeviationsMatrix;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public HrmMatrix MinimizeNumberOfDeviationsMatrix {
            get { return _MinimizeNumberOfDeviationsMatrix; }
            set { SetPropertyValue<HrmMatrix>("MinimizeNumberOfDeviationsMatrix", ref _MinimizeNumberOfDeviationsMatrix, value); }
        }

        private HrmMatrix _MatrixPlan;
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public HrmMatrix MatrixPlan {
            get { return _MatrixPlan; }
            set { SetPropertyValue<HrmMatrix>("MatrixPlan", ref _MatrixPlan, value); }

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
                        LoadMatrixOrder(MinimizeMaximumDeviationsMatrix, null, OrderItemBases);
                        LoadMatrixDepartment(MinimizeMaximumDeviationsMatrix, null, DepartmentItemBases);
                    }
                    break;
                case HrmMatrixVariant.MINIMIZE_NUMBER_OF_DEVIATIONS_VARIANT:
                    if (MinimizeNumberOfDeviationsMatrix != null) {
                        LoadMatrixOrder(MinimizeNumberOfDeviationsMatrix, null, OrderItemBases);
                        LoadMatrixDepartment(MinimizeNumberOfDeviationsMatrix, null, DepartmentItemBases);
                    }
                    break;
                case HrmMatrixVariant.PROPORTIONS_METHOD_VARIANT:
                    if (ProportionsMethodMatrix != null) {
                        LoadMatrixOrder(ProportionsMethodMatrix, null, OrderItemBases);
                        LoadMatrixDepartment(ProportionsMethodMatrix, null, DepartmentItemBases);
                    }
                    break;
            }
        }

        protected override void orderCreate() {
            if (MatrixPlan != null)
                LoadMatrixOrder(MatrixPlan, null, OrderItemBases);
            if (MinimizeNumberOfDeviationsMatrix != null)
                LoadMatrixOrder(MinimizeNumberOfDeviationsMatrix, null, OrderItemBases);
            if (MinimizeMaximumDeviationsMatrix != null)
                LoadMatrixOrder(MinimizeMaximumDeviationsMatrix, null, OrderItemBases);
            if (ProportionsMethodMatrix != null)
                LoadMatrixOrder(ProportionsMethodMatrix, null, OrderItemBases);
        }

        protected override void departmentCreate() {
            if (MatrixPlan != null)
                LoadMatrixDepartment(MatrixPlan, null, DepartmentItemBases);
            if (MinimizeNumberOfDeviationsMatrix != null)
                LoadMatrixDepartment(MinimizeNumberOfDeviationsMatrix, null, DepartmentItemBases);
            if (MinimizeMaximumDeviationsMatrix != null)
                LoadMatrixDepartment(MinimizeMaximumDeviationsMatrix, null, DepartmentItemBases);
            if (ProportionsMethodMatrix != null)
                LoadMatrixDepartment(ProportionsMethodMatrix, null, DepartmentItemBases);
        }
       

        protected override void LoadMatrixDepartmentLogic(HrmMatrix matrix, HrmMatrixColumn col, HrmMatrixRow row, DepartmentItemBase item2) {
            DepartmentItem1 item = (DepartmentItem1)item2;
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

        protected override void LoadMatrixOrderLogic(HrmMatrix matrix, HrmMatrixColumn col, HrmMatrixRow row, OrderItemBase item2) {
            OrderItem1 item = (OrderItem1)item2;
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

        protected override DepartmentItemBase DepartmentItemCreate() {
            return new DepartmentItem1(this.Session);
        }

        protected override OrderItemBase OrderItemCreate() {
            return new OrderItem1(this.Session);
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