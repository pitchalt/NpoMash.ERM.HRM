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
            this.AcceptAllocParameters = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // AcceptAllocParameters
            // 
            this.AcceptAllocParameters.Caption = "HrmPeriodAllocParameterVC_AcceptAllocParameters";
            this.AcceptAllocParameters.Id = "HrmPeriodAllocParameterVC_AcceptAllocParameters";
            this.AcceptAllocParameters.ImageName = null;
            this.AcceptAllocParameters.Shortcut = null;
            this.AcceptAllocParameters.Tag = null;
            this.AcceptAllocParameters.TargetObjectsCriteria = null;
            this.AcceptAllocParameters.TargetObjectType = typeof(NpoMash.Erm.Hrm.Salary.HrmPeriodAllocParameter);
            this.AcceptAllocParameters.TargetViewId = null;
            this.AcceptAllocParameters.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.AcceptAllocParameters.ToolTip = null;
            this.AcceptAllocParameters.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.AcceptAllocParameters.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AcceptAllocParameters_Execute);
            // 
            // HrmPeriodAllocParameterVC
            // 
            this.TargetObjectType = typeof(NpoMash.Erm.Hrm.Salary.HrmPeriodAllocParameter);
            this.TypeOfView = typeof(DevExpress.ExpressApp.View);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction AcceptAllocParameters;
    }
}
