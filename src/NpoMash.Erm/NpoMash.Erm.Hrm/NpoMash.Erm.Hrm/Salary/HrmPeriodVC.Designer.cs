namespace NpoMash.Erm.Hrm.Salary {
    partial class HrmPeriodVC {
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
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem6 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem7 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem8 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            this.GetSourceDataAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.BringingMatrixAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
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
            // 
            // BringingMatrixAction
            // 
            this.BringingMatrixAction.Caption = "BringingMatrixAction";
            this.BringingMatrixAction.Category = "Edit";
            this.BringingMatrixAction.ConfirmationMessage = null;
            this.BringingMatrixAction.Id = "BringingMatrixAction";
            this.BringingMatrixAction.ImageName = null;
            choiceActionItem6.Caption = "ProportionsMethod";
            choiceActionItem6.ImageName = null;
            choiceActionItem6.Shortcut = null;
            choiceActionItem6.ToolTip = null;
            choiceActionItem7.Caption = "MinimizeDifferenceNumber";
            choiceActionItem7.ImageName = null;
            choiceActionItem7.Shortcut = null;
            choiceActionItem7.ToolTip = null;
            choiceActionItem8.Caption = "MinimizeMaxDifference";
            choiceActionItem8.ImageName = null;
            choiceActionItem8.Shortcut = null;
            choiceActionItem8.ToolTip = null;
            this.BringingMatrixAction.Items.Add(choiceActionItem6);
            this.BringingMatrixAction.Items.Add(choiceActionItem7);
            this.BringingMatrixAction.Items.Add(choiceActionItem8);
            this.BringingMatrixAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.BringingMatrixAction.Shortcut = null;
            this.BringingMatrixAction.ShowItemsOnClick = true;
            this.BringingMatrixAction.Tag = null;
            this.BringingMatrixAction.TargetObjectsCriteria = null;
            this.BringingMatrixAction.TargetObjectType = typeof(NpoMash.Erm.Hrm.HrmPeriod);
            this.BringingMatrixAction.TargetViewId = null;
            this.BringingMatrixAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.BringingMatrixAction.ToolTip = null;
            this.BringingMatrixAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.BringingMatrixAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.BringingMatrixAction_Execute);
            // 
            // HrmMatrixVC
            // 
            this.TypeOfView = typeof(DevExpress.ExpressApp.View);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction GetSourceDataAction;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction BringingMatrixAction;
    }
}
