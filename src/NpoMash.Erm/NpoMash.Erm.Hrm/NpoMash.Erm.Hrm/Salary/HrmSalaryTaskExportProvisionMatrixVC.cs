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

    public partial class HrmSalaryTaskExportProvisionMatrixVC : ViewController {
        public HrmSalaryTaskExportProvisionMatrixVC() {InitializeComponent(); 
            RegisterActions(components);
        }
        protected override void OnActivated() { base.OnActivated(); }
        protected override void OnViewControlsCreated() { base.OnViewControlsCreated(); }
        protected override void OnDeactivated() { base.OnDeactivated(); }

        private void ExportProvisionMatrix_Execute(object sender, SimpleActionExecuteEventArgs e) {
            HrmSalaryTaskExportProvisionMatrix task = e.CurrentObject as HrmSalaryTaskExportProvisionMatrix;
            HrmSalaryTaskExportProvisionMatrixLogic.ExportProvisonMatrix(task);
            foreach (var m in task.Period.Matrixs) {
                if (m.Status == HrmMatrixStatus.MATRIX_PRIMARY_ACCEPTED && m.TypeMatrix == HrmMatrixTypeMatrix.MATRIX_RESERVE) {
                    m.Status = HrmMatrixStatus.MATRIX_EXPORTED;
                }
            }
            task.Period.setStatus(HrmPeriodStatus.RESERVE_MATRIX_UPLOADED);
            task.Complete();
            ObjectSpace.CommitChanges();
            Window win = Frame as Window;
            if (win != null) win.Close();
        }
    }
}