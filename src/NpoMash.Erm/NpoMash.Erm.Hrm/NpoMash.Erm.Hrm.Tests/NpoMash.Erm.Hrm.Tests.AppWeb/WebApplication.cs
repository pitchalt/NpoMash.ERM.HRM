using System;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.Web;
using System.Collections.Generic;
//using DevExpress.ExpressApp.Security;

namespace NpoMash.Erm.Hrm.Tests.Web
{
    // You can override various virtual methods and handle corresponding events to manage various aspects of your XAF application UI and behavior.
    public partial class NpoErmHrmTestsWebApplication : WebApplication
    { // http://documentation.devexpress.com/#Xaf/DevExpressExpressAppWebWebApplicationMembersTopicAll
        private DevExpress.ExpressApp.SystemModule.SystemModule module1;
        private DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule module2;
        private NpoMash.Erm.Hrm.Tests.NpoErmHrmTestsModule module3;
        private NpoMash.Erm.Hrm.Tests.Web.NpoErmHrmTestsWebModule module4;
        private IntecoAG.Erm.IagErmModule iagErmModule1;
        private IntecoAG.XafExt.IagXafExtModule iagXafExtModule1;
        private NpoErmHrmModule npoErmHrmModule1;
        private System.Data.SqlClient.SqlConnection sqlConnection1;

        public NpoErmHrmTestsWebApplication()
        {
            InitializeComponent();
        }

        // Override to execute custom code after a logon has been performed, the SecuritySystem object is initialized, logon parameters have been saved and user model differences are loaded.
        protected override void OnLoggedOn(LogonEventArgs args)
        { // http://documentation.devexpress.com/#Xaf/DevExpressExpressAppXafApplication_LoggedOntopic
            base.OnLoggedOn(args);
        }

        // Override to execute custom code after a user has logged off.
        protected override void OnLoggedOff()
        { // http://documentation.devexpress.com/#Xaf/DevExpressExpressAppXafApplication_LoggedOfftopic
            base.OnLoggedOff();
        }

        protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args)
        {
            args.ObjectSpaceProvider = new XPObjectSpaceProviderThreadSafe(args.ConnectionString, args.Connection);
        }

        private void TestsAspNetApplication_DatabaseVersionMismatch(object sender, DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs e)
        {
#if EASYTEST
			e.Updater.Update();
			e.Handled = true;
#else
            if (System.Diagnostics.Debugger.IsAttached)
            {
                e.Updater.Update();
                e.Handled = true;
            }
            else
            {
                string message = "The application cannot connect to the specified database, because the latter doesn't exist or its version is older than that of the application.\r\n" +
                    "This error occurred  because the automatic database update was disabled when the application was started without debugging.\r\n" +
                    "To avoid this error, you should either start the application under Visual Studio in debug mode, or modify the " +
                    "source code of the 'DatabaseVersionMismatch' event handler to enable automatic database update, " +
                    "or manually create a database using the 'DBUpdater' tool.\r\n" +
                    "Anyway, refer to the following help topics for more detailed information:\r\n" +
                    "'Update Application and Database Versions' at http://www.devexpress.com/Help/?document=ExpressApp/CustomDocument2795.htm\r\n" +
                    "'Database Security References' at http://www.devexpress.com/Help/?document=ExpressApp/CustomDocument3237.htm\r\n" +
                    "If this doesn't help, please contact our Support Team at http://www.devexpress.com/Support/Center/";

                if (e.CompatibilityError != null && e.CompatibilityError.Exception != null)
                {
                    message += "\r\n\r\nInner exception: " + e.CompatibilityError.Exception.Message;
                }
                throw new InvalidOperationException(message);
            }
#endif
        }

        private void InitializeComponent()
        {
            this.module1 = new DevExpress.ExpressApp.SystemModule.SystemModule();
            this.module2 = new DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule();
            this.module3 = new NpoMash.Erm.Hrm.Tests.NpoErmHrmTestsModule();
            this.module4 = new NpoMash.Erm.Hrm.Tests.Web.NpoErmHrmTestsWebModule();
            this.sqlConnection1 = new System.Data.SqlClient.SqlConnection();
            this.iagErmModule1 = new IntecoAG.Erm.IagErmModule();
            this.iagXafExtModule1 = new IntecoAG.XafExt.IagXafExtModule();
            this.npoErmHrmModule1 = new NpoMash.Erm.Hrm.NpoErmHrmModule();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // sqlConnection1
            // 
            this.sqlConnection1.ConnectionString = "Integrated Security=SSPI;Pooling=false;Data Source=.\\SQLEXPRESS;Initial Catalog=N" +
    "poMash.Erm.Hrm.Tests";
            this.sqlConnection1.FireInfoMessageEventOnUserErrors = false;
            // 
            // NpoErmHrmTestsWebApplication
            // 
            this.ApplicationName = "NpoMash.Erm.Hrm.Tests";
            this.Connection = this.sqlConnection1;
            this.Modules.Add(this.module1);
            this.Modules.Add(this.module2);
            this.Modules.Add(this.iagErmModule1);
            this.Modules.Add(this.iagXafExtModule1);
            this.Modules.Add(this.npoErmHrmModule1);
            this.Modules.Add(this.module3);
            this.Modules.Add(this.module4);
            this.DatabaseVersionMismatch += new System.EventHandler<DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs>(this.TestsAspNetApplication_DatabaseVersionMismatch);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
    }
}
