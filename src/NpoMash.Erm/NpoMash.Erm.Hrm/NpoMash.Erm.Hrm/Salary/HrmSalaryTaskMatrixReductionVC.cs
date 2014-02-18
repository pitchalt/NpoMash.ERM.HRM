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

using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.FM.Order;

namespace NpoMash.Erm.Hrm.Salary {
    // For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppViewControllertopic.
    public partial class HrmSalaryTaskMatrixReductionVC : ViewController {
        public HrmSalaryTaskMatrixReductionVC() {
            InitializeComponent();
            RegisterActions(components);
            // Target required Views (via the TargetXXX properties) and create their Actions.
            //
            // Заполним список Items на основе enum HrmMatrixVariant
            foreach (object current in Enum.GetValues(typeof(HrmMatrixVariant))) {
                EnumDescriptor ed = new EnumDescriptor(typeof(HrmMatrixVariant));
                ChoiceActionItem item = new ChoiceActionItem(ed.GetCaption(current), current);
                BringingMatrixInReducAction.Items.Add(item);
                item = new ChoiceActionItem(ed.GetCaption(current), current);
                AcceptCoercedMatrixAction.Items.Add(item);
            }
        }
        protected override void OnActivated() {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            UpdateActionsItemsState();
            View.CurrentObjectChanged += new EventHandler(View_CurrentObjectChanged);
        }

        void View_CurrentObjectChanged(object sender, EventArgs e) {
            UpdateActionsItemsState();
        }
        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated() {
            View.CurrentObjectChanged -= new EventHandler(View_CurrentObjectChanged);
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        public void UpdateActionsItemsState() {
            HrmSalaryTaskMatrixReduction task = (HrmSalaryTaskMatrixReduction) View.CurrentObject as HrmSalaryTaskMatrixReduction;
            if (task == null)
                return;
            foreach (ChoiceActionItem  choice_item in BringingMatrixInReducAction.Items) {
                SetItemState(choice_item, task, false);
            }
            foreach (ChoiceActionItem choice_item in AcceptCoercedMatrixAction.Items) {
                SetItemState(choice_item, task, true);
            }
        }

        private void SetItemState(ChoiceActionItem choice_item, HrmSalaryTaskMatrixReduction task, Boolean value) {
            HrmMatrixVariant variant = (HrmMatrixVariant)choice_item.Data;
            switch (variant) {
                case HrmMatrixVariant.MINIMIZE_MAXIMUM_DEVIATIONS_VARIANT:
                    if (task.MinimizeMaximumDeviationsMatrix != null)
                        choice_item.Active.SetItemValue(this.GetType().FullName, value);
                    else
                        choice_item.Active.SetItemValue(this.GetType().FullName, !value);
                    break;
                case HrmMatrixVariant.MINIMIZE_NUMBER_OF_DEVIATIONS_VARIANT:
                    if (task.MinimizeNumberOfDeviationsMatrix != null)
                        choice_item.Active.SetItemValue(this.GetType().FullName, value);
                    else
                        choice_item.Active.SetItemValue(this.GetType().FullName, !value);
                    break;
                case HrmMatrixVariant.PROPORTIONS_METHOD_VARIANT:
                    if (task.ProportionsMethodMatrix != null)
                        choice_item.Active.SetItemValue(this.GetType().FullName, value);
                    else
                        choice_item.Active.SetItemValue(this.GetType().FullName, !value);
                    break;
            }
        }

        private void BringingMatrixInReduc_Execute(object sender, SingleChoiceActionExecuteEventArgs e) {
            HrmSalaryTaskMatrixReduction task = (HrmSalaryTaskMatrixReduction)e.CurrentObject;
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                task = os.GetObject<HrmSalaryTaskMatrixReduction>(task);
                if (task.Period.Status == HrmPeriodStatus.READY_TO_CALCULATE_COERCED_MATRIXS) {
                    //HrmMatrixVariant bringing_method = HrmSalaryTaskMatrixReductionLogic.DetermineSelectedBringingMethod(e);
                    HrmMatrixVariant bringing_method = (HrmMatrixVariant)e.SelectedChoiceActionItem.Data;
                    HrmSalaryTaskMatrixReductionLogic.CreateMatrixInReduc(task, os, task.GroupDep, bringing_method, task.Period);
                    os.CommitChanges();
                }
            }
            UpdateActionsItemsState();
        }

        private void AcceptCoercedMatrixAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e) {
            HrmSalaryTaskMatrixReduction task = (HrmSalaryTaskMatrixReduction)e.CurrentObject;
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                task = os.GetObject<HrmSalaryTaskMatrixReduction>(task);
                HrmMatrixVariant bringing_method = (HrmMatrixVariant)e.SelectedChoiceActionItem.Data;
                HrmMatrix matrix_to_accept = null;
                switch (bringing_method) {
                    case HrmMatrixVariant.MINIMIZE_MAXIMUM_DEVIATIONS_VARIANT:
                        matrix_to_accept = task.MinimizeMaximumDeviationsMatrix;
                        break;
                    case HrmMatrixVariant.MINIMIZE_NUMBER_OF_DEVIATIONS_VARIANT:
                        matrix_to_accept = task.MinimizeNumberOfDeviationsMatrix;
                        break;
                    case HrmMatrixVariant.PROPORTIONS_METHOD_VARIANT:
                        matrix_to_accept = task.ProportionsMethodMatrix;
                        break;
                }
                if (matrix_to_accept != null && matrix_to_accept.Status == HrmMatrixStatus.MATRIX_SAVED) {
                    HrmSalaryTaskMatrixReductionLogic.AcceptSelectedMatrix(task, matrix_to_accept);
                    if (HrmSalaryTaskMatrixReductionLogic.AllCoercedMatrixesAccepted(matrix_to_accept, task.Period))
                        task.Period.setStatus(HrmPeriodStatus.READY_TO_EXPORT_CORCED_MATRIXS);
                }
                task.Complete();
                os.CommitChanges();
            }
            ObjectSpace.CommitChanges();
            UpdateActionsItemsState();
        }

        private void ExportCoercedMatrixAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            HrmSalaryTaskMatrixReduction reduc = (HrmSalaryTaskMatrixReduction)e.CurrentObject;
            IObjectSpace os = ObjectSpace;
            HrmPeriod current_period = os.GetObject<HrmPeriod>(reduc.Period);
            if (reduc.Period.Status == HrmPeriodStatus.READY_TO_EXPORT_CORCED_MATRIXS
                && reduc.GroupDep == DepartmentGroupDep.DEPARTMENT_KB) {
                HrmSalaryTaskMatrixReductionLogic.ExportMatrixes(current_period);
                current_period.setStatus(HrmPeriodStatus.COERCED_MATRIXES_EXPORTED);
                os.CommitChanges();
            }
        }

        private void refresher(Object sender, EventArgs e) {
            Frame.GetController<RefreshController>().RefreshAction.DoExecute();
        }
    }
}