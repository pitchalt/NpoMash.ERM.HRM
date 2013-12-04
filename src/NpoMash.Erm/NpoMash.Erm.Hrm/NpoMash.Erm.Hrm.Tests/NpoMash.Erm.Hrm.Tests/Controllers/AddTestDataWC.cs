using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

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

using IntecoAG.Erm.HRM;
using IntecoAG.Erm.FM.Order;
using NpoMash.Erm.Hrm.Salary;
using IntecoAG.Erm.HRM.Organization;

namespace NpoMash.Erm.Hrm.Tests.Controllers
{
    public partial class AddTestDataWC : WindowController
    {

        private void addNewData_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            #region Constants

            int hrmPeriodCount = 10;
            Random random = new Random();

            #endregion

            #region Collections and objectSpace

            IObjectSpace objectSpace = Application.CreateObjectSpace();

            for (int i = 0; i < hrmPeriodCount; i++)
            {
                HrmPeriod hrmPeriod = objectSpace.CreateObject<HrmPeriod>();
            }

            /*
            for (int i = 0; i < hrmPeriodCount+4; i++)
            {
                HrmSalaryPayType hrmSalaryPayType = objectSpace.CreateObject<HrmSalaryPayType>();
            }
            */
            objectSpace.CommitChanges();

            var hrmPeriodCollection = objectSpace.GetObjects<HrmPeriod>();
            var hrmSalaryPayTypeCollection = objectSpace.GetObjects<HrmSalaryPayType>();
            var hrmPeriodPayTypeCollection = objectSpace.GetObjects<HrmPeriodPayType>();

            #endregion

            #region HrmPeriod Generation

            /*
            int salaryPayTypeCode = 10000;


            foreach (var a in hrmSalaryPayTypeCollection)
            {
                a.Code = salaryPayTypeCode.ToString();
                a.Name = salaryPayTypeCode.ToString();
                salaryPayTypeCode += 1;
            }
            */

            foreach (var each in hrmPeriodCollection)
            {
                each.Year = Convert.ToInt16(random.Next(1990, 2021));
                each.Month = Convert.ToInt16(random.Next(1, 13));
                if (random.Next(50) < 25)
                {
                    each.Status = HrmPeriodStatus.Opened;
                }
                else
                {
                    each.Status = HrmPeriodStatus.closed;
                }
                each.HrmPeriodAllocParameter = objectSpace.CreateObject<HrmPeriodAllocParameter>();
                each.HrmPeriodAllocParameter.HrmPeriod = each;
                /*
                foreach (var b in hrmSalaryPayTypeCollection)
                {
                    HrmPeriodAllocParameter eachParameter = each.HrmPeriodAllocParameter;
                    HrmPeriodPayType hrmPeriodPayType = objectSpace.CreateObject<HrmPeriodPayType>();
                    hrmPeriodPayType.PayType = b;
                    eachParameter.PeriodPayTypes.Add(hrmPeriodPayType);
                }
                */
            }
            objectSpace.CommitChanges();

            #endregion
        }

        public AddTestDataWC() { InitializeComponent(); RegisterActions(components); }
        protected override void OnActivated()
        { base.OnActivated(); }
        protected override void OnDeactivated()
        { base.OnDeactivated(); }

    }
}
