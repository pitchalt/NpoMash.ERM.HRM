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
            task.Period.CurrentKBmatrixReduction.MinimizeNumberOfDeviationsMatrix.Status = HrmMatrixStatus.MATRIX_ACCEPTED;

            task.Complete();

            ObjectSpace.CommitChanges();

            Window win = Frame as Window;
            if (win != null) win.Close();
        }

        private void AcceptCompareOZM_Execute(object sender, SimpleActionExecuteEventArgs e) {
            HrmSalaryTaskCompareWorkTime task = e.CurrentObject as HrmSalaryTaskCompareWorkTime;
            task.Period.CurrentOZMmatrixReduction.MinimizeNumberOfDeviationsMatrix.Status = HrmMatrixStatus.MATRIX_ACCEPTED;

            task.Complete();

            ObjectSpace.CommitChanges();

            Window win = Frame as Window;
            if (win != null) win.Close();
        }
    }
}
