namespace NpoMash.Erm.Hrm.Salary
{
    partial class AcceptAllocParameterVC
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
            this.Accept = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // Accept
            // 
            this.Accept.Caption = "Accept";
            this.Accept.ConfirmationMessage = null;
            this.Accept.Id = "Accept";
            this.Accept.ImageName = null;
            this.Accept.Shortcut = null;
            this.Accept.Tag = null;
            this.Accept.TargetObjectsCriteria = null;
            this.Accept.TargetObjectType = typeof(NpoMash.Erm.Hrm.Salary.HrmPeriodAllocParameter);
            this.Accept.TargetViewId = null;
            this.Accept.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.Accept.ToolTip = null;
            this.Accept.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.Accept.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Accept_Execute);
            // 
            // AcceptAllocParameterVC
            // 
            this.TargetObjectType = typeof(NpoMash.Erm.Hrm.Salary.HrmPeriodAllocParameter);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction Accept;
    }
}
