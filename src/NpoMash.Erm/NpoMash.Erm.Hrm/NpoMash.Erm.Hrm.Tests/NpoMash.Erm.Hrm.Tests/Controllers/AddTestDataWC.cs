using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
//
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Templates;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Model.NodeGenerators;
//
using NpoMash.Erm.Hrm.Salary;
using IntecoAG.Erm.HRM;
using IntecoAG.Erm.FM.Order;
using IntecoAG.Erm.HRM.Organization;

namespace NpoMash.Erm.Hrm.Tests.Controllers
{
    public partial class AddTestDataWC : WindowController
    {

        private void addNewData_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            #region Collections and objectSpace

            IObjectSpace objectSpace = Application.CreateObjectSpace();
            fmCOrder fmCorder = objectSpace.CreateObject<fmCOrder>();
            HrmSalaryPayType hrmSalaryPayType = objectSpace.CreateObject<HrmSalaryPayType>();

            var fmCorderCollection = objectSpace.GetObjects<fmCOrder>();
            var hrmSalaryPayTypeCollection = objectSpace.GetObjects<HrmSalaryPayType>();
            
            #endregion

            int i = 1;

            foreach (var each in fmCorderCollection)
            {
                
            }
            
        }

        public AddTestDataWC() { InitializeComponent(); RegisterActions(components); }
        protected override void OnActivated()
        { base.OnActivated(); }
        protected override void OnDeactivated()
        { base.OnDeactivated(); }

    }
}
