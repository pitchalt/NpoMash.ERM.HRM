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

namespace NpoMash.Erm.Hrm.Salary {

    public class HrmPeriodTimeSheetBaseObject : HrmSalaryPeriodObjectBase {

        private HrmTimeSheet _TimeSheet;
        public HrmTimeSheet TimeSheet {
            get { return _TimeSheet; }
            set { SetPropertyValue<HrmTimeSheet>("TimeSheet", ref _TimeSheet, value); }
        }









        public HrmPeriodTimeSheetBaseObject(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }
}
