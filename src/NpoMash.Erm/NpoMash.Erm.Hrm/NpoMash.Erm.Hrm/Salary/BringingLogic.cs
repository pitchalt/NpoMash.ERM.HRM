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
    public static class BringingLogic {

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
                        if (order_controls.ContainsKey(cell_plan.Row.Order.Code))
                            order.isControlled = order_controls[cell_plan.Row.Order.Code];
                        else order.isControlled = false;
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



        public static void BringUncontrolledOrders(Matrix mat) {
            foreach (Dep dep in mat.deps.Values) {
                if (dep.fact >= dep.planControlled) {
                    List<Cell> non_zero_uncontrolled = new List<Cell>();
                    Int32 total_uncontrolled_sum = 0;
                    foreach (Cell cell in dep.cells) {
                        if (!cell.order.isControlled && cell.time != 0) {
                            total_uncontrolled_sum += cell.time;
                            non_zero_uncontrolled.Add(cell);
                        }
                    }
                    Double coefficient = (dep.fact - dep.planControlled) / total_uncontrolled_sum;
                    foreach (Cell cell in non_zero_uncontrolled) {
                        Int32 difference = (Int32)Math.Round(cell.time * coefficient) - cell.time;
                        if (difference > 0) mat.journal.MakeOperation(difference, null, cell);
                        if (difference < 0) {
                            if (difference == cell.time) difference++;
                            if (difference < 0)
                                mat.journal.MakeOperation(-difference, cell, null);
                        }
                    }
                    Int32 plan_fact_difference = dep.fact - dep.plan;
                    List<Cell>.Enumerator en = non_zero_uncontrolled.GetEnumerator();
                    if (plan_fact_difference > 0) {
                        mat.journal.MakeOperation(plan_fact_difference, null, en.Current);
                    }

                    if (plan_fact_difference < 0) {
                        while (plan_fact_difference < 0 && en.Current != null) {
                            Int32 x = Math.Min(en.Current.time - 1, -plan_fact_difference);
                            if (x > 0) {
                                mat.journal.MakeOperation(x, en.Current, null);
                                plan_fact_difference += x;
                            }
                            en.MoveNext();
                        }
                    }
                }
            }
        }

        public static void BringMicroDepartments(Matrix mat) {

        }

        public static void BringBigDepartments(Matrix mat) {
            List<Dep> big_deps = new List<Dep>();
            List<Dep> small_deps = new List<Dep>();
            foreach (Dep dep in mat.deps.Values) {
                if (dep.freeSpace < 0) big_deps.Add(dep);
                if (dep.freeSpace > 0) small_deps.Add(dep);
            }
            big_deps.OrderBy(x => x.freeSpace);
            small_deps.OrderByDescending(x => x.freeSpace);


        }

        public static void PutDataInRealMatrix(HrmMatrix real_matrix, Matrix bringing_structure) {
            foreach (HrmMatrixColumn real_dep in real_matrix.Columns)
                foreach (HrmMatrixCell real_cell in real_dep.Cells) {
                    Tuple<Dep, Ord> tuple = new Tuple<Dep, Ord>(bringing_structure.deps[real_cell.Column.Department.Code], bringing_structure.orders[real_cell.Row.Order.Code]);
                    if(bringing_structure.cellsInDictionary.ContainsKey(tuple))
                        real_cell.Time = (Int16)bringing_structure.cellsInDictionary[tuple].time;
                }
        }


    }
}
