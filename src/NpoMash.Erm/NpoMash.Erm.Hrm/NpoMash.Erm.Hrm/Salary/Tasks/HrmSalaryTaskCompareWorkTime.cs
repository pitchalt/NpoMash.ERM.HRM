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
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
//
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.FM.Order;

namespace NpoMash.Erm.Hrm.Salary {

    [MapInheritance(MapInheritanceType.ParentTable)]
    [Appearance(null, AppearanceItemType = "ViewItem", TargetItems = "AllocResultOZM.Status,AllocResultOZM.TypeMatrix,AllocResultOZM.Type,AllocResultOZM.GroupDep,AllocResultOZM.Columns,AllocResultOZM.Rows,AllocResultOZM.Name", Criteria = "GroupDep=='DEPARTMENT_KB'", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, AppearanceItemType = "ViewItem", TargetItems = "AllocResultKB.Status,AllocResultKB.TypeMatrix,AllocResultKB.Type,AllocResultKB.GroupDep,AllocResultKB.Columns,AllocResultKB.Rows,AllocResultKB.Name", Criteria = "GroupDep=='DEPARTMENT_OZM'", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "AcceptCompareKB", Criteria = "GroupDep=='DEPARTMENT_OZM'", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "AcceptCompareOZM", Criteria = "GroupDep=='DEPARTMENT_KB'", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "AcceptCompareKB", Criteria = "State=='HRM_SALARY_TASK_COMPLETED'", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "AcceptCompareOZM", Criteria = "State=='HRM_SALARY_TASK_COMPLETED'", Context = "Any", Visibility = ViewItemVisibility.Hide)]

    public class HrmSalaryTaskCompareWorkTime : 

