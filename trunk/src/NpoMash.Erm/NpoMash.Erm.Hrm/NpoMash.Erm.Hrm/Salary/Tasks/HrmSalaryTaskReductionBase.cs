using System;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;
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
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.FM.Order;

namespace NpoMash.Erm.Hrm.Salary {

    [MapInheritance(MapInheritanceType.ParentTable)]
    public class HrmSalaryTaskReductionBase : HrmSalaryTask {

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



        [NonPersistent]
        public class DepartmentItem : XPCustomObject {
            public Department Department;
            public DepartmentGroupDep Group;
            public Decimal DepartmentPlan;
            public Decimal MinimizeNumberOfDeviationsAlloc;
            public Decimal MinimizeMaximumDeviationsAlloc;
            public Decimal ProportionsMethodAlloc;
            //Поля для контроля трудоемкости
            public Decimal DepartmentTravelPlan;
            public Decimal ConstantOrderType;
            public Decimal DepartmentFact;
            public Decimal DepartmentTravelFact;
            public Decimal Plan_Fact;
            //
            public IList<OrderItem> OrderItems = new List<OrderItem>();
            public DepartmentItem(Session session) : base(session) { }
        }

        [NonPersistent]
        public class OrderItem : XPCustomObject {
            public fmCOrder Order;
            public FmCOrderTypeControl TypeControl;
            public Decimal OrderPlan;
            public Decimal MinimizeNumberOfDeviationsAlloc;
            public Decimal MinimizeMaximumDeviationsAlloc;
            public Decimal ProportionsMethodAlloc;
            //Поля для контроля трудоемкости
            public Decimal TravelPlan;
            public Decimal ConstantOrderType;
            public Decimal OrderFact_ConstantOrderType;
            public Decimal TravelFact;
            public Decimal Plan_Fact;
            //
            public IList<DepartmentItem> DepartmentItems = new List<DepartmentItem>();
            public OrderItem(Session session) : base(session) { }
        }






        public HrmSalaryTaskReductionBase(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }
}
