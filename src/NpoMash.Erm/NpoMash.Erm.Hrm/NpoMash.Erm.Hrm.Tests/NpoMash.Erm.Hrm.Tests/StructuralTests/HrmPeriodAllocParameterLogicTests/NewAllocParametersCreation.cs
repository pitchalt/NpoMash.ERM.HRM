using System;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;

using IntecoAG.ERM.FM.Order;
using NpoMash.Erm.Hrm.Salary;
using NpoMash.Erm.Hrm.Tests.Controllers;

using NUnit.Framework;

namespace NpoMash.Erm.Hrm.Tests.StructuralTests.HrmPeriodAllocParameterLogicTests {


    [TestFixture]
    public class NewAllocParametersCreation : PrimaryAllocParameters {

        [SetUp]
        protected override void SetUp() {
            IObjectSpaceProvider object_space_provider = new XPObjectSpaceProvider(new MemoryDataStoreProvider());
            application = new TestApplication();
            ModuleBase test_module = new ModuleBase();
            test_module.AdditionalExportedTypes.Add(typeof(HrmPeriod));
            application.Modules.Add(test_module);
            application.Setup("TestApplication", object_space_provider);
            IObjectSpace object_space = application.CreateObjectSpace();
            TestWCLogic.referenceClassesGenerate(object_space);
            object_space.CommitChanges();
        }

        [Test]
        public void CheckNewAllocParametersStatus() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var new_alloc_parameters = HrmPeriodAllocParameterLogic.createParameters(test_object_space);
            Assert.AreEqual(HrmPeriodAllocParameterStatus.CREATED, new_alloc_parameters.Status);
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
        public void OrderControllCollectionIsNotEmty() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var new_alloc_parameters = HrmPeriodAllocParameterLogic.createParameters(test_object_space);
            Assert.IsNotEmpty(new_alloc_parameters.OrderControls);
        }

        [Test]
        public void PeriodPayTypeCOllectionIsExist() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var new_alloc_parameters = HrmPeriodAllocParameterLogic.createParameters(test_object_space);
            Assert.IsNotNull(new_alloc_parameters.PeriodPayTypes);
        }

        [Test]
        public void PeriodPayTypeCOllectionIsNotEmpty() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var new_alloc_parameters = HrmPeriodAllocParameterLogic.createParameters(test_object_space);
            Assert.IsNotEmpty(new_alloc_parameters.PeriodPayTypes);
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
            Assert.AreEqual(HrmPeriodLogic.INIT_MONTH, new_alloc_parameters.Month);
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
    }
}