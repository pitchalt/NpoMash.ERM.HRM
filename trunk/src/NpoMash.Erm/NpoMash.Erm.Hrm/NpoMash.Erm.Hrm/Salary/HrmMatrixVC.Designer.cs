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
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem1 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem2 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem3 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem4 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem5 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
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
            // 
            // GetSourceDataAction
            // 
            this.GetSourceDataAction.Caption = "Get Source Data Action";
            this.GetSourceDataAction.Category = "Edit";
            this.GetSourceDataAction.ConfirmationMessage = null;
            this.GetSourceDataAction.Id = "GetSourceDataAction";
            this.GetSourceDataAction.ImageName = null;
            choiceActionItem1.Caption = "GenerateTestData";
            choiceActionItem1.ImageName = null;
            choiceActionItem1.Shortcut = null;
            choiceActionItem1.ToolTip = null;
            choiceActionItem2.Caption = "GetDataFromServer";
            choiceActionItem2.ImageName = null;
            choiceActionItem2.Shortcut = null;
            choiceActionItem2.ToolTip = null;
            choiceActionItem3.Caption = "GetDataFromFile";
            choiceActionItem3.ImageName = null;
            choiceActionItem4.Caption = "XmlFile";
            choiceActionItem4.ImageName = null;
            choiceActionItem4.Shortcut = null;
            choiceActionItem4.ToolTip = null;
            choiceActionItem5.Caption = "StructuredFile";
            choiceActionItem5.ImageName = null;
            choiceActionItem5.Shortcut = null;
            choiceActionItem5.ToolTip = null;
            choiceActionItem3.Items.Add(choiceActionItem4);
            choiceActionItem3.Items.Add(choiceActionItem5);
            choiceActionItem3.Shortcut = null;
            choiceActionItem3.ToolTip = null;
            this.GetSourceDataAction.Items.Add(choiceActionItem1);
            this.GetSourceDataAction.Items.Add(choiceActionItem2);
            this.GetSourceDataAction.Items.Add(choiceActionItem3);
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

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction BringingMatrix;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction GetSourceDataAction;
    }
}
