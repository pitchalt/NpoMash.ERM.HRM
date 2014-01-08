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
using DevExpress.ExpressApp.ConditionalAppearance;
//using System.Windows.Forms;
using IntecoAG.XafExt.UI;

namespace NpoMash.Erm.Hrm.Salary
{
    // For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppViewControllertopic.
    public partial class HrmPeriodAllocParameterVC : ViewController
    {
        private const String MESSAGE_BOX_TEXT_PATH = @"Messages\HrmPariodAllocParameterVC";

        public HrmPeriodAllocParameterVC()
        {
            InitializeComponent();
            RegisterActions(components);
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            NewObjectViewController new_controller = Frame.GetController<NewObjectViewController>();
            new_controller.ObjectCreating += new EventHandler<ObjectCreatingEventArgs>(new_controller_ObjectCreating);
            HrmPeriodAllocParameter param = View.CurrentObject as HrmPeriodAllocParameter;
            if (param != null)
                UpdateActionState(param);
        }

        protected void UpdateActionState(HrmPeriodAllocParameter param) {
            if (param.Status == HrmPeriodAllocParameterStatus.AllocParametersAccepted)
                AcceptControlledOrderList.Active.SetItemValue(typeof(HrmPeriodAllocParameterVC).FullName, false);
            else
                AcceptControlledOrderList.Active.SetItemValue(typeof(HrmPeriodAllocParameterVC).FullName, true);
        }

        void new_controller_ObjectCreating(object sender, ObjectCreatingEventArgs e) {
            try {
                e.NewObject = HrmPeriodAllocParameterLogic.createParameters(e.ObjectSpace);
            }
            catch (OpenPeriodExistsException) {
                e.Cancel = true;
                ShowViewParameters view_params = new ShowViewParameters();
                var dialog_controller = MessageBox.InitMessageBox(Application, view_params,
                    CaptionHelper.GetLocalizedText(MESSAGE_BOX_TEXT_PATH, "MessageBoxCaption"),
                    CaptionHelper.GetLocalizedText(MESSAGE_BOX_TEXT_PATH, "MessageBoxText"));
                dialog_controller.Accepting += new EventHandler<DialogControllerAcceptingEventArgs>(dialog_controller_Accepting);
                dialog_controller.Cancelling += new EventHandler(dialog_controller_Cancelling);
                Application.ShowViewStrategy.ShowView(view_params, new ShowViewSource(Frame, null));
            }
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            NewObjectViewController new_controller = Frame.GetController<NewObjectViewController>();
            new_controller.ObjectCreating -= new EventHandler<ObjectCreatingEventArgs>(new_controller_ObjectCreating);
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void CreateAllocParameters_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace root_object_space = Application.CreateObjectSpace();
            try {
                HrmPeriodAllocParameter created_alloc_parameters = HrmPeriodAllocParameterLogic.createParameters(root_object_space);
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(root_object_space, created_alloc_parameters);
            }
            catch (OpenPeriodExistsException) {
                var dialog_controller = MessageBox.InitMessageBox(Application, e.ShowViewParameters, 
                    CaptionHelper.GetLocalizedText(MESSAGE_BOX_TEXT_PATH, "MessageBoxCaption"), 
                    CaptionHelper.GetLocalizedText(MESSAGE_BOX_TEXT_PATH, "MessageBoxText"));
                dialog_controller.Accepting += new EventHandler<DialogControllerAcceptingEventArgs>(dialog_controller_Accepting);
                dialog_controller.Cancelling += new EventHandler(dialog_controller_Cancelling);
            }
        }

        void dialog_controller_Cancelling(object sender, EventArgs e) {
        }

        void dialog_controller_Accepting(object sender, DialogControllerAcceptingEventArgs e) {
            IObjectSpace root_object_space = Application.CreateObjectSpace();
            HrmPeriodAllocParameter existing_alloc_parameters =
                HrmPeriodLogic.findLastPeriod(root_object_space).CurrentAllocParameter;
            e.ShowViewParameters.CreatedView = Application.CreateDetailView(root_object_space, existing_alloc_parameters);
        }

        private void AcceptAllocParameters_Execute(object sender, SimpleActionExecuteEventArgs e) {
//            IObjectSpace os = ObjectSpace;
            HrmPeriodAllocParameter alloc_parameters = e.CurrentObject as HrmPeriodAllocParameter;
            if (alloc_parameters != null && alloc_parameters.Status != HrmPeriodAllocParameterStatus.AllocParametersAccepted)
            {
             
                //string message = "";
                //string caption = "";
                //if (alloc_parameters.Status == HrmPeriodAllocParameterStatus.OpenToEdit) {
                //    caption = "”тверждение списка контролируемых заказов";
                //    message = "¬ы уверены что хотите утвердить список контролируемых заказов?";
                //}
                //if (alloc_parameters.Status == HrmPeriodAllocParameterStatus.ListOfOrderAccepted) {
                //    caption = "”тверждение параметров расчета";
                //    message = "¬ы уверены что хотите утвердить параметры расчета? ¬ случае подтверждени€ дальнейшее редактирование данных параметров расчета будет невозможно";
                //}
                //var result = MessageBox.Show(message, caption,
                //                             MessageBoxButtons.YesNo,
                //                             MessageBoxIcon.Question);
                //if (result == DialogResult.Yes) {

                ObjectSpace.CommitChanges();
                using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                    HrmPeriodAllocParameterLogic.acceptParameters(os, os.GetObject<HrmPeriodAllocParameter>(alloc_parameters));
                    os.CommitChanges();
                }
                ObjectSpace.CommitChanges();
                UpdateActionState(alloc_parameters);
                //}
            }
        }

        private void simpleAction1_Execute(object sender, SimpleActionExecuteEventArgs e) {

        }

        private void AcceptAllocParameters1_Execute(object sender, SimpleActionExecuteEventArgs e) {

        }


    }
}
