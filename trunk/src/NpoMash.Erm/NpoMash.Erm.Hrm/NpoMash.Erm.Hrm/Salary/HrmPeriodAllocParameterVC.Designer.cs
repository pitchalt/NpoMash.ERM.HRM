namespace NpoMash.Erm.Hrm.Salary
{
    partial class HrmPeriodAllocParameterVC
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.CreateAllocParameters = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.AcceptAllocParameters = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // CreateAllocParameters
            // 
            this.CreateAllocParameters.Caption = "Create Alloc Parameters";
            this.CreateAllocParameters.ConfirmationMessage = null;
            this.CreateAllocParameters.Id = "CreateAllocParameters";
            this.CreateAllocParameters.ImageName = null;
            this.CreateAllocParameters.Shortcut = null;
            this.CreateAllocParameters.Tag = null;
            this.CreateAllocParameters.TargetObjectsCriteria = null;
            this.CreateAllocParameters.TargetObjectType = typeof(NpoMash.Erm.Hrm.Salary.HrmPeriodAllocParameter);
            this.CreateAllocParameters.TargetViewId = null;
            this.CreateAllocParameters.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.CreateAllocParameters.ToolTip = null;
            this.CreateAllocParameters.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.CreateAllocParameters.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CreateAllocParameters_Execute);
            // 
            // AcceptAllocParameters
            // 
            this.AcceptAllocParameters.Caption = "Accept Alloc Parameters";
            this.AcceptAllocParameters.ConfirmationMessage = null;
            this.AcceptAllocParameters.Id = "AcceptAllocParameters";
            this.AcceptAllocParameters.ImageName = null;
            this.AcceptAllocParameters.Shortcut = null;
            this.AcceptAllocParameters.Tag = null;
            this.AcceptAllocParameters.TargetObjectsCriteria = null;
            this.AcceptAllocParameters.TargetObjectType = typeof(NpoMash.Erm.Hrm.Salary.HrmPeriodAllocParameter);
            this.AcceptAllocParameters.TargetViewId = null;
            this.AcceptAllocParameters.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.AcceptAllocParameters.ToolTip = null;
            this.AcceptAllocParameters.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            // 
            // HrmPeriodAllocParameterVC
            // 
            this.TargetObjectType = typeof(NpoMash.Erm.Hrm.Salary.HrmPeriodAllocParameter);
            this.TypeOfView = typeof(DevExpress.ExpressApp.View);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction CreateAllocParameters;
        private DevExpress.ExpressApp.Actions.SimpleAction AcceptAllocParameters;
    }
}
