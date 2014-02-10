namespace NpoMash.Erm.Hrm.Salary {
    partial class HrmSalaryTaskProvisionMatrixReductionVC {
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
            this.BringingProvisionMatrix = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // BringingProvisionMatrix
            // 
            this.BringingProvisionMatrix.Caption = "Bringing Provision Matrix";
            this.BringingProvisionMatrix.ConfirmationMessage = null;
            this.BringingProvisionMatrix.Id = "BringingProvisionMatrix";
            this.BringingProvisionMatrix.ImageName = null;
            this.BringingProvisionMatrix.Shortcut = null;
            this.BringingProvisionMatrix.Tag = null;
            this.BringingProvisionMatrix.TargetObjectsCriteria = null;
            this.BringingProvisionMatrix.TargetObjectType = typeof(NpoMash.Erm.Hrm.HrmPeriod);
            this.BringingProvisionMatrix.TargetViewId = null;
            this.BringingProvisionMatrix.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.BringingProvisionMatrix.ToolTip = null;
            this.BringingProvisionMatrix.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.BringingProvisionMatrix.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.BringingProvisionMatrix_Execute);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction BringingProvisionMatrix;
    }
}
