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

        private const int _REFERENCE_COUNT = 10;
        private const int _ALLOCPARAMETER_COUNT = 3;

        public static int intRandomValue() {
            var random = new Random();
            return random.Next( 1000, 100000 );
        }

        public static void referenceClassesGenerate( IObjectSpace local_object_space ) {
            var random_way = new Random();
            for ( int i = 0 ; i < _REFERENCE_COUNT ; i++ ) {
                var fmCorder = local_object_space.CreateObject<fmCOrder>();
                var hrmSalaryPayType = local_object_space.CreateObject<HrmSalaryPayType>();
                int type_control = random_way.Next( 1, 4 );
                int type_constancy = random_way.Next( 1, 3 );
                fmCorder.Code = Convert.ToString( intRandomValue() );
                if ( type_control == 1 ) { fmCorder.TypeControl = fmCOrderTypeCOntrol.FOT; }
                if ( type_control == 2 ) { fmCorder.TypeControl = fmCOrderTypeCOntrol.No_Ordered; }
                if ( type_control == 3 ) { fmCorder.TypeControl = fmCOrderTypeCOntrol.TrudEmk_FOT; }
                if ( type_constancy == 1 ) { fmCorder.TypeConstancy = fmCOrdertypeConstancy.Null; }
                if ( type_constancy == 2 ) { fmCorder.TypeConstancy = fmCOrdertypeConstancy.One; }
                fmCorder.NormKB = Convert.ToDecimal( intRandomValue() );
                fmCorder.NormOZM = Convert.ToDecimal( intRandomValue() );
                hrmSalaryPayType.Code = Convert.ToString( intRandomValue() );
                hrmSalaryPayType.Name = Convert.ToString( intRandomValue() );
            }
        }

        public static void addTestData( IObjectSpace a_object_space ) {
            referenceClassesGenerate( a_object_space );
            for ( int i = 0 ; i < _ALLOCPARAMETER_COUNT ; i++ ) {
                var alloc_parameter = AllocParametersLogic.createParameters( a_object_space );
                foreach ( var each in a_object_space.GetObjects<HrmPeriod>( null, true ) ) {
                    each.Status = HrmPeriodStatus.closed;
                }
            }
        }
    }
}
