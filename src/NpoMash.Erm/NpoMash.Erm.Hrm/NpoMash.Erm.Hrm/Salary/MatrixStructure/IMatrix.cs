using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntecoAG.XafExt.IndexedList;
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.FM.Order;

namespace NpoMash.Erm.Hrm.Salary.MatrixStructure {
    public interface IMatrix : IMatrixBase {
        IMatrixSliceCollection SlicedMatrixesCollection { get; }
    }
}
