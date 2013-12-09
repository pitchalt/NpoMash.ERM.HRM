using System;
using System.Text;
using System.Collections.Generic;

using IntecoAG.Erm.HRM;
using IntecoAG.Erm.FM.Order;
using NpoMash.Erm.Hrm.Salary;
using IntecoAG.Erm.HRM.Organization;

using DevExpress.ExpressApp;

namespace NpoMash.Erm.Hrm.Tests.Controllers {


    public static class TestWCLogic {

        private const int _HRM_PERIOD_COUNT = 3;

        public static int intRandomValue() {
            var random = new Random();
            return random.Next( 1000, 10000000 );
        }

        public static void addTestData( IObjectSpace a_object_space ) {
            var period_list = new List<HrmPeriod>();
            for ( int i = 0 ; i < _HRM_PERIOD_COUNT ; i++ ) {
                var period = HrmPeriodLogic.createPeriod( a_object_space );
                if ( i < _HRM_PERIOD_COUNT - 1 ) {
                    period.Status = HrmPeriodStatus.closed;
                }
                else {
                    period.Status = HrmPeriodStatus.Opened;
                }
            }
        }
    }
}