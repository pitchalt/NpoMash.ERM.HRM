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

namespace NpoMash.Erm.Hrm.Salary {
    // For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppViewControllertopic.
    public partial class HrmAllocParameterVC : ViewController {
        private const String MESSAGE_BOX_TEXT_PATH = @"Messages\HrmPariodAllocParameterVC";

        public HrmAllocParameterVC() {
            InitializeComponent();
            RegisterActions(components);
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }

        protected override void OnActivated() {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            NewObjectViewController new_controller = Frame.GetController<NewObjectViewController>();
            new_controller.ObjectCreating += new EventHandler<ObjectCreatingEventArgs>(new_controller_ObjectCreating);
            HrmAllocParameter param = View.CurrentObject as HrmAllocParameter;
            if (param != null)
                UpdateActionState(param);
        }

        protected void UpdateActionState(HrmAllocParameter param) {
            if (param.Status == HrmPeriodAllocParameterStatus.ALLOC_PARAMETERS_ACCEPTED)
                AcceptOrderListFirst.Active.SetItemValue(typeof(HrmAllocParameterVC).FullName, false);
            else
                AcceptOrderListFirst.Active.SetItemValue(typeof(HrmAllocParameterVC).FullName, true);
        }

        void new_controller_ObjectCreating(object sender, ObjectCreatingEventArgs e) {
            try {
                e.NewObject = HrmAllocParameterLogic.createParameters(e.ObjectSpace);
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
        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated() {
            NewObjectViewController new_controller = Frame.GetController<NewObjectViewController>();
            new_controller.ObjectCreating -= new EventHandler<ObjectCreatingEventArgs>(new_controller_ObjectCreating);
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void CreateAllocParameters_Execute(object sender, SimpleActionExecuteEventArgs e) {
            IObjectSpace root_object_space = Application.CreateObjectSpace();
            try {
                HrmAllocParameter created_alloc_parameters = HrmAllocParameterLogic.createParameters(root_object_space);
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
            HrmAllocParameter existing_alloc_parameters =
                HrmPeriodLogic.findLastPeriod(root_object_space).CurrentAllocParameter;
            e.ShowViewParameters.CreatedView = Application.CreateDetailView(root_object_space, existing_alloc_parameters);
        }



        private void AcceptOrderList_Execute(object sender, SimpleActionExecuteEventArgs e) {
            HrmAllocParameter alloc_parameters = e.CurrentObject as HrmAllocParameter;
            if (alloc_parameters != null && alloc_parameters.Status != HrmPeriodAllocParameterStatus.ALLOC_PARAMETERS_ACCEPTED) {
                ObjectSpace.CommitChanges();
                using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                    HrmAllocParameterLogic.acceptParameters(os, os.GetObject<HrmAllocParameter>(alloc_parameters));
                    os.CommitChanges();
                }
                ObjectSpace.CommitChanges();
                UpdateActionState(alloc_parameters);
                Window win = Frame as Window;
                if (win != null) win.Close();
            }
        }

        private void AcceptOrderListLast_Execute(object sender, SimpleActionExecuteEventArgs e) {
            HrmAllocParameter alloc_parameters = e.CurrentObject as HrmAllocParameter;
            if (alloc_parameters != null && alloc_parameters.Status != HrmPeriodAllocParameterStatus.ALLOC_PARAMETERS_ACCEPTED) {
                ObjectSpace.CommitChanges();
                using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                    HrmAllocParameterLogic.acceptParameters(os, os.GetObject<HrmAllocParameter>(alloc_parameters));
                    os.CommitChanges();
                }
                ObjectSpace.CommitChanges();
                UpdateActionState(alloc_parameters);
                Window win = Frame as Window;
                if (win != null) win.Close();
            }
        }
    }
}