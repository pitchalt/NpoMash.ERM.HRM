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


        private HrmPeriodAllocParameter _AllocParameters;  // Параметры расчета
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public HrmPeriodAllocParameter AllocParameters {
            get { return _AllocParameters; }
            set { SetPropertyValue<HrmPeriodAllocParameter>("AllocParameters", ref _AllocParameters, value); }
        }


        private HrmMatrix _MatrixAlloc;  // Приведенная матрица
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public HrmMatrix MatrixAlloc {
            get { return _MatrixAlloc; }
            set { SetPropertyValue<HrmMatrix>("MatrixAlloc", ref _MatrixAlloc, value); }
        }

        private HrmMatrix _MatrixPlan;  // Плановая матрица
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public HrmMatrix MatrixPlan {
            get { return _MatrixPlan; }
            set { SetPropertyValue<HrmMatrix>("MatrixPlan", ref _MatrixPlan, value); }
        }

        private HrmMatrix _ProvisionMatrix;  // Матрица резерва
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public HrmMatrix ProvisionMatrix {
            get { return _ProvisionMatrix; }
            set { SetPropertyValue<HrmMatrix>("ProvisionMatrix", ref _ProvisionMatrix, value); }
        }

        private HrmAccountOperation _AcountOperation;  // Проводка
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public HrmAccountOperation AcountOperation {
            get { return _AcountOperation; }
            set { SetPropertyValue<HrmAccountOperation>("AcountOperation", ref _AcountOperation, value); }
        }

        [NonPersistent]
        public class OrderSet : XPCustomObject {
            public fmCOrder Order;
            public FmCOrderTypeControl TypeControl;
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
            public Int64 DepartmentPlan;
            public Int64 PlannedTravels;
            public Int64 PrefactoryDepartmentFact;
            public Int64 FactTravels;
            public Int64 DepartmentProvision;
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

        protected void orderCreate() { LoadMatrixOrder(MatrixPlan, null, Order); }
        protected void departmentCreate() { LoadMatrixDepartment(MatrixPlan, null, Department); }






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