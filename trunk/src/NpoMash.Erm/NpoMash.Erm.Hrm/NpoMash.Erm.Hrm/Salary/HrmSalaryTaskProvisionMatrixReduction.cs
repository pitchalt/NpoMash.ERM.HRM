using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
//
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
//
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.HRM.Organization;

namespace NpoMash.Erm.Hrm.Salary {
    public class HrmSalaryTaskProvisionMatrixReduction : HrmSalaryTask {


        private HrmPeriodAllocParameter _AllocParameters;  //Параметры расчета
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public HrmPeriodAllocParameter AllocParameters {
            get { return _AllocParameters; }
            set { SetPropertyValue<HrmPeriodAllocParameter>("AllocParameters", ref _AllocParameters, value); }
        }

        private HrmMatrix _MatrixPlanKB; //Плановая КБ
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public HrmMatrix MatrixPlanKB {
            get { return _MatrixPlanKB; }
            set { SetPropertyValue<HrmMatrix>("MatrixPlanKB", ref _MatrixPlanKB, value); }
        }

        private HrmMatrix _MatrixPlanOZM; //Плановая ОЗМ
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public HrmMatrix MatrixPlanOZM {
            get { return _MatrixPlanOZM; }
            set { SetPropertyValue<HrmMatrix>("MatrixPlanOZM", ref _MatrixPlanOZM, value); }
        }

        private HrmMatrix _MatrixAllocKB;  //Приведенная матрица КБ
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public HrmMatrix MatrixAllocKB {
            get { return _MatrixAllocKB; }
            set { SetPropertyValue<HrmMatrix>("MatrixAllocKB", ref _MatrixAllocKB, value); }
        }

        private HrmMatrix _MatrixAllocOZM;  //Приведенная матрица ОЗМ
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public HrmMatrix MatrixAllocOZM {
            get { return _MatrixAllocOZM; }
            set { SetPropertyValue<HrmMatrix>("MatrixAllocOZM", ref _MatrixAllocOZM, value); }
        }

        private HrmMatrix _MatrixPlanMoney; // Матрица с деньгами
        [Browsable(false)]
        public HrmMatrix MatrixPlanMoney {
            get { return _MatrixPlanMoney; }
            set { SetPropertyValue<HrmMatrix>("MatrixPlanMoney", ref _MatrixPlanMoney, value); }
        }
        
        private HrmMatrix _ProvisionMatrix;  //Матрица резерва
        [Browsable(false)]
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public HrmMatrix ProvisionMatrix {
            get { return _ProvisionMatrix; }
            set { SetPropertyValue<HrmMatrix>("ProvisionMatrix", ref _ProvisionMatrix, value); }
        }

        private HrmTimeSheet _TimeSheet; //Табель
        [Browsable(false)]
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public HrmTimeSheet TimeSheet {
            get { return _TimeSheet; }
            set { SetPropertyValue<HrmTimeSheet>("TimeSheet", ref _TimeSheet, value); }
        }

        private HrmTimeSheet _CurrentTimeSheetKB; // Ссылка на HrmTimeSheet
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public HrmTimeSheet CurrentTimeSheetKB {
            get { return _CurrentTimeSheetKB; }
            set { SetPropertyValue<HrmTimeSheet>("CurrentTimeSheetKB", ref _CurrentTimeSheetKB, value); }
        }

        private HrmTimeSheet _CurrentTimeSheetOZM; // Ссылка на HrmTimeSheet
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public HrmTimeSheet CurrentTimeSheetOZM {
            get { return _CurrentTimeSheetOZM; }
            set { SetPropertyValue<HrmTimeSheet>("CurrentTimeSheetOZM", ref _CurrentTimeSheetOZM, value); }
        }

