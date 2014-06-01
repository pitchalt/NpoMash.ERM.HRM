namespace NpoMash.Erm.Hrm.Salary {
    partial class HrmSalaryTaskCompareWorkTimeVC {
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
            this.AcceptCompareKB = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.AcceptCompareOZM = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // AcceptCompareKB
            // 
            this.AcceptCompareKB.Caption = "AcceptCompareKB";
            this.AcceptCompareKB.ConfirmationMessage = null;
            this.AcceptCompareKB.Id = "AcceptCompareKB";
            this.AcceptCompareKB.ImageName = null;
            this.AcceptCompareKB.Shortcut = null;
            this.AcceptCompareKB.Tag = null;
            this.AcceptCompareKB.TargetObjectsCriteria = null;
            this.AcceptCompareKB.TargetObjectType = typeof(NpoMash.Erm.Hrm.Salary.HrmSalaryTaskCompareWorkTime);
            this.AcceptCompareKB.TargetViewId = null;
            this.AcceptCompareKB.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.AcceptCompareKB.ToolTip = null;
            this.AcceptCompareKB.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.AcceptCompareKB.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AcceptCompareKB_Execute);
            // 
            // AcceptCompareOZM
            // 
            this.AcceptCompareOZM.Caption = "AcceptCompareOZM";
            this.AcceptCompareOZM.ConfirmationMessage = null;
            this.AcceptCompareOZM.Id = "AcceptCompareOZM";
            this.AcceptCompareOZM.ImageName = null;
            this.AcceptCompareOZM.Shortcut = null;
            this.AcceptCompareOZM.Tag = null;
            this.AcceptCompareOZM.TargetObjectsCriteria = null;
            this.AcceptCompareOZM.TargetObjectType = typeof(NpoMash.Erm.Hrm.Salary.HrmSalaryTaskCompareWorkTime);
            this.AcceptCompareOZM.TargetViewId = null;
            this.AcceptCompareOZM.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.AcceptCompareOZM.ToolTip = null;
            this.AcceptCompareOZM.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.AcceptCompareOZM.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AcceptCompareOZM_Execute);
            // 
            // TaskKompareWorkTimeVC
            // 
            this.TargetObjectType = typeof(NpoMash.Erm.Hrm.Salary.HrmSalaryTaskCompareWorkTime);
            this.TypeOfView = typeof(DevExpress.ExpressApp.View);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction AcceptCompareKB;
        private DevExpress.ExpressApp.Actions.SimpleAction AcceptCompareOZM;
    }
}
