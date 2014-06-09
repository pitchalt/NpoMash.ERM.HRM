using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.FM.Order;
using M = IntecoAG.XafExt.DataStruct.Matrix;

namespace NpoMash.Erm.Hrm.Salary.Matrix {

//    class test : DevExpress.Xpo.XPCollection<Test>
    public interface ISliced<Td, Ts, Tm, Tc, Tr, Te, Tv> :
        M.ISliced<Td, Ts, DepartmentGroupDep, Tm, Tc, Department, Tr, fmCOrder, Te, Tv>, 
        IMatrix<Tm, Tc, Tr, Te, Tv>
        where Td : ISliced<Td, Ts, Tm, Tc, Tr, Te, Tv>
        where Ts : ISlice<Td, Ts, Tm, Tc, Tr, Te, Tv>
        where Tm : IMatrix<Tm, Tc, Tr, Te, Tv>
        where Tr : IRow<Tm, Tc, Tr, Te, Tv>
        where Tc : IColumn<Tm, Tc, Tr, Te, Tv>
        where Te : ICell<Tm, Tc, Tr, Te, Tv> 
    {
    }

}
