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
using NpoMash.Erm.Hrm;


namespace NpoMash.Erm.Hrm.Salary {
    public partial class HrmSalaryTaskCompareKBAccountOperationVC : ViewController {
        public HrmSalaryTaskCompareKBAccountOperationVC() {
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
            HrmSalaryTaskCompareKBAccountOperation task = e.CurrentObject as HrmSalaryTaskCompareKBAccountOperation;
            task.MatrixAllocResultKB.Status = HrmMatrixStatus.MATRIX_ACCEPTED;
            if (task.Period.CurrentMatrixAllocResultKB.Status == HrmMatrixStatus.MATRIX_ACCEPTED) {
                task.Period.CurrentKBmatrixReduction.MinimizeNumberOfDeviationsMatrix.Status = HrmMatrixStatus.MATRIX_ACCEPTED;
                task.Complete();
            }

            if (task.Period.CurrentAllocParameter.Status == HrmPeriodAllocParameterStatus.ALLOC_PARAMETERS_ACCEPTED && HrmPeriodLogic.AccountOperationCompared(task.Period)) {
                task.Period.setStatus(HrmPeriodStatus.READY_TO_RESERVE_MATRIX_CREATE);
            }
            
            ObjectSpace.CommitChanges();
            Window win = Frame as Window;
            if (win != null) win.Close();
        }
    }
}