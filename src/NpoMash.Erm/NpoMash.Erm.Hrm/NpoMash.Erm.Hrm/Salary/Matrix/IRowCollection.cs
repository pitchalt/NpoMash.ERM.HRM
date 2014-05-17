using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using M=IntecoAG.Erm.XafExt.DataStruct.Matrix;
using IntecoAG.ERM.FM.Order;

namespace NpoMash.Erm.Hrm.Salary.Matrix {

    public interface IRowCollection<Tm, Tc, Tr, Te, Tv> : M.IRowCollection<Tm, Tc, Tr, Te, Tv>
        where Tm : IMatrix<Tm, Tc, Tr, Te, Tv>
        where Tr : IRow<Tm, Tc, Tr, Te, Tv>
        where Tc : IColumn<Tm, Tc, Tr, Te, Tv>
        where Te : ICell<Tm, Tc, Tr, Te, Tv> 
    {
        Tr this[fmCOrder order] { get; }
    }
}
