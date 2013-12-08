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

    public enum HrmPeriodAllocParameterStatus
    {
        OpenToEdit = 1,
        ListOfOrderAccepted = 2,
        AllocParametersAccepted = 3
    }
    [Persistent("HrmPeriodAllocParameter")]
    [NavigationItem("A1 Integration")]
    public class HrmPeriodAllocParameter : BaseObject
    {

        private HrmPeriodAllocParameterStatus _Status;
        public HrmPeriodAllocParameterStatus Status {
               get { return _Status; }
               set { SetPropertyValue<HrmPeriodAllocParameterStatus>("Status", ref _Status, value); } 
        }

        private Decimal _NormNoControlKB;
        public Decimal NormNoControlKB {
            get { return _NormNoControlKB; }
            set { SetPropertyValue<Decimal>("NormNoControlKB", ref _NormNoControlKB, value); }
        }

        private Decimal _NormNoControlOZM;
        public Decimal NormNoControlOZM {
            get { return _NormNoControlKB; }
            set { SetPropertyValue<Decimal>("NormNoControlOZM", ref _NormNoControlOZM, value); }
        }

        private HrmPeriod _Period; // סגÿחü ס HrmPeriod
        [Association("Period-AllocParameters")]
        public HrmPeriod Period {
            get { return _Period; }
            set { SetPropertyValue<HrmPeriod>("Period", ref _Period, value); }
        }  

        [Association("AllocParameter-OrderControls"), Aggregated]  // סגÿחü ס HrmPeriodOrderControl
        public XPCollection<HrmPeriodOrderControl> OrderControls {
               get{ return GetCollection<HrmPeriodOrderControl>("OrderControls");} 
        }

        [Association("AllocParameter-PeriodPayTypes"), Aggregated]  // סגÿחü ס HrmPeriodPayTypes
        public XPCollection<HrmPeriodPayType> PeriodPayTypes
        {
            get { return GetCollection<HrmPeriodPayType>("PeriodPayTypes"); }
        }
       
        public HrmPeriodAllocParameter(Session session) : base(session) { }

        public override void AfterConstruction(){
            base.AfterConstruction();
            Status = HrmPeriodAllocParameterStatus.OpenToEdit;
        }
    
    }
}
