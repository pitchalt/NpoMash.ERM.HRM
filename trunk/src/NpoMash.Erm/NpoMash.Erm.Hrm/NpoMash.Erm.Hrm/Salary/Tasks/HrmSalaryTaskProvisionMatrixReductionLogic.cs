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

namespace NpoMash.Erm.Hrm.Salary {
    public static class HrmSalaryTaskProvisionMatrixReductionLogic {

        public static void ExportMatrixes(HrmPeriod current_period) {
            foreach (HrmMatrix m in current_period.Matrixs)
                if (m.TypeMatrix == HrmMatrixTypeMatrix.MATRIX_RESERVE && m.Status == HrmMatrixStatus.MATRIX_ACCEPTED)
                    m.Status = HrmMatrixStatus.MATRIX_EXPORTED;
        }

        public static void PrimaryAcceptSelectedMatrix(HrmSalaryTaskProvisionMatrixReduction card, HrmMatrix matrix_to_accept) {
            HrmMatrix matrix_to_reject = null;
            if (card.ReserveMatrixSimplex == matrix_to_accept)
                matrix_to_reject = card.ReserveMatrixEvristic;
            else matrix_to_reject = card.ReserveMatrixSimplex;
            matrix_to_accept.Status = HrmMatrixStatus.MATRIX_PRIMARY_ACCEPTED;
            matrix_to_reject.Status = HrmMatrixStatus.MATRIX_CLOSED;
            card.Period.CurrentMatrixProvision = matrix_to_accept;
        }

        public static bool MatrixIsPrimaryAccepted(/*HrmMatrix matrix_to_accept,*/ HrmPeriod current_period) {
            // bool provision_accepted = false;
            // а это на кой черт надо - проверять статус матрицы по ее группе подразделений????
            //if (matrix_to_accept.GroupDep == DepartmentGroupDep.DEPARTMENT_KB_OZM)
            //    provision_accepted = true;
            //else provision_accepted = false;
            return current_period.Matrixs.Where(x => 
                x.TypeMatrix == HrmMatrixTypeMatrix.MATRIX_COERCED &&
                x.Status == HrmMatrixStatus.MATRIX_PRIMARY_ACCEPTED &&
                x.GroupDep == DepartmentGroupDep.DEPARTMENT_KB_OZM)
                .FirstOrDefault() != null;
        }



