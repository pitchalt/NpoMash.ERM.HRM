namespace NpoMash.Erm.Hrm.Salary {
    partial class HrmSalaryTaskImportAccountOperationSummaryVC {
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
            this.AcceptImport = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // AcceptImport
            // 
            this.AcceptImport.Caption = "HrmSalaryTaskImportAccountOperationSummaryVC_AcceptImport";
            this.AcceptImport.ConfirmationMessage = null;
            this.AcceptImport.Id = "HrmSalaryTaskImportAccountOperationSummaryVC_AcceptImport";
            this.AcceptImport.ImageName = null;
            this.AcceptImport.Shortcut = null;
            this.AcceptImport.Tag = null;
            this.AcceptImport.TargetObjectsCriteria = null;
            this.AcceptImport.TargetObjectType = typeof(NpoMash.Erm.Hrm.Salary.HrmSalaryTaskImportAccountOperationSummary);
            this.AcceptImport.TargetViewId = null;
            this.AcceptImport.ToolTip = null;
            this.AcceptImport.TypeOfView = null;
            this.AcceptImport.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AcceptImport_Execute);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction AcceptImport;
    }
}
