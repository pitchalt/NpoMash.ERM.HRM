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
    public partial class HrmSalaryTaskMatrixReductionVC : ViewController {
        public HrmSalaryTaskMatrixReductionVC() {
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

/*        private void BringingOZMMatrixInReduc_Execute(object sender, SingleChoiceActionExecuteEventArgs e) {

        }*/

        private void BringingMatrixInReduc_Execute(object sender, SingleChoiceActionExecuteEventArgs e) {

        }

        private void AcceptCoercedMatrix_Execute(object sender, SingleChoiceActionExecuteEventArgs e) {
            HrmSalaryTaskMatrixReduction reduc = (HrmSalaryTaskMatrixReduction)e.CurrentObject;
            HrmMatrix matrix_to_accept = null;
            HrmPeriod current_period = ObjectSpace.GetObject<HrmPeriod>(reduc.Period);
            if (e.SelectedChoiceActionItem.Id == "AcceptProportionsMethod") {
                matrix_to_accept = reduc.ProportionsMethodMatrix;
            }
            if (e.SelectedChoiceActionItem.Id == "AcceptMinimizeNumberOfDeviationsMethod")
                matrix_to_accept = reduc.MinimizeNumberOfDeviationsMatrix;
            if (e.SelectedChoiceActionItem.Id == "AcceptMinimizeDeviationsMethod")
                matrix_to_accept = reduc.MinimizeMaximumDeviationsMatrix;
            if (matrix_to_accept != null && matrix_to_accept.Status == HRM_MATRIX_STATUS.Saved) {
                if (reduc.MinimizeMaximumDeviationsMatrix != null)
                    reduc.MinimizeMaximumDeviationsMatrix.Status = HRM_MATRIX_STATUS.Closed;
                if (reduc.MinimizeNumberOfDeviationsMatrix != null)
                    reduc.MinimizeNumberOfDeviationsMatrix.Status = HRM_MATRIX_STATUS.Closed;
                if (reduc.ProportionsMethodMatrix != null)
                    reduc.ProportionsMethodMatrix.Status = HRM_MATRIX_STATUS.Closed;
                matrix_to_accept.Status = HRM_MATRIX_STATUS.Accepted;
                bool kb_accepted = false;
                bool ozm_accepted = false;
                if (matrix_to_accept.GroupDep == IntecoAG.ERM.HRM.Organization.DEPARTMENT_GROUP_DEP.KB)
                    kb_accepted = true;
                else ozm_accepted = true;
                foreach (var m in current_period.Matrixs) {
                    if (m.TypeMatrix == HRM_MATRIX_TYPE_MATRIX.Coerced && m.Status == HRM_MATRIX_STATUS.Accepted)
                        if (m.GroupDep == IntecoAG.ERM.HRM.Organization.DEPARTMENT_GROUP_DEP.KB)
                            kb_accepted = true;
                        else ozm_accepted = true;
                }
                if (kb_accepted && ozm_accepted)
                    current_period.setStatus(HrmPeriodStatus.ReadyToExportCoercedMatrixs);
                ObjectSpace.CommitChanges();
            }

        }

        private void ExportCoercedMatrix_Execute(object sender, SimpleActionExecuteEventArgs e) {
            HrmSalaryTaskMatrixReduction reduc = (HrmSalaryTaskMatrixReduction)e.CurrentObject;
            HrmPeriod current_period = ObjectSpace.GetObject<HrmPeriod>(reduc.Period);
            if (reduc.Period.Status == HrmPeriodStatus.ReadyToExportCoercedMatrixs
                && reduc.GroupDep == IntecoAG.ERM.HRM.Organization.DEPARTMENT_GROUP_DEP.KB) {
                foreach (HrmMatrix m in current_period.Matrixs)
                    if (m.TypeMatrix == HRM_MATRIX_TYPE_MATRIX.Coerced && m.Status == HRM_MATRIX_STATUS.Accepted)
                        m.Status = HRM_MATRIX_STATUS.Exported;
                current_period.setStatus(HrmPeriodStatus.CoercedMatrixesExported);
                ObjectSpace.CommitChanges();
            }

        }


    }
}
