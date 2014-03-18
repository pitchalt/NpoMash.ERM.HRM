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
                    }
                    ProvCell current_cell = new ProvCell();
                    current_ord.cells.Add(current_cell);
                    current_cell.ord = current_ord;
                    current_dep.cells.Add(current_cell);
                    current_cell.dep = current_dep;
                    current_cell.plan = source_cell.PlanMoney;
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

        public static void BringEasyDeps(ProvMat mat) {


        }


    }

}
