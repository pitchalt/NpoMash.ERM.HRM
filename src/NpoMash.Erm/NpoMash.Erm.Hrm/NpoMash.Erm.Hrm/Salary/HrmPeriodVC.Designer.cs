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
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem9 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem10 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem11 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            this.GetSourceDataAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.BringingKBMatrixAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.BringingOZMMatrixAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
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
            this.GetSourceDataAction.ShowItemsOnClick = true;
            this.GetSourceDataAction.Tag = null;
            this.GetSourceDataAction.TargetObjectsCriteria = null;
            this.GetSourceDataAction.TargetObjectType = typeof(NpoMash.Erm.Hrm.HrmPeriod);
            this.GetSourceDataAction.TargetViewId = null;
            this.GetSourceDataAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.GetSourceDataAction.ToolTip = null;
            this.GetSourceDataAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.GetSourceDataAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.GetSourceDataAction_Execute);
            // 
            // BringingKBMatrixAction
            // 
            this.BringingKBMatrixAction.Caption = "BringingMatrixAction";
            this.BringingKBMatrixAction.Category = "Edit";
            this.BringingKBMatrixAction.ConfirmationMessage = null;
            this.BringingKBMatrixAction.Id = "BringingMatrixAction";
            this.BringingKBMatrixAction.ImageName = null;
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
            this.BringingKBMatrixAction.Items.Add(choiceActionItem6);
            this.BringingKBMatrixAction.Items.Add(choiceActionItem7);
            this.BringingKBMatrixAction.Items.Add(choiceActionItem8);
            this.BringingKBMatrixAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.BringingKBMatrixAction.Shortcut = null;
            this.BringingKBMatrixAction.ShowItemsOnClick = true;
            this.BringingKBMatrixAction.Tag = null;
            this.BringingKBMatrixAction.TargetObjectsCriteria = null;
            this.BringingKBMatrixAction.TargetObjectType = typeof(NpoMash.Erm.Hrm.HrmPeriod);
            this.BringingKBMatrixAction.TargetViewId = null;
            this.BringingKBMatrixAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.BringingKBMatrixAction.ToolTip = null;
            this.BringingKBMatrixAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.BringingKBMatrixAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.BringingMatrixAction_Execute);
            // 
            // BringingOZMMatrixAction
            // 
            this.BringingOZMMatrixAction.Caption = "BringingOZMMatrixAction";
            this.BringingOZMMatrixAction.Category = "Edit";
            this.BringingOZMMatrixAction.ConfirmationMessage = null;
            this.BringingOZMMatrixAction.Id = "BringingOZMMatrixAction";
            this.BringingOZMMatrixAction.ImageName = null;
            choiceActionItem9.Caption = "ProportionsMethod";
            choiceActionItem9.ImageName = null;
            choiceActionItem9.Shortcut = null;
            choiceActionItem9.ToolTip = null;
            choiceActionItem10.Caption = "MinimizeDifferenceNumber";
            choiceActionItem10.ImageName = null;
            choiceActionItem10.Shortcut = null;
            choiceActionItem10.ToolTip = null;
            choiceActionItem11.Caption = "MinimizeMaxDifference";
            choiceActionItem11.ImageName = null;
            choiceActionItem11.Shortcut = null;
            choiceActionItem11.ToolTip = null;
            this.BringingOZMMatrixAction.Items.Add(choiceActionItem9);
            this.BringingOZMMatrixAction.Items.Add(choiceActionItem10);
            this.BringingOZMMatrixAction.Items.Add(choiceActionItem11);
            this.BringingOZMMatrixAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.BringingOZMMatrixAction.Shortcut = null;
            this.BringingOZMMatrixAction.ShowItemsOnClick = true;
            this.BringingOZMMatrixAction.Tag = null;
            this.BringingOZMMatrixAction.TargetObjectsCriteria = null;
            this.BringingOZMMatrixAction.TargetObjectType = typeof(NpoMash.Erm.Hrm.HrmPeriod);
            this.BringingOZMMatrixAction.TargetViewId = null;
            this.BringingOZMMatrixAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.BringingOZMMatrixAction.ToolTip = null;
            this.BringingOZMMatrixAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.BringingOZMMatrixAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.BringingOZMMatrixAction_Execute);
            // 
            // HrmPeriodVC
            // 
            this.TypeOfView = typeof(DevExpress.ExpressApp.View);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction GetSourceDataAction;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction BringingKBMatrixAction;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction BringingOZMMatrixAction;
    }
}
