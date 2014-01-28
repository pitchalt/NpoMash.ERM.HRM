using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using NpoMash.Erm.Hrm.Salary.BringingStructure;

namespace NpoMash.Erm.Hrm.Salary {
    public static class BringingLogic{
        
        public static Matrix PrepareBringingStructure(HrmMatrix mat_plan, HrmTimeSheet time_sheet) {
            Matrix mat = new Matrix();
            foreach (HrmMatrixColumn department_plan in mat_plan.Columns) {
                Dep department = new Dep();
                mat.deps.Add(department_plan.Department.Code, department);
                department.matrix = mat;
                foreach (HrmMatrixCell cell_plan in department_plan.Cells) {
                    Cell cell = new Cell();
                    cell.dep = department;
                    department.cells.Add(cell);
                    Ord order = null;
                    if (mat.orders.ContainsKey(cell_plan.Row.Order.Code))
                        order = mat.orders[cell_plan.Row.Order.Code];
                    else {
                        order = new Ord();

                    }
                }
            }

            return mat;
        }
        
    }

}
