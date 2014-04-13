using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntecoAG.XafExt.IndexedList;

namespace NpoMash.Erm.Hrm.Salary.Matrix {
    interface IMatrixBase: IIndexable<ICellValue>, ICellValue {
        IRowCollection Rows { get; }
        IColumnCollection Columns { get; }
    }
}
