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
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem12 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem13 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem14 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            this.GetSourceDataAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.BringingKBMatrixAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.BringingOZMMatrixAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.ExportBringingMatrix = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ImportAccountOperation = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ExportReserveMatrix = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.CreateReportKB = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.CreateReportOZM = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ImportAccountOperationLast = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.CreateReportSummary = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ClosePeriod = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.AccountOperationImport = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.BringProvisionMatrix = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.RevertState = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // GetSourceDataAction
            // 
            this.GetSourceDataAction.Caption = "Get Source Data Action";
            this.GetSourceDataAction.Category = "Edit";
            this.GetSourceDataAction.ConfirmationMessage = null;
            this.GetSourceDataAction.Id = "GetSourceDataAction";
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
            this.GetSourceDataAction.ShowItemsOnClick = true;
            this.GetSourceDataAction.TargetObjectType = typeof(NpoMash.Erm.Hrm.HrmPeriod);
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
            choiceActionItem6.Caption = "MinimizeDifferenceNumber";
            choiceActionItem6.ImageName = null;
            choiceActionItem6.Shortcut = null;
            choiceActionItem6.ToolTip = null;
            choiceActionItem7.Caption = "MinimizeMaxDifference";
            choiceActionItem7.ImageName = null;
            choiceActionItem7.Shortcut = null;
            choiceActionItem7.ToolTip = null;
            choiceActionItem8.Caption = "ProportionsMethodVariant";
            choiceActionItem8.ImageName = null;
            choiceActionItem8.Shortcut = null;
            choiceActionItem8.ToolTip = null;
            this.BringingKBMatrixAction.Items.Add(choiceActionItem6);
            this.BringingKBMatrixAction.Items.Add(choiceActionItem7);
            this.BringingKBMatrixAction.Items.Add(choiceActionItem8);
            this.BringingKBMatrixAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.BringingKBMatrixAction.ShowItemsOnClick = true;
            this.BringingKBMatrixAction.TargetObjectType = typeof(NpoMash.Erm.Hrm.HrmPeriod);
            this.BringingKBMatrixAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.BringingKBMatrixAction.ToolTip = null;
            this.BringingKBMatrixAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.BringingKBMatrixAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.BringingKBMatrixAction_Execute);
            // 
            // BringingOZMMatrixAction
            // 
            this.BringingOZMMatrixAction.Caption = "BringingOZMMatrixAction";
            this.BringingOZMMatrixAction.Category = "Edit";
            this.BringingOZMMatrixAction.ConfirmationMessage = null;
            this.BringingOZMMatrixAction.Id = "BringingOZMMatrixAction";
            choiceActionItem9.Caption = "MinimizeDifferenceNumber";
            choiceActionItem9.ImageName = null;
            choiceActionItem9.Shortcut = null;
            choiceActionItem9.ToolTip = null;
            choiceActionItem10.Caption = "MinimizeMaxDifference";
            choiceActionItem10.ImageName = null;
            choiceActionItem10.Shortcut = null;
            choiceActionItem10.ToolTip = null;
            choiceActionItem11.Caption = "ProportionsMethodVariant";
            choiceActionItem11.ImageName = null;
            choiceActionItem11.Shortcut = null;
            choiceActionItem11.ToolTip = null;
            this.BringingOZMMatrixAction.Items.Add(choiceActionItem9);
            this.BringingOZMMatrixAction.Items.Add(choiceActionItem10);
            this.BringingOZMMatrixAction.Items.Add(choiceActionItem11);
            this.BringingOZMMatrixAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.BringingOZMMatrixAction.ShowItemsOnClick = true;
            this.BringingOZMMatrixAction.TargetObjectType = typeof(NpoMash.Erm.Hrm.HrmPeriod);
            this.BringingOZMMatrixAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.BringingOZMMatrixAction.ToolTip = null;
            this.BringingOZMMatrixAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.BringingOZMMatrixAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.BringingOZMMatrixAction_Execute);
            // 
            // ExportBringingMatrix
            // 
            this.ExportBringingMatrix.Caption = "HrmPeriodVC_ExportBringingMatrix";
            this.ExportBringingMatrix.Category = "Edit";
            this.ExportBringingMatrix.ConfirmationMessage = null;
            this.ExportBringingMatrix.Id = "HrmPeriodVC_ExportBringingMatrix";
            this.ExportBringingMatrix.TargetObjectType = typeof(NpoMash.Erm.Hrm.HrmPeriod);
            this.ExportBringingMatrix.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ExportBringingMatrix.ToolTip = null;
            this.ExportBringingMatrix.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ExportBringingMatrix.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ExportBringingMatrix_Execute);
            // 
            // ImportAccountOperation
            // 
            this.ImportAccountOperation.Caption = "ImportAccountOperation";
            this.ImportAccountOperation.Category = "Edit";
            this.ImportAccountOperation.ConfirmationMessage = null;
            this.ImportAccountOperation.Id = "HrmPeriodVC_ImportAccountOperation";
            this.ImportAccountOperation.TargetObjectType = typeof(NpoMash.Erm.Hrm.HrmPeriod);
            this.ImportAccountOperation.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ImportAccountOperation.ToolTip = null;
            this.ImportAccountOperation.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ImportAccountOperation.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ImportAccountOperation_Execute);
            // 
            // ExportReserveMatrix
            // 
            this.ExportReserveMatrix.Caption = "Export Reserve Matrix";
            this.ExportReserveMatrix.ConfirmationMessage = null;
            this.ExportReserveMatrix.Id = "ExportReserveMatrix";
            this.ExportReserveMatrix.TargetObjectType = typeof(NpoMash.Erm.Hrm.HrmPeriod);
            this.ExportReserveMatrix.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ExportReserveMatrix.ToolTip = null;
            this.ExportReserveMatrix.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ExportReserveMatrix.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ExportReserveMatrix_Execute);
            // 
            // CreateReportKB
            // 
            this.CreateReportKB.Caption = "HrmPeriodVC_CreateReportKB";
            this.CreateReportKB.Category = "Edit";
            this.CreateReportKB.ConfirmationMessage = null;
            this.CreateReportKB.Id = "HrmPeriodVC_CreateReportKB";
            this.CreateReportKB.TargetObjectType = typeof(NpoMash.Erm.Hrm.HrmPeriod);
            this.CreateReportKB.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.CreateReportKB.ToolTip = null;
            this.CreateReportKB.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.CreateReportKB.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CreateReportKB_Execute);
            // 
            // CreateReportOZM
            // 
            this.CreateReportOZM.Caption = "HrmPeriodVC_CreateReportOZM";
            this.CreateReportOZM.Category = "Edit";
            this.CreateReportOZM.ConfirmationMessage = null;
            this.CreateReportOZM.Id = "HrmPeriodVC_CreateReportOZM";
            this.CreateReportOZM.TargetObjectType = typeof(NpoMash.Erm.Hrm.HrmPeriod);
            this.CreateReportOZM.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.CreateReportOZM.ToolTip = null;
            this.CreateReportOZM.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.CreateReportOZM.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CreateReportOZM_Execute);
            // 
            // ImportAccountOperationLast
            // 
            this.ImportAccountOperationLast.Caption = "HrmPeriodVC_ImportAccountOperationLast";
            this.ImportAccountOperationLast.Category = "Edit";
            this.ImportAccountOperationLast.ConfirmationMessage = null;
            this.ImportAccountOperationLast.Id = "HrmPeriodVC_ImportAccountOperationLast";
            this.ImportAccountOperationLast.TargetObjectType = typeof(NpoMash.Erm.Hrm.HrmPeriod);
            this.ImportAccountOperationLast.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ImportAccountOperationLast.ToolTip = null;
            this.ImportAccountOperationLast.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ImportAccountOperationLast.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ImportAccountOperationLast_Execute);
            // 
            // CreateReportSummary
            // 
            this.CreateReportSummary.Caption = "HrmPeriodVC_CreateReportSummary";
            this.CreateReportSummary.ConfirmationMessage = null;
            this.CreateReportSummary.Id = "HrmPeriodVC_CreateReportSummary";
            this.CreateReportSummary.TargetObjectType = typeof(NpoMash.Erm.Hrm.HrmPeriod);
            this.CreateReportSummary.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.CreateReportSummary.ToolTip = null;
            this.CreateReportSummary.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.CreateReportSummary.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CreateReportSummary_Execute);
            // 
            // ClosePeriod
            // 
            this.ClosePeriod.Caption = "HrmPeriodVC_ClosePeriod";
            this.ClosePeriod.ConfirmationMessage = null;
            this.ClosePeriod.Id = "HrmPeriodVC_ClosePeriod";
            this.ClosePeriod.TargetObjectType = typeof(NpoMash.Erm.Hrm.HrmPeriod);
            this.ClosePeriod.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ClosePeriod.ToolTip = null;
            this.ClosePeriod.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ClosePeriod.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ClosePeriod_Execute);
            // 
            // AccountOperationImport
            // 
            this.AccountOperationImport.Caption = "Account Operation Import";
            this.AccountOperationImport.ConfirmationMessage = null;
            this.AccountOperationImport.Id = "AccountOperationImport";
            choiceActionItem12.Caption = "GenerateTestData";
            choiceActionItem12.ImageName = null;
            choiceActionItem12.Shortcut = null;
            choiceActionItem12.ToolTip = null;
            choiceActionItem13.Caption = "FromFile";
            choiceActionItem13.ImageName = null;
            choiceActionItem13.Shortcut = null;
            choiceActionItem13.ToolTip = null;
            this.AccountOperationImport.Items.Add(choiceActionItem12);
            this.AccountOperationImport.Items.Add(choiceActionItem13);
            this.AccountOperationImport.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.AccountOperationImport.ShowItemsOnClick = true;
            this.AccountOperationImport.TargetObjectType = typeof(NpoMash.Erm.Hrm.HrmPeriod);
            this.AccountOperationImport.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.AccountOperationImport.ToolTip = null;
            this.AccountOperationImport.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.AccountOperationImport.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.AccountOperationImport_Execute);
            // 
            // BringProvisionMatrix
            // 
            this.BringProvisionMatrix.Caption = "BringProvisionMatrix";
            this.BringProvisionMatrix.ConfirmationMessage = null;
            this.BringProvisionMatrix.Id = "BringProvisionMatrix";
            choiceActionItem14.Caption = "Simplex";
            choiceActionItem14.ImageName = null;
            choiceActionItem14.Shortcut = null;
            choiceActionItem14.ToolTip = null;
            this.BringProvisionMatrix.Items.Add(choiceActionItem14);
            this.BringProvisionMatrix.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.BringProvisionMatrix.TargetObjectType = typeof(NpoMash.Erm.Hrm.HrmPeriod);
            this.BringProvisionMatrix.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.BringProvisionMatrix.ToolTip = null;
            this.BringProvisionMatrix.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.BringProvisionMatrix.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.BringProvisionMatrix_Execute);
            // 
            // RevertState
            // 
            this.RevertState.Caption = "HrmPeriodVC_RevertState";
            this.RevertState.Category = "Edit";
            this.RevertState.ConfirmationMessage = null;
            this.RevertState.Id = "HrmPeriodVC_RevertState";
            this.RevertState.TargetObjectType = typeof(NpoMash.Erm.Hrm.HrmPeriod);
            this.RevertState.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.RevertState.ToolTip = null;
            this.RevertState.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.RevertState.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.RevertState_Execute);
            // 
            // HrmPeriodVC
            // 
            this.TargetObjectType = typeof(NpoMash.Erm.Hrm.HrmPeriod);
            this.TypeOfView = typeof(DevExpress.ExpressApp.View);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction GetSourceDataAction;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction BringingKBMatrixAction;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction BringingOZMMatrixAction;
        private DevExpress.ExpressApp.Actions.SimpleAction ExportBringingMatrix;
        private DevExpress.ExpressApp.Actions.SimpleAction ImportAccountOperation;
        private DevExpress.ExpressApp.Actions.SimpleAction ExportReserveMatrix;
        private DevExpress.ExpressApp.Actions.SimpleAction CreateReportKB;
        private DevExpress.ExpressApp.Actions.SimpleAction CreateReportOZM;
        private DevExpress.ExpressApp.Actions.SimpleAction ImportAccountOperationLast;
        private DevExpress.ExpressApp.Actions.SimpleAction CreateReportSummary;
        private DevExpress.ExpressApp.Actions.SimpleAction ClosePeriod;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction AccountOperationImport;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction BringProvisionMatrix;
        private DevExpress.ExpressApp.Actions.SimpleAction RevertState;
    }
}
