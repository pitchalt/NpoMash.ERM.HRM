namespace NpoMash.Erm.Hrm.Salary {
    partial class HrmSalaryTaskImportAccountOperationVC {
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
            DevExpress.ExpressApp.Actions.SimpleAction ImportAccountOperation;
            ImportAccountOperation = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ImportAccountOperation
            // 
            ImportAccountOperation.Caption = "Import Account Operation";
            ImportAccountOperation.ConfirmationMessage = null;
            ImportAccountOperation.Id = "ImportAccountOperation";
            ImportAccountOperation.ImageName = null;
            ImportAccountOperation.Shortcut = null;
            ImportAccountOperation.Tag = null;
            ImportAccountOperation.TargetObjectsCriteria = null;
            ImportAccountOperation.TargetObjectType = typeof(NpoMash.Erm.Hrm.HrmPeriod);
            ImportAccountOperation.TargetViewId = null;
            ImportAccountOperation.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            ImportAccountOperation.ToolTip = null;
            ImportAccountOperation.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            ImportAccountOperation.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleAction1_Execute);

        }

        #endregion

    }
}