        public static HrmMatrixProvision MergeAllMatrixes(IObjectSpace os, HrmSalaryTaskProvisionMatrixReduction card) {
            HrmMatrixProvision result = os.CreateObject<HrmMatrixProvision>();
            HrmMatrix m_plan_kb = card.MatrixPlanKB;
            HrmMatrix m_plan_ozm = card.MatrixPlanOZM;
            HrmMatrix m_res_kb = card.AllocResultKB;
            HrmMatrix m_res_ozm = card.AllocResultOZM;

            // это чтобы посмотреть, нет ли повторений среди колонок матриц
            //Dictionary<String, HrmMatrixColumn> cols_in_kb_plan = m_plan_kb.Columns.ToDictionary(x => x.Department.BuhCode);
            //Dictionary<String, HrmMatrixColumn> cols_in_ozm_plan = m_plan_ozm.Columns.ToDictionary(x => x.Department.BuhCode);
            //Dictionary<String, HrmMatrixColumn> cols_in_kb_res = m_res_kb.Columns.ToDictionary(x => x.Department.BuhCode);
            //Dictionary<String, HrmMatrixColumn> cols_in_ozm_res = m_res_ozm.Columns.ToDictionary(x => x.Department.BuhCode);

            Dictionary<String, Dictionary<String, HrmMatrixCell>> res_mat = new Dictionary<string, Dictionary<String, HrmMatrixCell>>();
            foreach (HrmMatrixColumn col in m_res_kb.Columns.Concat(m_res_ozm.Columns)) {
                String dep_code = col.Department.BuhCode;
                Dictionary<String, HrmMatrixCell> dict = col.Cells.ToDictionary(x => x.Row.Order.Code);
                res_mat.Add(dep_code, dict);
            }

            Dictionary<String, HrmMatrixRow> created_rows = new Dictionary<string, HrmMatrixRow>();

            int bad_cells = 0;
            int good_cells = 0;
            foreach (HrmMatrixColumn current_column in m_plan_kb.Columns.Concat(m_plan_ozm.Columns)) {
                HrmMatrixColumn result_column = os.CreateObject<HrmMatrixColumn>();
                String dep_code = current_column.Department.BuhCode;
                result_column.Matrix = result;
                result.Columns.Add(result_column);
                result_column.Department = current_column.Department;
                foreach (HrmMatrixCell current_cell in current_column.Cells) {
                    HrmMatrixRow current_row = current_cell.Row;
                    String ord_code = current_row.Order.Code;
                    HrmMatrixRow result_row = null;
                    if (created_rows.ContainsKey(ord_code)) {
                        result_row = created_rows[ord_code];
                    }
                    else {
                        result_row = os.CreateObject<HrmMatrixRow>();
                        result.Rows.Add(result_row);
                        result_row.Matrix = result;
                        result_row.Order = current_row.Order;
                        created_rows.Add(ord_code, result_row);
                    }

                    HrmMatrixCell result_cell = os.CreateObject<HrmMatrixCell>();
                    result_cell.Row = result_row;
                    result_row.Cells.Add(result_cell);
                    result_cell.Column = result_column;
                    result_column.Cells.Add(result_cell);
                    result_cell.Time = current_cell.Time;
                    // а это две самые страшные операции, как бы тут все в тартарары не улетело
                    if (res_mat.ContainsKey(dep_code) && res_mat[dep_code].ContainsKey(ord_code)) {
                        result_cell.SourceProvision = res_mat[dep_code][ord_code].SourceProvision;
                        result_cell.MoneyNoReserve = res_mat[dep_code][ord_code].MoneyNoReserve;
                        good_cells++;
                    }
                    else {
                        result_cell.SourceProvision = 0;
                        result_cell.MoneyNoReserve = 0;
                        bad_cells++;
                    }
                }
            }
            return result;
        }