        HrmSalaryTaskReductionBase {
        
        [NonPersistent]
        public class DepartmentItem2 : 
            DepartmentItemBase {
            public DepartmentItem2(Session session) : base(session) { }
            public DepartmentItem2() { }
            [Browsable(false)]
            public override IList<OrderItemBase> OrderItemBases {
                get { return new ListConverter<OrderItemBase, OrderItem2>(OrderItems); }
            }
            protected IList<OrderItem2> _OrderItems = new List<OrderItem2>();
            public IList<OrderItem2> OrderItems {
                get {
                    return _OrderItems;
                }
            }
            //���� ��� �������� ������������
            [ModelDefault("DisplayFormat", "{0:N}")]
            public Decimal DepartmentTravelFact;
            [ModelDefault("DisplayFormat", "{0:N}")]
            public Decimal DepartmentPlan;
            [ModelDefault("DisplayFormat", "{0:N}")]
            public Decimal DepartmentTravelPlan;
            [ModelDefault("DisplayFormat", "{0:N}")]
            public Decimal ConstantDepTime;
            [ModelDefault("DisplayFormat", "{0:N}")]
            private Decimal fact_Constant;
            [ModelDefault("DisplayFormat", "{0:N}")]
            public Decimal Fact_Constant {
                get { return fact_Constant = DepartmentFact - ConstantDepTime; }
            }
            public Decimal DepartmentFact;
            [ModelDefault("DisplayFormat", "{0:N}")]
            public Decimal TravelFact;
            [ModelDefault("DisplayFormat", "{0:N}")]
            private Decimal plan_Fact;
            [ModelDefault("DisplayFormat", "{0:N}")]
            public Decimal Plan_Fact {
                get { return plan_Fact=DepartmentPlan - DepartmentFact; }
        }
            [ModelDefault("DisplayFormat", "{0:N}")]
            public Decimal CoercedValue;
        }

        [NonPersistent]
        public class OrderItem2 : OrderItemBase {
            public OrderItem2(Session session) : base(session) { }
            public OrderItem2() { }
            [Browsable(false)]
            public override IList<DepartmentItemBase> DepartmentItemBases {
                get { return new ListConverter<DepartmentItemBase, DepartmentItem2>(DepartmentItems); }
            }
            public IList<DepartmentItem2> _DepartmentItems = new List<DepartmentItem2>();
            public IList<DepartmentItem2> DepartmentItems {
                get { return _DepartmentItems; }
            }
            //���� ��� �������� ������������
            public Decimal OrderTravelFact;
            [ModelDefault("DisplayFormat", "{0:N}")]
            public Decimal OrderPlan;
            [ModelDefault("DisplayFormat", "{0:N}")]
            public Decimal OrderFact;
            [ModelDefault("DisplayFormat", "{0:N}")]
            public Decimal OrderFact1;
            [ModelDefault("DisplayFormat", "{0:N}")]
            public Decimal TravelPlan;
            [ModelDefault("DisplayFormat", "{0:N}")]
            public Decimal ConstantOrderTime;
            [ModelDefault("DisplayFormat", "{0:N}")]
            private Decimal orderFact_ConstantOrderTime;
            [ModelDefault("DisplayFormat", "{0:N}")]
            public Decimal OrderFact_ConstantOrderTime {
                get { return orderFact_ConstantOrderTime = OrderFact - ConstantOrderTime; }
            }
            [ModelDefault("DisplayFormat", "{0:N}")]
            public Decimal TravelFact;
            [ModelDefault("DisplayFormat", "{0:N}")]
            private Decimal plan_Fact;
            [ModelDefault("DisplayFormat", "{0:N}")]
            public Decimal Plan_Fact {
                get { return plan_Fact = OrderPlan - OrderFact1; }
        }
            [ModelDefault("DisplayFormat", "{0:N}")]
            public Decimal CoercedValue;
        }

        protected IList<DepartmentItem2> _DepartmentItems;
        [NonPersistent]
        public IList<DepartmentItem2> DepartmentItems {
            get {
                if (_DepartmentItems == null) {
                    _DepartmentItems = new List<DepartmentItem2>();
                    departmentCreate();
                }
                return _DepartmentItems;
            }
        }
        [Browsable(false)]
        public  override IList<DepartmentItemBase> DepartmentItemBases {
            get {
                return new ListConverter<DepartmentItemBase, DepartmentItem2>(DepartmentItems);
            }
        }

        protected IList<OrderItem2> _OrderItems;
        [NonPersistent]
        public IList<OrderItem2> OrderItems {
            get {
                if (_OrderItems == null) {
                    _OrderItems = new List<OrderItem2>();
                    orderCreate();
                }
                return _OrderItems;
            }
        }
        [Browsable(false)]
        public override IList<OrderItemBase> OrderItemBases {
            get {
                return new ListConverter<OrderItemBase, OrderItem2>(OrderItems);
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


        private HrmMatrix _AllocResultKB; //��������� �������� ��
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public HrmMatrix AllocResultKB {
            get { return _AllocResultKB; }
            set { SetPropertyValue<HrmMatrix>("AllocResultKB", ref _AllocResultKB, value); }
        }

        private HrmMatrix _AllocResultOZM; //��������� �������� ���
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public HrmMatrix AllocResultOZM {
            get { return _AllocResultOZM; }
            set { SetPropertyValue<HrmMatrix>("AllocResultOZM", ref _AllocResultOZM, value); }
        }

        private HrmTimeSheet _CurrentTimeSheetKB; // ������ �� HrmTimeSheet
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public HrmTimeSheet CurrentTimeSheetKB {
            get { return _CurrentTimeSheetKB; }
            set { SetPropertyValue<HrmTimeSheet>("CurrentTimeSheetKB", ref _CurrentTimeSheetKB, value); }
        }

        private HrmTimeSheet _CurrentTimeSheetOZM; // ������ �� HrmTimeSheet
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public HrmTimeSheet CurrentTimeSheetOZM {
            get { return _CurrentTimeSheetOZM; }
            set { SetPropertyValue<HrmTimeSheet>("CurrentTimeSheetOZM", ref _CurrentTimeSheetOZM, value); }
        }

        protected override void orderCreate() {
            if (MatrixPlan != null)
                LoadMatrixOrder(MatrixPlan, null, OrderItemBases);
            if (MinimizeNumberOfDeviationsMatrix != null)
                LoadMatrixOrder(MinimizeNumberOfDeviationsMatrix, null, OrderItemBases);
            if (GroupDep == DepartmentGroupDep.DEPARTMENT_KB && AllocResultKB != null)
                LoadMatrixOrder(AllocResultKB, null, OrderItemBases);
            if (GroupDep == DepartmentGroupDep.DEPARTMENT_OZM && AllocResultOZM != null)
                LoadMatrixOrder(AllocResultOZM, null, OrderItemBases);
        }

        protected override void departmentCreate() {
            if (MatrixPlan != null)
                LoadMatrixDepartment(MatrixPlan, null, DepartmentItemBases);
            if (MinimizeNumberOfDeviationsMatrix != null)
                LoadMatrixDepartment(MinimizeNumberOfDeviationsMatrix, null, DepartmentItemBases);
            if (GroupDep == DepartmentGroupDep.DEPARTMENT_KB && AllocResultKB != null)
                LoadMatrixDepartment(AllocResultKB, null, DepartmentItemBases);
            if (GroupDep == DepartmentGroupDep.DEPARTMENT_OZM && AllocResultOZM != null)
                LoadMatrixDepartment(AllocResultOZM, null, DepartmentItemBases);
        }


        

        protected override void LoadMatrixOrderLogic(HrmMatrix matrix, HrmMatrixColumn col, HrmMatrixRow row, OrderItemBase item2) {
            OrderItem2 item = (OrderItem2)item2;
           
            foreach (HrmMatrixCell cell in row.Cells) {
                if (col != null && cell.Column != col)
                    continue;

                if (matrix.TypeMatrix == HrmMatrixTypeMatrix.MATRIX_PLANNED && matrix.Type == HrmMatrixType.TYPE_MATIX) {
                    item.OrderPlan += cell.Time;
                    item.ConstantOrderTime+=cell.ConstOrderTime;
                    item.TravelPlan += cell.TravelTime;
                }
                else if (matrix.TypeMatrix == HrmMatrixTypeMatrix.MATRIX_COERCED && matrix.Type == HrmMatrixType.TYPE_MATIX) {
                    item.CoercedValue += cell.Time;
                }
                else if (matrix.Type == HrmMatrixType.TYPE_ALLOC_RESULT) {
                    item.OrderFact1 +=cell.Time;
                    item.OrderTravelFact += cell.TravelTime;
                    
                }
                
            }

        }


        protected override void LoadMatrixDepartmentLogic(HrmMatrix matrix, HrmMatrixColumn col, HrmMatrixRow row, DepartmentItemBase item2) {
            DepartmentItem2 item = (DepartmentItem2)item2;
            foreach (HrmMatrixCell cell in col.Cells) {
                if (row != null && cell.Row != row)
                    continue;
                if (matrix.TypeMatrix == HrmMatrixTypeMatrix.MATRIX_PLANNED && matrix.Type==HrmMatrixType.TYPE_MATIX) {
                    item.DepartmentPlan += cell.Time;
                    item.DepartmentTravelPlan += cell.TravelTime;
                }
                else if (matrix.TypeMatrix == HrmMatrixTypeMatrix.MATRIX_COERCED && matrix.Type == HrmMatrixType.TYPE_MATIX) {
                    item.CoercedValue += cell.Time;
                }
                else if (matrix.Type == HrmMatrixType.TYPE_ALLOC_RESULT) {
                    item.DepartmentFact += cell.Time;
                    item.DepartmentTravelFact += cell.TravelTime;
                }
                
        }
        }

        protected override DepartmentItemBase DepartmentItemCreate() {
            return new DepartmentItem2(this.Session);
        }

        protected override OrderItemBase OrderItemCreate() {
            return new OrderItem2(this.Session);
        }

             protected override void InObjectsLoad() {
                 if (MinimizeNumberOfDeviationsMatrix != null)
                     InObjects.Add(MinimizeNumberOfDeviationsMatrix);
                 if (AllocResultKB != null)
                     InObjects.Add(AllocResultKB);
                 if (AllocResultOZM != null)
                     InObjects.Add(AllocResultOZM);
                 if (CurrentTimeSheetKB != null)
                     InObjects.Add(CurrentTimeSheetKB);
                 if (CurrentTimeSheetOZM != null)
                     InObjects.Add(CurrentTimeSheetOZM);
            if (MatrixPlan != null)
                InObjects.Add(MatrixPlan);
        }


             public String Name {
                 get {
                     return "�������� ������������";
                 }
             }

        public HrmSalaryTaskCompareWorkTime(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }



    }
}
