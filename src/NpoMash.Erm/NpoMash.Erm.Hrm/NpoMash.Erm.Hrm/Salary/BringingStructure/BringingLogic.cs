﻿using System;
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
using IntecoAG.ERM.HRM;
using IntecoAG.ERM.FM.Order;

namespace NpoMash.Erm.Hrm.Salary.BringingStructure {

    public static class BringingLogic {

        public static BringingStructure.Matrix PrepareBringingStructure(HrmSalaryTaskMatrixReduction reduc) {
            HrmMatrix mat_plan = reduc.MatrixPlan;
            HrmTimeSheet time_sheet = reduc.TimeSheet;
            HrmAllocParameter alloc_parameters = reduc.AllocParameters;
            IDictionary<String, bool> order_controls = new Dictionary<String, bool>();
            foreach (HrmAllocParameterOrderControl oc in alloc_parameters.OrderControls)
                if (oc.TypeControl == FmCOrderTypeControl.TRUDEMK_FOT)
                    order_controls.Add(oc.Order.Code, true);
                else order_controls.Add(oc.Order.Code, false);

            IDictionary<String, HrmTimeSheetDep> time_sheet_dictionary = reduc.TimeSheet.TimeSheetDeps
                .Where<HrmTimeSheetDep>(x => x.BaseWorkTime > 0)
                .ToDictionary<HrmTimeSheetDep, String>(x => x.Department.BuhCode);
            int errors_in_ts = 0;
            BringingStructure.Matrix mat = new BringingStructure.Matrix();
            foreach (HrmMatrixColumn department_plan in mat_plan.Columns) {

                //continue;

                Dep department = new Dep();
                if (time_sheet_dictionary.ContainsKey(department_plan.Department.BuhCode)) {

                    department.fact += time_sheet_dictionary[department_plan.Department.BuhCode].BaseWorkTime;
                }
                else {
                    errors_in_ts++; // это неплохо бы показать в логах
                    department.fact += 0;
                }
                department.realDepartment = department_plan.Department;
                mat.deps.Add(department_plan.Department.BuhCode, department);
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

            return mat;
        }


        /// <summary>
        /// Приведение трудозатрат подразделений до уровня плана
        /// за счет неконтролируемых заказов 
        /// </summary>
        /// <param name="mat"></param>
        public static void BringUncontrolledOrders(BringingStructure.Matrix mat) {
            foreach (Dep dep in mat.deps.Values) {
                if (dep.fact >= dep.planControlled) {
                    IList<Cell> non_zero_uncontrolled = new List<Cell>();
                    Decimal total_uncontrolled_sum = 0;
                    foreach (Cell cell in dep.cells) {
                        if (!cell.order.isControlled && cell.isNotZero) {
                            if (cell.time == 0) // думаю без этого никак, иначе начнем делить на ноль в некоторых случаях
                                cell.time += 1;
                            total_uncontrolled_sum += cell.time;
                            non_zero_uncontrolled.Add(cell);
                        }
                    }
                    Decimal summ_rsp = dep.fact - dep.planControlled;
                    foreach (Cell cell in non_zero_uncontrolled) {
                        Decimal delta_dep = Math.Round(cell.time * summ_rsp / total_uncontrolled_sum);
                        summ_rsp -= delta_dep;
                        total_uncontrolled_sum -= cell.time;
                        Decimal difference = delta_dep - cell.time;
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

        public static void BringMicroDepartments(BringingStructure.Matrix mat) {
            IEnumerable<Dep> micro_departments = mat.deps.Values
                .Where<Dep>(x => x.planControlled < x.fact && x.nonZeroUncontrolled == 0)
                .OrderBy<Dep, Int64>(x => x.nonZeroControlled);
            foreach (Dep dep in micro_departments) {
                bool is_not_stuck = true;
                while (dep.freeSpace > 0 && is_not_stuck) {
                    Cell best_cell_to_take = null;
                    Decimal best_size = 0;
                    Cell cell_in_this_dep_to_put = null;
                    bool is_first_iter = true;
                    foreach (Cell cell in dep.cells.Where<Cell>(x => x.isNotZero && x.order.isControlled)) {
                        Decimal size;
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
                        Decimal size_of_transfer = Math.Min(Math.Abs(best_size), dep.freeSpace);
                        mat.journal.MakeOperation(size_of_transfer, best_cell_to_take, cell_in_this_dep_to_put);
                    }
                }
            }
        }

        public static void BringBigDepartments(BringingStructure.Matrix mat) {
            IEnumerable<Dep> big_deps = mat.deps.Values.Where<Dep>(x => x.freeSpace < 0)
                .OrderBy<Dep, Decimal>(x => x.freeSpace);

            foreach (Dep dep in big_deps) {
                bool is_not_stucked = true;
                while (dep.freeSpace < 0 && is_not_stucked) {
                    Cell best_cell_to_put_in = null;
                    Cell cell_in_this_dep_to_take = null;
                    Decimal best_size = 0;
                    bool is_first_iter = true;
                    foreach (Cell cell in dep.cells.Where<Cell>(x => x.time > 0 && x.order.isControlled)) {
                        Decimal size;
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
                        Decimal size_of_transfer = Math.Min(Math.Abs(dep.freeSpace), Math.Min(best_size, cell_in_this_dep_to_take.time));
                        mat.journal.MakeOperation(size_of_transfer, cell_in_this_dep_to_take, best_cell_to_put_in);
                    }
                }
            }


        }

        public static void RestoreInitialFact(BringingStructure.Matrix mat) {
            foreach (Cell cell in mat.cellsInDictionary.Values) {
                if (cell.isNeedsToRestore) {
                    cell.time += 1;
                    cell.dep.fact += 1;
                }
            }
        }

        public static void PutDataInRealMatrix(HrmMatrix real_matrix, BringingStructure.Matrix bringing_structure) {
            RestoreInitialFact(bringing_structure);
            foreach (HrmMatrixColumn real_dep in real_matrix.Columns)
                foreach (HrmMatrixCell real_cell in real_dep.Cells) {
                    try {
                        Tuple<Dep, Ord> tuple = new Tuple<Dep, Ord>(bringing_structure.deps[real_cell.Column.Department.BuhCode], bringing_structure.orders[real_cell.Row.Order.Code]);
                        if (bringing_structure.cellsInDictionary.ContainsKey(tuple))
                            real_cell.Time = (Int64)bringing_structure.cellsInDictionary[tuple].time;
                    }
                    catch (KeyNotFoundException) { }
                }
        }
    }
}