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
            this.OpenPeriodAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // OpenPeriodAction
            // 
            this.OpenPeriodAction.Caption = null;
            this.OpenPeriodAction.ConfirmationMessage = null;
            this.OpenPeriodAction.Id = "f3295b73-2a35-47b4-a01a-51e36a15b2af";
            this.OpenPeriodAction.ImageName = null;
            this.OpenPeriodAction.Shortcut = null;
            this.OpenPeriodAction.Tag = null;
            this.OpenPeriodAction.TargetObjectsCriteria = null;
            this.OpenPeriodAction.TargetViewId = null;
            this.OpenPeriodAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.OpenPeriodAction.ToolTip = null;
            this.OpenPeriodAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.OpenPeriodAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.OpenPeriodAction_Execute);
            // 
            // HrmPeriodVC
            // 
            this.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.ListView);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction OpenPeriodAction;
    }
}
