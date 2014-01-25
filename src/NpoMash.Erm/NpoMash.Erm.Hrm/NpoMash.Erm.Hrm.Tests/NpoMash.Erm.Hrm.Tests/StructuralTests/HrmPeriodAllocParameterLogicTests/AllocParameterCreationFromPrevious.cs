using System;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;

using IntecoAG.ERM.FM.Order;
using NpoMash.Erm.Hrm.Salary;

using NUnit.Framework;

namespace NpoMash.Erm.Hrm.Tests.StructuralTests.HrmPeriodAllocParameterLogicTests {


    [TestFixture]
    public class AllocParameterCreationFromPrevious : AllreadyExistAllocParameters {

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
        public void ConditionalAppearanceIsEnabledCreate() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var new_alloc_parameters = HrmPeriodAllocParameterLogic.createParameters(test_object_space);
            detail_view = application.CreateDetailView(test_object_space, new_alloc_parameters);
            controller.SetView(detail_view);
            controller.RefreshItemAppearance(detail_view, "ViewItem", "SpouseSate", target, new_alloc_parameters);
            Assert.IsTrue(target.Enabled);
        }

        [Test]
        public void ConditionalAppearanceIsEnabledFirstAccept() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var new_alloc_parameters = HrmPeriodAllocParameterLogic.createParameters(test_object_space);
            HrmPeriodAllocParameterLogic.acceptParameters(test_object_space, new_alloc_parameters);
            detail_view = application.CreateDetailView(test_object_space, new_alloc_parameters);
            controller.SetView(detail_view);
            controller.RefreshItemAppearance(detail_view, "ViewItem", "SpouseSate", target, new_alloc_parameters);
            Assert.IsTrue(target.Enabled);
        }
        [Test]
        public void ConditionalAppearanceIsEnabledLastAccept() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var new_alloc_parameters = HrmPeriodAllocParameterLogic.createParameters(test_object_space);
            HrmPeriodAllocParameterLogic.acceptParameters(test_object_space, new_alloc_parameters);
            HrmPeriodAllocParameterLogic.acceptParameters(test_object_space, new_alloc_parameters);
            detail_view = application.CreateDetailView(test_object_space, new_alloc_parameters);
            controller.SetView(detail_view);
            controller.RefreshItemAppearance(detail_view, "ViewItem", "SpouseSate", target, new_alloc_parameters);
            Assert.IsTrue(target.Enabled);
        }

        [Test]
        public void PeriodLinkIsNotNull() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var new_alloc_parameters = HrmPeriodAllocParameterLogic.createParameters(test_object_space);
            Assert.IsNotNull(new_alloc_parameters.Period);
        }

        [Test]
        public void OrderControllCollectionIsExist() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var new_alloc_parameters = HrmPeriodAllocParameterLogic.createParameters(test_object_space);
            Assert.IsNotNull(new_alloc_parameters.OrderControls);
        }

        [Test]
        public void PeriodPayTypeCOllectionIsExist() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var new_alloc_parameters = HrmPeriodAllocParameterLogic.createParameters(test_object_space);
            Assert.IsNotNull(new_alloc_parameters.PeriodPayTypes);
        }

        [Test]
        public void IterationNumberIsCorrect() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var new_alloc_parameters = HrmPeriodAllocParameterLogic.createParameters(test_object_space);
            Assert.AreEqual(1, new_alloc_parameters.IterationNumber);
        }

        [Test]
        public void YearMonthValueIsCorrect() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var new_alloc_parameters = HrmPeriodAllocParameterLogic.createParameters(test_object_space);
            Assert.AreEqual(HrmPeriodLogic.INIT_YEAR, new_alloc_parameters.Year);
            Assert.AreEqual(HrmPeriodLogic.INIT_MONTH + 1, new_alloc_parameters.Month);
        }

        [Test]
        public void NormNoContolKbValueIsCorrect() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var new_alloc_parameters = HrmPeriodAllocParameterLogic.createParameters(test_object_space);
            Assert.AreEqual(HrmPeriodAllocParameterLogic.INIT_NORM_NO_CONTROL_KB, new_alloc_parameters.NormNoControlKB);
        }

        [Test]
        public void NormNoControlOZMValueIsCorrect() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var new_alloc_parameters = HrmPeriodAllocParameterLogic.createParameters(test_object_space);
            Assert.AreEqual(HrmPeriodAllocParameterLogic.INIT_NORM_NO_CONTROL_OZM, new_alloc_parameters.NormNoControlOZM);
        }

        [Test]
        public void NoOrderedOrdersAreNotExist() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var new_alloc_parameters = HrmPeriodAllocParameterLogic.createParameters(test_object_space);
            foreach (var order in new_alloc_parameters.OrderControls) {
                Assert.AreNotEqual(FmCOrderTypeControl.NO_ORDERED, order.TypeControl);
            }
        }

        [Test]
        public void PeriodLinkIsNotNullFirstAccept() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var new_alloc_parameters = HrmPeriodAllocParameterLogic.createParameters(test_object_space);
            HrmPeriodAllocParameterLogic.acceptParameters(test_object_space, new_alloc_parameters);
            Assert.IsNotNull(new_alloc_parameters.Period);
        }

        [Test]
        public void OrderControllCollectionIsExistFirstAccept() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var new_alloc_parameters = HrmPeriodAllocParameterLogic.createParameters(test_object_space);
            HrmPeriodAllocParameterLogic.acceptParameters(test_object_space, new_alloc_parameters);
            Assert.IsNotNull(new_alloc_parameters.OrderControls);
        }

        [Test]
        public void PeriodPayTypeCOllectionIsExistFirstAccept() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var new_alloc_parameters = HrmPeriodAllocParameterLogic.createParameters(test_object_space);
            HrmPeriodAllocParameterLogic.acceptParameters(test_object_space, new_alloc_parameters);
            Assert.IsNotNull(new_alloc_parameters.PeriodPayTypes);
        }

        [Test]
        public void IterationNumberIsCorrectFirstAccept() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var new_alloc_parameters = HrmPeriodAllocParameterLogic.createParameters(test_object_space);
            HrmPeriodAllocParameterLogic.acceptParameters(test_object_space, new_alloc_parameters);
            Assert.AreEqual(1, new_alloc_parameters.IterationNumber);
        }

        [Test]
        public void YearMonthValueIsCorrectFirstAccept() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var new_alloc_parameters = HrmPeriodAllocParameterLogic.createParameters(test_object_space);
            HrmPeriodAllocParameterLogic.acceptParameters(test_object_space, new_alloc_parameters);
            Assert.AreEqual(HrmPeriodLogic.INIT_YEAR, new_alloc_parameters.Year);
            Assert.AreEqual(HrmPeriodLogic.INIT_MONTH + 1, new_alloc_parameters.Month);
        }

        [Test]
        public void NormNoContolKbValueIsCorrectFirstAccept() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var new_alloc_parameters = HrmPeriodAllocParameterLogic.createParameters(test_object_space);
            HrmPeriodAllocParameterLogic.acceptParameters(test_object_space, new_alloc_parameters);
            Assert.AreEqual(HrmPeriodAllocParameterLogic.INIT_NORM_NO_CONTROL_KB, new_alloc_parameters.NormNoControlKB);
        }

        [Test]
        public void NormNoControlOZMValueIsCorrectFirstAccept() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var new_alloc_parameters = HrmPeriodAllocParameterLogic.createParameters(test_object_space);
            HrmPeriodAllocParameterLogic.acceptParameters(test_object_space, new_alloc_parameters);
            Assert.AreEqual(HrmPeriodAllocParameterLogic.INIT_NORM_NO_CONTROL_OZM, new_alloc_parameters.NormNoControlOZM);
        }

        [Test]
        public void NoOrderedOrdersAreNotExistFirstAccept() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var new_alloc_parameters = HrmPeriodAllocParameterLogic.createParameters(test_object_space);
            HrmPeriodAllocParameterLogic.acceptParameters(test_object_space, new_alloc_parameters);
            foreach (var order in new_alloc_parameters.OrderControls) {
                Assert.AreNotEqual(FmCOrderTypeControl.NO_ORDERED, order.TypeControl);
            }
        }

        [Test]
        public void PeriodLinkIsNotNullLastAccept() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var new_alloc_parameters = HrmPeriodAllocParameterLogic.createParameters(test_object_space);
            HrmPeriodAllocParameterLogic.acceptParameters(test_object_space, new_alloc_parameters);
            HrmPeriodAllocParameterLogic.acceptParameters(test_object_space, new_alloc_parameters);
            Assert.IsNotNull(new_alloc_parameters.Period);
        }

        [Test]
        public void OrderControllCollectionIsExistLastAccept() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var new_alloc_parameters = HrmPeriodAllocParameterLogic.createParameters(test_object_space);
            HrmPeriodAllocParameterLogic.acceptParameters(test_object_space, new_alloc_parameters);
            HrmPeriodAllocParameterLogic.acceptParameters(test_object_space, new_alloc_parameters);
            Assert.IsNotNull(new_alloc_parameters.OrderControls);
        }

        [Test]
        public void PeriodPayTypeCOllectionIsExistLasttAccept() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var new_alloc_parameters = HrmPeriodAllocParameterLogic.createParameters(test_object_space);
            HrmPeriodAllocParameterLogic.acceptParameters(test_object_space, new_alloc_parameters);
            HrmPeriodAllocParameterLogic.acceptParameters(test_object_space, new_alloc_parameters);
            Assert.IsNotNull(new_alloc_parameters.PeriodPayTypes);
        }

        [Test]
        public void IterationNumberIsCorrectLastAccept() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var new_alloc_parameters = HrmPeriodAllocParameterLogic.createParameters(test_object_space);
            HrmPeriodAllocParameterLogic.acceptParameters(test_object_space, new_alloc_parameters);
            HrmPeriodAllocParameterLogic.acceptParameters(test_object_space, new_alloc_parameters);
            Assert.AreEqual(1, new_alloc_parameters.IterationNumber);
        }

        [Test]
        public void YearMonthValueIsCorrectLastAccept() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var new_alloc_parameters = HrmPeriodAllocParameterLogic.createParameters(test_object_space);
            HrmPeriodAllocParameterLogic.acceptParameters(test_object_space, new_alloc_parameters);
            HrmPeriodAllocParameterLogic.acceptParameters(test_object_space, new_alloc_parameters);
            Assert.AreEqual(HrmPeriodLogic.INIT_YEAR, new_alloc_parameters.Year);
            Assert.AreEqual(HrmPeriodLogic.INIT_MONTH + 1, new_alloc_parameters.Month);
        }

        [Test]
        public void NormNoContolKbValueIsCorrectLastAccept() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var new_alloc_parameters = HrmPeriodAllocParameterLogic.createParameters(test_object_space);
            HrmPeriodAllocParameterLogic.acceptParameters(test_object_space, new_alloc_parameters);
            HrmPeriodAllocParameterLogic.acceptParameters(test_object_space, new_alloc_parameters);
            Assert.AreEqual(HrmPeriodAllocParameterLogic.INIT_NORM_NO_CONTROL_KB, new_alloc_parameters.NormNoControlKB);
        }

        [Test]
        public void NormNoControlOZMValueIsCorrectLastAccept() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var new_alloc_parameters = HrmPeriodAllocParameterLogic.createParameters(test_object_space);
            HrmPeriodAllocParameterLogic.acceptParameters(test_object_space, new_alloc_parameters);
            HrmPeriodAllocParameterLogic.acceptParameters(test_object_space, new_alloc_parameters);
            Assert.AreEqual(HrmPeriodAllocParameterLogic.INIT_NORM_NO_CONTROL_OZM, new_alloc_parameters.NormNoControlOZM);
        }

        [Test]
        public void NoOrderedOrdersAreNotExistLastAccept() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var new_alloc_parameters = HrmPeriodAllocParameterLogic.createParameters(test_object_space);
            HrmPeriodAllocParameterLogic.acceptParameters(test_object_space, new_alloc_parameters);
            HrmPeriodAllocParameterLogic.acceptParameters(test_object_space, new_alloc_parameters);
            foreach (var order in new_alloc_parameters.OrderControls) {
                Assert.AreNotEqual(FmCOrderTypeControl.NO_ORDERED, order.TypeControl);
            }
        }
    }
}