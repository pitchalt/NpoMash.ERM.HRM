using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.HRM.Organization;

namespace NpoMash.Erm.Hrm.Salary.Matrix.MatrixPlan {

    public interface IRow : IRow<IMatrix, IColumn, IRow, ICell, IValue>, IValue {

    }

}
