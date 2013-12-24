using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.Layout;

using IntecoAG.ERM.FM.Order;
using NpoMash.Erm.Hrm;
using NpoMash.Erm.Hrm.Salary;
using NpoMash.Erm.Hrm.Tests.Controllers;

namespace NpoMash.Erm.Hrm.Tests.StructuralTests {


    [TestFixture]
    public class AllocParameterVCUnitTest {
  //      IObjectSpace object_space;
        TestApplication application;
        [SetUp]
        public void setUp() {
            IObjectSpaceProvider object_space_provider = new XPObjectSpaceProvider(new MemoryDataStoreProvider());
            application = new TestApplication();
            ModuleBase test_module = new ModuleBase();
            test_module.AdditionalExportedTypes.Add(typeof(HrmPeriod));
            application.Modules.Add(test_module);
            application.Setup("TestApplication", object_space_provider);
//            object_space = application.CreateObjectSpace();
        }

        [Test]
        public void TestHrmPeriodAllocParameter() {
            TesterWC test_wc = application.CreateController<TesterWC>();
            ICollection<Controller> controllers = new List<Controller>();
            controllers.Add(test_wc);
            Window test_window = application.CreateWindow(TemplateContext.ApplicationWindow, controllers, true);
            test_wc.PopulateAction.DoExecute();
            IObjectSpace os = application.CreateObjectSpace();
            IList<HrmPeriod> periods = new List<HrmPeriod>(os.GetObjects<HrmPeriod>().OrderBy(x => x.Year * 100 + x.Month));
            Assert.AreEqual(periods.Count, 3);
            Assert.AreEqual(periods[0].Year, 2013);
            Assert.AreEqual(periods[1].Year, 2014);
            Assert.AreEqual(periods[2].Year, 2014);
            Assert.AreEqual(periods[2].Month, 2);
        }
    }

    public class TestApplication : XafApplication {
        public TestApplication() : base() {
            this.DatabaseVersionMismatch += new System.EventHandler<DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs>(this.TestsWindowsFormsApplication_DatabaseVersionMismatch);
        }
        protected override LayoutManager CreateLayoutManagerCore(bool simple) {
            return null;
        }
        private void TestsWindowsFormsApplication_DatabaseVersionMismatch(object sender, DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs e) {
            e.Updater.Update();
            e.Handled = true;
        }
    }
}