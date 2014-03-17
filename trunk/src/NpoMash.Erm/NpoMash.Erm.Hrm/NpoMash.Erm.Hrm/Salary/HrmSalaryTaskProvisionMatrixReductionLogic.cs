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
            HrmMatrix m_plan_kb = card.MatrixplanKB;
            HrmMatrix m_plan_ozm = card.MatrixPlanOZM;
            HrmMatrix m_res_kb = card.AllocResultKB;
            HrmMatrix m_res_ozm = card.AllocResultOZM;
            
            Dictionary<String, Dictionary<String,HrmMatrixCell>> res_mat = new Dictionary<string, Dictionary<String,HrmMatrixCell>>();
            foreach(HrmMatrixColumn col in m_res_kb.Columns.Concat(m_res_ozm.Columns)){
                String dep_code = col.Department.Code;
                Dictionary<String, HrmMatrixCell> dict = col.Cells.ToDictionary(x => x.Row.Order.Code);
                res_mat.Add(dep_code, dict);
            }

            Dictionary<String, HrmMatrixRow> created_rows = new Dictionary<string, HrmMatrixRow>();

            foreach (HrmMatrixColumn current_column in m_plan_kb.Columns.Concat(m_plan_ozm.Columns)) {
                HrmMatrixColumn result_column = os.CreateObject<HrmMatrixColumn>();
                String dep_code = current_column.Department.Code;
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
                    result_cell.MoneyReserve = res_mat[dep_code][ord_code].MoneyReserve;
                    result_cell.MoneyNoReserve = res_mat[dep_code][ord_code].MoneyNoReserve;
                }
            }
            return result;
        }

        public static HrmSalaryTaskProvisionMatrixReduction initProvisonMatrixTask(IObjectSpace os, HrmPeriod period, DepartmentGroupDep group_dep) {
            HrmSalaryTaskProvisionMatrixReduction task_provision_matrix_reduction = os.CreateObject<HrmSalaryTaskProvisionMatrixReduction>();
            period.PeriodTasks.Add(task_provision_matrix_reduction);

            //Initiate provision matrix task
            HrmMatrix provision_matrix = os.CreateObject<HrmMatrix>();
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
                    matrix.Status == HrmMatrixStatus.MATRIX_EXPORTED &&
                    matrix.GroupDep == DepartmentGroupDep.DEPARTMENT_KB) {
                    task_provision_matrix_reduction.MatrixAllocKB = matrix;
                }
                else if (matrix.TypeMatrix == HrmMatrixTypeMatrix.MATRIX_COERCED &&
                    matrix.Status == HrmMatrixStatus.MATRIX_EXPORTED &&
                    matrix.GroupDep == DepartmentGroupDep.DEPARTMENT_OZM) {
                    task_provision_matrix_reduction.MatrixAllocOZM = matrix;
                }
            }

            // Get alloc result from period
            foreach (HrmMatrix matrix in period.Matrixs) {
                if (matrix.GroupDep == DepartmentGroupDep.DEPARTMENT_KB && matrix.Type == HrmMatrixType.TYPE_ALLOC_RESULT &&
                    matrix.Status == HrmMatrixStatus.MATRIX_ACCEPTED) {
                    task_provision_matrix_reduction.AllocResultKB = matrix;
                }
                else if (matrix.GroupDep == DepartmentGroupDep.DEPARTMENT_OZM && matrix.Type == HrmMatrixType.TYPE_ALLOC_RESULT &&
                    matrix.Status == HrmMatrixStatus.MATRIX_ACCEPTED) {
                    task_provision_matrix_reduction.AllocResultOZM = matrix;
                }
            }

            // Get Planned Matrixs
            foreach (HrmMatrix matrix in period.Matrixs) {
                if (matrix.GroupDep == DepartmentGroupDep.DEPARTMENT_KB && matrix.Type == HrmMatrixType.TYPE_MATIX &&
                    matrix.Status == HrmMatrixStatus.MATRIX_ACCEPTED && matrix.TypeMatrix==HrmMatrixTypeMatrix.MATRIX_PLANNED) {
                    task_provision_matrix_reduction.MatrixplanKB = matrix;
                }
                else if (matrix.GroupDep == DepartmentGroupDep.DEPARTMENT_OZM && matrix.Type == HrmMatrixType.TYPE_MATIX &&
                    matrix.Status == HrmMatrixStatus.MATRIX_ACCEPTED && matrix.TypeMatrix == HrmMatrixTypeMatrix.MATRIX_PLANNED) {
                    task_provision_matrix_reduction.MatrixPlanOZM = matrix;
                }
            }

            return task_provision_matrix_reduction;
        }


        // Merge planned matrix
        public static HrmMatrix mergePlanMatrixes(IObjectSpace os, HrmSalaryTaskProvisionMatrixReduction card) {
            HrmMatrix result_plan_matrix = os.CreateObject<HrmMatrix>();
            var kb_plan_matrix = card.MatrixplanKB;
            var ozm_plan_matrix = card.MatrixPlanOZM;

            //+KB
            foreach (var plan_col in kb_plan_matrix.Columns) {
                HrmMatrixColumn new_col = os.CreateObject<HrmMatrixColumn>();
                result_plan_matrix.Columns.Add(new_col);
                new_col.Department = plan_col.Department;
            }

           foreach (var plan_row in kb_plan_matrix.Rows) {
               HrmMatrixRow new_row = os.CreateObject<HrmMatrixRow>();
               result_plan_matrix.Rows.Add(new_row);
               new_row.Order = plan_row.Order;
           }

           foreach (var plan_row in kb_plan_matrix.Rows) {
               HrmMatrixRow row = null;
               foreach (HrmMatrixRow res_row in result_plan_matrix.Rows) {
                   if (plan_row.Order == res_row.Order) {
                       row = res_row;
                       break;
                   }
               }
               foreach (var plan_cell in plan_row.Cells) {
                    HrmMatrixCell cell = os.CreateObject<HrmMatrixCell>();
                    HrmMatrixColumn col = null;
                    foreach (HrmMatrixColumn res_col in result_plan_matrix.Columns) {
                        if (plan_cell.Column.Department == res_col.Department) {
                            col = res_col;
                            break;
                        }
                    }
                    col.Cells.Add(cell);
                    row.Cells.Add(cell);
                    cell.Time = plan_cell.Time;
                   // Скопировать значения времени в созданную ячейку
                }
           }

            //+OZM

           foreach (var plan_col in ozm_plan_matrix.Columns) {
               HrmMatrixColumn new_col = os.CreateObject<HrmMatrixColumn>();
               result_plan_matrix.Columns.Add(new_col);
               new_col.Department = plan_col.Department;
           }

           foreach (var plan_row in ozm_plan_matrix.Rows) {
               HrmMatrixRow new_row = os.CreateObject<HrmMatrixRow>();
               result_plan_matrix.Rows.Add(new_row);
               new_row.Order = plan_row.Order;
           }

           foreach (var plan_row in ozm_plan_matrix.Rows) {
               HrmMatrixRow row = null;
               foreach (HrmMatrixRow res_row in result_plan_matrix.Rows) {
                   if (plan_row.Order == res_row.Order) {
                       row = res_row;
                       break;
                   }
               }
               foreach (var plan_cell in plan_row.Cells) {
                   HrmMatrixCell cell = os.CreateObject<HrmMatrixCell>();
                   HrmMatrixColumn col = null;
                   foreach (HrmMatrixColumn res_col in result_plan_matrix.Columns) {
                       if (plan_cell.Column.Department == res_col.Department) {
                           col = res_col;
                           break;
                       }
                   }
                   col.Cells.Add(cell);
                   row.Cells.Add(cell);
                   cell.Time = plan_cell.Time;
                   // Скопировать значения времени в созданную ячейку
               }
           }


            return result_plan_matrix; 
        }

        // Merge corced matrix
        public static HrmMatrix mergeCorcedMatrixs(IObjectSpace os, HrmSalaryTaskProvisionMatrixReduction card) {
            HrmMatrix result_coerced_matrix = os.CreateObject<HrmMatrix>();
            var kb_corced_matrix = card.MatrixAllocKB;
            var ozm_coerced_matrix = card.MatrixAllocOZM;
            //+KB
            foreach (var plan_col in kb_corced_matrix.Columns) {
                HrmMatrixColumn new_col = os.CreateObject<HrmMatrixColumn>();
                result_coerced_matrix.Columns.Add(new_col);
                new_col.Department = plan_col.Department;
            }

            foreach (var plan_row in kb_corced_matrix.Rows) {
                HrmMatrixRow new_row = os.CreateObject<HrmMatrixRow>();
                result_coerced_matrix.Rows.Add(new_row);
                new_row.Order = plan_row.Order;
            }

            foreach (var plan_row in kb_corced_matrix.Rows) {
                HrmMatrixRow row = null;
                foreach (HrmMatrixRow res_row in result_coerced_matrix.Rows) {
                    if (plan_row.Order == res_row.Order) {
                        row = res_row;
                        break;
                    }
                }
                foreach (var plan_cell in plan_row.Cells) {
                    HrmMatrixCell cell = os.CreateObject<HrmMatrixCell>();
                    HrmMatrixColumn col = null;
                    foreach (HrmMatrixColumn res_col in result_coerced_matrix.Columns) {
                        if (plan_cell.Column.Department == res_col.Department) {
                            col = res_col;
                            break;
                        }
                    }
                    col.Cells.Add(cell);
                    row.Cells.Add(cell);
                    cell.Time = plan_cell.Time;
                    // Скопировать значения времени в созданную ячейку
                }
            }

            //+OZM
            foreach (var plan_col in ozm_coerced_matrix.Columns) {
                HrmMatrixColumn new_col = os.CreateObject<HrmMatrixColumn>();
                result_coerced_matrix.Columns.Add(new_col);
                new_col.Department = plan_col.Department;
            }

            foreach (var plan_row in ozm_coerced_matrix.Rows) {
                HrmMatrixRow new_row = os.CreateObject<HrmMatrixRow>();
                result_coerced_matrix.Rows.Add(new_row);
                new_row.Order = plan_row.Order;
            }

            foreach (var plan_row in ozm_coerced_matrix.Rows) {
                HrmMatrixRow row = null;
                foreach (HrmMatrixRow res_row in result_coerced_matrix.Rows) {
                    if (plan_row.Order == res_row.Order) {
                        row = res_row;
                        break;
                    }
                }
                foreach (var plan_cell in plan_row.Cells) {
                    HrmMatrixCell cell = os.CreateObject<HrmMatrixCell>();
                    HrmMatrixColumn col = null;
                    foreach (HrmMatrixColumn res_col in result_coerced_matrix.Columns) {
                        if (plan_cell.Column.Department == res_col.Department) {
                            col = res_col;
                            break;
                        }
                    }
                    col.Cells.Add(cell);
                    row.Cells.Add(cell);
                    cell.Time = plan_cell.Time;
                    // Скопировать значения времени в созданную ячейку
                }
            }

            return result_coerced_matrix;
        }
        // Create money matrix
        public static HrmMatrix createMoneyMatrix(IObjectSpace os, HrmSalaryTaskProvisionMatrixReduction card) {
            HrmMatrix money_matrix = os.CreateObject<HrmMatrix>();
            var alloc_parameters = card.AllocParameters;
            var plan_matrix = HrmSalaryTaskProvisionMatrixReductionLogic.mergePlanMatrixes(os, card);
            //Шагаем по строкам плановой(труд) матрицы
            foreach (var plan_orders in plan_matrix.Rows) {

                // Заходим с заказом из этой строчки в параметры расчета
                foreach (var control_order in alloc_parameters.OrderControls) {
                    //Если Код_заказа и Тип_контроля заказа из плановой матрицы совпали с кодом и типом из параметров расчета
                    if (plan_orders.Order.Code == control_order.Order.Code && plan_orders.Order.TypeControl == control_order.Order.TypeControl) {
                        // Проверяем ФОТ или Труд+ФОТ
                        if (plan_orders.Order.TypeControl == FmCOrderTypeControl.FOT || plan_orders.Order.TypeControl == FmCOrderTypeControl.TRUDEMK_FOT) {
                            // Если да, то проверяем идем по ячейкам и проверяемпринадлежность к подразделению;
                            foreach (var plan_cell in plan_orders.Cells) {
                                //Если вдруг КБ
                                if (plan_cell.Column.Department.GroupDep == DepartmentGroupDep.DEPARTMENT_KB) {
                                    plan_cell.PlanMoney = control_order.NormKB * (plan_cell.Time);

                                }
                                else if (plan_cell.Column.Department.GroupDep == DepartmentGroupDep.DEPARTMENT_OZM) {
                                    plan_cell.PlanMoney = control_order.NormOZM * (plan_cell.Time);
                                }


                            }
                        } // Если случилось такое что заказ неконтролируемый
                        else if (plan_orders.Order.TypeControl == FmCOrderTypeControl.NO_ORDERED) {
                            foreach (var plan_cell in plan_orders.Cells) {
                                // HrmMatrixRow new_row = os.CreateObject<HrmMatrixRow>();
                                // HrmMatrixColumn new_column = os.CreateObject<HrmMatrixColumn>();
                                //Если вдруг КБ
                                if (plan_cell.Column.Department.GroupDep == DepartmentGroupDep.DEPARTMENT_KB) {
                                 

                                }
                                else if (plan_cell.Column.Department.GroupDep == DepartmentGroupDep.DEPARTMENT_OZM) {
                                   
                                }
                            }
                        }
                    }
                }
            }
            money_matrix = plan_matrix;
            return money_matrix;
        }

        public static HrmMatrix mergeAllocResults(IObjectSpace os, HrmSalaryTaskProvisionMatrixReduction card) {
            HrmMatrix alloc_result_kb_ozm = os.CreateObject<HrmMatrix>();
            var kb_alloc_result = card.AllocResultKB;
            var ozm_alloc_result = card.AllocResultOZM;
            //+KB
            foreach (var plan_col in kb_alloc_result.Columns) {
                HrmMatrixColumn new_col = os.CreateObject<HrmMatrixColumn>();
                alloc_result_kb_ozm.Columns.Add(new_col);
                new_col.Department = plan_col.Department;
            }

            foreach (var plan_row in kb_alloc_result.Rows) {
                HrmMatrixRow new_row = os.CreateObject<HrmMatrixRow>();
                alloc_result_kb_ozm.Rows.Add(new_row);
                new_row.Order = plan_row.Order;
            }

            foreach (var plan_row in kb_alloc_result.Rows) {
                HrmMatrixRow row = null;
                foreach (HrmMatrixRow res_row in alloc_result_kb_ozm.Rows) {
                    if (plan_row.Order == res_row.Order) {
                        row = res_row;
                        break;
                    }
                }
                foreach (var plan_cell in plan_row.Cells) {
                    HrmMatrixCell cell = os.CreateObject<HrmMatrixCell>();
                    HrmMatrixColumn col = null;
                    foreach (HrmMatrixColumn res_col in alloc_result_kb_ozm.Columns) {
                        if (plan_cell.Column.Department == res_col.Department) {
                            col = res_col;
                            break;
                        }
                    }
                    col.Cells.Add(cell);
                    row.Cells.Add(cell);
                    cell.MoneyNoReserve = plan_cell.MoneyNoReserve;
                    cell.MoneyReserve = plan_cell.MoneyReserve;
                    // Скопировать значения времени в созданную ячейку
                }
            }

            //+OZM
            foreach (var plan_col in ozm_alloc_result.Columns) {
                HrmMatrixColumn new_col = os.CreateObject<HrmMatrixColumn>();
                alloc_result_kb_ozm.Columns.Add(new_col);
                new_col.Department = plan_col.Department;
            }

            foreach (var plan_row in ozm_alloc_result.Rows) {
                HrmMatrixRow new_row = os.CreateObject<HrmMatrixRow>();
                alloc_result_kb_ozm.Rows.Add(new_row);
                new_row.Order = plan_row.Order;
            }

            foreach (var plan_row in ozm_alloc_result.Rows) {
                HrmMatrixRow row = null;
                foreach (HrmMatrixRow res_row in alloc_result_kb_ozm.Rows) {
                    if (plan_row.Order == res_row.Order) {
                        row = res_row;
                        break;
                    }
                }
                foreach (var plan_cell in plan_row.Cells) {
                    HrmMatrixCell cell = os.CreateObject<HrmMatrixCell>();
                    HrmMatrixColumn col = null;
                    foreach (HrmMatrixColumn res_col in alloc_result_kb_ozm.Columns) {
                        if (plan_cell.Column.Department == res_col.Department) {
                            col = res_col;
                            break;
                        }
                    }
                    col.Cells.Add(cell);
                    row.Cells.Add(cell);
                    cell.MoneyNoReserve = plan_cell.MoneyNoReserve;
                    cell.MoneyReserve = plan_cell.MoneyReserve;
                    // Скопировать значения времени в созданную ячейку
                }
            }

            return alloc_result_kb_ozm;
        }

        public static HrmMatrix combineMatrixes(IObjectSpace os, HrmSalaryTaskProvisionMatrixReduction card) {
            HrmMatrix provision = os.CreateObject<HrmMatrix>();
            HrmMatrix plan_mat = card.MatrixPlanMoney;
            HrmMatrix alloc_result = card.AllocResultKBOZM;
            provision.Type = HrmMatrixType.TYPE_MATIX;
            provision.TypeMatrix = HrmMatrixTypeMatrix.MATRIX_RESERVE;
            provision.Status = HrmMatrixStatus.MATRIX_DOWNLOADED;
            Dictionary<String, HrmMatrixRow> created_rows = new Dictionary<string, HrmMatrixRow>();
            //словарь с ячейками по ключу "код колонки,код строки"
            Dictionary<String, Dictionary<String, HrmMatrixCell>> dictionary_of_plan = new Dictionary<String, Dictionary<String, HrmMatrixCell>>();
            foreach (HrmMatrixColumn col in plan_mat.Columns) {
                Dictionary<String, HrmMatrixCell> d = col.Cells.ToDictionary<HrmMatrixCell, String>(x => x.Row.Order.Code);
                dictionary_of_plan.Add(col.Department.Code, d);
            }

            foreach (HrmMatrixColumn col in alloc_result.Columns) {
                String col_code = col.Department.Code;
                if (!dictionary_of_plan.ContainsKey(col_code))
                    throw new Exception("There is now department in alloc_result with code " + col_code);
                HrmMatrixColumn current_column = os.CreateObject<HrmMatrixColumn>();
                current_column.Department = col.Department;
                current_column.Matrix = provision;
                provision.Columns.Add(current_column);
                foreach (HrmMatrixCell cell in col.Cells) {
                    String row_code = cell.Row.Order.Code;
                    if (!dictionary_of_plan[col_code].ContainsKey(row_code))
                        throw new Exception("There is now cell in alloc_result with dep_code " + col_code + "and row_code "+row_code);
                    HrmMatrixRow current_row = null;
                    if (created_rows.ContainsKey(row_code))
                        current_row = created_rows[row_code];
                    else {
                        current_row = os.CreateObject<HrmMatrixRow>();
                        current_row.Order = cell.Row.Order;
                        current_row.Matrix = provision;
                        provision.Rows.Add(current_row);
                        created_rows.Add(row_code, current_row);
                    }
                    HrmMatrixCell current_cell = os.CreateObject<HrmMatrixCell>();
                    current_cell.Column = current_column;
                    current_column.Cells.Add(current_cell);
                    current_cell.Row = current_row;
                    current_row.Cells.Add(current_cell);
                    current_cell.MoneyReserve = cell.MoneyReserve;
                    current_cell.MoneyNoReserve = cell.MoneyNoReserve;
                    current_cell.PlanMoney = dictionary_of_plan[col_code][row_code].PlanMoney;
                }
            }
            return provision;
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
                    department_provision += cell.MoneyReserve;
                    cell.MoneyReserve = 0;
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
                            if (difference > 0) { cell.MoneyReserve += difference; department_provision -= difference; zero_difference_orders++; }
                        
                        }

                        while (department_provision != 0) {
                            Decimal min_waste = diffs_list.Min();

                            if (department_provision - (min_waste * controlled_cells_count) >= 0) {
                                foreach (var cell in column.Cells) {
                                    if (cell.PlanMoney - cell.MoneyNoReserve == 0) {
                                        cell.MoneyReserve += min_waste;
                                        department_provision -= min_waste;
                                    }
                                }
                                diffs_list.Remove(min_waste);
                            }
                            else {
                                foreach (var cell in column.Cells) {
                                    if (cell.PlanMoney - cell.MoneyNoReserve == 0) {
                                        cell.MoneyReserve += department_provision / zero_difference_orders;
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
                                cell.MoneyReserve += department_provision / zero_difference_orders;
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
                                        cell.MoneyReserve += difference;
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
                                cell.MoneyReserve += department_provision / uncontrolled_orders_count;
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
                                        cell.MoneyReserve += department_provision / controlled_provide_orders;
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
                            if (difference > 0) { cell.MoneyReserve += difference; department_provision -= difference; zero_difference_orders++; }

                        }

                        while (department_provision != 0) {
                            Decimal min_waste = diffs_list.Min();

                            if (department_provision - (min_waste * controlled_cells_count) >= 0) {
                                foreach (var cell in column.Cells) {
                                    if (cell.PlanMoney - cell.MoneyNoReserve == 0) {
                                        cell.MoneyReserve += min_waste;
                                        department_provision -= min_waste;
                                    }
                                }
                                diffs_list.Remove(min_waste);
                            }
                            else {
                                foreach (var cell in column.Cells) {
                                    if (cell.PlanMoney - cell.MoneyNoReserve == 0) {
                                        cell.MoneyReserve += department_provision / zero_difference_orders;
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
                                cell.MoneyReserve += department_provision / zero_difference_orders;
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