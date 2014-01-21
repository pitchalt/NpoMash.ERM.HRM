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
    public class OpenPeriodExistsException : Exception
    {
        public OpenPeriodExistsException() : base() { }
        public OpenPeriodExistsException(string message):base(message){}
    }


    public static class HrmPeriodLogic {

      private const Int16 INIT_YEAR = 2012;
      private const Int16 INIT_MONTH = 11;

        public static HrmPeriod findLastPeriod(IObjectSpace os) {
            var period_list = os.GetObjects<HrmPeriod>(null,true);
            HrmPeriod last_period=null;
            if (period_list.Count() != 0) {
                var maxYear = period_list.Max(Period => Period.Year);
                List<HrmPeriod> HrmPeriodMaxYearsCollection = new List<HrmPeriod>(); //Список периодов с максимальным годом
                //Формируем этот лист
                foreach (HrmPeriod a in period_list) {
                    if (a.Year == maxYear) {
                        HrmPeriodMaxYearsCollection.Add(a);
                    }
                }
                Int16 maxMonth = HrmPeriodMaxYearsCollection.Max(myProd => myProd.Month); //Максимальный месяц в этой коллекции
                foreach (HrmPeriod a in period_list) {
                    if ((a.Year == maxYear) && (a.Month == maxMonth)) {
                        last_period = a;
                    }
                }
            }
            return last_period; // Возваращем последний период
        }

        public static void addMonth(HrmPeriod period_with_next_month, Int16 y, Int16 m) {
            m++;
            if (m > 12) {
                m = 1;
                y++;
            }
            period_with_next_month.Init(y, m);
        }

        public static HrmPeriod createPeriod(IObjectSpace os) {
            HrmPeriod last_period = findLastPeriod(os);
            if (last_period != null && last_period.Status != HrmPeriodStatus.CLOSED){
                throw new OpenPeriodExistsException("Есть незакрытый период");
            }
            HrmPeriod new_period = os.CreateObject<HrmPeriod>();
            if (last_period == null) {
                new_period.PeriodPrevious = new_period;
                new_period.Init(INIT_YEAR,INIT_MONTH);
            }
            else {
                addMonth(new_period, last_period.Year, last_period.Month);
                new_period.PeriodPrevious = last_period;
            }
            new_period.setStatus(HrmPeriodStatus.OPENED);
            return new_period;
        }

        
    }
}
