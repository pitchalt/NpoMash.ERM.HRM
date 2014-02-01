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
                    cell.startTime = cell_plan.Time;
                }
            }

            foreach (HrmTimeSheetDep tsd in time_sheet.TimeSheetDeps)
                if (mat.deps.ContainsKey(tsd.Department.Code))
                    mat.deps[tsd.Department.Code].fact += tsd.MatrixWorkTime;
            return mat;
        }



        public static void BringUncontrolledOrders(Matrix mat) {
            foreach (Dep dep in mat.deps.Values) {
                if (dep.fact >= dep.planControlled) {
                    List<Cell> non_zero_uncontrolled = new List<Cell>();
                    Int64 total_uncontrolled_sum = 0;
                    foreach (Cell cell in dep.cells) {
                        if (!cell.order.isControlled && cell.isNotZero) {
                            if (cell.time == 0)
                                cell.time += 1;
                            total_uncontrolled_sum += cell.time;
                            non_zero_uncontrolled.Add(cell);
                        }
                    }
                    //Double coefficient = ((Double)dep.fact - dep.planControlled) / total_uncontrolled_sum;
                    Int64 chislitel = dep.fact - dep.planControlled;
                    foreach (Cell cell in non_zero_uncontrolled) {
                        Int64 difference = cell.time * chislitel / total_uncontrolled_sum - cell.time;
                        if (difference > 0) mat.journal.MakeOperation(difference, null, cell);
                        if (difference < 0) {
                            //if (difference == cell.time) mat.journal.MakeOperation(1,null, cell);
                            //if (difference < 0)
                            mat.journal.MakeOperation(difference, cell, null);
                        }
                    }
                    Int64 plan_fact_difference = dep.fact - dep.plan;
                    List<Cell>.Enumerator en = non_zero_uncontrolled.GetEnumerator();
                    if (en.Current == null) en.MoveNext();
                    if (plan_fact_difference > 0)
                        mat.journal.MakeOperation(plan_fact_difference, null, en.Current);
                    

                    while (plan_fact_difference < 0 && en.Current != null) {
                        Int64 x = Math.Min(en.Current.time, -plan_fact_difference);
                        mat.journal.MakeOperation(x, en.Current, null);
                        plan_fact_difference += x;
                        en.MoveNext();
                    }
                }
            }
        }

        public static void BringMicroDepartments(Matrix mat) {
            IEnumerable<Dep> micro_departments = mat.deps.Values.
                Where<Dep>(x => x.planControlled < x.fact && x.nonZeroUncontrolled == 0)
                .OrderByDescending<Dep, Int64>(x => x.freeSpace);
            foreach (Dep dep in micro_departments) {
                bool is_not_stuck = true;
                while (dep.freeSpace > 0 && is_not_stuck){
                    Cell best_cell_to_take = null;
                    Int64 best_size = 0;
                    Cell cell_in_this_dep_to_put = null;
                    bool is_first_iter = true;
                    foreach (Cell cell in dep.cells.Where<Cell>(x => x.isNotZero)) {
                        Int64 size;
                        Cell cell_to_take = cell.BestCellToTakeFrom(out size);
                        if (is_first_iter) {
                            best_cell_to_take = cell_to_take;
                            best_size = size;
                            cell_in_this_dep_to_put = cell;
                            is_first_iter = false;
                        }
                        else if (size > best_size) {
                            best_cell_to_take = cell_to_take;
                            best_size = size;
                            cell_in_this_dep_to_put = cell;
                        }
                    }
                    if (best_cell_to_take == null)
                        throw new Exception("Can't bring fully conrolled department with code"+dep.realDepartment.Code);//is_not_stuck = false; это нам для отладки
                    else {
                        Int64 size_of_transfer = Math.Min(Math.Abs(best_size), dep.freeSpace);
                        mat.journal.MakeOperation(size_of_transfer, best_cell_to_take, cell_in_this_dep_to_put);
                    }
                }
            }
        }

        public static void BringBigDepartments(Matrix mat) {
            IEnumerable<Dep> big_deps = mat.deps.Values.Where<Dep>(x => x.freeSpace < 0)
                .OrderBy<Dep, Int64>(x => x.freeSpace);
            foreach (Dep dep in big_deps) {
                Cell best_cell_to_put_in = null;
                Cell cell_in_this_dep_to_take = null;
                Int64 best_size = 0;
                bool is_first_iter = true;
                foreach (Cell cell in dep.cells) {
                    Int64 size;
                    Cell cell_to_put = cell.BestCellToPutIn(out size);
                    if (is_first_iter) {

                    }

                }

            }


        }

        public static void RestoreInitialFact(Matrix mat) {
            foreach (Cell cell in mat.cellsInDictionary.Values) {
                if (cell.isNotZero) {
                    cell.time += 1;
                    cell.dep.fact += 1;
                }
            }
        }

        public static void PutDataInRealMatrix(HrmMatrix real_matrix, Matrix bringing_structure) {
            RestoreInitialFact(bringing_structure);
            foreach (HrmMatrixColumn real_dep in real_matrix.Columns)
                foreach (HrmMatrixCell real_cell in real_dep.Cells) {
                    Tuple<Dep, Ord> tuple = new Tuple<Dep, Ord>(bringing_structure.deps[real_cell.Column.Department.Code], bringing_structure.orders[real_cell.Row.Order.Code]);
                    if(bringing_structure.cellsInDictionary.ContainsKey(tuple))
                        real_cell.Time = bringing_structure.cellsInDictionary[tuple].time;
                }
        }


    }
}
