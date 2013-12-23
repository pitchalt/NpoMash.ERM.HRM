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
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem11 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem12 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem13 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem14 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem15 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            this.BringingMatrix = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.GetSourceDataAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
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
            this.BringingMatrix.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.BringingMatrix_Execute);
            // 
            // GetSourceDataAction
            // 
            this.GetSourceDataAction.Caption = "Get Source Data Action";
            this.GetSourceDataAction.Category = "Edit";
            this.GetSourceDataAction.ConfirmationMessage = null;
            this.GetSourceDataAction.Id = "GetSourceDataAction";
            this.GetSourceDataAction.ImageName = null;
            choiceActionItem11.Caption = "GenerateTestData";
            choiceActionItem11.ImageName = null;
            choiceActionItem11.Shortcut = null;
            choiceActionItem11.ToolTip = null;
            choiceActionItem12.Caption = "GetDataFromServer";
            choiceActionItem12.ImageName = null;
            choiceActionItem12.Shortcut = null;
            choiceActionItem12.ToolTip = null;
            choiceActionItem13.Caption = "GetDataFromFile";
            choiceActionItem13.ImageName = null;
            choiceActionItem14.Caption = "XmlFile";
            choiceActionItem14.ImageName = null;
            choiceActionItem14.Shortcut = null;
            choiceActionItem14.ToolTip = null;
            choiceActionItem15.Caption = "StructuredFile";
            choiceActionItem15.ImageName = null;
            choiceActionItem15.Shortcut = null;
            choiceActionItem15.ToolTip = null;
            choiceActionItem13.Items.Add(choiceActionItem14);
            choiceActionItem13.Items.Add(choiceActionItem15);
            choiceActionItem13.Shortcut = null;
            choiceActionItem13.ToolTip = null;
            this.GetSourceDataAction.Items.Add(choiceActionItem11);
            this.GetSourceDataAction.Items.Add(choiceActionItem12);
            this.GetSourceDataAction.Items.Add(choiceActionItem13);
            this.GetSourceDataAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.GetSourceDataAction.Shortcut = null;
            this.GetSourceDataAction.Tag = null;
            this.GetSourceDataAction.TargetObjectsCriteria = null;
            this.GetSourceDataAction.TargetObjectType = typeof(NpoMash.Erm.Hrm.HrmPeriod);
            this.GetSourceDataAction.TargetViewId = null;
            this.GetSourceDataAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.GetSourceDataAction.ToolTip = null;
            this.GetSourceDataAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.GetSourceDataAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.GetSourceDataAction_Execute);
            // 
            // HrmMatrixVC
            // 
            this.TypeOfView = typeof(DevExpress.ExpressApp.View);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction BringingMatrix;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction GetSourceDataAction;
    }
}
