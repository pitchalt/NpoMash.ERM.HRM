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

            Dictionary<String, HrmTimeSheetDep> time_sheet_dictionary = reduc.TimeSheet.TimeSheetDeps
                .Where<HrmTimeSheetDep>( x => x.MatrixWorkTime > 0)
                .ToDictionary<HrmTimeSheetDep, String>(x => x.Department.Code);

            Matrix mat = new Matrix();
            foreach (HrmMatrixColumn department_plan in mat_plan.Columns) {
                if (!time_sheet_dictionary.ContainsKey(department_plan.Department.Code))
                    continue;
                Dep department = new Dep();
                department.fact += time_sheet_dictionary[department_plan.Department.Code].MatrixWorkTime;
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

            /*foreach (HrmTimeSheetDep tsd in time_sheet.TimeSheetDeps)
                if (mat.deps.ContainsKey(tsd.Department.Code))
                    mat.deps[tsd.Department.Code].fact += tsd.MatrixWorkTime;*/
            return mat;
        }


        /*
        public static void BringUncontrolledOrders2(Matrix mat) {
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
        }*/
        /// <summary>
        /// Приведение трудозатрат подразделений до уровня плана
        /// за счет неконтролируемых заказов 
        /// </summary>
        /// <param name="mat"></param>
        public static void BringUncontrolledOrders(Matrix mat) {
            foreach (Dep dep in mat.deps.Values) {
                if (dep.fact >= dep.planControlled) {
                    List<Cell> non_zero_uncontrolled = new List<Cell>();
                    Int64 total_uncontrolled_sum = 0;
                    foreach (Cell cell in dep.cells) {
                        if (!cell.order.isControlled && cell.isNotZero) {
                            if (cell.time == 0) // думаю без этого никак, иначе начнем делить на ноль в некоторых случаях
                                cell.time += 1;
                            total_uncontrolled_sum += cell.time;
                            non_zero_uncontrolled.Add(cell);
                        }
                    }
                    Int64 summ_rsp = dep.fact - dep.planControlled;
                    foreach (Cell cell in non_zero_uncontrolled) {
                        Int64 delta_dep = cell.time * summ_rsp / total_uncontrolled_sum;
                        summ_rsp -= delta_dep;
                        total_uncontrolled_sum -= cell.time;
                        Int64 difference = delta_dep - cell.time;
                        if (difference > 0)
                            mat.journal.MakeOperation(difference, null, cell);
                        else if (difference < 0)
                            mat.journal.MakeOperation(difference, cell, null);
                        else
                            continue;
                    }
                }
            }
        }

        public static void BringMicroDepartments(Matrix mat) {
            IEnumerable<Dep> micro_departments = mat.deps.Values
                .Where<Dep>(x => x.planControlled < x.fact && x.nonZeroUncontrolled == 0)
                .OrderBy<Dep, Int64>(x => x.nonZeroControlled);
                //.OrderByDescending<Dep, Int64>(x => x.freeSpace);
            foreach (Dep dep in micro_departments) {
                bool is_not_stuck = true;
                while (dep.freeSpace > 0 && is_not_stuck){
                    Cell best_cell_to_take = null;
                    Int64 best_size = 0;
                    Cell cell_in_this_dep_to_put = null;
                    bool is_first_iter = true;
                    foreach (Cell cell in dep.cells.Where<Cell>(x => x.isNotZero && x.order.isControlled)) {
                        Int64 size;
                        Cell cell_to_take = cell.BestCellToTakeFrom(out size);
                        if (is_first_iter && cell_to_take != null) {
                            best_cell_to_take = cell_to_take;
                            best_size = size;
                            cell_in_this_dep_to_put = cell;
                            is_first_iter = false;
                        }
                        else if (size > best_size && cell_to_take != null) {
                            best_cell_to_take = cell_to_take;
                            best_size = size;
                            cell_in_this_dep_to_put = cell;
                        }
                    }
                    if (best_cell_to_take == null)
                        //is_not_stuck = false;
                        throw new Exception("Can't bring fully controlled department with code " + dep.realDepartment.Code);//is_not_stuck = false; это нам для отладки
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
                bool is_not_stucked = true;
                while (dep.freeSpace < 0 && is_not_stucked) {
                    Cell best_cell_to_put_in = null;
                    Cell cell_in_this_dep_to_take = null;
                    Int64 best_size = 0;
                    bool is_first_iter = true;
                    foreach (Cell cell in dep.cells.Where<Cell>(x => x.time > 0 && x.order.isControlled)) {
                        Int64 size;
                        Cell cell_to_put = cell.BestCellToPutIn(out size);
                        if (is_first_iter && cell_to_put != null) {
                            best_cell_to_put_in = cell_to_put;
                            cell_in_this_dep_to_take = cell;
                            best_size = size;
                            is_first_iter = false;
                        }
                        else if (!is_first_iter && size > best_size && cell_to_put != null) {
                            best_cell_to_put_in = cell_to_put;
                            cell_in_this_dep_to_take = cell;
                            best_size = size;
                        }
                    }
                    if (best_cell_to_put_in == null) {
                        is_not_stucked = false;
                        throw new Exception("Can't bring overloaded department with code " + dep.realDepartment.Code);
                    }
                    else {
                        Int64 size_of_transfer = Math.Min(Math.Abs(dep.freeSpace), Math.Min(best_size, cell_in_this_dep_to_take.time));
                        mat.journal.MakeOperation(size_of_transfer, cell_in_this_dep_to_take, best_cell_to_put_in);
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
                    try {
                        Tuple<Dep, Ord> tuple = new Tuple<Dep, Ord>(bringing_structure.deps[real_cell.Column.Department.Code], bringing_structure.orders[real_cell.Row.Order.Code]);
                        if (bringing_structure.cellsInDictionary.ContainsKey(tuple))
                            real_cell.Time = bringing_structure.cellsInDictionary[tuple].time;
                    }
                    catch (KeyNotFoundException) { }
                }

        }


    }
}
