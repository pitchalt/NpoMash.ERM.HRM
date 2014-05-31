using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DevExpress.ExpressApp.DC;

using IntecoAG.ERM.HRM.Organization;

namespace NpoMash.Erm.Hrm.Salary {

    [DomainComponent]
    public interface ITask: ILogSupport {
        IList<ITaskObject> InObjects { get; }
    }

    [DomainComponent]
    public interface ITaskObject {
        Type TaskObjectType { get; }
        DepartmentGroupDep GroupDep { get; }
        String TaskObjectName { get; }
    }

}
