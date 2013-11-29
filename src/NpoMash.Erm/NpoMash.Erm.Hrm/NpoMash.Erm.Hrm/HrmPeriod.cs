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
               set { SetPropertyValue<Int16>("Month", ref _Month, value); } }


        public enum HrmPeriodStatus
        { Opened=0,closed=1 }
        private HrmPeriodStatus _Status;
        public HrmPeriodStatus Status {
               get { return _Status; }
               set { SetPropertyValue<HrmPeriodStatus>("Status", ref _Status, value); } }
        

        //////////////////////�����

        // ����� � HrmPeriodAllocParameter
        private HrmPeriodAllocParameter _HrmPeriodAllocParameter;
        public HrmPeriodAllocParameter HrmPeriodAllocParameter {
               get { return _HrmPeriodAllocParameter; }
               set { SetPropertyValue<HrmPeriodAllocParameter>("HrmPeriodAllocParameter", ref _HrmPeriodAllocParameter, value); } }

        // ������ �� ������ ���� 
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

        public void addMonth()
        {
            Int16 m = Month;
            m++;
            if (m > 12)
            {
                m = 1;
                Int16 y = Year;
                y++;
                SetPropertyValue<Int16>("Year", ref _Year, y);
            }
            SetPropertyValue<Int16>("Month", ref _Month, m);
        }        
    }
}
