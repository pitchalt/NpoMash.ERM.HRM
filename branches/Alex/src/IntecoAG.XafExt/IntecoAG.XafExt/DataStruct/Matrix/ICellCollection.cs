using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntecoAG.Erm.XafExt.DataStruct.Matrix {

    public interface ICellCollection<Tm, Tc, Tr, Te, Tv> : IEnumerable<Te>
        where Tm : IMatrix<Tm, Tc, Tr, Te, Tv>
        where Tr : IRow<Tm, Tc, Tr, Te, Tv>
        where Tc : IColumn<Tm, Tc, Tr, Te, Tv>
        where Te : ICell<Tm, Tc, Tr, Te, Tv> 
    {
    }

}
