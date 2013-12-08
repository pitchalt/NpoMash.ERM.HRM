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
    [Persistent("HrmPeriodLogic")]
    public static class HrmPeriodLogic : BaseObject {

    public static void createPeriod(IObjectSpace os, HrmPeriod obj) {  }

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
        


        public HrmPeriodLogic(Session session): base(session){ }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
        
    }
}
