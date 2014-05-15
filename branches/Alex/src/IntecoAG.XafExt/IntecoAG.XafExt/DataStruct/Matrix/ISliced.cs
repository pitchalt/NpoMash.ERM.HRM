using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntecoAG.XafExt.DataStruct.Matrix {

    public interface ISliced<Td, Ts, Tsk, Tm, Tc, Tck, Tr, Trk, Te, Tv> : IMatrix<Tm, Tc, Tck, Tr, Trk, Te, Tv>
        where Td : ISliced<Td, Ts, Tsk, Tm, Tc, Tck, Tr, Trk, Te, Tv>
        where Ts : ISlice<Td, Ts, Tsk, Tm, Tc, Tck, Tr, Trk, Te, Tv>
        where Tm : IMatrix<Tm, Tc, Tck, Tr, Trk, Te, Tv>
        where Tr : IRow<Tm, Tc, Tck, Tr, Trk, Te, Tv>
        where Tc : IColumn<Tm, Tc, Tck, Tr, Trk, Te, Tv>
        where Te : ICell<Tm, Tc, Tck, Tr, Trk, Te, Tv> 
    {
        IIndexedList<Tsk, Ts> Slices { get; }
    }

}
