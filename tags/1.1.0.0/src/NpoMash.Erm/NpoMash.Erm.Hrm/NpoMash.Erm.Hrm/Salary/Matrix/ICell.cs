using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using M=IntecoAG.XafExt.DataStruct.Matrix;
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.FM.Order;

namespace NpoMash.Erm.Hrm.Salary.Matrix {

    public interface ICell<Tm, Tc, Tr, Te, Tv> : M.ICell<Tm, Tc, Department, Tr, fmCOrder, Te, Tv>, IValue<Tv>
        where Tm : IMatrix<Tm, Tc, Tr, Te, Tv>
        where Tr : IRow<Tm, Tc, Tr, Te, Tv>
        where Tc : IColumn<Tm, Tc, Tr, Te, Tv>
        where Te : ICell<Tm, Tc, Tr, Te, Tv> 
    {
//        Tc Column { get; }
//        Tr Row { get; }
    }

}
