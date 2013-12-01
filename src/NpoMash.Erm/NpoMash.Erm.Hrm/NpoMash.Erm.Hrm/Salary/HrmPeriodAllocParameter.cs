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
//
using IntecoAG.Erm.HRM;

namespace NpoMash.Erm.Hrm.Salary
{
    [Persistent("HrmPeriodAllocParameter")]
    public class HrmPeriodAllocParameter : BaseObject
    {

        public enum HrmPeriodAllocParameterStatus
        { 
            OpenToEdit=1,
            ListOfOrderAccepted=2,
            AllocParametersAccepted=3
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

        [Association("AllocParameter-PeriodPayTypes"), Aggregated]  // связь с HrmPeriodPayTypes
        public XPCollection<HrmPeriodPayType> PeriodPayTypes
        {
            get { return GetCollection<HrmPeriodPayType>("PeriodPayTypes"); }
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
