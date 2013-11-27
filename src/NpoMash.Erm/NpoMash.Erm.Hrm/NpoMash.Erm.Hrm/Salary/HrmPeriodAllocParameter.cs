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

namespace NpoMash.Erm.Hrm.Salary
{
    using IntecoAG.Erm.HRM;

    public enum HrmPeriodAllocParameterStatus
    { }

    [Persistent("HrmPeriodAllocParameter")]
    public class HrmPeriodAllocParameter : BaseObject
    { 
       
        private HrmPeriodAllocParameterStatus _Status;
        public HrmPeriodAllocParameterStatus Status {
               get { return _Status; }
               set { SetPropertyValue<HrmPeriodAllocParameterStatus>("Status", ref _Status, value); } 
        }


        [Association("PeriodAllocParameters-OrderControls"), Aggregated]  // связь с HrmPeriodOrderControl
        public XPCollection<HrmPeriodOrderControl> OrderControls {
               get{ return GetCollection<HrmPeriodOrderControl>("OrderControls");} 
        }


        private HrmPeriod _HrmPeriod;  //Связь с HrmPeriod
        public HrmPeriod HrmPeriod { 
               get { return _HrmPeriod; }
               set { SetPropertyValue<HrmPeriod>("HrmPeriod", ref _HrmPeriod, value); }
        }

      
        public HrmPeriodAllocParameter(Session session) : base(session) { }

        public override void AfterConstruction(){
            base.AfterConstruction();
        }
    
    }
}
