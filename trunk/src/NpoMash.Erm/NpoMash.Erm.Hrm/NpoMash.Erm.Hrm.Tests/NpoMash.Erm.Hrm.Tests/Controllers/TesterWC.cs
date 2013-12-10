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
            TestWCLogic.addTestData( object_space );
            var hrmPeriodCollection = object_space.GetObjects<HrmPeriod>( null, true );
            Int16 year = 2013;
            Int16 month = 9;
            foreach ( var i in hrmPeriodCollection ) {
                i.Init( year, month );
            }
            object_space.CommitChanges();
        }
    }
}
