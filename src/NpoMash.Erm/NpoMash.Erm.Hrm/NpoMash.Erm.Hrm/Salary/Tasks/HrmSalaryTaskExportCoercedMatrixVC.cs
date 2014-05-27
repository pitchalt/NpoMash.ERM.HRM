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
    public partial class HrmSalaryTaskExportCoercedMatrixVC : ViewController {
        public HrmSalaryTaskExportCoercedMatrixVC() {
            InitializeComponent();
            RegisterActions(components);
        }
        protected override void OnActivated() { base.OnActivated(); }
        protected override void OnViewControlsCreated() { base.OnViewControlsCreated(); }
        protected override void OnDeactivated() { base.OnDeactivated(); }

        private void ExportCoercedMatrix_Execute(object sender, SimpleActionExecuteEventArgs e) {
            HrmSalaryTaskExportCoercedMatrix task = e.CurrentObject as HrmSalaryTaskExportCoercedMatrix;
            HrmSalaryTaskExportCoercedMatrixLogic.ExportCoercedMatrix(task);
            HrmSalaryTaskMatrixReductionLogic.ExportMatrixes(task.Period);
            task.Period.setStatus(HrmPeriodStatus.COERCED_MATRIXES_EXPORTED);
            task.Complete();
            ObjectSpace.CommitChanges();
            Window win = Frame as Window;
            if (win != null) win.Close();
        }
    }
}