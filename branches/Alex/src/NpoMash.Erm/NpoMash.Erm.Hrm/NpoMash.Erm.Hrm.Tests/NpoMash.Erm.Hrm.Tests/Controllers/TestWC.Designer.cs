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
            this.AddReferenceData = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
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
            // AddReferenceData
            // 
            this.AddReferenceData.Caption = "TesterWC_AddReferenceData";
            this.AddReferenceData.Category = "Tools";
            this.AddReferenceData.ConfirmationMessage = null;
            this.AddReferenceData.Id = "TesterWC_AddReferenceData";
            this.AddReferenceData.ImageName = null;
            this.AddReferenceData.Shortcut = null;
            this.AddReferenceData.Tag = null;
            this.AddReferenceData.TargetObjectsCriteria = null;
            this.AddReferenceData.TargetViewId = null;
            this.AddReferenceData.ToolTip = null;
            this.AddReferenceData.TypeOfView = null;
            this.AddReferenceData.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AddReferenceData_Execute);

        }

        #endregion

        public DevExpress.ExpressApp.Actions.SimpleAction PopulateAction;
        private DevExpress.ExpressApp.Actions.SimpleAction AddReferenceData;

    }
}
