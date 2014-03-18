namespace NpoMash.Erm.Hrm.Salary {
    partial class HrmSalaryTaskExportProvisionMatrixVC {
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
            this.ExportProvisionMatrix = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ExportProvisionMatrix
            // 
            this.ExportProvisionMatrix.Caption = "HrmSalaryTaskExportProvisionMatrixVC_ExportProvisionMatrix";
            this.ExportProvisionMatrix.ConfirmationMessage = null;
            this.ExportProvisionMatrix.Id = "HrmSalaryTaskExportProvisionMatrixVC_ExportProvisionMatrix";
            this.ExportProvisionMatrix.ImageName = null;
            this.ExportProvisionMatrix.Shortcut = null;
            this.ExportProvisionMatrix.Tag = null;
            this.ExportProvisionMatrix.TargetObjectsCriteria = null;
            this.ExportProvisionMatrix.TargetObjectType = typeof(NpoMash.Erm.Hrm.Salary.HrmSalaryTaskExportProvisionMatrix);
            this.ExportProvisionMatrix.TargetViewId = null;
            this.ExportProvisionMatrix.ToolTip = null;
            this.ExportProvisionMatrix.TypeOfView = null;
            this.ExportProvisionMatrix.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ExportProvisionMatrix_Execute);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ExportProvisionMatrix;
    }
}
