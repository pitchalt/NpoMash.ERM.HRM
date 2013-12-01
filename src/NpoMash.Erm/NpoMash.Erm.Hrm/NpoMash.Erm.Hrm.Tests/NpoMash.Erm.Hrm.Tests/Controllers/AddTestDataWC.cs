using System;
using System.IO;
using System.Linq;
using System.Text;
using IntecoAG.Erm.HRM;
using IntecoAG.Erm.FM.Order;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Actions;
using IntecoAG.Erm.HRM.Organization;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Templates;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Model.NodeGenerators;
using NpoMash.Erm.Hrm.Salary;
namespace NpoMash.Erm.Hrm.Tests.Controllers
{
    public partial class AddTestDataWC : WindowController
    {
        public AddTestDataWC()
        { InitializeComponent(); RegisterActions(components); }
        protected override void OnActivated()
        { base.OnActivated(); }
        protected override void OnDeactivated()
        { base.OnDeactivated(); }

        private void addNewData_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            #region Collections and objectSpaces

            IObjectSpace objectSpace = Application.CreateObjectSpace();
            fmCOrder fmCorder = objectSpace.CreateObject<fmCOrder>();
            HrmSalaryPayType hrmSalaryPayType = objectSpace.CreateObject<HrmSalaryPayType>();
            Department department = objectSpace.CreateObject<Department>();

            var fmCorderCollection = objectSpace.GetObjects<fmCOrder>();
            var hrmSalaryPayTypeCollection = objectSpace.GetObjects<HrmSalaryPayType>();
            var departmentCollection = objectSpace.GetObjects<Department>();
            
            #endregion

          //  string line;
           // StreamReader streamReader = new StreamReader("Dep.dat");
           // while (!streamReader.EndOfStream)
          //  {
             //   line = streamReader.ReadLine();
          //  }
            //streamReader.Close();

           IObjectSpace os = objectSpace.CreateNestedObjectSpace();

           HrmPeriod period = os.CreateObject<HrmPeriod>();
           period.Month = 10;
           period.Year = 2012;       
            
            
            
            
               HrmPeriodAllocParameter period_parameters = objectSpace.CreateObject<HrmPeriodAllocParameter>();
               period.HrmPeriodAllocParameter = period_parameters;
               e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, period_parameters);
        }
    }
}
