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
using System.Windows.Forms;
//using IntecoAG.Erm.FM.Order;

namespace NpoMash.Erm.Hrm.Salary
{
    // For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppViewControllertopic.
    public partial class HrmPeriodAllocParameterVC : ViewController
    {
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
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void CreateAllocParameters_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace root_object_space = Application.CreateObjectSpace();
            try {
                HrmPeriodAllocParameter created_alloc_parameters = AllocParametersLogic.createParameters(root_object_space);
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(root_object_space, created_alloc_parameters);
            }
            catch (OpenPeriodExistsException) {
                HrmPeriodAllocParameter existing_alloc_parameters =
                    HrmPeriodLogic.findLastPeriod(root_object_space).CurrentAllocParameter;
                if (existing_alloc_parameters.Status == HrmPeriodAllocParameterStatus.AllocParametersAccepted) {
                    const string message = "Открыть карточку параметров периода для просмотра?";
                    const string caption = "Параметры расчета для данного периода уже утверждены.";
                    var result = MessageBox.Show(message, caption,
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Question);
                    if (result == DialogResult.Yes) {
                        e.ShowViewParameters.CreatedView = Application.CreateDetailView(root_object_space,
                            existing_alloc_parameters);
                    }
                }
                else {
                    const string message = "Хотите открыть параметры расчета текущего периода для редактирования?";
                    const string caption = "Параметры расчета для данного периода уже существуют.";
                    var result = MessageBox.Show(message, caption,
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Question);
                    if (result == DialogResult.Yes) {
                        e.ShowViewParameters.CreatedView = Application.CreateDetailView(root_object_space,
                            existing_alloc_parameters);
                    }
                }
            }
        }

        private void AcceptAllocParameters_Execute(object sender, SimpleActionExecuteEventArgs e) {
            HrmPeriodAllocParameter alloc_parameters = (HrmPeriodAllocParameter)e.CurrentObject;
            alloc_parameters.Status = HrmPeriodAllocParameterStatus.AllocParametersAccepted;
            ObjectSpace.CommitChanges();
        }
    }
}
