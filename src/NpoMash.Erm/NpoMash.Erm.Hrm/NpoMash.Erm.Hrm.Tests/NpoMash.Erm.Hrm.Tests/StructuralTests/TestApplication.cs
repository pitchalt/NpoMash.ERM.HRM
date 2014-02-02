using System;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Layout;

namespace NpoMash.Erm.Hrm.Tests.StructuralTests {


    public class TestApplication : XafApplication {

        protected override LayoutManager CreateLayoutManagerCore(bool simple) {
            return null;
        }

        private void TestsAppWin_DatabaseVersionMismatch(Object sender, DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs e) {
            e.Updater.Update();
            e.Handled = true;
        }

        public TestApplication() : base() {
            this.DatabaseVersionMismatch += new System.EventHandler<DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs>(this.TestsAppWin_DatabaseVersionMismatch);
        }
    }
}