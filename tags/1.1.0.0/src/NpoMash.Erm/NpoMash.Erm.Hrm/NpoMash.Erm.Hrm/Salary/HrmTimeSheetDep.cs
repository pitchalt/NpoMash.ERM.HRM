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

namespace NpoMash.Erm.Hrm.Salary {

    [Persistent("HrmSalaryTimeSheetDep")]
    [Appearance("", AppearanceItemType = "Action", TargetItems = "Delete, New", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    public class HrmTimeSheetDep : XPObject {

        private String _BuhCode;
        public String BuhCode {
            get { return _BuhCode; }
            set { SetPropertyValue<String>("BuhCode", ref _BuhCode, value); }
        }

        private Department _Department; //—сылка на Department
        public Department Department {
            get { return _Department; }
            set { SetPropertyValue<Department>("Department", ref _Department, value); }
        }

        private HrmTimeSheet _TimeSheet; //—сылка на HrmTimeSheet
        [Association("TimeSheet-TimeSheetDeps")]
        public HrmTimeSheet TimeSheet {
            get { return _TimeSheet; }
            set { SetPropertyValue<HrmTimeSheet>("TimeSheet", ref _TimeSheet, value); }
        }


        [ModelDefault("DisplayFormat", "{0:N}")]
        public Decimal MatrixWorkTime {
            get { return BaseWorkTime + AdditionWorkTime + TravelWorkTime + ConstantWorkTime; }

        }

        private Decimal _TravelWorkTime;
        [ModelDefault("DisplayFormat", "{0:N}")]
        public Decimal TravelWorkTime {
            get { return _TravelWorkTime; }
            set { SetPropertyValue<Decimal>("TravelWorkTime", ref _TravelWorkTime, value); }
        }

        private Decimal _ConstantWorkTime;
        [ModelDefault("DisplayFormat", "{0:N}")]
        public Decimal ConstantWorkTime {
            get { return _ConstantWorkTime; }
            set { SetPropertyValue<Decimal>("TravelWorkTime", ref _ConstantWorkTime, value); }
        }

        private Decimal _BaseWorkTime;
        [ModelDefault("DisplayFormat", "{0:N}")]
        public Decimal BaseWorkTime {
            get { return _BaseWorkTime; }
            set { SetPropertyValue<Decimal>("BaseWorkTime", ref _BaseWorkTime, value); }
        }

        private Decimal _AdditionWorkTime;
        [ModelDefault("DisplayFormat", "{0:N}")]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Decimal AdditionWorkTime {
            get { return _AdditionWorkTime; }
            set { SetPropertyValue<Decimal>("AdditionWorkTime", ref _AdditionWorkTime, value); }
        }
        public HrmTimeSheetDep(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }
}