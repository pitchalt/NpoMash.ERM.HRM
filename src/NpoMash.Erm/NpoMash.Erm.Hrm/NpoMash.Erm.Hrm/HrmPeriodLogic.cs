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


        public static HrmPeriod findLastPeriod(IObjectSpace os) {

            var period_list = os.GetObjects<HrmPeriod>();
            HrmPeriod last_period=null;
            var maxYear = period_list.Max(Period => Period.Year);
            List<HrmPeriod> HrmPeriodMaxYearsCollection = new List<HrmPeriod>(); //Список периодов с максимальным годом
            //Формируем этот лист
            foreach (var a in period_list) {
                if (a.Year == maxYear) {
                    HrmPeriodMaxYearsCollection.Add(a);
                }
            }
            var maxMonth = HrmPeriodMaxYearsCollection.Max(myProd => myProd.Month); //Максимальный месяц в этой коллекции
            //var last_period = os.CreateObject<HrmPeriod>();

            foreach (var a in period_list) {
                if ((a.Year == maxYear) && (a.Month == maxMonth)) {
                    last_period=a;
                }
            }
            return last_period; // Возваращем последний период
        }

    public static void createPeriod(IObjectSpace os, HrmPeriod obj) { 
    
    
    
    }

    /*public void addMonth() {
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
        */


        public HrmPeriodLogic(Session session): base(session){ }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
        
    }
}
