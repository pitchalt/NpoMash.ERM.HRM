﻿namespace NpoMash.Erm.Hrm.Salary {
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
            this.BringingMatrixInReducAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.AcceptCoercedMatrixAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.ExportCoercedMatrix = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // BringingMatrixInReducAction
            // 
            this.BringingMatrixInReducAction.Caption = "Bringing TYPE_MATIX In Reduc";
            this.BringingMatrixInReducAction.ConfirmationMessage = null;
            this.BringingMatrixInReducAction.Id = "HrmSalaryTYaskMatrixReductionVC_BringingMatrixInReducAction";
            this.BringingMatrixInReducAction.ImageName = null;
            this.BringingMatrixInReducAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.BringingMatrixInReducAction.Shortcut = null;
            this.BringingMatrixInReducAction.ShowItemsOnClick = true;
            this.BringingMatrixInReducAction.Tag = null;
            this.BringingMatrixInReducAction.TargetObjectsCriteria = null;
            this.BringingMatrixInReducAction.TargetObjectType = typeof(NpoMash.Erm.Hrm.Salary.HrmSalaryTaskMatrixReduction);
            this.BringingMatrixInReducAction.TargetViewId = null;
            this.BringingMatrixInReducAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.BringingMatrixInReducAction.ToolTip = null;
            this.BringingMatrixInReducAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.BringingMatrixInReducAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.BringingMatrixInReduc_Execute);
            // 
            // AcceptCoercedMatrixAction
            // 
            this.AcceptCoercedMatrixAction.Caption = "Accept MATRIX_COERCED TYPE_MATIX";
            this.AcceptCoercedMatrixAction.ConfirmationMessage = null;
            this.AcceptCoercedMatrixAction.Id = "HrmSalaryTaskMatrixReductionVC_AcceptCoercedMatrixAction";
            this.AcceptCoercedMatrixAction.ImageName = null;
            this.AcceptCoercedMatrixAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.AcceptCoercedMatrixAction.Shortcut = null;
            this.AcceptCoercedMatrixAction.ShowItemsOnClick = true;
            this.AcceptCoercedMatrixAction.Tag = null;
            this.AcceptCoercedMatrixAction.TargetObjectsCriteria = null;
            this.AcceptCoercedMatrixAction.TargetObjectType = typeof(NpoMash.Erm.Hrm.Salary.HrmSalaryTaskMatrixReduction);
            this.AcceptCoercedMatrixAction.TargetViewId = null;
            this.AcceptCoercedMatrixAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.AcceptCoercedMatrixAction.ToolTip = null;
            this.AcceptCoercedMatrixAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.AcceptCoercedMatrixAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.AcceptCoercedMatrixAction_Execute);
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
            this.ExportCoercedMatrix.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ExportCoercedMatrixAction_Execute);
            // 
            // HrmSalaryTaskMatrixReductionVC
            // 
            this.TargetObjectType = typeof(NpoMash.Erm.Hrm.Salary.HrmSalaryTaskMatrixReduction);
            this.TypeOfView = typeof(DevExpress.ExpressApp.View);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction BringingMatrixInReducAction;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction AcceptCoercedMatrixAction;
        private DevExpress.ExpressApp.Actions.SimpleAction ExportCoercedMatrix;
    }
}
