using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
//
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

namespace NpoMash.Erm.Hrm.Salary
{

    public partial class AcceptAllocParameterVC : ViewController
    {


        private void Accept_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            HrmPeriodAllocParameter ap = (HrmPeriodAllocParameter)e.CurrentObject;
            ap.Status = HrmPeriodAllocParameterStatus.AllocParametersAccepted;
            //HrmPeriod hp=ObjectSpace.GetObject<HrmPeriod>(ap.HrmPeriod);
            //hp.Status = HrmPeriod.HrmPeriodStatus.closed;
            ObjectSpace.CommitChanges();
        }

        public AcceptAllocParameterVC() { InitializeComponent(); RegisterActions(components); }
        protected override void OnActivated()
        { base.OnActivated(); }
        protected override void OnViewControlsCreated()
        { base.OnViewControlsCreated(); }
        protected override void OnDeactivated()
        { base.OnDeactivated(); }
    }
}
