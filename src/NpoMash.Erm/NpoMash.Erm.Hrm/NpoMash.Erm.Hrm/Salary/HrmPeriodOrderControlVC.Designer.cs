namespace NpoMash.Erm.Hrm.Salary
{
    partial class HrmPeriodOrderControlVC
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.FillHrmPeriodOrderCOntrol = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // FillHrmPeriodOrderCOntrol
            // 
            this.FillHrmPeriodOrderCOntrol.Caption = "CreateAllocParameters";
            this.FillHrmPeriodOrderCOntrol.Category = "View";
            this.FillHrmPeriodOrderCOntrol.ConfirmationMessage = null;
            this.FillHrmPeriodOrderCOntrol.Id = "FillHrmPeriodOrderCOntrol";
            this.FillHrmPeriodOrderCOntrol.ImageName = null;
            this.FillHrmPeriodOrderCOntrol.Shortcut = null;
            this.FillHrmPeriodOrderCOntrol.Tag = null;
            this.FillHrmPeriodOrderCOntrol.TargetObjectsCriteria = null;
            this.FillHrmPeriodOrderCOntrol.TargetViewId = null;
            this.FillHrmPeriodOrderCOntrol.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.FillHrmPeriodOrderCOntrol.ToolTip = null;
            this.FillHrmPeriodOrderCOntrol.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.FillHrmPeriodOrderCOntrol.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.FillHrmPeriodOrderCOntrol_Execute);
            // 
            // HrmPeriodOrderControlVC
            // 
            this.TargetObjectType = typeof(NpoMash.Erm.Hrm.Salary.HrmPeriodOrderControl);
            this.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.ListView);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction FillHrmPeriodOrderCOntrol;
    }
}
