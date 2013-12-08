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
    //[Persistent("HrmPeriodLogic")]
    public static class HrmPeriodLogic : BaseObject {


        public static HrmPeriod findLastPeriod(IObjectSpace os) {

            var period_list = os.GetObjects<HrmPeriod>();
            HrmPeriod last_period=null;
            var maxYear = period_list.Max(Period => Period.Year);
            List<HrmPeriod> HrmPeriodMaxYearsCollection = new List<HrmPeriod>(); //������ �������� � ������������ �����
            //��������� ���� ����
            foreach (var a in period_list) {
                if (a.Year == maxYear) {
                    HrmPeriodMaxYearsCollection.Add(a);
                }
            }
            var maxMonth = HrmPeriodMaxYearsCollection.Max(myProd => myProd.Month); //������������ ����� � ���� ���������
            //var last_period = os.CreateObject<HrmPeriod>();

            foreach (var a in period_list) {
                if ((a.Year == maxYear) && (a.Month == maxMonth)) {
                    last_period=a;
                }
            }
            return last_period; // ���������� ��������� ������
        }

        public static HrmPeriod createPeriod(IObjectSpace os) {
            HrmPeriod last_period = findLastPeriod(os);
            if (last_period.Status == HrmPeriodStatus.Opened){
                throw new Exception("���� ���������� ������");
            }
            HrmPeriod new_period = os.CreateObject<HrmPeriod>();
            addMonth(new_period, last_period.Year, last_period.Month);
            new_period.Status = HrmPeriodStatus.Opened;
            new_period.PeriodPrevious = last_period;
            return new_period;
        }

        public static void addMonth(HrmPeriod hp, Int16 m, Int16 y) {
            m++;
            if (m > 12) {
                m = 1;
                y++;
            }
            hp.Init(m, y);
        }

        public HrmPeriodLogic(Session session): base(session){ }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
        
    }
}
