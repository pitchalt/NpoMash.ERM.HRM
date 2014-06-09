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
using IntecoAG.ERM.HRM.Organization;

namespace NpoMash.Erm.Hrm.Salary {
    public partial class HrmSalaryTaskImportSourceDataVC : ViewController {
        public HrmSalaryTaskImportSourceDataVC() {
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

        private void AcceptImport_Execute(object sender, SimpleActionExecuteEventArgs e) {
            HrmSalaryTaskImportSourceData task = e.CurrentObject as HrmSalaryTaskImportSourceData;
            task.MatrixPlanKB.Status = HrmMatrixStatus.MATRIX_ACCEPTED;
            task.MatrixPlanOZM.Status = HrmMatrixStatus.MATRIX_ACCEPTED;
            task.TimeSheetKB.SetStatus(HrmTimeSheetStatus.ACCEPTED);
            task.TimeSheetOZM.SetStatus(HrmTimeSheetStatus.ACCEPTED);
            task.Period.setStatus(HrmPeriodStatus.READY_TO_CALCULATE_COERCED_MATRIXS);
            task.GroupDep = DepartmentGroupDep.DEPARTMENT_KB_OZM;
            task.Complete();
            ObjectSpace.CommitChanges();
            Window win = Frame as Window;
            if (win != null) win.Close();
        }
    }
}