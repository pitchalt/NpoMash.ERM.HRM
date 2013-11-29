using System;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;
//
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace NpoMash.Erm.Hrm
{
    using NpoMash.Erm.Hrm.Salary;

    

    [NavigationItem("A1 Integration")]
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
               set {
                   if (value > 12)
                   {
                       Int16 newYear = Convert.ToInt16(Year + Convert.ToInt16(value /12));
                       SetPropertyValue<Int16>("Year", ref _Year, newYear);
                       value %= 12;
                   }
                   SetPropertyValue<Int16>("Month", ref _Month, value); } }


        public enum HrmPeriodStatus
        { Opened=0,closed=1 }
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

        // Сслыка на самого себя 
        private HrmPeriod _HrmPeriod;
        public HrmPeriod hrmPeriod {
            get { return _HrmPeriod; }
            set { SetPropertyValue<HrmPeriod>("hrmPeriod", ref _HrmPeriod, value); }
        }

        public HrmPeriod(Session session) : base(session) { }
        public override void AfterConstruction()
        { base.AfterConstruction();
        Status = HrmPeriodStatus.Opened;
        }
        
    }
}
