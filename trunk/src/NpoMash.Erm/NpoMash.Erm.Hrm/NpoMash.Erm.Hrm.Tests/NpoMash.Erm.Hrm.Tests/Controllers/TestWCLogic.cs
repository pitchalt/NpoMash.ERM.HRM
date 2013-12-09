using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

using IntecoAG.Erm.HRM;
using IntecoAG.Erm.FM.Order;
using NpoMash.Erm.Hrm.Salary;
using IntecoAG.Erm.HRM.Organization;

using DevExpress.ExpressApp;

namespace NpoMash.Erm.Hrm.Tests.Controllers {


    public static class TestWCLogic {

        public static int intRandomValue() {
            var random = new Random();
            return random.Next( 1000, 10000000 );
        }

        public static void addTestData( IObjectSpace a_object_space ) {
            int period_count = 3;
            Int16 start_month = 0;
            Int16 start_year = 2000;
            var period_list = new List<HrmPeriod>();
            for ( int i = 0 ; i < period_count ; i++ ) {
                var period = HrmPeriodLogic.createPeriod( a_object_space );
                if ( i < period_count - 1 ) {
                    period.Status = HrmPeriodStatus.closed;
                }
                else {
                    period.Status = HrmPeriodStatus.Opened;
                }
            }
        }
    }
}