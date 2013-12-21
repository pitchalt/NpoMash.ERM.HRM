namespace NpoMash.Erm.Hrm.Salary {
    partial class HrmMatrixVC {
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
            this.BringingMatrix = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.GetDataSource = new DevExpress.ExpressApp.Actions.ParametrizedAction(this.components);
            // 
            // BringingMatrix
            // 
            this.BringingMatrix.Caption = "Bringing Matrix";
            this.BringingMatrix.ConfirmationMessage = null;
            this.BringingMatrix.Id = "BringingMatrix";
            this.BringingMatrix.ImageName = null;
            this.BringingMatrix.Shortcut = null;
            this.BringingMatrix.Tag = null;
            this.BringingMatrix.TargetObjectsCriteria = null;
            this.BringingMatrix.TargetObjectType = typeof(NpoMash.Erm.Hrm.HrmPeriod);
            this.BringingMatrix.TargetViewId = null;
            this.BringingMatrix.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.BringingMatrix.ToolTip = null;
            this.BringingMatrix.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            // 
            // GetDataSource
            // 
            this.GetDataSource.Caption = "GetDataSource";
            this.GetDataSource.ConfirmationMessage = null;
            this.GetDataSource.Id = "GetDataSource";
            this.GetDataSource.ImageName = null;
            this.GetDataSource.NullValuePrompt = null;
            this.GetDataSource.ShortCaption = null;
            this.GetDataSource.Shortcut = null;
            this.GetDataSource.Tag = null;
            this.GetDataSource.TargetObjectsCriteria = null;
            this.GetDataSource.TargetObjectType = typeof(NpoMash.Erm.Hrm.HrmPeriod);
            this.GetDataSource.TargetViewId = null;
            this.GetDataSource.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.GetDataSource.ToolTip = null;
            this.GetDataSource.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction BringingMatrix;
        private DevExpress.ExpressApp.Actions.ParametrizedAction GetDataSource;
    }
}
