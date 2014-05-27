namespace NpoMash.Erm.Hrm.Salary {
    partial class HrmSalaryTaskExportCoercedMatrixVC {
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
            this.ExportCoercedMatrix = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ExportCoercedMatrix
            // 
            this.ExportCoercedMatrix.Caption = "HrmSalaryTaskExportCoercedMatrixVC_ExportCoercedMatrix";
            this.ExportCoercedMatrix.ConfirmationMessage = null;
            this.ExportCoercedMatrix.Id = "HrmSalaryTaskExportCoercedMatrixVC_ExportCoercedMatrix";
            this.ExportCoercedMatrix.ImageName = null;
            this.ExportCoercedMatrix.Shortcut = null;
            this.ExportCoercedMatrix.Tag = null;
            this.ExportCoercedMatrix.TargetObjectsCriteria = null;
            this.ExportCoercedMatrix.TargetObjectType = typeof(NpoMash.Erm.Hrm.Salary.HrmSalaryTaskExportCoercedMatrix);
            this.ExportCoercedMatrix.TargetViewId = null;
            this.ExportCoercedMatrix.ToolTip = null;
            this.ExportCoercedMatrix.TypeOfView = null;
            this.ExportCoercedMatrix.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ExportCoercedMatrix_Execute);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ExportCoercedMatrix;
    }
}
