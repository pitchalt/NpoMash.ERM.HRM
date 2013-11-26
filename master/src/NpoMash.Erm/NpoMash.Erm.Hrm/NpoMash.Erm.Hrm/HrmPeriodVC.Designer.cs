namespace NpoMash.Erm.Hrm
{
    partial class HrmPeriodVC
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
            this.CreatePeriod = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // CreatePeriod
            // 
            this.CreatePeriod.Caption = "CreatePeriod";
            this.CreatePeriod.ConfirmationMessage = null;
            this.CreatePeriod.Id = "OpenPeriodAction";
            this.CreatePeriod.ImageName = null;
            this.CreatePeriod.Shortcut = null;
            this.CreatePeriod.Tag = null;
            this.CreatePeriod.TargetObjectsCriteria = null;
            this.CreatePeriod.TargetViewId = null;
            this.CreatePeriod.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.CreatePeriod.ToolTip = null;
            this.CreatePeriod.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.CreatePeriod.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.OpenPeriodAction_Execute);
            // 
            // HrmPeriodVC
            // 
            this.TargetObjectType = typeof(NpoMash.Erm.Hrm.HrmPeriod);
            this.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.ListView);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction CreatePeriod;
    }
}
