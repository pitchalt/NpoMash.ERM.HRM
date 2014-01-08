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
using IntecoAG.ERM.HRM;
using IntecoAG.ERM.FM.Order;

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
    [Appearance(null, TargetItems = "*", Criteria = "Status = 'AllocParametersAccepted'", Context = "Any",  Enabled = false)]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "Delete", Context = "Any", Visibility = ViewItemVisibility.Hide, Enabled=false)]
    [Appearance("", AppearanceItemType = "Action", TargetItems = "AcceptAllocParameters", Context = "Any", Visibility = ViewItemVisibility.Show, Criteria = "Status=='ListOfOrderAccepted'")]
    [Appearance("", AppearanceItemType = "Action", TargetItems = "HrmPeriodAllocParameterVC_AcceptAllocParameters", Context = "Any", Visibility = ViewItemVisibility.Show, Criteria = "Status=='OpenToEdit'")]    
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
        [ModelDefault("DisplayFormat", "{0:N}")]
        [RuleValueComparison(null, DefaultContexts.Save, ValueComparisonType.GreaterThan, 0)]
        public Decimal NormNoControlKB {
            get { return _NormNoControlKB; }
            set { SetPropertyValue<Decimal>("NormNoControlKB", ref _NormNoControlKB, value); }
        }

        private Decimal _NormNoControlOZM;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [RuleRequiredField(DefaultContexts.Save)]
        [ModelDefault("DisplayFormat", "{0:N}")]
        [RuleValueComparison(null, DefaultContexts.Save, ValueComparisonType.GreaterThan, 0)]
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

        private Int16 _IterationNumber;
        public Int16 IterationNumber {
            get { return _IterationNumber; }
            set { SetPropertyValue<Int16>("IterationNumber", ref _IterationNumber, value); }
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
            StatusSet(HrmPeriodAllocParameterStatus.OpenToEdit);
        }

        public void StatusSet(HrmPeriodAllocParameterStatus status){
//            _Status = status;
            SetPropertyValue<HrmPeriodAllocParameterStatus>("Status", ref _Status, status);
        }

    }

    
    
}


