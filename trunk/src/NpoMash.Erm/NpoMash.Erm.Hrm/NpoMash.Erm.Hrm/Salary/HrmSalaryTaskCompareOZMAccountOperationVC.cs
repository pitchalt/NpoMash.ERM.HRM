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
    public partial class HrmSalaryTaskCompareOZMAccountOperationVC : ViewController {
        public HrmSalaryTaskCompareOZMAccountOperationVC() {
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


        private void AcceptCompareOZM_Execute(object sender, SimpleActionExecuteEventArgs e) {
            HrmSalaryTaskCompareOZMAccountOperation task = e.CurrentObject as HrmSalaryTaskCompareOZMAccountOperation;
            task.MatrixAllocResultOZM.Status = HrmMatrixStatus.MATRIX_ACCEPTED;
            if (task.Period.CurrentMatrixAllocResultOZM.Status == HrmMatrixStatus.MATRIX_ACCEPTED) {
                task.Period.CurrentOZMmatrixReduction.MinimizeNumberOfDeviationsMatrix.Status = HrmMatrixStatus.MATRIX_ACCEPTED;
            }
            if (task.Period.CurrentAllocParameter.Status == HrmPeriodAllocParameterStatus.ALLOC_PARAMETERS_ACCEPTED && HrmPeriodLogic.AccountOperationCompared(task.Period)) {
                task.Period.setStatus(HrmPeriodStatus.READY_TO_RESERVE_MATRIX_CREATE);
            }

            task.Complete();
            ObjectSpace.CommitChanges();
            Window win = Frame as Window;
            if (win != null) win.Close();
        }
    }
}