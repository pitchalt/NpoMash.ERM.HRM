using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.ExpressApp.DC;
//
using IntecoAG.ERM.HRM.Organization;
//
namespace NpoMash.Erm.Hrm {


    [DomainComponent]
    public interface IPeriod : ILogSupport {
        IList<IPeriodObject> PeriodObjects { get; }
    }

    //public enum HrmSalaryPeriodObjectStatus {
    //    TEST    = 0
    //}

    [DomainComponent]
    public interface IPeriodObject {
        String PeriodObjectStatus { get; }
        Type PeriodObjectType { get; }
        DepartmentGroupDep GroupDep { get; }
    }

}
