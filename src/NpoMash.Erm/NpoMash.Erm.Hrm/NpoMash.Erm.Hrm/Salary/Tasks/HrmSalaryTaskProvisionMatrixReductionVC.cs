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
    public partial class HrmSalaryTaskProvisionMatrixReductionVC : ViewController {




        public HrmSalaryTaskProvisionMatrixReductionVC() { InitializeComponent(); RegisterActions(components); }
        protected override void OnActivated() {
            base.OnActivated();
        }
        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();
        }
        protected override void OnDeactivated() {
            base.OnDeactivated();
        }


        private void BringProvisionMatrix_Execute(object sender, SingleChoiceActionExecuteEventArgs e) {
            if (e.SelectedChoiceActionItem.Id == "EkvilibristicMethod") {
                IObjectSpace os = Application.CreateObjectSpace();
                HrmPeriod period = os.GetObject<HrmPeriod>((HrmPeriod)e.CurrentObject);
                DepartmentGroupDep group_dep = DepartmentGroupDep.DEPARTMENT_KB_OZM;
                HrmSalaryTaskProvisionMatrixReduction card = null;
                if (period.CurrentProvisionMatrix == null) {
                    card = HrmSalaryTaskProvisionMatrixReductionLogic.initProvisonMatrixTask(os, period, group_dep);
                }
                else card = os.GetObject<HrmSalaryTaskProvisionMatrixReduction>(period.CurrentProvisionMatrix);
                card.Period = period;
                os.CommitChanges();
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(os, card);
                e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                os.Committed += new EventHandler(refresher);
            }
        }

        private void AcceptProvisionMatrix_Execute(object sender, SingleChoiceActionExecuteEventArgs e) {
            HrmSalaryTaskProvisionMatrixReduction task = (HrmSalaryTaskProvisionMatrixReduction)e.CurrentObject;

            IObjectSpace os = ObjectSpace.CreateNestedObjectSpace();
            task = os.GetObject<HrmSalaryTaskProvisionMatrixReduction>(task);
            HrmMatrix matrix_to_accept = null;
            if (e.SelectedChoiceActionItem.Id == "EkvilibristicMethod")
                matrix_to_accept = task.ReserveMatrixEvristic;
            if (e.SelectedChoiceActionItem.Id == "SimplexMethod")
                matrix_to_accept = task.ReserveMatrixSimplex;

            if (matrix_to_accept != null && matrix_to_accept.Status == HrmMatrixStatus.MATRIX_SAVED) {
                HrmSalaryTaskProvisionMatrixReductionLogic.PrimaryAcceptSelectedMatrix(task, matrix_to_accept);
                task.Period.setStatus(HrmPeriodStatus.READY_TO_RESERVE_MATRIX_UPLOAD);
                task.Period.CurrentMatrixProvision = matrix_to_accept;
            }
            task.Complete();
            os.CommitChanges();
            ObjectSpace.CommitChanges();
            Window win = Frame as Window;
            if (win != null) win.Close();
        }



        private void refresher(Object sender, EventArgs e) {
            Frame.GetController<RefreshController>().RefreshAction.DoExecute();
        }


    }
}