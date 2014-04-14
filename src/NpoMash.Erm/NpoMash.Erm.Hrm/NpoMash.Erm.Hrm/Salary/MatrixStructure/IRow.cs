using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntecoAG.XafExt.IndexedList;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.HRM.Organization;

namespace NpoMash.Erm.Hrm.Salary.MatrixStructure {
    public interface IRow : IIndexValue<ICellValue, fmCOrder> {
        IIndex<ICellValue, Department> Columns { get; }
    }
}
