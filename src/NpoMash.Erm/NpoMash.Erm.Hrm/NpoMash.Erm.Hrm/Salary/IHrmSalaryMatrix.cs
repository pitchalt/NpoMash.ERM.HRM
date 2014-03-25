using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.HRM.Organization;

namespace NpoMash.Erm.Hrm.Salary {

    public interface IHrmSalaryMatrix {
        IList<IHrmSalaryMatrixRow> Rows { get; }
        IList<IHrmSalaryMatrixColumn> Columns { get; }
    }

    public interface IHrmSalaryMatrixRow {
        IHrmSalaryMatrix Matrix { get; }
        IList<IHrmSalaryMatrixCell> Cells { get; }

        fmCOrder Order { get; }
    }

    public interface IHrmSalaryMatrixColumn {
        IHrmSalaryMatrix Matrix { get; }
        IList<IHrmSalaryMatrixCell> Cells { get; }

        Department Department { get; }
    }

    public interface IHrmSalaryMatrixCell {
        IHrmSalaryMatrixColumn Column { get; }
        IHrmSalaryMatrixRow Row { get; }
    }
}
