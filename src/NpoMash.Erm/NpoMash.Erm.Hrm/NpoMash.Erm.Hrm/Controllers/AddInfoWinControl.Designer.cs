namespace NpoMash.Erm.Hrm.Controllers
{
    partial class AddInfoWinControl
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
            this.AddDemoInfo = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // AddDemoInfo
            // 
            this.AddDemoInfo.Caption = "Add Demo Info";
            this.AddDemoInfo.Category = "Export";
            this.AddDemoInfo.ConfirmationMessage = null;
            this.AddDemoInfo.Id = "AddDemoInfo";
            this.AddDemoInfo.ImageName = null;
            this.AddDemoInfo.Shortcut = null;
            this.AddDemoInfo.Tag = null;
            this.AddDemoInfo.TargetObjectsCriteria = null;
            this.AddDemoInfo.TargetViewId = null;
            this.AddDemoInfo.ToolTip = null;
            this.AddDemoInfo.TypeOfView = null;
            this.AddDemoInfo.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AddDemoInfo_Execute);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction AddDemoInfo;
    }
}
