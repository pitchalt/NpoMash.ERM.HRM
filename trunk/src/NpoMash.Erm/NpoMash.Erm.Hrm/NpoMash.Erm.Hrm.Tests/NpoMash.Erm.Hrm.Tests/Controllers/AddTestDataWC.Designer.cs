namespace NpoMash.Erm.Hrm.Tests.Controllers
{
    partial class AddTestDataWC
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
            this.addNewData = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // addNewData
            // 
            this.addNewData.Caption = "Add Test Data";
            this.addNewData.ConfirmationMessage = null;
            this.addNewData.Id = "AddTestData";
            this.addNewData.ImageName = null;
            this.addNewData.Shortcut = null;
            this.addNewData.Tag = null;
            this.addNewData.TargetObjectsCriteria = null;
            this.addNewData.TargetViewId = null;
            this.addNewData.ToolTip = null;
            this.addNewData.TypeOfView = null;
            this.addNewData.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.addNewData_Execute);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction addNewData;
    }
}
