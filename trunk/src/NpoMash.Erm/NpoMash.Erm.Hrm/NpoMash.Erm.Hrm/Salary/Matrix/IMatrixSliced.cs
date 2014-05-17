using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntecoAG.XafExt.IndexedList;
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.FM.Order;

namespace NpoMash.Erm.Hrm.Salary.Matrix {

    public interface IMatrixSliced<Ts, Tc, Tr, Tv> : IMatrix<Tc, Tr, Tv>
        where Ts : IMatrixSlice<Tc, Tr, Tv>
        where Tc : IColumn<Tv>
        where Tr : IRow<Tv>
    {
        IMatrixSliceCollection<Ts, Tc, Tr, Tv> Slices { get; }
    }

}
