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
    // For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppViewControllertopic.
    public partial class HrmPeriodAllocParameterVC : ViewController
    {
        public HrmPeriodAllocParameterVC()
        {
            InitializeComponent();
            RegisterActions(components);
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void CreateAllocParameters_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            HrmPeriodAllocParameter par = (HrmPeriodAllocParameter)e.CurrentObject;
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                //    e.ShowViewParameters.CreatedView.ObjectSpace.CreateNestedObjectSpace();
                var OrderControlsCollection = os.GetObjects<HrmPeriodOrderControl>();
                //List<HrmPeriodOrderControl> Controlled = new List<HrmPeriodOrderControl>();
                foreach (var a in OrderControlsCollection) {
                    if (a.TypeControl != HrmPeriodOrderControl.HrmPeriodOrderTypeControl.No_Ordered) {
                        par.OrderControls.Add(a);
                        a.AllocParameter = par;
                    }
                }
                os.CommitChanges();
            }
        }
    }
}
