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
                    //card.MatrixPlan = HrmSalaryTaskProvisionMatrixReductionLogic.mergePlanMatrixes(os, card);
                    // card.MatrixAlloc = HrmSalaryTaskProvisionMatrixReductionLogic.mergeCorcedMatrixs(os, card);
                    //card.MatrixPlanMoney = HrmSalaryTaskProvisionMatrixReductionLogic.createMoneyMatrix(os, card);
                    // card.AllocResultKBOZM = HrmSalaryTaskProvisionMatrixReductionLogic.mergeAllocResults(os, card);
                    // card.ProvisionMatrix = HrmSalaryTaskProvisionMatrixReductionLogic.combineMatrixes(os, card);
                    // card.ProvisionMatrix = HrmSalaryTaskProvisionMatrixReductionLogic.calculateProvisionMatrix(os, card);

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

            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                task = os.GetObject<HrmSalaryTaskProvisionMatrixReduction>(task);
                if (e.SelectedChoiceActionItem.Id == "EkvilibristicMethod") {

                    task.AllocParameters.Period.setStatus(HrmPeriodStatus.READY_TO_RESERVE_MATRIX_UPLOAD);
                    foreach (var m in task.AllocParameters.Period.Matrixs) {
                        if (m.TypeMatrix == HrmMatrixTypeMatrix.MATRIX_RESERVE) {
                            m.Status = HrmMatrixStatus.MATRIX_PRIMARY_ACCEPTED;
                            task.Period.CurrentProvisionMatrix.ProvisionMatrix.Status = HrmMatrixStatus.MATRIX_PRIMARY_ACCEPTED;
                        }

                    }
                    task.Complete();
                    os.CommitChanges();
                }
                if (e.SelectedChoiceActionItem.Id == "SimplexMethod") {
                    task = os.GetObject<HrmSalaryTaskProvisionMatrixReduction>(task);
                    //task.Period.setStatus(HrmPeriodStatus.READY_TO_RESERVE_MATRIX_UPLOAD);
                    HrmMatrix matrix_to_accept = null;
                    matrix_to_accept = task.ProvisionMatrix;

                    if (matrix_to_accept != null && matrix_to_accept.Status == HrmMatrixStatus.MATRIX_SAVED) {
                        HrmSalaryTaskProvisionMatrixReductionLogic.AcceptSelectedMatrix(task, matrix_to_accept);
                        if (HrmSalaryTaskProvisionMatrixReductionLogic.MatrixAccepted(matrix_to_accept, task.Period))
                            task.Period.setStatus(HrmPeriodStatus.READY_TO_RESERVE_MATRIX_UPLOAD);

                        task.Complete();
                        os.CommitChanges();
                    }


                }


            }
            ObjectSpace.CommitChanges();

            Window win = Frame as Window;
            if (win != null) win.Close();
        }

        private void refresher(Object sender, EventArgs e) {
            Frame.GetController<RefreshController>().RefreshAction.DoExecute();
        }


    }
}