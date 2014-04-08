namespace NpoMash.Erm.Hrm.Salary {
    partial class HrmSalaryTaskRevertVC {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.Revert = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // Revert
            // 
            this.Revert.Caption = "HrmSalaryTaskRevertVC_Revert";
            this.Revert.ConfirmationMessage = null;
            this.Revert.Id = "HrmSalaryTaskRevertVC_Revert";
            this.Revert.ImageName = null;
            this.Revert.Shortcut = null;
            this.Revert.Tag = null;
            this.Revert.TargetObjectsCriteria = null;
            this.Revert.TargetObjectType = typeof(NpoMash.Erm.Hrm.Salary.HrmSalaryTaskRevert);
            this.Revert.TargetViewId = null;
            this.Revert.ToolTip = null;
            this.Revert.TypeOfView = null;
            this.Revert.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Revert_Execute);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction Revert;
    }
}
