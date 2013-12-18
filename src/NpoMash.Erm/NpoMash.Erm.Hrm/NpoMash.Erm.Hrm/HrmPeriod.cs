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

    public enum HrmPeriodStatus { Opened = 1, closed = 2 }
    [NavigationItem("A1 Integration")]
    [Persistent("HrmPeriod")]
    [RuleCombinationOfPropertiesIsUnique("", DefaultContexts.Save, "Year, Month")]
    [Appearance("Enabled", TargetItems = "*", Criteria = "Status = 'closed'", Context = "Any", Enabled = false)]
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

        private HrmTimeSheet _CurrentTimeSheet; // ������ �� HrmTimeSheet
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public HrmTimeSheet CurrentTimeSheet {
            get { return _CurrentTimeSheet; }
            set { SetPropertyValue<HrmTimeSheet>("CurrentTimeSheet", ref _CurrentTimeSheet, value); }
        }

        [Association("Period-TimeSheets")] // ��������� HrmTimeSheet
        public XPCollection<HrmTimeSheet> TimeSheets {
            get { return GetCollection<HrmTimeSheet>("TimeSheets"); }
        }



        private HrmPeriodStatus _Status;
        [RuleRequiredField(DefaultContexts.Save)]
        public HrmPeriodStatus Status {
            get { return _Status; }
            set { SetPropertyValue<HrmPeriodStatus>("Status", ref _Status, value); }
        }


        private HrmPeriodAllocParameter _CurrentAllocParameter; // ������ �� HrmPeriodAllocParameter
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public HrmPeriodAllocParameter CurrentAllocParameter {
            get { return _CurrentAllocParameter; }
            set { SetPropertyValue<HrmPeriodAllocParameter>("CurrentAllocParameter", ref _CurrentAllocParameter, value); }
        }

        [Association("Period-AllocParameters")]   // ��������� HrmPeriodAllocParameter
        public XPCollection<HrmPeriodAllocParameter> AllocParameters {
            get { return GetCollection<HrmPeriodAllocParameter>("AllocParameters"); }
        }

        private HrmPeriod _PeriodPrevious; // ������ �� ������ ����
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public HrmPeriod PeriodPrevious {
            get { return _PeriodPrevious; }
            set { SetPropertyValue<HrmPeriod>("PeriodPrevious", ref _PeriodPrevious, value); }
        }


        [Association("Period-Matrixs"), Aggregated] //��������� Matrixs
        public XPCollection<HrmMatrix> Matrixs {
            get { return GetCollection<HrmMatrix>("Matrixs"); }
        }

        public void Init(Int16 y, Int16 m) {
            SetPropertyValue<Int16>("Year", ref _Year, y);
            SetPropertyValue<Int16>("Month", ref _Month, m);
        }

        public HrmPeriod(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            Status = HrmPeriodStatus.Opened;
        }
    }
}
