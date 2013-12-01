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
    public partial class HrmPeriodAllocParameterVC : ViewController
    {

        private void CreateAllocParameters_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            HrmPeriodAllocParameter par = e.CurrentObject as HrmPeriodAllocParameter;
            if (par == null) return;
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {

                var OrderControlsCollection = os.GetObjects<HrmPeriodOrderControl>();
                foreach (var a in OrderControlsCollection) {
                    if (a.TypeControl != HrmPeriodOrderControl.HrmPeriodOrderTypeControl.No_Ordered) {
                        par.OrderControls.Add(a);
                        a.AllocParameter = par;
                    }
                }
                os.CommitChanges();
            }
        }

        public HrmPeriodAllocParameterVC() { InitializeComponent(); RegisterActions(components); }
        protected override void OnActivated()
        { base.OnActivated(); }
        protected override void OnViewControlsCreated()
        { base.OnViewControlsCreated(); }
        protected override void OnDeactivated()
        { base.OnDeactivated(); }

    }
}
