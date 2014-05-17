using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntecoAG.XafExt.DataStruct.Matrix {

    public interface IValue<Tv> {
        Tv Value { get; }
        //fmCOrder Order { get; }
        //Department Department { get; }
        //DepartmentGroupDep GroupDep { get; }
        //FmCOrderTypeControl TypeControl { get; }
    }

}
