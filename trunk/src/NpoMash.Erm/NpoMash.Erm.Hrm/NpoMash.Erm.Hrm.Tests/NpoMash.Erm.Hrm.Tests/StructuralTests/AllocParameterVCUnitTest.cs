using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.Layout;

using NpoMash.Erm.Hrm.Salary;
using NpoMash.Erm.Hrm.Tests.Controllers;

namespace NpoMash.Erm.Hrm.Tests.StructuralTests {


    class TestApplication : XafApplication {
        protected override LayoutManager CreateLayoutManagerCore( bool simple ) {
            return null;
        }
    }

    [TestFixture]
    public class AllocParameterVCUnitTest {
        IObjectSpace object_space;
        TestApplication application;
        [SetUp]
        public void setUp() {
            IObjectSpaceProvider object_space_provider = new XPObjectSpaceProvider( new MemoryDataStoreProvider() );
            application = new TestApplication();
            ModuleBase test_module = new ModuleBase();
            test_module.AdditionalExportedTypes.Add(typeof(HrmPeriod));
            application.Modules.Add( test_module );
            application.Setup("TestApplication", object_space_provider);
            object_space = application.CreateObjectSpace();
        }

        [Test]
        public void TestHrmPeriodAllocParameter() { }
    }
}