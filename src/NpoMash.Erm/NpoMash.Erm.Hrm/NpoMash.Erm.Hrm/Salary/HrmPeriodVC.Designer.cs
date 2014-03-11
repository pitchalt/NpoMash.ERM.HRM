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
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem12 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem13 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem14 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem15 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem16 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem17 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem18 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem19 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem20 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem21 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem22 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
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
            this.simpleAction1 = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // GetSourceDataAction
            // 
            this.GetSourceDataAction.Caption = "Get Source Data Action";
            this.GetSourceDataAction.Category = "Edit";
            this.GetSourceDataAction.ConfirmationMessage = null;
            this.GetSourceDataAction.Id = "GetSourceDataAction";
            this.GetSourceDataAction.ImageName = null;
            choiceActionItem12.Caption = "GenerateTestData";
            choiceActionItem12.ImageName = null;
            choiceActionItem12.Shortcut = null;
            choiceActionItem12.ToolTip = null;
            choiceActionItem13.Caption = "GetDataFromServer";
            choiceActionItem13.ImageName = null;
            choiceActionItem13.Shortcut = null;
            choiceActionItem13.ToolTip = null;
            choiceActionItem14.Caption = "GetDataFromFile";
            choiceActionItem14.ImageName = null;
            choiceActionItem15.Caption = "XmlFile";
            choiceActionItem15.ImageName = null;
            choiceActionItem15.Shortcut = null;
            choiceActionItem15.ToolTip = null;
            choiceActionItem16.Caption = "StructuredFile";
            choiceActionItem16.ImageName = null;
            choiceActionItem16.Shortcut = null;
            choiceActionItem16.ToolTip = null;
            choiceActionItem14.Items.Add(choiceActionItem15);
            choiceActionItem14.Items.Add(choiceActionItem16);
            choiceActionItem14.Shortcut = null;
            choiceActionItem14.ToolTip = null;
            this.GetSourceDataAction.Items.Add(choiceActionItem12);
            this.GetSourceDataAction.Items.Add(choiceActionItem13);
            this.GetSourceDataAction.Items.Add(choiceActionItem14);
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
            choiceActionItem17.Caption = "MinimizeDifferenceNumber";
            choiceActionItem17.ImageName = null;
            choiceActionItem17.Shortcut = null;
            choiceActionItem17.ToolTip = null;
            choiceActionItem18.Caption = "MinimizeMaxDifference";
            choiceActionItem18.ImageName = null;
            choiceActionItem18.Shortcut = null;
            choiceActionItem18.ToolTip = null;
            choiceActionItem19.Caption = "ProportionsMethodVariant";
            choiceActionItem19.ImageName = null;
            choiceActionItem19.Shortcut = null;
            choiceActionItem19.ToolTip = null;
            this.BringingKBMatrixAction.Items.Add(choiceActionItem17);
            this.BringingKBMatrixAction.Items.Add(choiceActionItem18);
            this.BringingKBMatrixAction.Items.Add(choiceActionItem19);
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
            this.BringingKBMatrixAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.BringingKBMatrixAction_Execute);
            // 
            // BringingOZMMatrixAction
            // 
            this.BringingOZMMatrixAction.Caption = "BringingOZMMatrixAction";
            this.BringingOZMMatrixAction.Category = "Edit";
            this.BringingOZMMatrixAction.ConfirmationMessage = null;
            this.BringingOZMMatrixAction.Id = "BringingOZMMatrixAction";
            this.BringingOZMMatrixAction.ImageName = null;
            choiceActionItem20.Caption = "MinimizeDifferenceNumber";
            choiceActionItem20.ImageName = null;
            choiceActionItem20.Shortcut = null;
            choiceActionItem20.ToolTip = null;
            choiceActionItem21.Caption = "MinimizeMaxDifference";
            choiceActionItem21.ImageName = null;
            choiceActionItem21.Shortcut = null;
            choiceActionItem21.ToolTip = null;
            choiceActionItem22.Caption = "ProportionsMethodVariant";
            choiceActionItem22.ImageName = null;
            choiceActionItem22.Shortcut = null;
            choiceActionItem22.ToolTip = null;
            this.BringingOZMMatrixAction.Items.Add(choiceActionItem20);
            this.BringingOZMMatrixAction.Items.Add(choiceActionItem21);
            this.BringingOZMMatrixAction.Items.Add(choiceActionItem22);
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
            // ExportBringingMatrix
            // 
            this.ExportBringingMatrix.Caption = "HrmPeriodVC_ExportBringingMatrix";
            this.ExportBringingMatrix.Category = "Edit";
            this.ExportBringingMatrix.ConfirmationMessage = null;
            this.ExportBringingMatrix.Id = "HrmPeriodVC_ExportBringingMatrix";
            this.ExportBringingMatrix.ImageName = null;
            this.ExportBringingMatrix.Shortcut = null;
            this.ExportBringingMatrix.Tag = null;
            this.ExportBringingMatrix.TargetObjectsCriteria = null;
            this.ExportBringingMatrix.TargetObjectType = typeof(NpoMash.Erm.Hrm.HrmPeriod);
            this.ExportBringingMatrix.TargetViewId = null;
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
            this.ImportAccountOperation.ImageName = null;
            this.ImportAccountOperation.Shortcut = null;
            this.ImportAccountOperation.Tag = null;
            this.ImportAccountOperation.TargetObjectsCriteria = null;
            this.ImportAccountOperation.TargetObjectType = typeof(NpoMash.Erm.Hrm.HrmPeriod);
            this.ImportAccountOperation.TargetViewId = null;
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
            this.ExportReserveMatrix.ImageName = null;
            this.ExportReserveMatrix.Shortcut = null;
            this.ExportReserveMatrix.Tag = null;
            this.ExportReserveMatrix.TargetObjectsCriteria = null;
            this.ExportReserveMatrix.TargetObjectType = typeof(NpoMash.Erm.Hrm.HrmPeriod);
            this.ExportReserveMatrix.TargetViewId = null;
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
            this.CreateReportKB.ImageName = null;
            this.CreateReportKB.Shortcut = null;
            this.CreateReportKB.Tag = null;
            this.CreateReportKB.TargetObjectsCriteria = null;
            this.CreateReportKB.TargetObjectType = typeof(NpoMash.Erm.Hrm.HrmPeriod);
            this.CreateReportKB.TargetViewId = null;
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
            this.CreateReportOZM.ImageName = null;
            this.CreateReportOZM.Shortcut = null;
            this.CreateReportOZM.Tag = null;
            this.CreateReportOZM.TargetObjectsCriteria = null;
            this.CreateReportOZM.TargetObjectType = typeof(NpoMash.Erm.Hrm.HrmPeriod);
            this.CreateReportOZM.TargetViewId = null;
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
            this.ImportAccountOperationLast.ImageName = null;
            this.ImportAccountOperationLast.Shortcut = null;
            this.ImportAccountOperationLast.Tag = null;
            this.ImportAccountOperationLast.TargetObjectsCriteria = null;
            this.ImportAccountOperationLast.TargetObjectType = typeof(NpoMash.Erm.Hrm.HrmPeriod);
            this.ImportAccountOperationLast.TargetViewId = null;
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
            this.CreateReportSummary.ImageName = null;
            this.CreateReportSummary.Shortcut = null;
            this.CreateReportSummary.Tag = null;
            this.CreateReportSummary.TargetObjectsCriteria = null;
            this.CreateReportSummary.TargetObjectType = typeof(NpoMash.Erm.Hrm.HrmPeriod);
            this.CreateReportSummary.TargetViewId = null;
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
            this.ClosePeriod.ImageName = null;
            this.ClosePeriod.Shortcut = null;
            this.ClosePeriod.Tag = null;
            this.ClosePeriod.TargetObjectsCriteria = null;
            this.ClosePeriod.TargetObjectType = typeof(NpoMash.Erm.Hrm.HrmPeriod);
            this.ClosePeriod.TargetViewId = null;
            this.ClosePeriod.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ClosePeriod.ToolTip = null;
            this.ClosePeriod.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ClosePeriod.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ClosePeriod_Execute);
            // 
            // simpleAction1
            // 
            this.simpleAction1.Caption = "Загрузить тестовую проводку";
            this.simpleAction1.ConfirmationMessage = null;
            this.simpleAction1.Id = "5dc904e2-0cbc-47bf-919c-aec072f21755";
            this.simpleAction1.ImageName = null;
            this.simpleAction1.Shortcut = null;
            this.simpleAction1.Tag = null;
            this.simpleAction1.TargetObjectsCriteria = null;
            this.simpleAction1.TargetViewId = null;
            this.simpleAction1.ToolTip = null;
            this.simpleAction1.TypeOfView = null;
            this.simpleAction1.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleAction1_Execute);
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
        private DevExpress.ExpressApp.Actions.SimpleAction simpleAction1;
    }
}
