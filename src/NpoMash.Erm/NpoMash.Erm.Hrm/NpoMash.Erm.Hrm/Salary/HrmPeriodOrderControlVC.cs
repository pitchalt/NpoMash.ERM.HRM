using System;
using System.Linq;
using System.Text;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Templates;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Model.NodeGenerators;

namespace NpoMash.Erm.Hrm.Salary
{

    public partial class HrmPeriodOrderControlVC : ViewController
    {


        public HrmPeriodOrderControlVC()
        { InitializeComponent(); RegisterActions(components);}
        protected override void OnActivated()
        { base.OnActivated(); }
        protected override void OnViewControlsCreated()
        { base.OnViewControlsCreated(); }
        protected override void OnDeactivated()
        { base.OnDeactivated(); }

        private void FillHrmPeriodOrderCOntrol_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            //комментарий
        }
    }
}
