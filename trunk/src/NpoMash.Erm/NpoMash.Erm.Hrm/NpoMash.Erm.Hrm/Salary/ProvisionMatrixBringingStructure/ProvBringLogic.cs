using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;

//
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
//
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.FM.Order;

namespace NpoMash.Erm.Hrm.Salary.ProvisionMatrixBringingStructure {

    static class ProvBringLogic {

        public static ProvMat CreateProvBringStructure(HrmSalaryTaskProvisionMatrixReduction card){
            ProvMat result = new ProvMat();
            HrmMatrix source_mat = card.ProvisionMatrix;
            Dictionary<String, HrmPeriodOrderControl> controlled_orders = card.AllocParameters.OrderControls
                .Where(x => x.TypeControl == FmCOrderTypeControl.FOT || x.TypeControl == FmCOrderTypeControl.TRUDEMK_FOT).
                ToDictionary(x => x.Order.Code);
            foreach (HrmMatrixColumn source_column in source_mat.Columns) {
                String dep_code = source_column.Department.BuhCode;
                ProvDep current_dep = new ProvDep();
                result.deps.Add(dep_code, current_dep);
                current_dep.code = dep_code;
                foreach (HrmMatrixCell source_cell in source_column.Cells) {
                    ProvOrd current_ord = null;
                    String ord_code = source_cell.Row.Order.Code;
                    if (result.ords.ContainsKey(ord_code)) {
                        current_ord = result.ords[ord_code];
                    }
                    else {
                        current_ord = new ProvOrd();
                        result.ords.Add(ord_code, current_ord);
                        current_ord.code = ord_code;
                        current_ord.isControlled = controlled_orders.ContainsKey(ord_code);
                        if (!current_ord.isControlled) current_dep.numberOfUncontrolledOrders++;
                        else current_dep.numberOfControlledOrders++;
                    }
                    ProvCell current_cell = new ProvCell();
                    current_ord.cells.Add(current_cell);
                    current_cell.ord = current_ord;
                    current_dep.cells.Add(current_cell);
                    current_cell.dep = current_dep;
                    current_cell.plan = source_cell.PlanMoney;
                    current_ord.ordPlan += current_cell.plan;
                    current_cell.constFact = source_cell.MoneyNoReserve;
                    //current_cell.reserve = source_cell.MoneyReserve;
                    current_dep.undistributedReserve += source_cell.MoneyReserve;
                    current_cell.refToRealCell = source_cell;
                    result.cells.Add(current_cell);
                }
            }
            return result;
        }


        public static void LoadProvBringResultInTask(ProvMat mat){
            foreach (ProvCell cell in mat.cells)
                cell.refToRealCell.MoneyReserve = cell.reserve;
        }

        public static void BringUncontrolledReserveInDep(ProvDep dep) {
            int uncontrolled = dep.numberOfUncontrolledOrders;
            foreach (ProvCell cell in dep.cells.Where(x => !x.ord.isControlled)) {
                Decimal diff = dep.undistributedReserve / uncontrolled;
                Math.Truncate(diff);
                cell.reserve = diff;
                dep.undistributedReserve -= diff;
                uncontrolled--;
            }
        }

        public static void BringEasyDeps(ProvMat mat) {
            IEnumerable<ProvDep> easyDeps = mat.deps.Values.Where(x => !x.isAlreadyBringed && x.numberOfUncontrolledOrders > 0 && x.numberOfControlledOrders > 0)
                .Where(x => x.cells
                    .Where(y => y.ord.isControlled && y.planFactDifference > 0)
                    .Sum(y => y.planFactDifference) < x.undistributedReserve);
            foreach (ProvDep dep in easyDeps) {
                foreach (ProvCell cell in dep.cells.Where(x => x.ord.isControlled && x.planFactDifference > 0)) {
                    decimal diff = cell.planFactDifference;
                    cell.reserve = diff;
                    dep.undistributedReserve -= diff;
                }
                BringUncontrolledReserveInDep(dep);
                dep.isAlreadyBringed = true;
            }
        }

        public static void BringVeryEasyDeps(ProvMat mat) {
            IEnumerable<ProvDep> veryEasyDeps = mat.deps.Values.Where(x => x.numberOfControlledOrders == 0 && x.numberOfUncontrolledOrders > 0 && !x.isAlreadyBringed);
            foreach (ProvDep dep in veryEasyDeps) {
                BringUncontrolledReserveInDep(dep);
                dep.isAlreadyBringed = true;
            }
        }

        public static void BringDepsWithLessOfReserve(ProvMat mat) {
            IEnumerable<ProvDep> less_deps = mat.deps.Values.Where(x => !x.isAlreadyBringed && x.numberOfControlledOrders > 0)
                .Where(x => x.cells
                    .Where(y => y.ord.isControlled && y.planFactDifference > 0)
                    .Sum(y => y.planFactDifference) > x.undistributedReserve);
            
            foreach (ProvDep dep in less_deps) {
                IEnumerable<ProvCell> less_cells = dep.cells.Where(x => x.planFactDifference > 0);
                while (dep.undistributedReserve > 0) {
                    //IEnumerable<ProvCell> ordered_cells = less_cells.OrderByDescending(x => x.planFactDifference);
                    Decimal max_diff = less_cells.Max(x => x.planFactDifference);
                    Decimal prev_max_diff = 0;
                    bool prev_max_diff_founded = false;
                    List<ProvCell> work_cells = new List<ProvCell>();
                    foreach (ProvCell cell in less_cells) {
                        if (cell.planFactDifference == max_diff) {
                            work_cells.Add(cell);
                        }
                        else {
                            if (cell.planFactDifference > prev_max_diff || prev_max_diff_founded == false) {
                                prev_max_diff = cell.planFactDifference;
                                prev_max_diff_founded = true;
                            }
                        }
                        if (!prev_max_diff_founded) {
                            int number_of_workcells = work_cells.Count;
                            foreach (ProvCell working_cell in work_cells) {
                                Decimal diff = dep.undistributedReserve / number_of_workcells;
                                Math.Truncate(diff);
                                cell.reserve = diff;
                                dep.undistributedReserve -= diff;
                                number_of_workcells--;
                            }
                            
                        }

                    }

                }
                dep.isAlreadyBringed = true;
            }
        }


        public static void BringFullyControlledOverloadedDeps(ProvMat mat){


        }


    }

}
