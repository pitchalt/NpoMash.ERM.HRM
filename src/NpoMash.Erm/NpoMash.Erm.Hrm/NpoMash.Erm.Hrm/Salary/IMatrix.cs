using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntecoAG.XafExt.IndexedList;

namespace NpoMash.Erm.Hrm.Salary {
    interface IMatrix: IIndexable<ICellValue> {
        HrmMatrixRow Row { get; }
        HrmMatrixColumn Column { get; }
    }
}
