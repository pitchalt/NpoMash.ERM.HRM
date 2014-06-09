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
        public HrmSalaryTaskExportProvisionMatrixVC() {
            InitializeComponent();
            RegisterActions(components);
        }
        protected override void OnActivated() { base.OnActivated(); }
        protected override void OnViewControlsCreated() { base.OnViewControlsCreated(); }
        protected override void OnDeactivated() { base.OnDeactivated(); }

        private void ExportProvisionMatrix_Execute(object sender, SimpleActionExecuteEventArgs e) {
            HrmSalaryTaskExportProvisionMatrix task = e.CurrentObject as HrmSalaryTaskExportProvisionMatrix;
            HrmSalaryTaskExportProvisionMatrixLogic.ExportProvisonMatrix(task);
            HrmSalaryTaskExportProvisionMatrixLogic.ExportProvisonMatrix(task);
            task.Period.setStatus(HrmPeriodStatus.RESERVE_MATRIX_UPLOADED);
            task.Complete();
            ObjectSpace.CommitChanges();
            Window win = Frame as Window;
            if (win != null) win.Close();
        }
        /*HrmSalaryTaskExportCoercedMatrix task = e.CurrentObject as HrmSalaryTaskExportCoercedMatrix;
            HrmSalaryTaskExportCoercedMatrixLogic.ExportCoercedMatrix(task);
            HrmSalaryTaskMatrixReductionLogic.ExportMatrixes(task.Period);
            task.Period.setStatus(HrmPeriodStatus.COERCED_MATRIXES_EXPORTED);
            task.Complete();
            ObjectSpace.CommitChanges();
            Window win = Frame as Window;
            if (win != null) win.Close();*/
    }
}