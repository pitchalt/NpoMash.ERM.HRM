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
using NpoMash.Erm.Hrm.Salary;


namespace NpoMash.Erm.Hrm.Tests.DatabaseUpdate {

    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }

        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            HrmPeriod first_period = ObjectSpace.FindObject<HrmPeriod>(CriteriaOperator.Parse("Year == '2014' && Month == '01'"));
            if (first_period == null) {
                first_period = HrmPeriodLogic.createPeriod(ObjectSpace);
                HrmPeriodAllocParameter first_alloc_parameters = ObjectSpace.CreateObject<HrmPeriodAllocParameter>();
                first_period.CurrentAllocParameter = first_alloc_parameters;
                first_period.AllocParameters.Add(first_period.CurrentAllocParameter);
                first_period.CurrentAllocParameter.NormNoControlKB = NpoMash.Erm.Hrm.Salary.HrmPeriodAllocParameterLogic.INIT_NORM_NO_CONTROL_KB;
                first_period.CurrentAllocParameter.NormNoControlOZM = NpoMash.Erm.Hrm.Salary.HrmPeriodAllocParameterLogic.INIT_NORM_NO_CONTROL_OZM;
                first_period.setStatus(HrmPeriodStatus.CLOSED);
                first_alloc_parameters.StatusSet(HrmPeriodAllocParameterStatus.ALLOC_PARAMETERS_ACCEPTED);
                first_period.Save();
                first_alloc_parameters.Save();
            }
        }
        public override void UpdateDatabaseBeforeUpdateSchema() { base.UpdateDatabaseBeforeUpdateSchema(); }
    }
}