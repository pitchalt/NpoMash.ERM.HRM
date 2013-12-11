namespace NpoMash.Erm.Hrm.Tests.Controllers {
    partial class TesterWC {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose( bool disposing ) {
            if ( disposing && ( components != null ) ) { components.Dispose(); }
            base.Dispose( disposing );
        }

        #region Component Designer generated code

        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.PopulateAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // PopulateAction
            // 
            this.PopulateAction.Caption = "TesterWC_PopulateAction";
            this.PopulateAction.Category = "Tools";
            this.PopulateAction.ConfirmationMessage = null;
            this.PopulateAction.Id = "TesterWC_PopulateAction";
            this.PopulateAction.ImageName = null;
            this.PopulateAction.Shortcut = null;
            this.PopulateAction.Tag = null;
            this.PopulateAction.TargetObjectsCriteria = null;
            this.PopulateAction.TargetViewId = null;
            this.PopulateAction.ToolTip = null;
            this.PopulateAction.TypeOfView = null;
            this.PopulateAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PopulateDB_Execute);

        }

        #endregion

        public DevExpress.ExpressApp.Actions.SimpleAction PopulateAction;

    }
}
