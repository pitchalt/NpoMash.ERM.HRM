namespace IntecoAG.XafExt.Tests.Web
{
    partial class IagXafExtTestsWebModule
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
            // 
            // IagXafExtTestsWebModule
            // 
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule));
            this.RequiredModuleTypes.Add(typeof(IntecoAG.XafExt.Web.IagXafExtWebModule));
            this.RequiredModuleTypes.Add(typeof(IntecoAG.XafExt.Tests.IagXafExtTestsModule));

        }

        #endregion
    }
}
