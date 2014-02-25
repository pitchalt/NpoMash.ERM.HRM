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
            this.AcceptCompare = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // AcceptCompare
            // 
            this.AcceptCompare.Caption = "HrmSalaryTaskCompareOZMAccountOperationVC_AcceptCompare";
            this.AcceptCompare.ConfirmationMessage = null;
            this.AcceptCompare.Id = "HrmSalaryTaskCompareOZMAccountOperationVC_AcceptCompare";
            this.AcceptCompare.ImageName = null;
            this.AcceptCompare.Shortcut = null;
            this.AcceptCompare.Tag = null;
            this.AcceptCompare.TargetObjectsCriteria = null;
            this.AcceptCompare.TargetObjectType = typeof(NpoMash.Erm.Hrm.Salary.HrmSalaryTaskCompareOZMAccountOperation);
            this.AcceptCompare.TargetViewId = null;
            this.AcceptCompare.ToolTip = null;
            this.AcceptCompare.TypeOfView = null;
            this.AcceptCompare.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AcceptCompare_Execute);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction AcceptCompare;
    }
}
