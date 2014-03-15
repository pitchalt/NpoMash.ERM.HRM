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

    [Persistent("HrmTimeSheetDep")]
    [Appearance("", AppearanceItemType = "Action", TargetItems = "Delete, New", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    public class HrmTimeSheetDep : BaseObject {

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

        public Int64 MatrixWorkTime {
            get { return BaseWorkTime + AdditionWorkTime; }

        }

        private Int64 _TravelWorkTime;
        public Int64 TravelWorkTime {
            get { return _TravelWorkTime; }
            set { SetPropertyValue<Int64>("TravelWorkTime", ref _TravelWorkTime, value); }
        }

        private Int64 _ConstantWorkTime;
        public Int64 ConstantWorkTime {
            get { return _ConstantWorkTime; }
            set { SetPropertyValue<Int64>("TravelWorkTime", ref _ConstantWorkTime, value); }
        }

        private Int64 _BaseWorkTime;
        public Int64 BaseWorkTime {
            get { return _BaseWorkTime; }
            set { SetPropertyValue<Int64>("BaseWorkTime", ref _BaseWorkTime, value); }
        }

        private Int64 _AdditionWorkTime;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Int64 AdditionWorkTime {
            get { return _AdditionWorkTime; }
            set { SetPropertyValue<Int64>("AdditionWorkTime", ref _AdditionWorkTime, value); }
        }
        public HrmTimeSheetDep(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction();}
    }
}