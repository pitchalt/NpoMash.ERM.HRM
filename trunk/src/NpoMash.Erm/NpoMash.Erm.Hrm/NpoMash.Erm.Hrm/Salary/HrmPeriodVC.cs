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
            ListPropertyEditor list_editor = (ListPropertyEditor)sender;
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
                    task.MatrixPlanKB.Status = HrmMatrixStatus.MATRIX_DOWNLOADED;
                    task.MatrixPlanOZM = HrmMatrixLogic.setTestData(os, current_period, DepartmentGroupDep.DEPARTMENT_OZM);
                    task.MatrixPlanOZM.Status = HrmMatrixStatus.MATRIX_DOWNLOADED;
                    HrmTimeSheetLogic.loadTimeSheetIntoPeriod(os, task);
                    e.ShowViewParameters.CreatedView = Application.CreateDetailView(os, task);
                    e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    os.Committed += new EventHandler(refresher);
                }
                if (e.SelectedChoiceActionItem.Id == "GetDataFromServer") {

                }
                if (e.SelectedChoiceActionItem.Id == "XmlFile") {

                }
                if (e.SelectedChoiceActionItem.Id == "StructuredFile") {
                    HrmSalaryTaskImportSourceDataLogic.ImportPlanMatrixes(os, task); 
                    HrmSalaryTaskImportSourceDataLogic.ImportTimeSheet(os, task);
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

        private void ExportBringingMatrix_Execute(object sender, SimpleActionExecuteEventArgs e) {
            IObjectSpace object_space = ObjectSpace;
            HrmPeriod period = object_space.GetObject<HrmPeriod>((HrmPeriod)e.CurrentObject);
            if (period.Status == HrmPeriodStatus.READY_TO_EXPORT_CORCED_MATRIXS) {
                HrmSalaryTaskMatrixReductionLogic.ExportMatrixes(period);
                period.setStatus(HrmPeriodStatus.COERCED_MATRIXES_EXPORTED);
                object_space.CommitChanges();
            }
        }

        private void ImportAccountOperation_Execute(object sender, SimpleActionExecuteEventArgs e) {
            IObjectSpace object_space = Application.CreateObjectSpace();
            HrmPeriod current_period = object_space.GetObject<HrmPeriod>((HrmPeriod)e.CurrentObject);
            if (current_period.Status == HrmPeriodStatus.COERCED_MATRIXES_EXPORTED) {
                HrmSalaryTaskImportAccountOperation task = object_space.CreateObject<HrmSalaryTaskImportAccountOperation>();
                current_period.PeriodTasks.Add(task);
                HrmSalaryTaskImportAccountOperationLogic.ImportAccountOperation(object_space, task);
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(object_space, task);
                e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                object_space.Committed += new EventHandler(refresher);
            }
        }

        private void refresher(Object sender, EventArgs e) {
            Frame.GetController<RefreshController>().RefreshAction.DoExecute();
        }

        private void ExportReserveMatrix_Execute(object sender, SimpleActionExecuteEventArgs e) {
            IObjectSpace object_space = ObjectSpace;
            HrmPeriod period = object_space.GetObject<HrmPeriod>((HrmPeriod)e.CurrentObject);
            if (period.Status == HrmPeriodStatus.READY_TO_RESERVE_MATRIX_UPLOAD) {
                period.setStatus(HrmPeriodStatus.RESERVE_MATRIX_UPLOADED);
                foreach (var m in period.Matrixs) {
                    if (m.Status == HrmMatrixStatus.MATRIX_PRIMARY_ACCEPTED && m.TypeMatrix == HrmMatrixTypeMatrix.MATRIX_RESERVE) {
                        m.Status = HrmMatrixStatus.MATRIX_EXPORTED;
                    }

                }
                object_space.CommitChanges();
            }
        }

        private void CreateReportKB_Execute(object sender, SimpleActionExecuteEventArgs e) {
            IObjectSpace object_space = Application.CreateObjectSpace();
            HrmPeriod current_period = object_space.GetObject<HrmPeriod>((HrmPeriod)e.CurrentObject);
            if (current_period.Status == HrmPeriodStatus.ACCOUNT_OPERATION_FIRST_IMPORTED) {
                HrmSalaryTaskCompareKBAccountOperation task = object_space.CreateObject<HrmSalaryTaskCompareKBAccountOperation>();
                current_period.PeriodTasks.Add(task);
                HrmSalaryTaskCompareKBAccountOperationLogic.CompareKBMatrix(object_space, task);
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(object_space, task);
                e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                object_space.Committed += new EventHandler(refresher);
            }
        }

        private void CreateReportOZM_Execute(object sender, SimpleActionExecuteEventArgs e) {
            IObjectSpace object_space = Application.CreateObjectSpace();
            HrmPeriod current_period = object_space.GetObject<HrmPeriod>((HrmPeriod)e.CurrentObject);
            if (current_period.Status == HrmPeriodStatus.ACCOUNT_OPERATION_FIRST_IMPORTED) {
                HrmSalaryTaskCompareOZMAccountOperation task = object_space.CreateObject<HrmSalaryTaskCompareOZMAccountOperation>();
                current_period.PeriodTasks.Add(task);
                HrmSalaryTaskCompareOZMAccountOperationLogic.CompareOZMMatrix(object_space, task);
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(object_space, task);
                e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                object_space.Committed += new EventHandler(refresher);
            }
        }

        private void ImportAccountOperationLast_Execute(object sender, SimpleActionExecuteEventArgs e) {
            IObjectSpace object_space = Application.CreateObjectSpace();
            HrmPeriod current_period = object_space.GetObject<HrmPeriod>((HrmPeriod)e.CurrentObject);
            if (current_period.Status == HrmPeriodStatus.RESERVE_MATRIX_UPLOADED) {
                HrmSalaryTaskImportAccountOperationSummary task = object_space.CreateObject<HrmSalaryTaskImportAccountOperationSummary>();
                current_period.PeriodTasks.Add(task);
                HrmSalaryTaskImportAccountOperationSummaryLogic.ImportAccountOperationSummary(object_space, task);
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(object_space, task);
                e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                object_space.Committed += new EventHandler(refresher);
            }
        }

        private void CreateReportSummary_Execute(object sender, SimpleActionExecuteEventArgs e) {
            IObjectSpace object_space = Application.CreateObjectSpace();
            HrmPeriod current_period = object_space.GetObject<HrmPeriod>((HrmPeriod)e.CurrentObject);
            if (current_period.Status == HrmPeriodStatus.ACCOUNT_OPERATION_LAST_IMPORTED) {
                HrmSalaryTaskCompareAccountOperationSummary task = object_space.CreateObject<HrmSalaryTaskCompareAccountOperationSummary>();
                current_period.PeriodTasks.Add(task);
                HrmSalaryTaskCompareAccountOperationSummaryLogic.CompareSummaryMatrix(object_space, task);
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(object_space, task);
                e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                object_space.Committed += new EventHandler(refresher);
            }
        }

        private void ClosePeriod_Execute(object sender, SimpleActionExecuteEventArgs e) {
            IObjectSpace object_space = ObjectSpace;
            HrmPeriod current_period = object_space.GetObject<HrmPeriod>((HrmPeriod)e.CurrentObject);
            if (current_period.Status == HrmPeriodStatus.ACCOUNT_OPERATION_LAST_IMPORTED && current_period.CurrentMatrixAllocResultSummary.Status == HrmMatrixStatus.MATRIX_ACCEPTED) {
                current_period.setStatus(HrmPeriodStatus.CLOSED);
                object_space.CommitChanges();
            }
        }

        private void simpleAction1_Execute(object sender, SimpleActionExecuteEventArgs e) {
            IObjectSpace object_space = Application.CreateObjectSpace();
            HrmPeriod current_period = object_space.GetObject<HrmPeriod>((HrmPeriod)e.CurrentObject);
            if (current_period.Status == HrmPeriodStatus.COERCED_MATRIXES_EXPORTED && current_period.CurrentAllocParameter.Status == HrmPeriodAllocParameterStatus.ALLOC_PARAMETERS_ACCEPTED) {
                HrmSalaryTaskImportAccountOperation task = object_space.CreateObject<HrmSalaryTaskImportAccountOperation>();
                current_period.PeriodTasks.Add(task);
                HrmSalaryTaskImportAccountOperationLogic.ImportAccountOperationTestData(object_space, task);
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(object_space, task);
                e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                object_space.Committed += new EventHandler(refresher);
            }
        }

        private void AccountOperationImport_Execute(object sender, SingleChoiceActionExecuteEventArgs e) {
            if (e.SelectedChoiceActionItem.Id == "GenerateTestData") {
                IObjectSpace object_space = Application.CreateObjectSpace();
                HrmPeriod current_period = object_space.GetObject<HrmPeriod>((HrmPeriod)e.CurrentObject);
                if (current_period.Status == HrmPeriodStatus.COERCED_MATRIXES_EXPORTED && current_period.CurrentAllocParameter.Status == HrmPeriodAllocParameterStatus.ALLOC_PARAMETERS_ACCEPTED) {
                    HrmSalaryTaskImportAccountOperation task = object_space.CreateObject<HrmSalaryTaskImportAccountOperation>();
                    current_period.PeriodTasks.Add(task);
                    HrmSalaryTaskImportAccountOperationLogic.ImportAccountOperationTestData(object_space, task);
                    e.ShowViewParameters.CreatedView = Application.CreateDetailView(object_space, task);
                    e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    object_space.Committed += new EventHandler(refresher);
                }
            }
        }



    }
}
