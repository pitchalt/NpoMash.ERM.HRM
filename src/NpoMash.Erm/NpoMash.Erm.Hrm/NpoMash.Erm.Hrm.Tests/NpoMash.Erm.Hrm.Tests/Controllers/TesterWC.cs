using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Templates;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Model.NodeGenerators;

using NpoMash.Erm.Hrm.Tests.ReferentialData;

namespace NpoMash.Erm.Hrm.Tests.Controllers {

    public partial class TesterWC : WindowController {
        public TesterWC() {
            InitializeComponent();
            RegisterActions( components );
        }
        protected override void OnActivated() { base.OnActivated(); }
        protected override void OnDeactivated() { base.OnDeactivated(); }

        private void PopulateDB_Execute( object sender, SimpleActionExecuteEventArgs e ) {
            IObjectSpace object_space = Application.CreateObjectSpace();
            TestWCLogic.referenceClassesGenerate(object_space);
            TestWCLogic.addTestData( object_space );
            object_space.CommitChanges();
        }

        private void AddReferenceData_Execute(object sender, SimpleActionExecuteEventArgs e) {
            /*
            IObjectSpace object_space = Application.CreateObjectSpace();
            TestWCLogic.ImportData(object_space);
            TestWCLogic.addTestData(object_space);
            object_space.CommitChanges();
            */
        }
    }
}