         private HrmMatrix _AllocResultKB; //Первичная проводка КБ
         [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
         public HrmMatrix AllocResultKB {
             get { return _AllocResultKB; }
             set { SetPropertyValue<HrmMatrix>("AllocResultKB", ref _AllocResultKB, value); }
         }

         private HrmMatrix _AllocResultOZM; //Первичная проводка ОЗМ
         [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
         public HrmMatrix AllocResultOZM {
             get { return _AllocResultOZM; }
             set { SetPropertyValue<HrmMatrix>("AllocResultOZM", ref _AllocResultOZM, value); }
         }




        [NonPersistent]
        public class OrderSet : XPCustomObject {
            public fmCOrder Order;
            public FmCOrderTypeControl TypeControl;
            public Int64 Base;
            public Int64 SourceProvision;
            public Int64 NewProvision;
            public Int64 DeltaProvision;
            public Int64 OrderPlan;
            public Int64 PlannedTravels;
            public Int64 PrefatoryOrderFact;
            public Int64 FactTravels;
            public Int64 PlanKB;
            public Int64 PlannedTrvaelsKB;
            public Int64 PrefatoryFactKB;
            public Int64 FactTravelsKB;
            public Int64 PlanOZM;
            public Int64 PlannedTravelsOZM;
            public Int64 PrefatoryFactOZM;
            public Int64 FactTravelsOZM;
            public IList<DepartmentSet> DepartmentItems = new List<DepartmentSet>();
            public OrderSet(Session session) : base(session) { }
        }

        [NonPersistent]
        public class DepartmentSet : XPCustomObject {
            public Department Department;
            public DepartmentGroupDep Group;
            public Int64 Base;
            public Int64 SourceProvision;
            public Int64 NewProvision;
            public Int64 DeltaProvision;
            public Int64 DepartmentPlan;
            public Int64 PlannedTravels;
            public Int64 PrefactoryDepartmentFact;
            public Int64 FactTravels;
            public IList<OrderSet> OrderItems = new List<OrderSet>();
            public DepartmentSet(Session session) : base(session) { }
        }


        private IList<OrderSet> _Order;
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public IList<OrderSet> Order {
            get {
                if (_Order == null) {
                    _Order = new List<OrderSet>();
                    orderCreate();
                }
                return _Order;
            }
        }



        private IList<DepartmentSet> _Department;
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public IList<DepartmentSet> Department {
            get {
                if (_Department == null) {
                    _Department = new List<DepartmentSet>();
                    departmentCreate();
                }
                return _Department;
            }
        }

        protected void orderCreate() { LoadMatrixOrder(ProvisionMatrix, null, Order); }
        protected void departmentCreate() { LoadMatrixDepartment(ProvisionMatrix, null, Department); }

        protected void LoadMatrixOrder(HrmMatrix matrix, HrmMatrixColumn col, IList<OrderSet> items  ) {
            foreach (HrmMatrixRow row in matrix.Rows) {
                if (col != null && row.Cells.FirstOrDefault(x => x.Column == col) == null)
                    continue;
                OrderSet item = items.FirstOrDefault(x => x.Order == row.Order);
                if (item == null) {
                    item = new OrderSet(this.Session) {
                        Order = row.Order
                    };
                    items.Add(item);
                }
                item.TypeControl = row.Order.TypeControl;


                  foreach (var c in row.Cells) {
                    item.OrderPlan += Convert.ToInt64(c.PlanMoney);
                    item.Base += Convert.ToInt64(c.MoneyNoReserve);
                    item.NewProvision += Convert.ToInt64(c.NewProvision);
                    item.SourceProvision += Convert.ToInt64(c.SourceProvision);
                    item.PlannedTravels += Convert.ToInt64(c.TravelMoney);
                    if (c.Column.Department.GroupDep == DepartmentGroupDep.DEPARTMENT_KB) { item.PlanKB += Convert.ToInt64(c.PlanMoney);
                    }
                    else if (c.Column.Department.GroupDep == DepartmentGroupDep.DEPARTMENT_OZM) { item.PlanOZM += Convert.ToInt64(c.PlanMoney); }
                }
                  item.DeltaProvision = (item.NewProvision - item.SourceProvision);
                  item.PrefatoryOrderFact = item.NewProvision + item.Base;

                  foreach (var c in row.Cells) {
                      if (c.Column.Department.GroupDep == DepartmentGroupDep.DEPARTMENT_KB) {
                          item.PrefatoryFactKB += Convert.ToInt64(c.MoneyNoReserve + c.NewProvision);
                      } else {
                          item.PrefatoryFactOZM += Convert.ToInt64(c.MoneyNoReserve + c.NewProvision);
                      }                  
                  }



                item.DepartmentItems = new List<DepartmentSet>();
                if (col == null)
                    LoadMatrixDepartment(matrix, row, item.DepartmentItems);
           
            }
        
        
        }
        protected void LoadMatrixDepartment(HrmMatrix matrix, HrmMatrixRow row, IList<DepartmentSet> items) {
            foreach (HrmMatrixColumn col in matrix.Columns) {
                if (row != null && col.Cells.FirstOrDefault(x => x.Row == row) == null)
                    continue;
                DepartmentSet item = items.FirstOrDefault(x => x.Department == col.Department);
                if (item == null) {
                    item = new DepartmentSet(this.Session) {
                        Department = col.Department // Подразделение
                    };
                }
                items.Add(item);
                item.OrderItems = new List<OrderSet>();
                item.Group = col.Department.GroupDep;
                foreach (var c in col.Cells) {
                    item.DepartmentPlan +=Convert.ToInt64(c.PlanMoney);
                    item.SourceProvision += Convert.ToInt64(c.SourceProvision);
                    item.NewProvision += Convert.ToInt64(c.NewProvision);
                    item.Base += Convert.ToInt64(c.MoneyNoReserve);
                }
                item.DeltaProvision=(item.NewProvision-item.SourceProvision);
                item.PrefactoryDepartmentFact = item.NewProvision + item.Base;
                if (row == null)
                    LoadMatrixOrder(matrix, col, item.OrderItems);
            }
        }


        
        public HrmSalaryTaskProvisionMatrixReduction(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }
}