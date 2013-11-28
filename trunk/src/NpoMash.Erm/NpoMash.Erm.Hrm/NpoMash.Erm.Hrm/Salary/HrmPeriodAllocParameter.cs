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

    
    [Persistent("HrmPeriodAllocParameter")]
    public class HrmPeriodAllocParameter : BaseObject
    {

        public enum HrmPeriodAllocParameterStatus
        { 
            OpenToEdit=0,
            ListOfOrderAccepted=1,
            AllocParametersAccepted=2
        }

        private HrmPeriodAllocParameterStatus _Status;
        public HrmPeriodAllocParameterStatus Status {
               get { return _Status; }
               set { SetPropertyValue<HrmPeriodAllocParameterStatus>("Status", ref _Status, value); } 
        }


        [Association("AllocParameter-OrderControls"), Aggregated]  // связь с HrmPeriodOrderControl
        public XPCollection<HrmPeriodOrderControl> OrderControls {
               get{ return GetCollection<HrmPeriodOrderControl>("OrderControls");} 
        }

        [Association("PeriodAllocParameters-HrmPeriodPayType"), Aggregated]  // связь с Linker
        public XPCollection<Linker> HrmPeriodPayType
        {
            get { return GetCollection<Linker>("HrmPeriodPayType"); }
        }

        private HrmPeriod _HrmPeriod;  //Связь с HrmPeriod
        public HrmPeriod HrmPeriod { 
               get { return _HrmPeriod; }
               set { SetPropertyValue<HrmPeriod>("HrmPeriod", ref _HrmPeriod, value); }
        }

      
        public HrmPeriodAllocParameter(Session session) : base(session) { }

        public override void AfterConstruction(){
            base.AfterConstruction();
            Status = HrmPeriodAllocParameterStatus.OpenToEdit;
        }
    
    }
}
