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
            this.AcceptProvisionMatrix = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            // 
            // AcceptProvisionMatrix
            // 
            this.AcceptProvisionMatrix.Caption = "Accept Provision Matrix";
            this.AcceptProvisionMatrix.ConfirmationMessage = null;
            this.AcceptProvisionMatrix.Id = "AcceptProvisionMatrix";
            this.AcceptProvisionMatrix.ImageName = null;
            choiceActionItem1.Caption = "EkvilibristicMethod";
            choiceActionItem1.ImageName = null;
            choiceActionItem1.Shortcut = null;
            choiceActionItem1.ToolTip = null;
            choiceActionItem2.Caption = "SimplexMethod";
            choiceActionItem2.ImageName = null;
            choiceActionItem2.Shortcut = null;
            choiceActionItem2.ToolTip = null;
            this.AcceptProvisionMatrix.Items.Add(choiceActionItem1);
            this.AcceptProvisionMatrix.Items.Add(choiceActionItem2);
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

        private DevExpress.ExpressApp.Actions.SingleChoiceAction AcceptProvisionMatrix;
    }
}
