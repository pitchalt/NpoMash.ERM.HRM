namespace NpoMash.Erm.Hrm.Salary {
    partial class HrmSalaryTaskProvisionMatrixReductionVC {
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
            this.BringProvisionMatrix = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.AcceptProvisionMatrix = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            // 
            // BringProvisionMatrix
            // 
            this.BringProvisionMatrix.Caption = "BringProvisionMatrix";
            this.BringProvisionMatrix.ConfirmationMessage = null;
            this.BringProvisionMatrix.Id = "BringProvisionMatrix";
            this.BringProvisionMatrix.ImageName = null;
            choiceActionItem1.Caption = "EkvilibristicMethod";
            choiceActionItem1.ImageName = null;
            choiceActionItem1.Shortcut = null;
            choiceActionItem1.ToolTip = null;
            choiceActionItem2.Caption = "SimplexMethod";
            choiceActionItem2.ImageName = null;
            choiceActionItem2.Shortcut = null;
            choiceActionItem2.ToolTip = null;
            this.BringProvisionMatrix.Items.Add(choiceActionItem1);
            this.BringProvisionMatrix.Items.Add(choiceActionItem2);
            this.BringProvisionMatrix.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.BringProvisionMatrix.Shortcut = null;
            this.BringProvisionMatrix.ShowItemsOnClick = true;
            this.BringProvisionMatrix.Tag = null;
            this.BringProvisionMatrix.TargetObjectsCriteria = null;
            this.BringProvisionMatrix.TargetObjectType = typeof(NpoMash.Erm.Hrm.HrmPeriod);
            this.BringProvisionMatrix.TargetViewId = null;
            this.BringProvisionMatrix.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.BringProvisionMatrix.ToolTip = null;
            this.BringProvisionMatrix.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.BringProvisionMatrix.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.BringProvisionMatrix_Execute);
            // 
            // AcceptProvisionMatrix
            // 
            this.AcceptProvisionMatrix.Caption = "Accept Provision Matrix";
            this.AcceptProvisionMatrix.ConfirmationMessage = null;
            this.AcceptProvisionMatrix.Id = "AcceptProvisionMatrix";
            this.AcceptProvisionMatrix.ImageName = null;
            choiceActionItem3.Caption = "EkvilibristicMethod";
            choiceActionItem3.ImageName = null;
            choiceActionItem3.Shortcut = null;
            choiceActionItem3.ToolTip = null;
            choiceActionItem4.Caption = "SimplexMethod";
            choiceActionItem4.ImageName = null;
            choiceActionItem4.Shortcut = null;
            choiceActionItem4.ToolTip = null;
            this.AcceptProvisionMatrix.Items.Add(choiceActionItem3);
            this.AcceptProvisionMatrix.Items.Add(choiceActionItem4);
            this.AcceptProvisionMatrix.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.AcceptProvisionMatrix.Shortcut = null;
            this.AcceptProvisionMatrix.ShowItemsOnClick = true;
            this.AcceptProvisionMatrix.Tag = null;
            this.AcceptProvisionMatrix.TargetObjectsCriteria = null;
            this.AcceptProvisionMatrix.TargetObjectType = typeof(NpoMash.Erm.Hrm.Salary.HrmSalaryTaskProvisionMatrixReduction);
            this.AcceptProvisionMatrix.TargetViewId = null;
            this.AcceptProvisionMatrix.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.AcceptProvisionMatrix.ToolTip = null;
            this.AcceptProvisionMatrix.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.AcceptProvisionMatrix.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.AcceptProvisionMatrix_Execute);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction BringProvisionMatrix;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction AcceptProvisionMatrix;
    }
}
