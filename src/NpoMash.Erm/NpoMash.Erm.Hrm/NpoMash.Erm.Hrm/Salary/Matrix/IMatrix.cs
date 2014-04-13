using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntecoAG.XafExt.IndexedList;

namespace NpoMash.Erm.Hrm.Salary.Matrix {
    interface IMatrix: IIndexable<ICellValue> {
        HrmMatrixRow Rows { get; }
        HrmMatrixColumn Columns { get; }

    }
}
