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
using NpoMash.Erm.Hrm.Salary;

namespace NpoMash.Erm.Hrm {

    public enum HrmPeriodStatus { 
        Opened = 0,
        SourceDataLoaded=1,
        ListOfControlledOrdersAccepted=2,
        ReadyToCalculateCoercedMatrixs=3,        
        Closed = 4 }

    [NavigationItem("A1 Integration")]
    [Persistent("HrmPeriod")]
    [RuleCombinationOfPropertiesIsUnique("", DefaultContexts.Save, "Year, Month")]
    [Appearance("Enabled", TargetItems = "*", Criteria = "Status = 'closed'", Context = "Any", Enabled = false)]
    [Appearance("Visibility", AppearanceItemType = "Action", TargetItems = "Delete, New", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [DefaultProperty("Status")]     
    public class HrmPeriod : BaseObject {

        [Persistent("Year")]
        private Int16 _Year;
        [Indexed("Month",Unique = true)]
        [PersistentAlias("_Year")]
        public Int16 Year {
            get { return _Year; }
        }
        
        [Persistent("Month")]
        private Int16 _Month;
        [PersistentAlias("_Month")]
        public Int16 Month {
            get { return _Month; }

        }

        private HrmTimeSheet _CurrentTimeSheet; // Ссылка на HrmTimeSheet
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public HrmTimeSheet CurrentTimeSheet {
            get { return _CurrentTimeSheet; }
            set { SetPropertyValue<HrmTimeSheet>("CurrentTimeSheet", ref _CurrentTimeSheet, value); }
        }

        [Association("Period-TimeSheets")] // Коллекция HrmTimeSheet
        public XPCollection<HrmTimeSheet> TimeSheets {
            get { return GetCollection<HrmTimeSheet>("TimeSheets"); }
        }


        [Persistent("Status")]
        private HrmPeriodStatus _Status;
        [RuleRequiredField(DefaultContexts.Save)]
        [PersistentAlias("_Status")]
        public HrmPeriodStatus Status {
            get { return _Status; }
            //set { SetPropertyValue<HrmPeriodStatus>("Status", ref _Status, value); }
        }

        private HrmSalaryTaskMatrixReduction _MatrixReduction;
        [Association("MatrixReduction-Period"), Aggregated]//ссылка на HrmSalaryTaskMatrixReduction
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public HrmSalaryTaskMatrixReduction MatrixReduction {
            get { return _MatrixReduction; }
            set { SetPropertyValue<HrmSalaryTaskMatrixReduction>("MatrixReduction", ref _MatrixReduction, value); }
        }

        private HrmPeriodAllocParameter _CurrentAllocParameter; // Ссылка на HrmPeriodAllocParameter
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public HrmPeriodAllocParameter CurrentAllocParameter {
            get { return _CurrentAllocParameter; }
            set { SetPropertyValue<HrmPeriodAllocParameter>("CurrentAllocParameter", ref _CurrentAllocParameter, value); }
        }

        [Association("Period-AllocParameters")]   // коллекция HrmPeriodAllocParameter
        public XPCollection<HrmPeriodAllocParameter> AllocParameters {
            get { return GetCollection<HrmPeriodAllocParameter>("AllocParameters"); }
        }

        private HrmPeriod _PeriodPrevious; // Сслыка на самого себя
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public HrmPeriod PeriodPrevious {
            get { return _PeriodPrevious; }
            set { SetPropertyValue<HrmPeriod>("PeriodPrevious", ref _PeriodPrevious, value); }
        }


        [Association("Period-Matrixs"), Aggregated] //Коллекция Matrixs
        [Index(0), VisibleInListView(true), VisibleInDetailView(true)] 
        public XPCollection<HrmMatrix> Matrixs {
            get { return GetCollection<HrmMatrix>("Matrixs"); }
        }


        private HrmSalaryTaskMatrixReduction _Card;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public HrmSalaryTaskMatrixReduction Card {
            get { return _Card; }
            set { SetPropertyValue<HrmSalaryTaskMatrixReduction>("Card", ref _Card, value); }
        }

        public void setStatus(HrmPeriodStatus stat) {
            if (Status == HrmPeriodStatus.SourceDataLoaded && stat == HrmPeriodStatus.ListOfControlledOrdersAccepted ||
                Status == HrmPeriodStatus.ListOfControlledOrdersAccepted && stat == HrmPeriodStatus.SourceDataLoaded)
                stat = HrmPeriodStatus.ReadyToCalculateCoercedMatrixs;
            SetPropertyValue<HrmPeriodStatus>("Status", ref _Status, stat); ;
        }




        public void Init(Int16 y, Int16 m) {
            SetPropertyValue<Int16>("Year", ref _Year, y);
            SetPropertyValue<Int16>("Month", ref _Month, m);
        }

        public HrmPeriod(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            setStatus(HrmPeriodStatus.Opened);
            //Status = HrmPeriodStatus.Opened;
        }
    }
}
