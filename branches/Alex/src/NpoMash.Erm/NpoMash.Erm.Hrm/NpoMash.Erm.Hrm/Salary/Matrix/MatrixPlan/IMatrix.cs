using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.FM.Order;

namespace NpoMash.Erm.Hrm.Salary.Matrix.MatrixPlan {

    public interface IMatrix : IMatrix<IMatrix, IColumn, IRow, ICell, IValue>, IValue {
//        IMatrixSliceCollection<> Slices { get; }
    }

}
