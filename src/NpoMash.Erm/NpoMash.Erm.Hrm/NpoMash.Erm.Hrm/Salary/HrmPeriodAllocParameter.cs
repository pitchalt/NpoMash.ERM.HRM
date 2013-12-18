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
using IntecoAG.Erm.HRM;
using IntecoAG.Erm.FM.Order;

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
    [Appearance("Enable", TargetItems = "*", Criteria = "Status = 'AllocParametersAccepted'", Context = "Any",  Enabled = false)]
    [Appearance("Visibility", AppearanceItemType = "Action", TargetItems = "Delete", Context = "Any", Criteria = "Status = 'AllocParametersAccepted' or Status = 'OpenToEdit' or Status = 'ListOfOrderAccepted'", Visibility = ViewItemVisibility.Hide)]
    [DefaultProperty("Status")]       
    public class HrmPeriodAllocParameter : BaseObject
    {
        
        [PersistentAlias("Period.Year")]
        public Int16 Year {
            get { return Period.Year; }
        }

        [PersistentAlias("Period.Month")]
        public Int16 Month {
            get { return Period.Month; }
        }

        [Persistent("Status")]
        private HrmPeriodAllocParameterStatus _Status;
        [RuleRequiredField(DefaultContexts.Save)]
        [PersistentAlias("_Status")]
        public HrmPeriodAllocParameterStatus Status {
               get { return _Status; }
               //set { SetPropertyValue<HrmPeriodAllocParameterStatus>("Status", ref _Status, value); } 
        }

        private Decimal _NormNoControlKB;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [RuleRequiredField(DefaultContexts.Save)]
        public Decimal NormNoControlKB {
            get { return _NormNoControlKB; }
            set { SetPropertyValue<Decimal>("NormNoControlKB", ref _NormNoControlKB, value); }
        }

        private Decimal _NormNoControlOZM;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [RuleRequiredField(DefaultContexts.Save)]
        public Decimal NormNoControlOZM {
            get { return _NormNoControlOZM; }
            set { SetPropertyValue<Decimal>("NormNoControlOZM", ref _NormNoControlOZM, value); }
        }

        private HrmPeriod _Period; // סגÿחü ס HrmPeriod
        [Association("Period-AllocParameters")]
        [RuleRequiredField(DefaultContexts.Save)]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
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
            setStatus(HrmPeriodAllocParameterStatus.OpenToEdit);
        }

        public void setStatus(HrmPeriodAllocParameterStatus s){
            SetPropertyValue<HrmPeriodAllocParameterStatus>("Status", ref _Status, s);
        }

    }

    
    
}


