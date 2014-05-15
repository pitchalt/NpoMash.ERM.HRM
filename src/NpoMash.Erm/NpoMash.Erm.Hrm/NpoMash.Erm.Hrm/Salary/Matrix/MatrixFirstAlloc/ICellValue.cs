using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.FM.Order;


namespace NpoMash.Erm.Hrm.Salary.Matrix.MatrixPlan {

    public interface ICellValue: Matrix.ICellValue {
        Decimal Time { get; }
        Decimal Summ { get; }
    }

}
