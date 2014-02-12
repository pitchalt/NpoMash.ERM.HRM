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

namespace NpoMash.Erm.Hrm.Salary {
    public class HrmSalaryTaskProvisionMatrixReduction : HrmSalaryTask {


        private HrmPeriodAllocParameter _AllocParameters;  // Параметры расчета
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public HrmPeriodAllocParameter AllocParameters {
            get { return _AllocParameters; }
            set { SetPropertyValue<HrmPeriodAllocParameter>("AllocParameters", ref _AllocParameters, value); }
        }


        private HrmMatrix _MatrixPlan;  // Плановая матрица
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public HrmMatrix MatrixPlan {
            get { return _MatrixPlan; }
            set { SetPropertyValue<HrmMatrix>("MatrixPlan", ref _MatrixPlan, value); }
        }


        private HrmMatrix _MatrixAlloc;  // Приведенная матрица
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public HrmMatrix MatrixAlloc {
            get { return _MatrixAlloc; }
            set { SetPropertyValue<HrmMatrix>("MatrixAlloc", ref _MatrixAlloc, value); }
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

            public OrderSet(Session session) : base(session) { }
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

        protected void orderCreate() { LoadMatrixOrder(  ); }
        protected void LoadMatrixOrder(  ) { }










        
        public HrmSalaryTaskProvisionMatrixReduction(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }
}
