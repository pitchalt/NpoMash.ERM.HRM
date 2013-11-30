using System;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using NpoMash.Erm.Hrm.Salary;
//kovtushenko checkout
//demin checkout
namespace NpoMash.Erm.Hrm {

    [NavigationItem("A1 Integration")]
    [Persistent("HrmPeriod")]
    public class HrmPeriod : BaseObject {
        private Int16 _Year;
        public Int16 Year {
            get { return _Year; }
            set { SetPropertyValue<Int16>("Year", ref _Year, value); }
        }

        private Int16 _Month;
        public Int16 Month {
            get { return _Month; }
            set { SetPropertyValue<Int16>("Month", ref _Month, value); }
        }


        public enum HrmPeriodStatus { Opened = 0, closed = 1 }
        private HrmPeriodStatus _Status;
        public HrmPeriodStatus Status {
            get { return _Status; }
            set { SetPropertyValue<HrmPeriodStatus>("Status", ref _Status, value); }
        }

        //////////////////////Связи

        // связь с HrmPeriodAllocParameter
        private HrmPeriodAllocParameter _HrmPeriodAllocParameter;
        public HrmPeriodAllocParameter HrmPeriodAllocParameter {
            get { return _HrmPeriodAllocParameter; }
            set { SetPropertyValue<HrmPeriodAllocParameter>("HrmPeriodAllocParameter", ref _HrmPeriodAllocParameter, value); }
        }

        // Сслыка на самого себя 
        private HrmPeriod _Previous;
        public HrmPeriod Previous {
            get { return _Previous; }
            set { SetPropertyValue<HrmPeriod>("Previous", ref _Previous, value); }
        }

        public HrmPeriod(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            Status = HrmPeriodStatus.Opened;
        }

        public void addMonth() {
            Int16 m = Month;
            m++;
            if (m > 12) {
                m = 1;
                Int16 y = Year;
                y++;
                SetPropertyValue<Int16>("Year", ref _Year, y);
            }
            SetPropertyValue<Int16>("Month", ref _Month, m);
        }
    }
}
