using System;
using System.Linq;
//
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Security;
//
using NpoMash.Erm.Hrm;


namespace NpoMash.Erm.Hrm.Tests.DatabaseUpdate
{

    public class Updater : ModuleUpdater
    {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }


        public override void UpdateDatabaseAfterUpdateSchema()
        {
            base.UpdateDatabaseAfterUpdateSchema();
            HrmPeriod hp=ObjectSpace.FindObject<HrmPeriod>(CriteriaOperator.Parse( "Year == '2013' && Month == '10'"));
            if (hp == null)
            {
               hp = ObjectSpace.CreateObject<HrmPeriod>();
               hp.Year = Convert.ToInt16(DateTime.Now.Year);
               hp.Month = Convert.ToInt16(DateTime.Now.Month);
               hp.Save();
            }            
        }

        public override void UpdateDatabaseBeforeUpdateSchema()
        { base.UpdateDatabaseBeforeUpdateSchema(); }
    }
}
