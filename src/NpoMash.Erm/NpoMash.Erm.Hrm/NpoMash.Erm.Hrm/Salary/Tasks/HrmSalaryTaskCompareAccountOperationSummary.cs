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
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "HrmSalaryTaskCompareAccountOperationSummaryVC_AcceptCompare", Criteria = "isCompareAccepted", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance("", AppearanceItemType = "Action", TargetItems = "Delete, New", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, TargetItems = "*", Context = "Any", Enabled = false)]

    public class HrmSalaryTaskCompareAccountOperationSummary : HrmSalaryTaskReductionBase {

        [NonPersistent]
        public class DepartmentAssemble :  DepartmentItemBase {
            public DepartmentAssemble(Session session) : base(session) { }
            public DepartmentAssemble() { }
            [Browsable(false)]
            public override IList<OrderItemBase> OrderItemBases  {
                get { return new ListConverter<OrderItemBase, OrderAssemble>(OrderItems); }
            }
            protected IList<OrderAssemble> _OrderItems = new List<OrderAssemble>();
            public IList<OrderAssemble> OrderItems
            {
                get
                {
                    return _OrderItems;
                }
            }
            //Поля для контроля трудоемкости
            [ModelDefault("DisplayFormat", "{0:N}")]
            public Decimal DepFotPlanMatrix;
            [ModelDefault("DisplayFormat", "{0:N}")]
            public Decimal DepFotReserveMatrix;
            [ModelDefault("DisplayFormat", "{0:N}")]
            public Decimal DepFotAllocResultMatrix;
            [ModelDefault("DisplayFormat", "{0:N}")]
            public Decimal DepWorkTimePlanMatrix;
            [ModelDefault("DisplayFormat", "{0:N}")]
            public Decimal DepWorkTimeAllocResultMatrix;
            [ModelDefault("DisplayFormat", "{0:N}")]
            public Decimal DepFotDeviation;
            [ModelDefault("DisplayFormat", "{0:N}")]
            public Decimal DepWorkTimeDeviation;
        }

        [NonPersistent]
        public class OrderAssemble : OrderItemBase
        {
            public OrderAssemble(Session session) : base(session) { }
            public OrderAssemble() { }
            [Browsable(false)]
            public override IList<DepartmentItemBase> DepartmentItemBases
            {
                get { return new ListConverter<DepartmentItemBase, DepartmentAssemble>(DepartmentItems); }
            }
            public IList<DepartmentAssemble> _DepartmentItems = new List<DepartmentAssemble>();
            public IList<DepartmentAssemble> DepartmentItems
            {
                get { return _DepartmentItems; }
            }
            //Поля для контроля трудоемкости
            [ModelDefault("DisplayFormat", "{0:N}")]
            public Decimal OrdFotPlanMatrix;
            [ModelDefault("DisplayFormat", "{0:N}")]
            public Decimal OrdFotReserveMatrix;
            [ModelDefault("DisplayFormat", "{0:N}")]
            public Decimal OrdFotAllocResultMatrix;
            [ModelDefault("DisplayFormat", "{0:N}")]
            public Decimal OrdWorkTimePlanMatrix;
            [ModelDefault("DisplayFormat", "{0:N}")]
            public Decimal OrdWorkTimeAllocResultMatrix;
            [ModelDefault("DisplayFormat", "{0:N}")]
            public Decimal OrdFotDeviation;
            [ModelDefault("DisplayFormat", "{0:N}")]
            public Decimal OrdWorkTimeDeviation;
            
        }

        protected IList<DepartmentAssemble> _DepartmentCollection;
        [NonPersistent]
        public IList<DepartmentAssemble> DepartmentCollection
        {
            get
            {
                if (_DepartmentCollection == null)
                {
                    _DepartmentCollection = new List<DepartmentAssemble>();
                    departmentCreate();
                }
                return _DepartmentCollection;
            }
        }
        [Browsable(false)]
        public override IList<DepartmentItemBase> DepartmentItemBases
        {
            get
            {
                return new ListConverter<DepartmentItemBase, DepartmentAssemble>(DepartmentCollection);
            }
        }



        protected IList<OrderAssemble> _OrderCollection;
        [NonPersistent]
        public IList<OrderAssemble> OrderCollection
        {
            get
            {
                if (_OrderCollection == null)
                {
                    _OrderCollection = new List<OrderAssemble>();
                    orderCreate();
                }
                return _OrderCollection;
            }
        }
        [Browsable(false)]
        public override IList<OrderItemBase> OrderItemBases
        {
            get
            {
                return new ListConverter<OrderItemBase, OrderAssemble>(OrderCollection);
            }
        }

        protected override void orderCreate()
        {
            if (ProvisionMatrix != null)
                LoadMatrixOrder(ProvisionMatrix, null, OrderItemBases);
            if (MatrixAllocPlanSummary != null)
                LoadMatrixOrder(MatrixAllocPlanSummary, null, OrderItemBases);
          //  if (MatrixAllocResultSummary != null)
            //    LoadMatrixOrder(MatrixAllocResultSummary, null, OrderItemBases);
        }

        protected override void departmentCreate()
        {
            if (ProvisionMatrix != null)
                LoadMatrixDepartment(ProvisionMatrix, null, DepartmentItemBases);
            if (MatrixAllocPlanSummary != null)
                LoadMatrixDepartment(MatrixAllocPlanSummary, null, DepartmentItemBases);
          //  if (MatrixAllocResultSummary != null)
              //  LoadMatrixDepartment(MatrixAllocResultSummary, null, DepartmentItemBases);
        }


        protected override void LoadMatrixOrderLogic(HrmMatrix matrix, HrmMatrixColumn col, HrmMatrixRow row, OrderItemBase item2)
        {
            OrderAssemble item = (OrderAssemble)item2;

            foreach (HrmMatrixCell cell in row.Cells)
            {
                if (col != null && cell.Column != col)
                    continue;

                if (matrix.TypeMatrix == HrmMatrixTypeMatrix.MATRIX_PLANNED && matrix.Type == HrmMatrixType.TYPE_MATIX)
                {
                   
                }
                else if (matrix.TypeMatrix == HrmMatrixTypeMatrix.MATRIX_COERCED && matrix.Type == HrmMatrixType.TYPE_MATIX)
                {
                   
                }
                else if (matrix.Type == HrmMatrixType.TYPE_ALLOC_RESULT)
                {
                    item.OrdFotAllocResultMatrix = cell.Time;
                    item.OrdFotReserveMatrix = cell.TravelTime;
                    item.OrdWorkTimeAllocResultMatrix = cell.TravelMoney;
                    item.OrdFotPlanMatrix = cell.MoneyNoReserve;
                    item.OrdWorkTimePlanMatrix = cell.ConstOrderTime;

                }

            }

        }


        protected override void LoadMatrixDepartmentLogic(HrmMatrix matrix, HrmMatrixColumn col, HrmMatrixRow row, DepartmentItemBase item2)
        {
            DepartmentAssemble item = (DepartmentAssemble)item2;
            foreach (HrmMatrixCell cell in col.Cells)
            {
                if (row != null && cell.Row != row)
                    continue;
                if (matrix.TypeMatrix == HrmMatrixTypeMatrix.MATRIX_PLANNED )
                {
                    item.DepWorkTimePlanMatrix = cell.Time;
                }
                else if (matrix.TypeMatrix == HrmMatrixTypeMatrix.MATRIX_RESERVE && matrix.Type == HrmMatrixType.TYPE_MATIX)
                {
                    item.DepFotReserveMatrix = cell.PlanMoney;
                }
                else if (matrix.Type == HrmMatrixType.TYPE_ALLOC_RESULT)
                {
                    //item.DepFotAllocResultMatrix = cell.Time;
                    
                   // item.DepWorkTimeAllocResultMatrix = cell.TravelMoney;
                   // item.DepFotPlanMatrix = cell.MoneyNoReserve;
                    //item.DepWorkTimePlanMatrix = cell.ConstOrderTime;
                    
                }

            }
        }


        private HrmMatrix _ProvisionMatrix;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public HrmMatrix ProvisionMatrix
        {
            get { return _ProvisionMatrix; }
            set { SetPropertyValue<HrmMatrix>("ProvisionMatrix", ref _ProvisionMatrix, value); }
        }

        private HrmMatrixPlan _MatrixAllocPlanSummary;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public HrmMatrixPlan MatrixAllocPlanSummary {
            get { return _MatrixAllocPlanSummary; }
            set { SetPropertyValue<HrmMatrixPlan>("MatrixAllocPlanSummary", ref _MatrixAllocPlanSummary, value); }
        }

        private HrmMatrixLastAccount _MatrixAllocResultSummary;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public HrmMatrixLastAccount MatrixAllocResultSummary {
            get { return _MatrixAllocResultSummary; }
            set { SetPropertyValue<HrmMatrixLastAccount>("MatrixAllocResultSummary", ref _MatrixAllocResultSummary, value); }
        }

        [Browsable(false)]
        private bool isCompareAccepted { get { return !(MatrixAllocResultSummary.Status == HrmMatrixStatus.MATRIX_DOWNLOADED); } }
    
        protected override void InObjectsLoad() {
        }





        public String Name {
            get {
                return "Сравнение проводки";
            }
        }

        public HrmSalaryTaskCompareAccountOperationSummary(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

        protected override HrmSalaryTaskReductionBase.DepartmentItemBase DepartmentItemCreate()
        {
            return new DepartmentAssemble(this.Session);
        }

        protected override HrmSalaryTaskReductionBase.OrderItemBase OrderItemCreate()
        {
            return new OrderAssemble(this.Session);
        }
    }
}