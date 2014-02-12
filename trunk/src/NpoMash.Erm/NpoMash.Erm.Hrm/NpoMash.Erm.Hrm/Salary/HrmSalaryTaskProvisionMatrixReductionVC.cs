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
//

namespace NpoMash.Erm.Hrm.Salary {
    public partial class HrmSalaryTaskProvisionMatrixReductionVC : ViewController {
















        public HrmSalaryTaskProvisionMatrixReductionVC() { InitializeComponent(); RegisterActions(components); }
        protected override void OnActivated() {
            base.OnActivated();
        }
        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();
        }
        protected override void OnDeactivated() {
            base.OnDeactivated();
        }

        private void BringingProvisionMatrix_Execute(object sender, SimpleActionExecuteEventArgs e) {
            IObjectSpace os = Application.CreateObjectSpace();
            HrmPeriod period = os.GetObject<HrmPeriod>((HrmPeriod)e.CurrentObject);
            HrmSalaryTaskProvisionMatrixReduction provisionMatrix = os.CreateObject<HrmSalaryTaskProvisionMatrixReduction>();
            if (period.Status == HrmPeriodStatus.READY_TO_RESERVE_MATRIX_CREATE) {

            }
        }
    }
}
