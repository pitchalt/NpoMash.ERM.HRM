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
    public partial class HrmSalaryTaskRevertVC : ViewController {
        public HrmSalaryTaskRevertVC() {
            InitializeComponent();
            RegisterActions(components);
        }
        protected override void OnActivated() { base.OnActivated(); }
        protected override void OnViewControlsCreated() { base.OnViewControlsCreated(); }
        protected override void OnDeactivated() { base.OnDeactivated(); }

        private void Revert_Execute(object sender, SimpleActionExecuteEventArgs e) {
            HrmSalaryTaskRevert task = e.CurrentObject as HrmSalaryTaskRevert;
            IObjectSpace local_object_space = Application.CreateObjectSpace();
            if (task.AllocParameter != null) {
                task.AllocParameter.StatusSet(HrmPeriodAllocParameterStatus.OPEN_TO_EDIT);
                //task.Period.CurrentAllocParameter = null;
                //task.Period.CurrentAllocParameter = HrmPeriodAllocParameterLogic.createParameters(local_object_space);

            }
            if (task.TimeSheetKB != null) {
                task.TimeSheetKB.SetStatus(HrmTimeSheetStatus.ARCHIVE);
                task.Period.CurrentTimeSheetKB = null;
            }
            if (task.TimeSheetOZM != null) {
                task.TimeSheetOZM.SetStatus(HrmTimeSheetStatus.ARCHIVE);
                task.Period.CurrentTimeSheetOZM = null;
            }
            if (task.MatrixPlanKB != null) {
                task.MatrixPlanKB.Status = HrmMatrixStatus.MATRIX_ARCHIVE;
                task.Period.CurrentMatrixAllocPlanKB = null;
            }
            if (task.MatrixPlanOZM != null) {
                task.MatrixPlanOZM.Status = HrmMatrixStatus.MATRIX_ARCHIVE;
                task.Period.CurrentMatrixAllocPlanOZM = null;
            }
            if (task.MatrixReductionKB != null) {
                task.MatrixReductionKB.Status = HrmMatrixStatus.MATRIX_ARCHIVE;
                task.Period.CurrentKBmatrixReduction.MinimizeMaximumDeviationsMatrix = null;
                task.Period.CurrentKBmatrixReduction.MinimizeNumberOfDeviationsMatrix = null;
                task.Period.CurrentKBmatrixReduction.ProportionsMethodMatrix = null;
            }
            if (task.MatrixReductionOZM != null) {
                task.MatrixReductionOZM.Status = HrmMatrixStatus.MATRIX_ARCHIVE;
                task.Period.CurrentOZMmatrixReduction.MinimizeMaximumDeviationsMatrix = null;
                task.Period.CurrentOZMmatrixReduction.MinimizeNumberOfDeviationsMatrix = null;
                task.Period.CurrentOZMmatrixReduction.ProportionsMethodMatrix = null;
            }
            if (task.MatrixAllocResultKB != null) {
                task.MatrixAllocResultKB.Status = HrmMatrixStatus.MATRIX_ARCHIVE;
                task.Period.CurrentMatrixAllocResultKB = null;
            }
            if (task.MatrixAllocResultOZM != null) {
                task.MatrixAllocResultOZM.Status = HrmMatrixStatus.MATRIX_ARCHIVE;
                task.Period.CurrentMatrixAllocResultOZM = null;
            }
            if (task.MatrixProvision != null) {
                task.MatrixProvision.Status = HrmMatrixStatus.MATRIX_ARCHIVE;
                task.Period.CurrentProvisionMatrix = null;
            }
            if (task.Period != null) { task.Period.setStatus(HrmPeriodStatus.OPENED); }
            task.Complete();
            ObjectSpace.CommitChanges();
            Window win = Frame as Window;
            if (win != null) win.Close();
        }
    }
}