using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntecoAG.ERM.FM.Order;
using IntecoAG.XafExt.IndexedList;

namespace NpoMash.Erm.Hrm.Salary.MatrixStructure {

    public interface IRowCollection : IIndex<ICellValue, fmCOrder> {
    }
}
