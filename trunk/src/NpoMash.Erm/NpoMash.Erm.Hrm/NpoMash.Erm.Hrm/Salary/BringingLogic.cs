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
using IntecoAG.ERM.HRM;
using IntecoAG.ERM.FM.Order;

namespace NpoMash.Erm.Hrm.Salary {
    public static class BringingLogic{
        
        public static Matrix PrepareBringingStructure(HrmSalaryTaskMatrixReduction reduc) {
            HrmMatrix mat_plan = reduc.MatrixPlan;
            HrmTimeSheet time_sheet = reduc.TimeSheet;
            HrmPeriodAllocParameter alloc_parameters = reduc.AllocParameters;
            Dictionary<String, bool> order_controls = new Dictionary<String, bool>();
            foreach (HrmPeriodOrderControl oc in alloc_parameters.OrderControls)
                if (oc.TypeControl == FmCOrderTypeControl.TRUDEMK_FOT)
                    order_controls.Add(oc.Order.Code, true);
                else order_controls.Add(oc.Order.Code, false);

            Matrix mat = new Matrix();
            foreach (HrmMatrixColumn department_plan in mat_plan.Columns) {
                Dep department = new Dep();
                department.realDepartment = department_plan.Department;
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
                        order.isControlled = order_controls[cell_plan.Row.Order.Code];
                        order.realOrder = cell_plan.Row.Order;
                        order.matrix = mat;
                        mat.orders.Add(order.realOrder.Code, order);
                    }

                    cell.order = order;
                    order.cells.Add(cell);
                    mat.cellsInDictionary.Add(new Tuple<Dep, Ord>(department, order), cell);
                    cell.time = cell_plan.Time;
                }
            }

            foreach (HrmTimeSheetDep tsd in time_sheet.TimeSheetDeps)
                mat.deps[tsd.Department.Code].fact = tsd.MatrixWorkTime;

            return mat;
        }


        
    }

}
