using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntecoAG.Erm.XafExt.DataStruct.Matrix {

    public interface IMatrixSliceCollection<Tm, Tc, Tr, Te, Tv> : IEnumerable<ISlice<Tm, Tc, Tr, Te, Tv>>
        where Tm : IMatrix<Tm, Tc, Tr, Te, Tv>
        where Tr : IRow<Tm, Tc, Tr, Te, Tv>
        where Tc : IColumn<Tm, Tc, Tr, Te, Tv>
        where Te : ICell<Tm, Tc, Tr, Te, Tv> 
    {
        ISlice<Tm, Tc, Tr, Te, Tv> this[int index] { get; }
    }

}