        public static HrmSalaryTaskProvisionMatrixReduction initProvisonMatrixTask(IObjectSpace os, HrmPeriod period, DepartmentGroupDep group_dep) {
            HrmSalaryTaskProvisionMatrixReduction task_provision_matrix_reduction = os.CreateObject<HrmSalaryTaskProvisionMatrixReduction>();
            period.PeriodTasks.Add(task_provision_matrix_reduction);

            task_provision_matrix_reduction.CurrentTimeSheetKB = period.CurrentTimeSheetKB;
            task_provision_matrix_reduction.CurrentTimeSheetOZM = period.CurrentTimeSheetOZM;
            period.CurrentProvisionMatrix = task_provision_matrix_reduction;
            

            // Get coerced matrix from period
            foreach (HrmMatrix matrix in period.Matrixs) {
                if (matrix.TypeMatrix == HrmMatrixTypeMatrix.MATRIX_COERCED &&
                    matrix.Status == HrmMatrixStatus.MATRIX_ACCEPTED &&
                    matrix.GroupDep == DepartmentGroupDep.DEPARTMENT_KB) {
                    task_provision_matrix_reduction.MatrixAllocKB = matrix;
                }
                else if (matrix.TypeMatrix == HrmMatrixTypeMatrix.MATRIX_COERCED &&
                    matrix.Status == HrmMatrixStatus.MATRIX_ACCEPTED &&
                    matrix.GroupDep == DepartmentGroupDep.DEPARTMENT_OZM) {
                    task_provision_matrix_reduction.MatrixAllocOZM = matrix;
                }
            }


            // Get alloc result from period
            foreach (HrmMatrix matrix in period.Matrixs) {
                if (matrix.GroupDep == DepartmentGroupDep.DEPARTMENT_KB && matrix.Type == HrmMatrixType.TYPE_ALLOC_RESULT) {
                    task_provision_matrix_reduction.AllocResultKB = matrix;
                }
                else if (matrix.GroupDep == DepartmentGroupDep.DEPARTMENT_OZM && matrix.Type == HrmMatrixType.TYPE_ALLOC_RESULT) {
                    task_provision_matrix_reduction.AllocResultOZM = matrix;
                }
            }

            // Get Planned Matrixs
            foreach (HrmMatrix matrix in period.Matrixs) {
                if (matrix.GroupDep == DepartmentGroupDep.DEPARTMENT_KB && matrix.Type == HrmMatrixType.TYPE_MATIX &&
                    matrix.Status == HrmMatrixStatus.MATRIX_ACCEPTED && matrix.TypeMatrix==HrmMatrixTypeMatrix.MATRIX_PLANNED) {
                    task_provision_matrix_reduction.MatrixPlanKB = matrix;
                }
                else if (matrix.GroupDep == DepartmentGroupDep.DEPARTMENT_OZM && matrix.Type == HrmMatrixType.TYPE_MATIX &&
                    matrix.Status == HrmMatrixStatus.MATRIX_ACCEPTED && matrix.TypeMatrix == HrmMatrixTypeMatrix.MATRIX_PLANNED) {
                    task_provision_matrix_reduction.MatrixPlanOZM = matrix;
                }
            }

            task_provision_matrix_reduction.GroupDep = DepartmentGroupDep.DEPARTMENT_KB_OZM;
            task_provision_matrix_reduction.AllocParameters = period.CurrentAllocParameter;

            task_provision_matrix_reduction.ReserveMatrixSimplex = createMoneyMatrix(os,task_provision_matrix_reduction);
            task_provision_matrix_reduction.ReserveMatrixSimplex.Status = HrmMatrixStatus.MATRIX_SAVED;
            task_provision_matrix_reduction.ReserveMatrixSimplex.Type = HrmMatrixType.TYPE_MATIX;
            task_provision_matrix_reduction.ReserveMatrixSimplex.TypeMatrix = HrmMatrixTypeMatrix.MATRIX_RESERVE;
            task_provision_matrix_reduction.ReserveMatrixSimplex.GroupDep = group_dep;
            task_provision_matrix_reduction.ReserveMatrixSimplex.Period = period;
            period.Matrixs.Add(task_provision_matrix_reduction.ReserveMatrixSimplex);

            task_provision_matrix_reduction.ReserveMatrixEvristic = createMoneyMatrix(os,task_provision_matrix_reduction);
            task_provision_matrix_reduction.ReserveMatrixEvristic.Status = HrmMatrixStatus.MATRIX_SAVED;
            task_provision_matrix_reduction.ReserveMatrixEvristic.Type = HrmMatrixType.TYPE_MATIX;
            task_provision_matrix_reduction.ReserveMatrixEvristic.TypeMatrix = HrmMatrixTypeMatrix.MATRIX_RESERVE;
            task_provision_matrix_reduction.ReserveMatrixEvristic.GroupDep = group_dep;
            task_provision_matrix_reduction.ReserveMatrixEvristic.Period = period;
            period.Matrixs.Add(task_provision_matrix_reduction.ReserveMatrixEvristic);

            return task_provision_matrix_reduction;
        }



        // Create money matrix
        public static HrmMatrixProvision createMoneyMatrix(IObjectSpace os, HrmSalaryTaskProvisionMatrixReduction card) {
            HrmAllocParameter alloc_parameters = card.AllocParameters;
            HrmMatrixProvision matrix = HrmSalaryTaskProvisionMatrixReductionLogic.MergeAllMatrixes(os, card);
            matrix.Period = card.Period;
            Dictionary<String, HrmAllocParameterOrderControl> order_controls =
                alloc_parameters.OrderControls.ToDictionary(x => x.Order.Code);
            foreach (HrmMatrixRow matrix_order in matrix.Rows) {
                String current_order_code = matrix_order.Order.Code;
                Decimal norm_kb = alloc_parameters.NormNoControlKB;
                Decimal norm_ozm = alloc_parameters.NormNoControlOZM;
                if (order_controls.ContainsKey(current_order_code)) {
                    norm_kb = order_controls[current_order_code].NormKB;
                    norm_ozm = order_controls[current_order_code].NormOZM;
                }
                foreach (var cell in matrix_order.Cells) {
                    if (cell.Column.Department.GroupDep == DepartmentGroupDep.DEPARTMENT_KB) {
                        cell.PlanMoney = norm_kb * cell.Time;
                    }
                    else {
                        cell.PlanMoney = norm_ozm * cell.Time;
                    }
                }
            }
            return matrix;
        }


    }
}