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

    [MapInheritance(MapInheritanceType.ParentTable)]
    public class HrmTimeSheetPeriodObject : HrmPeriodObject {

        [ExplicitLoading(0)]
        [DevExpress.Xpo.Aggregated]
        [Persistent("TimeSheet")]
        private HrmTimeSheet _TimeSheet;

        public override IPeriodObject Instance {
            get { return _TimeSheet; }
        }

        public override Type PeriodObjectType {
            get { return typeof(HrmTimeSheet); }
        }

        //private HrmTimeSheet _TimeSheet;
        //public HrmTimeSheet TimeSheet {
        //    get { return _TimeSheet; }
        //    set { SetPropertyValue<HrmTimeSheet>("TimeSheet", ref _TimeSheet, value); }
        //}

        public HrmTimeSheetPeriodObject(Session session)
            : base(session) {
        }
        public HrmTimeSheetPeriodObject(HrmTimeSheet instance)
            : base(instance.Session) {
            _TimeSheet = instance;
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }

    }
}
