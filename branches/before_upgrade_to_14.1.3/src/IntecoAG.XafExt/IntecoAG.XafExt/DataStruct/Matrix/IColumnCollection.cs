using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntecoAG.Erm.XafExt.DataStruct.Matrix {

    public interface IColumnCollection<Tm, Tc, Tr, Te, Tv> : IEnumerable<Tc>
        where Tm : IMatrix<Tm, Tc, Tr, Te, Tv>
        where Tr : IRow<Tm, Tc, Tr, Te, Tv>
        where Tc : IColumn<Tm, Tc, Tr, Te, Tv>
        where Te : ICell<Tm, Tc, Tr, Te, Tv> {

        Tc this[int index] { get; }
    }

}
