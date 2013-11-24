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
    using NpoMash.Erm.Hrm.Salary;

    public enum HrmPeriodStatus
    { }

    [NavigationItem("NpoMash.Hrm")]
    [Persistent("HrmPeriod")]


    public class HrmPeriod : BaseObject
    { 
        private Int16 _Year;
        public Int16 Year {
               get { return _Year; }
               set { SetPropertyValue<Int16>("Year", ref _Year, value); } }

        private Int16 _Month;
        public Int16 Month {
               get { return _Month; }
               set { SetPropertyValue<Int16>("Month", ref _Month, value); } }
        
        private HrmPeriodStatus _Status;
        public HrmPeriodStatus Status {
               get { return _Status; }
               set { SetPropertyValue<HrmPeriodStatus>("Status", ref _Status, value); } }

        //////////////////////Связи

        // связь с HrmPeriodAllocParameter
        private HrmPeriodAllocParameter _HrmPeriodAllocParameter;
        public HrmPeriodAllocParameter HrmPeriodAllocParameter {
               get { return _HrmPeriodAllocParameter; }
               set { SetPropertyValue<HrmPeriodAllocParameter>("HrmPeriodAllocParameter", ref _HrmPeriodAllocParameter, value); } }


        public HrmPeriod(Session session) : base(session) { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            
        }
        
    }
}
