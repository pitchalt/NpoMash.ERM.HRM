namespace NpoMash.Erm.Hrm.Tests.Win
{
    partial class NpoErmHrmTestsWinApplication
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
            this.module1 = new DevExpress.ExpressApp.SystemModule.SystemModule();
            this.module2 = new DevExpress.ExpressApp.Win.SystemModule.SystemWindowsFormsModule();
            this.module3 = new NpoMash.Erm.Hrm.Tests.NpoErmHrmTestsModule();
            this.module4 = new NpoMash.Erm.Hrm.Tests.Win.NpoErmHrmTestsWinModule();
            this.sqlConnection1 = new System.Data.SqlClient.SqlConnection();
            this.iagErmModule1 = new IntecoAG.Erm.IagErmModule();
            this.iagXafExtModule1 = new IntecoAG.XafExt.IagXafExtModule();
            this.npoErmHrmModule1 = new NpoMash.Erm.Hrm.NpoErmHrmModule();
            this.npoErmHrmWinModule1 = new NpoMash.Erm.Hrm.Win.NpoErmHrmWinModule();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // sqlConnection1
            // 
            this.sqlConnection1.ConnectionString = "Integrated Security=SSPI;Pooling=false;Data Source=.\\SQLEXPRESS;Initial Catalog=N" +
    "poMash.Erm.Hrm.Tests";
            this.sqlConnection1.FireInfoMessageEventOnUserErrors = false;
            // 
            // NpoErmHrmTestsWinApplication
            // 
            this.ApplicationName = "NpoMash.Erm.Hrm.Tests";
            this.Connection = this.sqlConnection1;
            this.Modules.Add(this.module1);
            this.Modules.Add(this.module2);
            this.Modules.Add(this.iagErmModule1);
            this.Modules.Add(this.iagXafExtModule1);
            this.Modules.Add(this.npoErmHrmModule1);
            this.Modules.Add(this.module3);
            this.Modules.Add(this.npoErmHrmWinModule1);
            this.Modules.Add(this.module4);
            this.DatabaseVersionMismatch += new System.EventHandler<DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs>(this.TestsWindowsFormsApplication_DatabaseVersionMismatch);
            this.CustomizeLanguagesList += new System.EventHandler<DevExpress.ExpressApp.CustomizeLanguagesListEventArgs>(this.TestsWindowsFormsApplication_CustomizeLanguagesList);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.ExpressApp.SystemModule.SystemModule module1;
        private DevExpress.ExpressApp.Win.SystemModule.SystemWindowsFormsModule module2;
        private NpoMash.Erm.Hrm.Tests.NpoErmHrmTestsModule module3;
        private NpoMash.Erm.Hrm.Tests.Win.NpoErmHrmTestsWinModule module4;
        private System.Data.SqlClient.SqlConnection sqlConnection1;
        private IntecoAG.Erm.IagErmModule iagErmModule1;
        private IntecoAG.XafExt.IagXafExtModule iagXafExtModule1;
        private NpoErmHrmModule npoErmHrmModule1;
        private Hrm.Win.NpoErmHrmWinModule npoErmHrmWinModule1;
    }
}
