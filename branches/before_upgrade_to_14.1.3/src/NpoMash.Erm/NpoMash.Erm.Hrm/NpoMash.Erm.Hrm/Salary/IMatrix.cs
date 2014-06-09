using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DevExpress.ExpressApp.DC;

namespace NpoMash.Erm.Hrm.Salary {

    [DomainComponent]
    public interface IMatrix: IPeriodObject, ITaskObject {
        String Name { get; }
    }

    [DomainComponent]
    public interface IMatrixSliced { 
    }
}
