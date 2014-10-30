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
    public partial class HrmSalaryTaskCompareWorkTimeVC : ViewController {

        public HrmSalaryTaskCompareWorkTimeVC() {
            InitializeComponent();
            RegisterActions(components);
        }
        protected override void OnActivated() {
            base.OnActivated();
        }
        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();
        }
        protected override void OnDeactivated() {
            base.OnDeactivated();
        }

        private void AcceptCompareKB_Execute(object sender, SimpleActionExecuteEventArgs e) {
            HrmSalaryTaskCompareWorkTime task = e.CurrentObject as HrmSalaryTaskCompareWorkTime;
            if (task.Period.CurrentKBmatrixReduction.MinimizeNumberOfDeviationsMatrix != null) {
                if (task.Period.CurrentOZMmatrixReduction.MinimizeNumberOfDeviationsMatrix.Status == HrmMatrixStatus.MATRIX_ACCEPTED && task.Period.CurrentAllocParameter.Status == HrmPeriodAllocParameterStatus.ALLOC_PARAMETERS_ACCEPTED) {
                    task.Period.CurrentKBmatrixReduction.MinimizeNumberOfDeviationsMatrix.Status = HrmMatrixStatus.MATRIX_ACCEPTED;
                    task.Period.setStatus(HrmPeriodStatus.READY_TO_RESERVE_MATRIX_CREATE);
                }
                else {
                    task.Period.CurrentKBmatrixReduction.MinimizeNumberOfDeviationsMatrix.Status = HrmMatrixStatus.MATRIX_ACCEPTED;
                }
            }
            if (task.Period.CurrentKBmatrixReduction.MinimizeMaximumDeviationsMatrix != null) {
                if (task.Period.CurrentOZMmatrixReduction.MinimizeMaximumDeviationsMatrix.Status == HrmMatrixStatus.MATRIX_ACCEPTED && task.Period.CurrentAllocParameter.Status == HrmPeriodAllocParameterStatus.ALLOC_PARAMETERS_ACCEPTED) {
                    task.Period.CurrentKBmatrixReduction.MinimizeMaximumDeviationsMatrix.Status = HrmMatrixStatus.MATRIX_ACCEPTED;
                    task.Period.setStatus(HrmPeriodStatus.READY_TO_RESERVE_MATRIX_CREATE);
                }
                else {
                    task.Period.CurrentKBmatrixReduction.MinimizeMaximumDeviationsMatrix.Status = HrmMatrixStatus.MATRIX_ACCEPTED;
                }
            }
            if (task.Period.CurrentKBmatrixReduction.ProportionsMethodMatrix != null) {
                if (task.Period.CurrentOZMmatrixReduction.ProportionsMethodMatrix.Status == HrmMatrixStatus.MATRIX_ACCEPTED && task.Period.CurrentAllocParameter.Status == HrmPeriodAllocParameterStatus.ALLOC_PARAMETERS_ACCEPTED) {
                    task.Period.CurrentKBmatrixReduction.ProportionsMethodMatrix.Status = HrmMatrixStatus.MATRIX_ACCEPTED;
                    task.Period.setStatus(HrmPeriodStatus.READY_TO_RESERVE_MATRIX_CREATE);
                }
                else {
                    task.Period.CurrentKBmatrixReduction.ProportionsMethodMatrix.Status = HrmMatrixStatus.MATRIX_ACCEPTED;
                }
            }
            task.Complete();
            ObjectSpace.CommitChanges();
            Window win = Frame as Window;
            if (win != null) win.Close();
        }

        private void AcceptCompareOZM_Execute(object sender, SimpleActionExecuteEventArgs e) {
            HrmSalaryTaskCompareWorkTime task = e.CurrentObject as HrmSalaryTaskCompareWorkTime;
            if (task.Period.CurrentOZMmatrixReduction.MinimizeNumberOfDeviationsMatrix != null) {
                if (task.Period.CurrentKBmatrixReduction.MinimizeNumberOfDeviationsMatrix.Status == HrmMatrixStatus.MATRIX_ACCEPTED && task.Period.CurrentAllocParameter.Status == HrmPeriodAllocParameterStatus.ALLOC_PARAMETERS_ACCEPTED) {
                    task.Period.CurrentOZMmatrixReduction.MinimizeNumberOfDeviationsMatrix.Status = HrmMatrixStatus.MATRIX_ACCEPTED;
                    task.Period.setStatus(HrmPeriodStatus.READY_TO_RESERVE_MATRIX_CREATE);
                }
                else {
                    task.Period.CurrentOZMmatrixReduction.MinimizeNumberOfDeviationsMatrix.Status = HrmMatrixStatus.MATRIX_ACCEPTED;
                }
            }
            if (task.Period.CurrentOZMmatrixReduction.MinimizeMaximumDeviationsMatrix != null) {
                if (task.Period.CurrentKBmatrixReduction.MinimizeMaximumDeviationsMatrix.Status == HrmMatrixStatus.MATRIX_ACCEPTED && task.Period.CurrentAllocParameter.Status == HrmPeriodAllocParameterStatus.ALLOC_PARAMETERS_ACCEPTED) {
                    task.Period.CurrentOZMmatrixReduction.MinimizeMaximumDeviationsMatrix.Status = HrmMatrixStatus.MATRIX_ACCEPTED;
                    task.Period.setStatus(HrmPeriodStatus.READY_TO_RESERVE_MATRIX_CREATE);
                }
                else {
                    task.Period.CurrentOZMmatrixReduction.MinimizeMaximumDeviationsMatrix.Status = HrmMatrixStatus.MATRIX_ACCEPTED;
                }
            }
            if (task.Period.CurrentOZMmatrixReduction.ProportionsMethodMatrix != null) {
                if (task.Period.CurrentKBmatrixReduction.ProportionsMethodMatrix.Status == HrmMatrixStatus.MATRIX_ACCEPTED && task.Period.CurrentAllocParameter.Status == HrmPeriodAllocParameterStatus.ALLOC_PARAMETERS_ACCEPTED) {
                    task.Period.CurrentOZMmatrixReduction.ProportionsMethodMatrix.Status = HrmMatrixStatus.MATRIX_ACCEPTED;
                    task.Period.setStatus(HrmPeriodStatus.READY_TO_RESERVE_MATRIX_CREATE);
                }
                else {
                    task.Period.CurrentOZMmatrixReduction.ProportionsMethodMatrix.Status = HrmMatrixStatus.MATRIX_ACCEPTED;
                }
            }
            task.Complete();
            ObjectSpace.CommitChanges();
            Window win = Frame as Window;
            if (win != null) win.Close();
        }
    }
}