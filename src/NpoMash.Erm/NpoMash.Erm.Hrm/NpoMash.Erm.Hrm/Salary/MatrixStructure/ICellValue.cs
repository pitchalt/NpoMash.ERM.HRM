using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.FM.Order;


namespace NpoMash.Erm.Hrm.Salary.MatrixStructure {
    public interface ICellValue {
        fmCOrder Order { get; }
        Department Department { get; }
        DepartmentGroupDep GroupDep { get; }
        FmCOrderTypeControl TypeControl { get; }
    }
}
