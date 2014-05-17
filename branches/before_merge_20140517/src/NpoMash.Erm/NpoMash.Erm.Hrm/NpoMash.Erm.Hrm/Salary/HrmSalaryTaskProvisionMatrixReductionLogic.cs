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
    public static class HrmSalaryTaskProvisionMatrixReductionLogic{

        public static HrmMatrix MergeAllMatrixes(IObjectSpace os, HrmSalaryTaskProvisionMatrixReduction card) {
            HrmMatrix result = os.CreateObject<HrmMatrix>();
            HrmMatrix m_plan_kb = card.MatrixPlanKB;
            HrmMatrix m_plan_ozm = card.MatrixPlanOZM;
            HrmMatrix m_res_kb = card.AllocResultKB;
            HrmMatrix m_res_ozm = card.AllocResultOZM;

            // это чтобы посмотреть, нет ли повторений среди колонок матриц
            //Dictionary<String, HrmMatrixColumn> cols_in_kb_plan = m_plan_kb.Columns.ToDictionary(x => x.Department.BuhCode);
            //Dictionary<String, HrmMatrixColumn> cols_in_ozm_plan = m_plan_ozm.Columns.ToDictionary(x => x.Department.BuhCode);
            //Dictionary<String, HrmMatrixColumn> cols_in_kb_res = m_res_kb.Columns.ToDictionary(x => x.Department.BuhCode);
            //Dictionary<String, HrmMatrixColumn> cols_in_ozm_res = m_res_ozm.Columns.ToDictionary(x => x.Department.BuhCode);

            Dictionary<String, Dictionary<String,HrmMatrixCell>> res_mat = new Dictionary<string, Dictionary<String,HrmMatrixCell>>();
            foreach(HrmMatrixColumn col in m_res_kb.Columns.Concat(m_res_ozm.Columns)){
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

      /*  public static HrmMatrix createtest(IObjectSpace os, HrmMatrix m) {
            HrmMatrix result_plan_matrix=m;
            var rand=new Random();

            foreach (var r in result_plan_matrix.Rows) {
                foreach (var c in r.Cells) {
                    c.MoneyReserve = rand.Next(1000, 100000);
                    c.MoneyNoReserve = rand.Next(100, 1000);
                }
            }
            return result_plan_matrix;
        }*/



        public static HrmSalaryTaskProvisionMatrixReduction initProvisonMatrixTask(IObjectSpace os, HrmPeriod period, DepartmentGroupDep group_dep) {
            HrmSalaryTaskProvisionMatrixReduction task_provision_matrix_reduction = os.CreateObject<HrmSalaryTaskProvisionMatrixReduction>();
            period.PeriodTasks.Add(task_provision_matrix_reduction);

            //Initiate provision matrix task
            HrmMatrix provision_matrix = os.CreateObject<HrmMatrix>();
            task_provision_matrix_reduction.GroupDep = DepartmentGroupDep.DEPARTMENT_KB_OZM;
            task_provision_matrix_reduction.AllocParameters = period.CurrentAllocParameter;
            task_provision_matrix_reduction.ProvisionMatrix = provision_matrix;
            task_provision_matrix_reduction.ProvisionMatrix.Status = HrmMatrixStatus.MATRIX_SAVED;
            task_provision_matrix_reduction.ProvisionMatrix.Type = HrmMatrixType.TYPE_MATIX;
            task_provision_matrix_reduction.ProvisionMatrix.TypeMatrix = HrmMatrixTypeMatrix.MATRIX_RESERVE;
            task_provision_matrix_reduction.ProvisionMatrix.GroupDep = group_dep;
            task_provision_matrix_reduction.CurrentTimeSheetKB = period.CurrentTimeSheetKB;
            task_provision_matrix_reduction.CurrentTimeSheetOZM = period.CurrentTimeSheetOZM;
            period.CurrentProvisionMatrix = task_provision_matrix_reduction;
            period.Matrixs.Add(task_provision_matrix_reduction.ProvisionMatrix);

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

           


            return task_provision_matrix_reduction;
        }


        
        // Create money matrix
        public static HrmMatrix createMoneyMatrix(IObjectSpace os, HrmSalaryTaskProvisionMatrixReduction card) {

            var alloc_parameters = card.AllocParameters;
            var matrix = HrmSalaryTaskProvisionMatrixReductionLogic.MergeAllMatrixes(os,card);
            Decimal norm_kb=0;
            Decimal norm_ozm = 0;
            bool key=false;

            foreach (var matrix_order in matrix.Rows) {
                key = false;
                foreach (var control_order in alloc_parameters.OrderControls) {
                    if (matrix_order.Order.Code == control_order.Order.Code) {
                        key = true;
                        norm_kb = control_order.Order.NormKB;
                        norm_ozm = control_order.Order.NormOZM;
                    }
                }

                if (key == true) {
                    foreach (var cell in matrix_order.Cells) {
                        if (cell.Column.Department.GroupDep == DepartmentGroupDep.DEPARTMENT_KB) { cell.PlanMoney = norm_kb * (cell.Time);  }
                        else { cell.PlanMoney = norm_ozm * (cell.Time); }
                    }
                }
                else if (key == false) {
                    foreach (var cell in matrix_order.Cells) {
                        if (cell.Column.Department.GroupDep == DepartmentGroupDep.DEPARTMENT_KB) { cell.PlanMoney = alloc_parameters.NormNoControlKB * (cell.Time); }
                        else { cell.PlanMoney = alloc_parameters.NormNoControlOZM *(cell.Time); }
                    }
                
                }

            }

            return matrix;
        }

       


        public static HrmMatrix calculateProvisionMatrix(IObjectSpace os, HrmSalaryTaskProvisionMatrixReduction card) {
            var matrix = card.ProvisionMatrix;

            //Цикл по подразделениям
            foreach (var column in matrix.Columns) {
                
                int cells_count = column.Cells.Count;
                int controlled_cells_count = 0;

                Decimal department_provision = 0;
                Decimal department_needs = 0;

                // Проверим сколько в подразделении контролируемых заказов
                foreach (var cell in column.Cells) {
                    if (cell.Row.Order.TypeControl == FmCOrderTypeControl.FOT || cell.Row.Order.TypeControl == FmCOrderTypeControl.TRUDEMK_FOT) {
                        controlled_cells_count++;
                    }
                }

                //Посчитаем резерв
                foreach (var cell in column.Cells) {
                    department_provision += cell.SourceProvision;
                    cell.SourceProvision = 0;
                }

                if (cells_count == controlled_cells_count) {
                    List<Decimal> diffs_list = new List<decimal>();
                    int zero_difference_orders = 0;

                    //Посчитаем сколько резерва надо на подразделение
                    foreach (var cell in column.Cells) {
                        Decimal difference = cell.PlanMoney - cell.MoneyNoReserve;
                        if (difference >= 0) {
                            department_needs += difference;
                        }
                        else { diffs_list.Add(Math.Abs(difference)); }
                    }

                    // Определимся хватит ли резерва
                    if (department_provision > department_needs) {

                        foreach (var cell in column.Cells) {
                            Decimal difference = cell.PlanMoney - cell.MoneyNoReserve;
                            if (difference > 0) { cell.SourceProvision += difference; department_provision -= difference; zero_difference_orders++; }
                        
                        }

                        while (department_provision != 0) {
                            Decimal min_waste = diffs_list.Min();

                            if (department_provision - (min_waste * controlled_cells_count) >= 0) {
                                foreach (var cell in column.Cells) {
                                    if (cell.PlanMoney - cell.MoneyNoReserve == 0) {
                                        cell.SourceProvision += min_waste;
                                        department_provision -= min_waste;
                                    }
                                }
                                diffs_list.Remove(min_waste);
                            }
                            else {
                                foreach (var cell in column.Cells) {
                                    if (cell.PlanMoney - cell.MoneyNoReserve == 0) {
                                        cell.SourceProvision += department_provision / zero_difference_orders;
                                        department_provision -= department_provision / zero_difference_orders;
                                    }
                                }
                            
                            }
                        }                    
                    
                                           
                    } else {
                        foreach (var cell in column.Cells) {
                            Decimal difference = cell.PlanMoney - cell.MoneyNoReserve;
                            if (difference > 0) { zero_difference_orders++; }

                        }
                        foreach (var cell in column.Cells) {
                            Decimal difference = cell.PlanMoney - cell.MoneyNoReserve;
                            if (difference > 0) {
                                cell.SourceProvision += department_provision / zero_difference_orders;
                                department_provision -= department_provision / zero_difference_orders;
                            }
                        }
                    }

                } else if (cells_count > controlled_cells_count) {
                    int uncontrolled_orders_count = cells_count-controlled_cells_count;
                    bool flag = false;


                    //Посчитаем сколько резерва надо на подразделение
                    foreach (var cell in column.Cells) {
                        foreach (var order in card.AllocParameters.OrderControls) {
                            if (cell.Row.Order.Code == order.Order.Code) {
                                var difference = cell.PlanMoney - cell.MoneyNoReserve;
                                if (difference > 0) {
                                    department_needs += difference;
                                }
                            }
                        }
                    }

                    if (department_provision >= department_needs) {


                        foreach (var cell in column.Cells) {
                            foreach (var order in card.AllocParameters.OrderControls) {
                                if (cell.Row.Order.Code == order.Order.Code) {
                                    var difference=cell.PlanMoney - cell.MoneyNoReserve;
                                    if ( difference > 0) {
                                        cell.SourceProvision += difference;
                                        department_provision -= difference;
                                    }
                                }
                            }
                        }


                        foreach (var cell in column.Cells) {
                            foreach (var order in card.AllocParameters.OrderControls) {
                                if (cell.Row.Order.Code == order.Order.Code) {
                                    flag = true;
                                }
                            }

                            if (flag == false) {
                                cell.SourceProvision += department_provision / uncontrolled_orders_count;
                            }  
                        }  
                    
                    } else {
                        int controlled_provide_orders = 0;

                        foreach (var cell in column.Cells) {
                            foreach (var order in card.AllocParameters.OrderControls) {
                                if (cell.Row.Order.Code == order.Order.Code) {
                                    var difference = cell.PlanMoney - cell.MoneyNoReserve;
                                    if (difference > 0) {
                                        controlled_provide_orders++;
                                    }
                                }
                            }
                        }

                        foreach (var cell in column.Cells) {
                            foreach (var order in card.AllocParameters.OrderControls) {
                                if (cell.Row.Order.Code == order.Order.Code) {
                                    var difference = cell.PlanMoney - cell.MoneyNoReserve;
                                    if (difference > 0) {
                                        cell.SourceProvision += department_provision / controlled_provide_orders;
                                    }
                                }
                            }
                        }
                    }

                } else if (controlled_cells_count == 0) {
                    List<Decimal> diffs_list = new List<decimal>();
                    int zero_difference_orders = 0;

                    //Посчитаем сколько резерва надо на подразделение
                    foreach (var cell in column.Cells) {
                        Decimal difference = cell.PlanMoney - cell.MoneyNoReserve;
                        if (difference >= 0) {
                            department_needs += difference;
                        }
                        else { diffs_list.Add(Math.Abs(difference)); }
                    }

                    // Определимся хватит ли резерва
                    if (department_provision > department_needs) {

                        foreach (var cell in column.Cells) {
                            Decimal difference = cell.PlanMoney - cell.MoneyNoReserve;
                            if (difference > 0) { cell.SourceProvision += difference; department_provision -= difference; zero_difference_orders++; }

                        }

                        while (department_provision != 0) {
                            Decimal min_waste = diffs_list.Min();

                            if (department_provision - (min_waste * controlled_cells_count) >= 0) {
                                foreach (var cell in column.Cells) {
                                    if (cell.PlanMoney - cell.MoneyNoReserve == 0) {
                                        cell.SourceProvision += min_waste;
                                        department_provision -= min_waste;
                                    }
                                }
                                diffs_list.Remove(min_waste);
                            }
                            else {
                                foreach (var cell in column.Cells) {
                                    if (cell.PlanMoney - cell.MoneyNoReserve == 0) {
                                        cell.SourceProvision += department_provision / zero_difference_orders;
                                        department_provision -= department_provision / zero_difference_orders;
                                    }
                                }

                            }
                        }


                    }
                    else {
                        foreach (var cell in column.Cells) {
                            Decimal difference = cell.PlanMoney - cell.MoneyNoReserve;
                            if (difference > 0) { zero_difference_orders++; }

                        }
                        foreach (var cell in column.Cells) {
                            Decimal difference = cell.PlanMoney - cell.MoneyNoReserve;
                            if (difference > 0) {
                                cell.SourceProvision += department_provision / zero_difference_orders;
                                department_provision -= department_provision / zero_difference_orders;
                            }
                        }
                    }
                
                
                }

            }

            return null;
        }

    }
}