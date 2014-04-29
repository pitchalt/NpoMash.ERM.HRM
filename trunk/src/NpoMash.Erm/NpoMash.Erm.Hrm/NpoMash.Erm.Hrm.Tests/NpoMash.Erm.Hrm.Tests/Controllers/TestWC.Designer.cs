namespace NpoMash.Erm.Hrm.Tests.Controllers {
    partial class TestWC {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose( bool disposing ) {
            if ( disposing && ( components != null ) ) { components.Dispose(); }
            base.Dispose( disposing );
        }

        #region Component Designer generated code

        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.PopulateAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.UpdateReferenceData = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
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
            // 
            // UpdateReferenceData
            // 
            this.UpdateReferenceData.Caption = "TesterWC_UpdateReferenceData";
            this.UpdateReferenceData.Category = "Tools";
            this.UpdateReferenceData.ConfirmationMessage = null;
            this.UpdateReferenceData.Id = "TesterWC_UpdateReferenceData";
            this.UpdateReferenceData.ImageName = null;
            this.UpdateReferenceData.Shortcut = null;
            this.UpdateReferenceData.Tag = null;
            this.UpdateReferenceData.TargetObjectsCriteria = null;
            this.UpdateReferenceData.TargetViewId = null;
            this.UpdateReferenceData.ToolTip = null;
            this.UpdateReferenceData.TypeOfView = null;
            this.UpdateReferenceData.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.UpdateReferenceData_Execute);

        }

        #endregion

        public DevExpress.ExpressApp.Actions.SimpleAction PopulateAction;
        private DevExpress.ExpressApp.Actions.SimpleAction UpdateReferenceData;

    }
}
