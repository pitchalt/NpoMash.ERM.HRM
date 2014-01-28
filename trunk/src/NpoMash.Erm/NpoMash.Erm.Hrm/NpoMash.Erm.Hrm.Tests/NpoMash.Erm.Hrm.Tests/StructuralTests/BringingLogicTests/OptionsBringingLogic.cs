using System;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;

using NpoMash.Erm.Hrm.Salary;
using NpoMash.Erm.Hrm.Tests.Controllers;

using NUnit.Framework;

namespace NpoMash.Erm.Hrm.Tests.StructuralTests.BringingLogicTests {


    [TestFixture]
    public class OptionsBringingLogic {

        protected TestApplication application;

        [TestFixtureSetUp]
        protected virtual void SetUp() {
            IObjectSpaceProvider object_space_provider = new XPObjectSpaceProvider(new MemoryDataStoreProvider());
            application = new TestApplication();
            ModuleBase test_module = new ModuleBase();
            test_module.AdditionalExportedTypes.Add(typeof(HrmSalaryTaskMatrixReduction));
            application.Modules.Add(test_module);
            application.Setup("BringingApp", object_space_provider);
            IObjectSpace object_space = application.CreateObjectSpace();
        }



    }
}