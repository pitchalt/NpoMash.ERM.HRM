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

            int collectionCount = 0;
            int startDate = 2013;
            int startMonth = 0;
            int hrmPeriodCount = 2;
            int fmCOrderCount = 10;
            int hrmPeriodPayTypeCount = 10;
            HrmPeriod tmpHrmPeriod = null;
            Random random = new Random();

            #endregion

            #region Collections and objectSpace

            IObjectSpace objectSpace = Application.CreateObjectSpace();

            for (int i = 0; i < hrmPeriodCount; i++)
            {
                HrmPeriod hrmPeriod = objectSpace.CreateObject<HrmPeriod>();
            }

            for (int i = 0; i < fmCOrderCount; i++)
            {
                fmCOrder fmCorder = objectSpace.CreateObject<fmCOrder>();
            }

            for (int i = 0; i < hrmPeriodPayTypeCount; i++)
            {
                HrmPeriodPayType hrmPeriodPayType = objectSpace.CreateObject<HrmPeriodPayType>();
            }

            var hrmPeriodCollection = objectSpace.GetObjects<HrmPeriod>(null,true);
            var fmCorderCollection = objectSpace.GetObjects<fmCOrder>(null, true);
            var hrmPeriodPayTypeColection = objectSpace.GetObjects<HrmPeriodPayType>(null, true);

            #endregion

            #region Data Generation

            foreach (var each in hrmPeriodCollection)
            {
                collectionCount += 1;
                each.Year = Convert.ToInt16(startDate);
                each.Month = Convert.ToInt16(startMonth);
                each.addMonth();
                startDate = each.Year;
                startMonth = each.Month;
                if (collectionCount == hrmPeriodCollection.Count())
                {
                    each.Status = HrmPeriodStatus.Opened;
                }
                else
                {
                    each.Status = HrmPeriodStatus.closed;
                }
                each.HrmPeriodAllocParameter = objectSpace.CreateObject<HrmPeriodAllocParameter>();
                each.HrmPeriodAllocParameter.HrmPeriod = each;
                if (collectionCount == 1)
                {
                    tmpHrmPeriod = each;
                    each.PeriodPrevious = each;
                }
                else
                {
                    each.PeriodPrevious = tmpHrmPeriod;
                    tmpHrmPeriod = each;
                }

                foreach (var order in fmCorderCollection)
                {
                    order.Code = random.Next(1000, 5000).ToString();
                    order.NormKB = random.Next(10000, 20000);
                    order.NormNoControl = random.Next(10000, 20000);
                    order.NormOZM = random.Next(10000, 20000);
                    int rnd = random.Next(99);
                    if (rnd < 33)
                    {
                        order.TypeControl = fmCOrderTypeCOntrol.FOT;
                        order.TypeConstancy = fmCOrdertypeConstancy.One;
                        HrmPeriodOrderControl hrmPeriodOrderControl = objectSpace.CreateObject<HrmPeriodOrderControl>();
                        hrmPeriodOrderControl.Order = order;
                        hrmPeriodOrderControl.TypeControl = HrmPeriodOrderTypeControl.FOT;
                        hrmPeriodOrderControl.NormKB = order.NormKB;
                        hrmPeriodOrderControl.NormNoControl = order.NormNoControl;
                        hrmPeriodOrderControl.NormOZM = order.NormOZM;
                        hrmPeriodOrderControl.AllocParameter = each.HrmPeriodAllocParameter;
                    }
                    if ((rnd >= 33) && (rnd < 66))
                    {
                        order.TypeControl = fmCOrderTypeCOntrol.No_Ordered;
                        order.TypeConstancy = fmCOrdertypeConstancy.One;
                        HrmPeriodOrderControl hrmPeriodOrderControl = objectSpace.CreateObject<HrmPeriodOrderControl>();
                        hrmPeriodOrderControl.Order = order;
                        hrmPeriodOrderControl.TypeControl = HrmPeriodOrderTypeControl.No_Ordered;
                        hrmPeriodOrderControl.NormKB = order.NormKB;
                        hrmPeriodOrderControl.NormNoControl = order.NormNoControl;
                        hrmPeriodOrderControl.NormOZM = order.NormOZM;
                        hrmPeriodOrderControl.AllocParameter = each.HrmPeriodAllocParameter;
                    }
                    if (rnd >= 66)
                    {
                        order.TypeControl = fmCOrderTypeCOntrol.TrudEmk_FOT;
                        order.TypeConstancy = fmCOrdertypeConstancy.One;
                        HrmPeriodOrderControl hrmPeriodOrderControl = objectSpace.CreateObject<HrmPeriodOrderControl>();
                        hrmPeriodOrderControl.Order = order;
                        hrmPeriodOrderControl.TypeControl = HrmPeriodOrderTypeControl.TrudEmk_FOT;
                        hrmPeriodOrderControl.NormKB = order.NormKB;
                        hrmPeriodOrderControl.NormNoControl = order.NormNoControl;
                        hrmPeriodOrderControl.NormOZM = order.NormOZM;
                        hrmPeriodOrderControl.AllocParameter = each.HrmPeriodAllocParameter;
                    }
                        
                 }

                 foreach (var periodPayType in hrmPeriodPayTypeColection)
                 {
                    HrmSalaryPayType hrmSalaryPayType = objectSpace.CreateObject<HrmSalaryPayType>();
                    hrmSalaryPayType.Code = random.Next(100000).ToString();
                    hrmSalaryPayType.Name = random.Next(10000000).ToString();
                    periodPayType.PayType = hrmSalaryPayType;
                    periodPayType.AllocParameter = each.HrmPeriodAllocParameter;
                 }
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
