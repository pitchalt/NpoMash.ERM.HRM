using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace NpoMash.Erm.Hrm
{
    [DefaultClassOptions]

    public enum HrmPeriodStatus
    { }

    public class HrmPeriod : BaseObject
    { 
        public HrmPeriod(Session session): base(session){ }

        private Int16 _Year;
        private Int16 _Month;
        private HrmPeriodStatus _Status;
        private HrmPeriodAllocParameter _HrmPeriodAllocParameter;

        public Int16 Year
        {
            get { return _Year; }
            set { SetPropertyValue<Int16>("Year", ref _Year, value); }
        }

        public Int16 Month
        {
            get { return _Month; }
            set { SetPropertyValue<Int16>("Month", ref _Month, value); }
        }

        public HrmPeriodStatus Status
        {
            get { return _Status; }
            set { SetPropertyValue<HrmPeriodStatus>("Status", ref _Status, value); }
        }

        //////////////////////Связи
        public HrmPeriodAllocParameter HrmPeriodAllocParameter // связь с HrmPeriodAllocParameter
        {
            get { return _HrmPeriodAllocParameter; }
            set { SetPropertyValue<HrmPeriodAllocParameter>("HrmPeriodAllocParameter", ref _HrmPeriodAllocParameter, value); }
        }




        public override void AfterConstruction()
        {
            base.AfterConstruction();
            
        }
        
    }
}
