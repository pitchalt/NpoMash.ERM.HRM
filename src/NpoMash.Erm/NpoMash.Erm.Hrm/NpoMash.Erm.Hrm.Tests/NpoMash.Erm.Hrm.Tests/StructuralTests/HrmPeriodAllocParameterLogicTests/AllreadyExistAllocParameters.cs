using System;

using DevExpress.ExpressApp;

using IntecoAG.ERM.FM.Order;
using NpoMash.Erm.Hrm.Salary;

using NUnit.Framework;

namespace NpoMash.Erm.Hrm.Tests.StructuralTests.HrmPeriodAllocParameterLogicTests {


    [TestFixture]
    public class AllreadyExistAllocParameters : OptionsAllocParameters {

        [Test]
        public void AddTestData_StableState_ComplexExpext() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var alloc_parameters_list = test_object_space.GetObjects<HrmPeriodAllocParameter>();
            foreach (var alloc_parameter in alloc_parameters_list) {
                Assert.IsNotNull(alloc_parameter.Period);
                Assert.IsNotNull(alloc_parameter.OrderControls);
                Assert.IsNotEmpty(alloc_parameter.OrderControls);
                Assert.IsNotNull(alloc_parameter.PeriodPayTypes);
                Assert.IsNotEmpty(alloc_parameter.PeriodPayTypes);
                Assert.GreaterOrEqual(alloc_parameter.IterationNumber, 0);
                Assert.AreEqual(HrmPeriodLogic.INIT_YEAR, alloc_parameter.Year);
                Assert.AreEqual(HrmPeriodLogic.INIT_MONTH, alloc_parameter.Month);
                Assert.AreEqual(HrmPeriodAllocParameterStatus.ALLOC_PARAMETERS_ACCEPTED, alloc_parameter.Status);
                Assert.AreEqual(HrmPeriodAllocParameterLogic.INIT_NORM_NO_CONTROL_KB, alloc_parameter.NormNoControlKB);
                Assert.AreEqual(HrmPeriodAllocParameterLogic.INIT_NORM_NO_CONTROL_OZM, alloc_parameter.NormNoControlOZM);
                //  foreach (var salary_pay_type in alloc_parameter.SimpleWorkButNotLegal) {
                //     Assert.IsNotNullOrEmpty(salary_pay_type.Name);
                //  Assert.Greater(Convert.ToInt32(salary_pay_type.Code), 0);
                // }
                foreach (var order in alloc_parameter.OrderControls) {
                    Assert.IsNotNull(order.AllocParameter);
                    Assert.GreaterOrEqual(order.NormKB, 0);
                    Assert.GreaterOrEqual(order.NormOZM, 0);
                    Assert.Greater(Convert.ToInt32(order.Order.Code), 0);
                    Assert.AreEqual(order.TypeControl, order.Order.TypeControl);
                    Assert.AreNotEqual(FmCOrderTypeControl.NO_ORDERED, order.TypeControl);
                }
                ValidateAllocParameterWithOrders(test_object_space, alloc_parameter);
            }
        }
    }
}