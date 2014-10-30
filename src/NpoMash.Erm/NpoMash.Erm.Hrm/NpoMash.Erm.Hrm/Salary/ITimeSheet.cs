﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DevExpress.ExpressApp.DC;

namespace NpoMash.Erm.Hrm.Salary {

    [DomainComponent]
    public interface ITimeSheet : IPeriodObject, ITaskObject {
        String Name { get; }
    }

}
