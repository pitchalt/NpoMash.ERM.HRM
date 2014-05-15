using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntecoAG.XafExt.DataStruct {

    public interface IIndexedList<Tk, Tv> : IList<Tv>
    {
        Tv this[Tk key] { get; set; }
    }

}
