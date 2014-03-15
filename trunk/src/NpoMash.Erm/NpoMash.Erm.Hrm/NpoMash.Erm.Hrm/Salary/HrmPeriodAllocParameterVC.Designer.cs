namespace NpoMash.Erm.Hrm.Salary
{
    partial class HrmPeriodAllocParameterVC
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
            this.AcceptOrderList = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // AcceptOrderList
            // 
            this.AcceptOrderList.Caption = "AcceptOrderList";
            this.AcceptOrderList.ConfirmationMessage = null;
            this.AcceptOrderList.Id = "AcceptOrderList";
            this.AcceptOrderList.ImageName = null;
            this.AcceptOrderList.Shortcut = null;
            this.AcceptOrderList.Tag = null;
            this.AcceptOrderList.TargetObjectsCriteria = null;
            this.AcceptOrderList.TargetObjectType = typeof(NpoMash.Erm.Hrm.Salary.HrmPeriodAllocParameter);
            this.AcceptOrderList.TargetViewId = null;
            this.AcceptOrderList.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.AcceptOrderList.ToolTip = null;
            this.AcceptOrderList.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.AcceptOrderList.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AcceptOrderList_Execute);
            // 
            // HrmPeriodAllocParameterVC
            // 
            this.TargetObjectType = typeof(NpoMash.Erm.Hrm.Salary.HrmPeriodAllocParameter);
            this.TypeOfView = typeof(DevExpress.ExpressApp.View);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction AcceptOrderList;
    }
}
