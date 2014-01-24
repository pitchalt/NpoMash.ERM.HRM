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

        protected override void OnActivated() { 
            base.OnActivated();
            DetailView detail_view = View as DetailView;
            // Если мы в карточке
            if (detail_view != null) {
                // Получим доступ к редактору списка задач периода
                ListPropertyEditor list_editor = detail_view.FindItem("PeriodTasks") as ListPropertyEditor;
                if (list_editor != null) 
                    // Подпишемся на событие создание реального контрола редактора (XAF использует ленивую загрузку контролов)
                    list_editor.ControlCreated += new EventHandler<EventArgs>(TaskListEditor_ControlCreated);
            }
        }

        void TaskListEditor_ControlCreated(object sender, EventArgs e) {
            // ПОдпишемся на события контроллера управляющего реакцией на действие открыть в списке
            ListPropertyEditor list_editor = (ListPropertyEditor) sender;
            // Найдем в фрейме редактора нужный нам контроллер 
            ListViewProcessCurrentObjectController list_view_controller = list_editor.Frame.GetController<ListViewProcessCurrentObjectController>();
            if (list_view_controller != null) 
                // Подпишемся на событие открыть объект в списке
                list_view_controller.CustomProcessSelectedItem += new EventHandler<CustomProcessListViewSelectedItemEventArgs>(TaskListView_CustomProcessSelectedItem);
        }

        void TaskListView_CustomProcessSelectedItem(object sender, CustomProcessListViewSelectedItemEventArgs e) {
            HrmSalaryTask task = e.InnerArgs.CurrentObject as HrmSalaryTask;
            if (task != null) {
                IObjectSpace os = Application.CreateObjectSpace();
                task = os.GetObject<HrmSalaryTask>(task);
                e.Handled = true;
                e.InnerArgs.ShowViewParameters.CreatedView = Application.CreateDetailView(os, task);
                e.InnerArgs.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                os.Committed += new EventHandler(refresher);
            }

        }

        protected override void OnViewControlsCreated() { 
            base.OnViewControlsCreated(); 
        }

        protected override void OnDeactivated() { 
            base.OnDeactivated(); 
        }

        private void ImportSourceData_Execute(object sender, ParametrizedActionExecuteEventArgs e) {

        }

        private void GetSourceDataAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e) {
            IObjectSpace os = Application.CreateObjectSpace();
            HrmPeriod period = (HrmPeriod)e.CurrentObject;
            HrmPeriod current_period = os.GetObject<HrmPeriod>(period);
            if (current_period.Status == HrmPeriodStatus.OPENED || 
                current_period.Status == HrmPeriodStatus.LIST_OF_CONTROLLED_ORDERS_ACCEPTED) {
                    HrmSalaryTaskImportSourceData task = os.CreateObject<HrmSalaryTaskImportSourceData>();
                    current_period.PeriodTasks.Add(task);
                    if (e.SelectedChoiceActionItem.Id == "GenerateTestData") {
                        task.MatrixPlanKB = HrmMatrixLogic.setTestData(os, current_period, DepartmentGroupDep.DEPARTMENT_KB);
                        task.MatrixPlanKB.Status = HrmMatrixStatus.MATRIX_ACCEPTED;
                        task.MatrixPlanOZM = HrmMatrixLogic.setTestData(os, current_period, DepartmentGroupDep.DEPARTMENT_OZM);
                        task.MatrixPlanOZM.Status = HrmMatrixStatus.MATRIX_ACCEPTED;
                        HrmTimeSheetLogic.loadTimeSheetIntoPeriod(os, task);
                        current_period.setStatus(HrmPeriodStatus.READY_TO_CALCULATE_COERCED_MATRIXS);
                        e.ShowViewParameters.CreatedView = Application.CreateDetailView(os, task);
                        e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        os.Committed += new EventHandler(refresher);
                    }
                    if (e.SelectedChoiceActionItem.Id == "GetDataFromServer") {

                    }
                    if (e.SelectedChoiceActionItem.Id == "XmlFile") {

                    }
                    if (e.SelectedChoiceActionItem.Id == "StructuredFile") {
//                        HrmMatrixAllocPlan matrixKB = null;
//                        HrmMatrixAllocPlan matrixOZM = null;
                        HrmSalaryTaskImportSourceDataLogic.ImportPlanMatrixes(os, task); //current_period, out matrixKB, out matrixOZM);
                        HrmSalaryTaskImportSourceDataLogic.ImportTimeSheet(os, task);
                        current_period.setStatus(HrmPeriodStatus.READY_TO_CALCULATE_COERCED_MATRIXS);
                        e.ShowViewParameters.CreatedView = Application.CreateDetailView(os, task);
                        e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        os.Committed += new EventHandler(refresher);
                    }

            }
        }

        private void BringingKBMatrixAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e) {
            IObjectSpace os = Application.CreateObjectSpace();
            HrmPeriod period = os.GetObject<HrmPeriod>((HrmPeriod)e.CurrentObject);
            DepartmentGroupDep group_dep = DepartmentGroupDep.DEPARTMENT_KB;
            if (period.Status == HrmPeriodStatus.READY_TO_CALCULATE_COERCED_MATRIXS) {
                HrmMatrixVariant bringing_method = HrmSalaryTaskMatrixReductionLogic.DetermineSelectedBringingMethod(e);
                HrmSalaryTaskMatrixReduction reduc = null;
                if (period.CurrentKBmatrixReduction == null)
                    reduc = HrmSalaryTaskMatrixReductionLogic.initTaskMatrixReduction(os, period, 
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
            DepartmentGroupDep group_dep = DepartmentGroupDep.DEPARTMENT_OZM;
            HrmPeriod period = os.GetObject<HrmPeriod>((HrmPeriod)e.CurrentObject);
            if (period.Status == HrmPeriodStatus.READY_TO_CALCULATE_COERCED_MATRIXS) {
                HrmMatrixVariant bringing_method = HrmSalaryTaskMatrixReductionLogic.DetermineSelectedBringingMethod(e);
                HrmSalaryTaskMatrixReduction reduc = null;
                if (period.CurrentOZMmatrixReduction == null)
                    reduc = HrmSalaryTaskMatrixReductionLogic.initTaskMatrixReduction(os, period,
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