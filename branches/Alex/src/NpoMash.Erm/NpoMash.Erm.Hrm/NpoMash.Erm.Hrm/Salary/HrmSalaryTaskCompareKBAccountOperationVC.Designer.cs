namespace NpoMash.Erm.Hrm.Salary {
    partial class HrmSalaryTaskCompareKBAccountOperationVC {
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
            this.AcceptCompareKB = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // AcceptCompareKB
            // 
            this.AcceptCompareKB.Caption = "Accept Compare KB";
            this.AcceptCompareKB.ConfirmationMessage = null;
            this.AcceptCompareKB.Id = "AcceptCompareKB";
            this.AcceptCompareKB.ImageName = null;
            this.AcceptCompareKB.Shortcut = null;
            this.AcceptCompareKB.Tag = null;
            this.AcceptCompareKB.TargetObjectsCriteria = null;
            this.AcceptCompareKB.TargetObjectType = typeof(NpoMash.Erm.Hrm.Salary.HrmSalaryTaskCompareKBAccountOperation);
            this.AcceptCompareKB.TargetViewId = null;
            this.AcceptCompareKB.ToolTip = null;
            this.AcceptCompareKB.TypeOfView = null;
            this.AcceptCompareKB.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AcceptCompareKB_Execute);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction AcceptCompareKB;

    }
}
