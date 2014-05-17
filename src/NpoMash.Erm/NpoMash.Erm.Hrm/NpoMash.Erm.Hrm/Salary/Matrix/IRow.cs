using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using M=IntecoAG.XafExt.DataStruct.Matrix;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.HRM.Organization;

namespace NpoMash.Erm.Hrm.Salary.Matrix {

    public interface IRow<Tm, Tc, Tr, Te, Tv> : M.IRow<Tm, Tc, Department, Tr, fmCOrder, Te, Tv>
        where Tm : IMatrix<Tm, Tc, Tr, Te, Tv>
        where Tr : IRow<Tm, Tc, Tr, Te, Tv>
        where Tc : IColumn<Tm, Tc, Tr, Te, Tv>
        where Te : ICell<Tm, Tc, Tr, Te, Tv> 
    {

        FmCOrderTypeControl OrderControl { get; }
        fmCOrder Order { get; }

    }

}
