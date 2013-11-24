using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
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
        public HrmPeriodAllocParameter(Session session): base(session){ }

        private HrmPeriodAllocParameterStatus _Status;
        private HrmPeriod _HrmPeriod;


        public HrmPeriodAllocParameterStatus Status
        {
            get { return _Status; }
            set { SetPropertyValue<HrmPeriodAllocParameterStatus>("Status", ref _Status, value); }
        }

        //////////////////Связи
        [Association("PeriodAllocParameters-OrderControls"), Aggregated]// связь с HrmPeriodOrderControl
        public XPCollection<HrmPeriodOrderControl> OrderControls
        {
            get
            {
                return GetCollection<HrmPeriodOrderControl>("OrderControls");
            }
        }

        public HrmPeriod HrmPeriod // связь с HrmPeriod
        {
            get { return _HrmPeriod; }
            set { SetPropertyValue<HrmPeriod>("HrmPeriod", ref _HrmPeriod, value); }
        }


        public XPCollection<HrmSalaryPayType> PayTypes // связь с HrmSalaryPayType
        {
            get
            {
                return GetCollection<HrmSalaryPayType>("PayTypes");
            }
        }


        public override void AfterConstruction()
        {
            base.AfterConstruction();
           
        }
    
    }
}
