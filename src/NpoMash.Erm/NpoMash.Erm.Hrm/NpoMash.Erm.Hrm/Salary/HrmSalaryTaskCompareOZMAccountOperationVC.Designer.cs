namespace NpoMash.Erm.Hrm.Salary {
    partial class HrmSalaryTaskCompareOZMAccountOperationVC {
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
            this.AcceptCompareOZM = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // AcceptCompareOZM
            // 
            this.AcceptCompareOZM.Caption = "Accept Compare OZM";
            this.AcceptCompareOZM.ConfirmationMessage = null;
            this.AcceptCompareOZM.Id = "AcceptCompareOZM";
            this.AcceptCompareOZM.ImageName = null;
            this.AcceptCompareOZM.Shortcut = null;
            this.AcceptCompareOZM.Tag = null;
            this.AcceptCompareOZM.TargetObjectsCriteria = null;
            this.AcceptCompareOZM.TargetObjectType = typeof(NpoMash.Erm.Hrm.Salary.HrmSalaryTaskCompareOZMAccountOperation);
            this.AcceptCompareOZM.TargetViewId = null;
            this.AcceptCompareOZM.ToolTip = null;
            this.AcceptCompareOZM.TypeOfView = null;
            this.AcceptCompareOZM.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AcceptCompareOZM_Execute);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction AcceptCompareOZM;

    }
}
