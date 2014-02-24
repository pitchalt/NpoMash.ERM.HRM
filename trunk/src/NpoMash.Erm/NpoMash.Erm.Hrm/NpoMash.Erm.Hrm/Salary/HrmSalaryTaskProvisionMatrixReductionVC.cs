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

        private void BringingProvisionMatrix_Execute(object sender, SimpleActionExecuteEventArgs e) {
           /* IObjectSpace os = Application.CreateObjectSpace();
            HrmPeriod period = os.GetObject<HrmPeriod>((HrmPeriod)e.CurrentObject);
            DepartmentGroupDep group_dep = DepartmentGroupDep.DEPARTMENT_KB;
            var card = HrmSalaryTaskProvisionMatrixReductionLogic.initProvisonMatrixTask(os, period, group_dep);
            os.CommitChanges();
            e.ShowViewParameters.CreatedView = Application.CreateDetailView(os, card);
            e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;*/
            
        }

        private void BringProvisionMatrix_Execute(object sender, SingleChoiceActionExecuteEventArgs e) {
            if (e.SelectedChoiceActionItem.Id == "EkvilibristicMethod") {
                IObjectSpace os = Application.CreateObjectSpace();
                HrmPeriod period = os.GetObject<HrmPeriod>((HrmPeriod)e.CurrentObject);
                DepartmentGroupDep group_dep = DepartmentGroupDep.DEPARTMENT_KB;
                var card = HrmSalaryTaskProvisionMatrixReductionLogic.initProvisonMatrixTask(os, period, group_dep);
                os.CommitChanges();
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(os, card);
                e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            }
        }

        private void AcceptProvisionMatrix_Execute(object sender, SingleChoiceActionExecuteEventArgs e) {
            if (e.SelectedChoiceActionItem.Id == "EkvilibristicMethod") {
              /*  IObjectSpace object_space = ObjectSpace;
                HrmPeriod period = object_space.GetObject<HrmPeriod>((HrmPeriod)e.CurrentObject);
                period.setStatus(HrmPeriodStatus.READY_TO_RESERVE_MATRIX_UPLOAD);

                HrmSalaryTaskProvisionMatrixReduction task = object_space.GetObject<HrmSalaryTaskProvisionMatrixReduction>((HrmSalaryTaskProvisionMatrixReduction)e.CurrentObject);
                task.ProvisionMatrix.Status = HrmMatrixStatus.MATRIX_PRIMARY_ACCEPTED;
                task.Complete();
                object_space.CommitChanges();*/
                }
            
            }
        }
    }