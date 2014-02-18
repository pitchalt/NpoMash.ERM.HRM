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

namespace NpoMash.Erm.Hrm.Salary {
    // For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppViewControllertopic.
    public partial class HrmSalaryTaskImportSourceDataVC : ViewController {
        public HrmSalaryTaskImportSourceDataVC() {
            InitializeComponent();
            RegisterActions(components);
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated() {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated() {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void AcceptImport_Execute(object sender, SimpleActionExecuteEventArgs e) {
            HrmSalaryTaskImportSourceData task = e.CurrentObject as HrmSalaryTaskImportSourceData;
            //IObjectSpace os = Application.CreateObjectSpace();
           // HrmSalaryTaskImportSourceData task = os.GetObject<HrmSalaryTaskImportSourceData>(t);

            task.MatrixPlanKB.Status = HrmMatrixStatus.MATRIX_ACCEPTED;
            task.MatrixPlanOZM.Status = HrmMatrixStatus.MATRIX_ACCEPTED;
            task.TimeSheetKB.SetStatus(HrmTimeSheetStatus.ACCEPTED);
            task.TimeSheetOZM.SetStatus(HrmTimeSheetStatus.ACCEPTED);
            task.Period.setStatus(HrmPeriodStatus.READY_TO_CALCULATE_COERCED_MATRIXS);
            task.Complete();
            ObjectSpace.CommitChanges();
        }
    }
}