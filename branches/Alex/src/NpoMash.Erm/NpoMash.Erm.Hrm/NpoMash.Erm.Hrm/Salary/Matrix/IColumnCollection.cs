﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntecoAG.XafExt.IndexedList;
using IntecoAG.ERM.HRM.Organization;

namespace NpoMash.Erm.Hrm.Salary.MatrixStructure {
    public interface IColumnCollection : IIndex<ICellValue, Department> {
    }
}
