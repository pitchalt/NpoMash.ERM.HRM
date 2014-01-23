namespace NpoMash.Erm.Hrm.Salary {
    partial class HrmSalaryTaskMatrixReductionVC {
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
            this.BringingMatrixInReduc = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.AcceptCoercedMatrix = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.ExportCoercedMatrix = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // BringingMatrixInReduc
            // 
            this.BringingMatrixInReduc.Caption = "Bringing TYPE_MATIX In Reduc";
            this.BringingMatrixInReduc.ConfirmationMessage = null;
            this.BringingMatrixInReduc.Id = "BringingMatrixInReduc";
            this.BringingMatrixInReduc.ImageName = null;
            choiceActionItem1.Caption = "1)Метод пропорции";
            choiceActionItem1.ImageName = null;
            choiceActionItem1.Shortcut = null;
            choiceActionItem1.ToolTip = null;
            choiceActionItem2.Caption = "2)Метод минимизаци числа отклонений";
            choiceActionItem2.ImageName = null;
            choiceActionItem2.Shortcut = null;
            choiceActionItem2.ToolTip = null;
            choiceActionItem3.Caption = "3)Метод минимизации максимальных отклонений";
            choiceActionItem3.ImageName = null;
            choiceActionItem3.Shortcut = null;
            choiceActionItem3.ToolTip = null;
            this.BringingMatrixInReduc.Items.Add(choiceActionItem1);
            this.BringingMatrixInReduc.Items.Add(choiceActionItem2);
            this.BringingMatrixInReduc.Items.Add(choiceActionItem3);
            this.BringingMatrixInReduc.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.BringingMatrixInReduc.Shortcut = null;
            this.BringingMatrixInReduc.ShowItemsOnClick = true;
            this.BringingMatrixInReduc.Tag = null;
            this.BringingMatrixInReduc.TargetObjectsCriteria = null;
            this.BringingMatrixInReduc.TargetObjectType = typeof(NpoMash.Erm.Hrm.Salary.HrmSalaryTaskMatrixReduction);
            this.BringingMatrixInReduc.TargetViewId = null;
            this.BringingMatrixInReduc.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.BringingMatrixInReduc.ToolTip = null;
            this.BringingMatrixInReduc.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.BringingMatrixInReduc.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.BringingMatrixInReduc_Execute);
            // 
            // AcceptCoercedMatrix
            // 
            this.AcceptCoercedMatrix.Caption = "Accept MATRIX_COERCED TYPE_MATIX";
            this.AcceptCoercedMatrix.ConfirmationMessage = null;
            this.AcceptCoercedMatrix.Id = "AcceptCoercedMatrix";
            this.AcceptCoercedMatrix.ImageName = null;
            choiceActionItem4.Caption = "AcceptMinimizeDeviationsMethod";
            choiceActionItem4.ImageName = null;
            choiceActionItem4.Shortcut = null;
            choiceActionItem4.ToolTip = null;
            choiceActionItem5.Caption = "AcceptMinimizeNumberOfDeviationsMethod";
            choiceActionItem5.ImageName = null;
            choiceActionItem5.Shortcut = null;
            choiceActionItem5.ToolTip = null;
            choiceActionItem6.Caption = "AcceptProportionsMethod";
            choiceActionItem6.ImageName = null;
            choiceActionItem6.Shortcut = null;
            choiceActionItem6.ToolTip = null;
            this.AcceptCoercedMatrix.Items.Add(choiceActionItem4);
            this.AcceptCoercedMatrix.Items.Add(choiceActionItem5);
            this.AcceptCoercedMatrix.Items.Add(choiceActionItem6);
            this.AcceptCoercedMatrix.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.AcceptCoercedMatrix.Shortcut = null;
            this.AcceptCoercedMatrix.ShowItemsOnClick = true;
            this.AcceptCoercedMatrix.Tag = null;
            this.AcceptCoercedMatrix.TargetObjectsCriteria = null;
            this.AcceptCoercedMatrix.TargetObjectType = typeof(NpoMash.Erm.Hrm.Salary.HrmSalaryTaskMatrixReduction);
            this.AcceptCoercedMatrix.TargetViewId = null;
            this.AcceptCoercedMatrix.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.AcceptCoercedMatrix.ToolTip = null;
            this.AcceptCoercedMatrix.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.AcceptCoercedMatrix.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.AcceptCoercedMatrix_Execute);
            // 
            // ExportCoercedMatrix
            // 
            this.ExportCoercedMatrix.Caption = "Export MATRIX_COERCED TYPE_MATIX";
            this.ExportCoercedMatrix.ConfirmationMessage = null;
            this.ExportCoercedMatrix.Id = "ExportCoercedMatrix";
            this.ExportCoercedMatrix.ImageName = null;
            this.ExportCoercedMatrix.Shortcut = null;
            this.ExportCoercedMatrix.Tag = null;
            this.ExportCoercedMatrix.TargetObjectsCriteria = null;
            this.ExportCoercedMatrix.TargetObjectType = typeof(NpoMash.Erm.Hrm.Salary.HrmSalaryTaskMatrixReduction);
            this.ExportCoercedMatrix.TargetViewId = null;
            this.ExportCoercedMatrix.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ExportCoercedMatrix.ToolTip = null;
            this.ExportCoercedMatrix.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ExportCoercedMatrix.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ExportCoercedMatrix_Execute);
            // 
            // HrmSalaryTaskMatrixReductionVC
            // 
            this.TargetObjectType = typeof(NpoMash.Erm.Hrm.Salary.HrmSalaryTaskMatrixReduction);
            this.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction BringingMatrixInReduc;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction AcceptCoercedMatrix;
        private DevExpress.ExpressApp.Actions.SimpleAction ExportCoercedMatrix;
    }
}
