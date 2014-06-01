using System;

using DevExpress.ExpressApp.Editors;

namespace NpoMash.Erm.Hrm.Tests.StructuralTests {

    public class AppearanceTarget : IAppearanceEnabled {

        private Boolean _Enabled;
        public Boolean Enabled {
            get { return _Enabled; }
            set { _Enabled = value; }
        }

        public void ResetEnabled() {
            Enabled = true;
        }
    }
}