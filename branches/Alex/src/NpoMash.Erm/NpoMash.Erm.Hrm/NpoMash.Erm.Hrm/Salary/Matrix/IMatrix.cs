using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.HRM.Organization;
using M = IntecoAG.XafExt.DataStruct.Matrix;

namespace NpoMash.Erm.Hrm.Salary.Matrix {

    public interface IMatrix<Tm, Tc, Tr, Te, Tv> : M.IMatrix<Tm, Tc, Department, Tr, fmCOrder, Te, Tv>, IValue<Tv>
        where Tm : IMatrix<Tm, Tc, Tr, Te, Tv>
        where Tr : IRow<Tm, Tc, Tr, Te, Tv>
        where Tc : IColumn<Tm, Tc, Tr, Te, Tv>
        where Te : ICell<Tm, Tc, Tr, Te, Tv> 
    {
    }

}
