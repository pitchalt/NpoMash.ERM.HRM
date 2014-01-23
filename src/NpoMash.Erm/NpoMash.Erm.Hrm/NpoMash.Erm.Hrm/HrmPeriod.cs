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
        OPENED = 0,
        SOURCE_DATA_LOADED=1,
        LIST_OF_CONTROLLED_ORDERS_ACCEPTED=2,
        READY_TO_CALCULATE_COERCED_MATRIXS=3,        
        CLOSED = 4,
        READY_TO_EXPORT_CORCED_MATRIXS = 5,
        COERCED_MATRIXES_EXPORTED = 6,
        ACCOUNT_OPERATION_FIRST_IMPORTED=7,
        READY_TO_RESERVE_MATRIX_CREATE=8,
        READY_TO_RESERVE_MATRIX_UPLOAD=9,
        RESERVE_MATRIX_UPLOADED = 10,
        ACCOUNT_OPERATION_LAST_IMPORTED=11
    }

    [NavigationItem("A1 Integration")]
    [Persistent("HrmPeriod")]
    [RuleCombinationOfPropertiesIsUnique("", DefaultContexts.Save, "Year, Month")]
    [Appearance("Enabled", TargetItems = "*", Criteria = "Status = 'closed'", Context = "Any", Enabled = false)]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "BringingMatrixAction, BringingOZMMatrixAction", Criteria = "isReadyToBringMatrixes", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "GetSourceDataAction", Criteria = "isSourceDataImported", Context = "Any", Visibility = ViewItemVisibility.Hide)]
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

        private HrmTimeSheet _CurrentTimeSheetKB; // Ссылка на HrmTimeSheet
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public HrmTimeSheet CurrentTimeSheetKB {
            get { return _CurrentTimeSheetKB; }
            set { SetPropertyValue<HrmTimeSheet>("CurrentTimeSheetKB", ref _CurrentTimeSheetKB, value); }
        }
        
        private HrmTimeSheet _CurrentTimeSheetOZM; // Ссылка на HrmTimeSheet
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public HrmTimeSheet CurrentTimeSheetOZM {
            get { return _CurrentTimeSheetOZM; }
            set { SetPropertyValue<HrmTimeSheet>("CurrentTimeSheetOZM", ref _CurrentTimeSheetOZM, value); }
        }
        
        private HrmSalaryTaskMatrixReduction _CurrentOZMmatrixReduction;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public HrmSalaryTaskMatrixReduction CurrentOZMmatrixReduction {
            get { return _CurrentOZMmatrixReduction; }
            set { SetPropertyValue<HrmSalaryTaskMatrixReduction>("CurrentOZMmatrixReduction", ref _CurrentOZMmatrixReduction, value); }
        }

        
        private HrmSalaryTaskMatrixReduction _CurrentKBmatrixReduction;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public HrmSalaryTaskMatrixReduction CurrentKBmatrixReduction {
            get { return _CurrentKBmatrixReduction; }
            set { SetPropertyValue<HrmSalaryTaskMatrixReduction>("CurrentKBmatrixReduction", ref _CurrentKBmatrixReduction, value); }
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

        [Association("HrmPeriod-HrmSalaryTask"), Aggregated]
        public XPCollection<HrmSalaryTask> PeriodTasks {
            get { return GetCollection<HrmSalaryTask>("PeriodTasks"); }
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


        //private HrmSalaryTaskMatrixReduction _Card;
        //[VisibleInDetailView(false)]
        //[VisibleInListView(false)]
        //[VisibleInLookupListView(false)]
        //public HrmSalaryTaskMatrixReduction Card {
            //get { return _Card; }
            //set { SetPropertyValue<HrmSalaryTaskMatrixReduction>("Card", ref _Card, value); }
        //}

        public void setStatus(HrmPeriodStatus stat) {
            /*if (Status == HrmPeriodStatus.SOURCE_DATA_LOADED && stat == HrmPeriodStatus.LIST_OF_CONTROLLED_ORDERS_ACCEPTED ||
                Status == HrmPeriodStatus.LIST_OF_CONTROLLED_ORDERS_ACCEPTED && stat == HrmPeriodStatus.SOURCE_DATA_LOADED)
                stat = HrmPeriodStatus.READY_TO_CALCULATE_COERCED_MATRIXS;*/
            if (stat == HrmPeriodStatus.READY_TO_CALCULATE_COERCED_MATRIXS) {
                try {
                    stat = HrmPeriodLogic.SetReadyToCalculateCoercedMatrixesStatus(this);
                }
                catch (InvalidOperationException) {
                    stat = Status;
                }
            }
            SetPropertyValue<HrmPeriodStatus>("Status", ref _Status, stat);
        }




        public void Init(Int16 y, Int16 m) {
            SetPropertyValue<Int16>("Year", ref _Year, y);
            SetPropertyValue<Int16>("Month", ref _Month, m);
        }

        public HrmPeriod(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            setStatus(HrmPeriodStatus.OPENED);
            //Status = HrmPeriodStatus.Opened;
        }

        [Browsable(false)]
        private bool isReadyToBringMatrixes { get { return !(Status == HrmPeriodStatus.READY_TO_CALCULATE_COERCED_MATRIXS); } }

        [Browsable(false)]
        private bool isSourceDataImported { get { return HrmPeriodLogic.SourceDataIsLoaded(this); } }
    }
}
