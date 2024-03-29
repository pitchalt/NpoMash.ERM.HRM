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
    public partial class HrmSalaryTaskCompareAccountOperationSummaryVC : ViewController {
        public HrmSalaryTaskCompareAccountOperationSummaryVC() {
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

        private void AcceptCompare_Execute(object sender, SimpleActionExecuteEventArgs e) {
            HrmSalaryTaskCompareAccountOperationSummary task = e.CurrentObject as HrmSalaryTaskCompareAccountOperationSummary;
            task.MatrixAllocResultSummary.Status = HrmMatrixStatus.MATRIX_ACCEPTED;

            foreach (var m in task.Period.Matrixs) {
                if (m.TypeMatrix == HrmMatrixTypeMatrix.MATRIX_RESERVE) { m.Status = HrmMatrixStatus.MATRIX_ACCEPTED; }
            }

            task.Complete();

            ObjectSpace.CommitChanges();

            Window win = Frame as Window;
            if (win != null) win.Close();
        }
    }
}