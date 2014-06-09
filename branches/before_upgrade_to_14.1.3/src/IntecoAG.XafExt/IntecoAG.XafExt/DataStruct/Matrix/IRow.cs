using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntecoAG.XafExt.DataStruct.Matrix {

    public interface IRow<Tm, Tc, Tck, Tr, Trk, Te, Tv> : IValue<Tv>
        where Tm : IMatrix<Tm, Tc, Tck, Tr, Trk, Te, Tv>
        where Tr : IRow<Tm, Tc, Tck, Tr, Trk, Te, Tv>
        where Tc : IColumn<Tm, Tc, Tck, Tr, Trk, Te, Tv>
        where Te : ICell<Tm, Tc, Tck, Tr, Trk, Te, Tv> 
    {
        Tm Matrix { get; }
        IIndexedList<Tck, Te> Cells { get; }
    }

}
