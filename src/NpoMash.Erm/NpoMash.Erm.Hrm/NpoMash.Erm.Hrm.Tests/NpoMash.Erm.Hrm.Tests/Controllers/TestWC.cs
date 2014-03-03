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

using NpoMash.Erm.Hrm.Tests.ImportReferentialData;

namespace NpoMash.Erm.Hrm.Tests.Controllers {

    public partial class TestWC : WindowController {
        public TestWC() {
            InitializeComponent();
            RegisterActions( components );
        }
        protected override void OnActivated() { base.OnActivated(); }
        protected override void OnDeactivated() { base.OnDeactivated(); }

        private void PopulateDB_Execute( object sender, SimpleActionExecuteEventArgs e ) {
            IObjectSpace object_space = Application.CreateObjectSpace();
            TestWCLogic.SalaryPayTypeGenerate(object_space);
            TestWCLogic.DepartmentsGenerate(object_space);
            TestWCLogic.OrdersGenerate(object_space);
            TestWCLogic.addTestData( object_space );
            object_space.CommitChanges();
        }

        private void AddReferenceData_Execute(object sender, SimpleActionExecuteEventArgs e) {
            IObjectSpace object_space = Application.CreateObjectSpace();
            TestWCLogic.ImportDepartments(object_space);
            TestWCLogic.ImportOrders(object_space);
            TestWCLogic.ImportPayTypes(object_space);
            TestWCLogic.addTestData(object_space);
            object_space.CommitChanges();
        }
    }
}