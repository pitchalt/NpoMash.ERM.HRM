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

namespace NpoMash.Erm.Hrm.Salary {

    [Persistent("HrmTimeSheet")]
    [Appearance("", AppearanceItemType = "Action", TargetItems = "Delete, New", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    public class HrmTimeSheet : HrmTimeSheetBase {

        [Association("TimeSheet-TimeSheetDeps"), Aggregated] // Коллекция TimeSheetDeps
        public XPCollection<HrmTimeSheetDep> TimeSheetDeps {
            get { return GetCollection<HrmTimeSheetDep>("TimeSheetDeps"); }
        }

        private HrmPeriod _Period; // Ссылка на HrmPeriod
        [Association("Period-TimeSheets")]
        public HrmPeriod Period {
            get { return _Period; }
            set { SetPropertyValue<HrmPeriod>("Period", ref _Period, value); }
        }

        private HrmTimeSheetGroup _KB;
        [VisibleInDetailView(true)]
        //[VisibleInListView(false)]
        //[VisibleInLookupListView(false)]
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public HrmTimeSheetGroup KB {
            get { return _KB; }
            set { SetPropertyValue<HrmTimeSheetGroup>("KB", ref _KB, value); }
        }

        private HrmTimeSheetGroup _OZM;
        [VisibleInDetailView(true)]
        //[VisibleInListView(false)]
        //[VisibleInLookupListView(false)]
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public HrmTimeSheetGroup OZM {
            get { return _OZM; }
            set { SetPropertyValue<HrmTimeSheetGroup>("OZM", ref _OZM, value); }
        }

        public HrmTimeSheet(Session session): base(session) { }
        public override void AfterConstruction() { 
            base.AfterConstruction();
            OZM = new HrmTimeSheetGroup(this.Session);
            KB = new HrmTimeSheetGroup(this.Session);
            OZM.TimeSheet = this;
            KB.TimeSheet = this;
        }
    }
}
