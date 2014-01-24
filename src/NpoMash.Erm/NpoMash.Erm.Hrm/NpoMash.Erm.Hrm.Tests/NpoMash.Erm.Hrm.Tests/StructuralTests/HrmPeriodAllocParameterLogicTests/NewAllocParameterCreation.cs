using System;

using DevExpress.ExpressApp;

using IntecoAG.ERM.FM.Order;
using NpoMash.Erm.Hrm.Salary;

using NUnit.Framework;

namespace NpoMash.Erm.Hrm.Tests.StructuralTests.HrmPeriodAllocParameterLogicTests {


    [TestFixture]
    public class NewAllocParameterCreation : PrimaryAllocParameters {

        [Test]
        public void CheckAllocParameterscCreateStatus() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var new_alloc_parameters = HrmPeriodAllocParameterLogic.createParameters(test_object_space);
            Assert.AreEqual(HrmPeriodAllocParameterStatus.CREATED, new_alloc_parameters.Status);
        }

        [Test]
        public void CheckAllocParametersFirstAcceptStatus() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var new_alloc_parameters = HrmPeriodAllocParameterLogic.createParameters(test_object_space);
            HrmPeriodAllocParameterLogic.acceptParameters(test_object_space, new_alloc_parameters);
            Assert.AreEqual(HrmPeriodAllocParameterStatus.LIST_OF_ORDER_ACCEPTED, new_alloc_parameters.Status);
        }

        [Test]
        public void CheckAllocParametersFinalAcceptStatus() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var new_alloc_parameters = HrmPeriodAllocParameterLogic.createParameters(test_object_space);
            HrmPeriodAllocParameterLogic.acceptParameters(test_object_space, new_alloc_parameters);
            HrmPeriodAllocParameterLogic.acceptParameters(test_object_space, new_alloc_parameters);
            Assert.AreEqual(HrmPeriodAllocParameterStatus.ALLOC_PARAMETERS_ACCEPTED, new_alloc_parameters.Status);
        }

        [Test]
        public void DoubleCreationAllocParameters() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var new_alloc_parameters = HrmPeriodAllocParameterLogic.createParameters(test_object_space);
            try { var double_param = HrmPeriodAllocParameterLogic.createParameters(test_object_space); }
            catch (OpenPeriodExistsException) { }
        }

        [Test]
        public void CreateAllocParametersFromPrevious() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var new_alloc_parameters = HrmPeriodAllocParameterLogic.createParameters(test_object_space);
            Assert.IsNotNull(new_alloc_parameters.Period);
            Assert.IsNotNull(new_alloc_parameters.OrderControls);
            Assert.IsNotNull(new_alloc_parameters.PeriodPayTypes);
            Assert.AreEqual(0, new_alloc_parameters.IterationNumber);
            Assert.AreEqual(HrmPeriodLogic.INIT_YEAR, new_alloc_parameters.Year);
            Assert.AreEqual(HrmPeriodLogic.INIT_MONTH + 1, new_alloc_parameters.Month);
            Assert.AreEqual(HrmPeriodAllocParameterLogic.INIT_NORM_NO_CONTROL_KB, new_alloc_parameters.NormNoControlKB);
            Assert.AreEqual(HrmPeriodAllocParameterLogic.INIT_NORM_NO_CONTROL_OZM, new_alloc_parameters.NormNoControlOZM);
            foreach (var salary_pay_type in new_alloc_parameters.SimpleWorkButNotLegal) {
                Assert.IsNotNullOrEmpty(salary_pay_type.Name);
                Assert.Greater(Convert.ToInt32(salary_pay_type.Code), 0);
            }
            foreach (var order in new_alloc_parameters.OrderControls) {
                Assert.IsNotNull(order.AllocParameter);
                Assert.GreaterOrEqual(order.NormKB, 0);
                Assert.GreaterOrEqual(order.NormOZM, 0);
                Assert.Greater(Convert.ToInt32(order.Order.Code), 0);
                Assert.AreEqual(order.TypeControl, order.Order.TypeControl);
                Assert.AreNotEqual(FmCOrderTypeControl.NO_ORDERED, order.TypeControl);
            }
            ValidateAllocParameterWithOrders(test_object_space, new_alloc_parameters);
        }
    }
}
