using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Collections;
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
using FileHelpers;
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.FM.Order;


namespace NpoMash.Erm.Hrm.Salary {
    public partial class HrmPeriodVC : ViewController {

        
        public HrmPeriodVC() { 
            InitializeComponent(); 
            RegisterActions(components); 

        }

        protected override void OnActivated() { base.OnActivated(); }
        protected override void OnViewControlsCreated() { base.OnViewControlsCreated(); }
        protected override void OnDeactivated() { base.OnDeactivated(); }

        private void ImportSourceData_Execute(object sender, ParametrizedActionExecuteEventArgs e) {

        }

        private void GetSourceDataAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e) {
            IObjectSpace os = Application.CreateObjectSpace();
            HrmPeriod period = (HrmPeriod)e.CurrentObject;
            HrmPeriod current_period = os.GetObject<HrmPeriod>(period);
            if (current_period.Status == HrmPeriodStatus.OPENED || 
                current_period.Status == HrmPeriodStatus.LIST_OF_CONTROLLED_ORDERS_ACCEPTED) {
                    if (e.SelectedChoiceActionItem.Id == "GenerateTestData") {
                        HrmMatrix matrixKB = HrmMatrixLogic.setTestData(os, current_period, DEPARTMENT_GROUP_DEP.KB);
                        matrixKB.Status = HRM_MATRIX_STATUS.ACCEPTED;
                        HrmMatrix matrixOZM = HrmMatrixLogic.setTestData(os, current_period, DEPARTMENT_GROUP_DEP.OZM);
                        matrixOZM.Status = HRM_MATRIX_STATUS.ACCEPTED;
                        HrmTimeSheetLogic.loadTimeSheetIntoPeriod(os, current_period);
                        current_period.setStatus(HrmPeriodStatus.SOURCE_DATA_LOADED);
                        e.ShowViewParameters.CreatedView = Application.CreateDetailView(os, matrixKB);
                        e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        os.Committed += new EventHandler(refresher);
                    }
                    if (e.SelectedChoiceActionItem.Id == "GetDataFromServer") {

                    }
                    if (e.SelectedChoiceActionItem.Id == "XmlFile") {

                    }
                    if (e.SelectedChoiceActionItem.Id == "StructuredFile") {
                        HrmMatrixAllocPlan matrixKB = null;
                        HrmMatrixAllocPlan matrixOZM = null;
                        HrmMatrixLogic.ImportPlanMatrixes(os, current_period, out matrixKB, out matrixOZM);
                        HrmTimeSheetLogic.ImportData(os, current_period);
                        current_period.setStatus(HrmPeriodStatus.SOURCE_DATA_LOADED);
                        e.ShowViewParameters.CreatedView = Application.CreateDetailView(os, matrixKB);
                        e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        os.Committed += new EventHandler(refresher);
                    }

            }
        }

        private void BringingKBMatrixAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e) {
            IObjectSpace os = Application.CreateObjectSpace();
            HrmPeriod period = os.GetObject<HrmPeriod>((HrmPeriod)e.CurrentObject);
            DEPARTMENT_GROUP_DEP group_dep = DEPARTMENT_GROUP_DEP.KB;
            if (period.Status == HrmPeriodStatus.READY_TO_CALCULATE_COERCED_MATRIXS) {
                HRM_MATRIX_VARIANT bringing_method = HrmSalaryTaskMatrixReductionLogic.DetermineSelectedBringingMethod(e);
                HrmSalaryTaskMatrixReduction reduc = null;
                if (period.CurrentKBmatrixReduction == null)
                    reduc = HrmSalaryTaskMatrixReductionLogic.initTaskMatrixReduction(period, os,
                        group_dep, bringing_method);
                else reduc = os.GetObject<HrmSalaryTaskMatrixReduction>(period.CurrentKBmatrixReduction);
                HrmSalaryTaskMatrixReductionLogic.CreateMatrixInReduc(reduc, os, group_dep, bringing_method, period);
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(os, reduc);
                e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                os.Committed += new EventHandler(refresher);
            }
        }

        private void BringingOZMMatrixAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e) {
            IObjectSpace os = Application.CreateObjectSpace();
            DEPARTMENT_GROUP_DEP group_dep = DEPARTMENT_GROUP_DEP.OZM;
            HrmPeriod period = os.GetObject<HrmPeriod>((HrmPeriod)e.CurrentObject);
            if (period.Status == HrmPeriodStatus.READY_TO_CALCULATE_COERCED_MATRIXS) {
                HRM_MATRIX_VARIANT bringing_method = HrmSalaryTaskMatrixReductionLogic.DetermineSelectedBringingMethod(e);
                HrmSalaryTaskMatrixReduction reduc = null;
                if (period.CurrentOZMmatrixReduction == null) 
                    reduc = HrmSalaryTaskMatrixReductionLogic.initTaskMatrixReduction(period, os,
                        group_dep, bringing_method);
                else reduc = os.GetObject<HrmSalaryTaskMatrixReduction>(period.CurrentOZMmatrixReduction);
                HrmSalaryTaskMatrixReductionLogic.CreateMatrixInReduc(reduc, os, group_dep, bringing_method, period);
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(os, reduc);
                e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                os.Committed += new EventHandler(refresher);
            }
        }

        private void refresher(Object sender, EventArgs e) {
            Frame.GetController<RefreshController>().RefreshAction.DoExecute();
        }

    }
}