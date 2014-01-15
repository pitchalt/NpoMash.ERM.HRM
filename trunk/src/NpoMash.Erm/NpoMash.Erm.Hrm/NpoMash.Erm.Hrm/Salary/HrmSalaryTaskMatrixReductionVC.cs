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

using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.FM.Order;

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
            IObjectSpace os = Application.CreateObjectSpace();
            HrmSalaryTaskMatrixReduction reduc = (HrmSalaryTaskMatrixReduction)e.CurrentObject;
            HrmPeriod period = os.GetObject<HrmPeriod>(reduc.Period);
            if (period.Status == HrmPeriodStatus.ReadyToCalculateCoercedMatrixs) {
                HRM_MATRIX_VARIANT bringing_method = HrmSalaryTaskMatrixReductionLogic.DetermineSelectedBringingMethod(e);
                /*if (e.SelectedChoiceActionItem.Id == "ProportionsMethod")
                    bringing_method = HRM_MATRIX_VARIANT.ProportionsMethod;
                if (e.SelectedChoiceActionItem.Id == "MinimizeDifferenceNumber")
                    bringing_method = HRM_MATRIX_VARIANT.MinimizeNumberOfDeviations;
                if (e.SelectedChoiceActionItem.Id == "MinimizeMaxDifference")
                    bringing_method = HRM_MATRIX_VARIANT.MinimizeMaximumDeviations;*/
                //HrmSalaryTaskMatrixReduction reduc = null;
                HrmSalaryTaskMatrixReductionLogic.CreateMatrixInReduc(reduc, os, reduc.GroupDep,
                        bringing_method, period);
                /*if (reduc.MinimizeMaximumDeviationsMatrix == null &&
                    bringing_method == HRM_MATRIX_VARIANT.MinimizeMaximumDeviations)
                    HrmMatrixLogic.makeAllocMatrix(reduc, os, reduc.GroupDep,
                        bringing_method, period);
                if (reduc.MinimizeNumberOfDeviationsMatrix == null &&
                    bringing_method == HRM_MATRIX_VARIANT.MinimizeNumberOfDeviations)
                    HrmMatrixLogic.makeAllocMatrix(reduc, os, reduc.GroupDep,
                        bringing_method, period);
                if (reduc.ProportionsMethodMatrix == null &&
                    bringing_method == HRM_MATRIX_VARIANT.ProportionsMethod)
                    HrmMatrixLogic.makeAllocMatrix(reduc, os, reduc.GroupDep,
                        bringing_method, period);*/
                }
        }

        private void AcceptCoercedMatrix_Execute(object sender, SingleChoiceActionExecuteEventArgs e) {
            HrmSalaryTaskMatrixReduction reduc = (HrmSalaryTaskMatrixReduction)e.CurrentObject;
            IObjectSpace os = Application.CreateObjectSpace();
            HrmPeriod current_period = os.GetObject<HrmPeriod>(reduc.Period);
            HrmMatrix matrix_to_accept = HrmSalaryTaskMatrixReductionLogic.DetermineSelectedMatrixToAccept(e, reduc);            
            if (matrix_to_accept != null && matrix_to_accept.Status == HRM_MATRIX_STATUS.Saved) {
                HrmSalaryTaskMatrixReductionLogic.AcceptSelectedMatrix(reduc, matrix_to_accept);
                /*if (reduc.MinimizeMaximumDeviationsMatrix != null)
                    reduc.MinimizeMaximumDeviationsMatrix.Status = HRM_MATRIX_STATUS.Closed;
                if (reduc.MinimizeNumberOfDeviationsMatrix != null)
                    reduc.MinimizeNumberOfDeviationsMatrix.Status = HRM_MATRIX_STATUS.Closed;
                if (reduc.ProportionsMethodMatrix != null)
                    reduc.ProportionsMethodMatrix.Status = HRM_MATRIX_STATUS.Closed;
                matrix_to_accept.Status = HRM_MATRIX_STATUS.Accepted;*/
                if (HrmSalaryTaskMatrixReductionLogic.AllCoercedMatrixesAccepted(matrix_to_accept, current_period))
                    current_period.setStatus(HrmPeriodStatus.ReadyToExportCoercedMatrixs);
                os.CommitChanges();
            }

        }

        private void ExportCoercedMatrix_Execute(object sender, SimpleActionExecuteEventArgs e) {
            HrmSalaryTaskMatrixReduction reduc = (HrmSalaryTaskMatrixReduction)e.CurrentObject;
            IObjectSpace os = Application.CreateObjectSpace();
            HrmPeriod current_period = os.GetObject<HrmPeriod>(reduc.Period);
            if (reduc.Period.Status == HrmPeriodStatus.ReadyToExportCoercedMatrixs
                && reduc.GroupDep == DEPARTMENT_GROUP_DEP.KB) {
                HrmSalaryTaskMatrixReductionLogic.ExportMatrixes(current_period);
                current_period.setStatus(HrmPeriodStatus.CoercedMatrixesExported);
                os.CommitChanges();
            }
        }


    }
}
