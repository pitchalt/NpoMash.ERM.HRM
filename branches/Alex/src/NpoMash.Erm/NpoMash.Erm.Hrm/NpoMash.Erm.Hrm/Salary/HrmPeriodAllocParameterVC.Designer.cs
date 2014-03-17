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
            this.AcceptOrderListFirst = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.AcceptOrderListLast = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // AcceptOrderListFirst
            // 
            this.AcceptOrderListFirst.Caption = "AcceptOrderListFirst";
            this.AcceptOrderListFirst.ConfirmationMessage = null;
            this.AcceptOrderListFirst.Id = "AcceptOrderListFirst";
            this.AcceptOrderListFirst.ImageName = null;
            this.AcceptOrderListFirst.Shortcut = null;
            this.AcceptOrderListFirst.Tag = null;
            this.AcceptOrderListFirst.TargetObjectsCriteria = null;
            this.AcceptOrderListFirst.TargetObjectType = typeof(NpoMash.Erm.Hrm.Salary.HrmPeriodAllocParameter);
            this.AcceptOrderListFirst.TargetViewId = null;
            this.AcceptOrderListFirst.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.AcceptOrderListFirst.ToolTip = null;
            this.AcceptOrderListFirst.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.AcceptOrderListFirst.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AcceptOrderList_Execute);
            // 
            // AcceptOrderListLast
            // 
            this.AcceptOrderListLast.Caption = "Accept Order List Last";
            this.AcceptOrderListLast.ConfirmationMessage = null;
            this.AcceptOrderListLast.Id = "AcceptOrderListLast";
            this.AcceptOrderListLast.ImageName = null;
            this.AcceptOrderListLast.Shortcut = null;
            this.AcceptOrderListLast.Tag = null;
            this.AcceptOrderListLast.TargetObjectsCriteria = null;
            this.AcceptOrderListLast.TargetObjectType = typeof(NpoMash.Erm.Hrm.Salary.HrmPeriodAllocParameter);
            this.AcceptOrderListLast.TargetViewId = null;
            this.AcceptOrderListLast.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.AcceptOrderListLast.ToolTip = null;
            this.AcceptOrderListLast.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.AcceptOrderListLast.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AcceptOrderListLast_Execute);
            // 
            // HrmPeriodAllocParameterVC
            // 
            this.TargetObjectType = typeof(NpoMash.Erm.Hrm.Salary.HrmPeriodAllocParameter);
            this.TypeOfView = typeof(DevExpress.ExpressApp.View);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction AcceptOrderListFirst;
        private DevExpress.ExpressApp.Actions.SimpleAction AcceptOrderListLast;
    }
}
