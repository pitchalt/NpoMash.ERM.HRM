using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using M=IntecoAG.Erm.XafExt.DataStruct.Matrix;
using IntecoAG.ERM.HRM.Organization;

namespace NpoMash.Erm.Hrm.Salary.Matrix {

    public interface IColumnCollection<Tm, Tc, Tr, Te, Tv> : M.IColumnCollection<Tm, Tc, Tr, Te, Tv>
        where Tm : IMatrix<Tm, Tc, Tr, Te, Tv>
        where Tr : IRow<Tm, Tc, Tr, Te, Tv>
        where Tc : IColumn<Tm, Tc, Tr, Te, Tv>
        where Te : ICell<Tm, Tc, Tr, Te, Tv> 
    {
        Tc this[Department dep] { get; }
    }

}
