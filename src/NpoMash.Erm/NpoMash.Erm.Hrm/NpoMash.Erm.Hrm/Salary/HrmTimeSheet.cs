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
    public class HrmTimeSheet : BaseObject {

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


        public HrmTimeSheet(Session session): base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }
}
