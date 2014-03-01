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
    public class HrmSalaryTaskProvisionMatrixReductionLogic : BaseObject {

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
            period.Matrixs.Add(task_provision_matrix_reduction.ProvisionMatrix);

            // Get coerced matrix from period
            foreach (HrmMatrix matrix in period.Matrixs) {
                if (matrix.TypeMatrix == HrmMatrixTypeMatrix.MATRIX_COERCED &&
                    matrix.Status == HrmMatrixStatus.MATRIX_EXPORTED &&
                    matrix.GroupDep == DepartmentGroupDep.DEPARTMENT_KB) {
                    task_provision_matrix_reduction.MatrixAllocKB = os.CreateObject<HrmMatrix>();
                    task_provision_matrix_reduction.MatrixAllocKB = matrix;
                }
                else if (matrix.TypeMatrix == HrmMatrixTypeMatrix.MATRIX_COERCED &&
                    matrix.Status == HrmMatrixStatus.MATRIX_EXPORTED &&
                    matrix.GroupDep == DepartmentGroupDep.DEPARTMENT_OZM) {
                    task_provision_matrix_reduction.MatrixAllocOZM = os.CreateObject<HrmMatrix>();
                    task_provision_matrix_reduction.MatrixAllocOZM = matrix;
                }
            }

            // Get alloc result from period
            foreach (HrmMatrix matrix in period.Matrixs) {
                if (matrix.GroupDep == DepartmentGroupDep.DEPARTMENT_KB && matrix.Type == HrmMatrixType.TYPE_ALLOC_RESULT &&
                    matrix.Status == HrmMatrixStatus.MATRIX_ACCEPTED) {
                    task_provision_matrix_reduction.AllocResultKB = os.CreateObject<HrmMatrix>();
                    task_provision_matrix_reduction.AllocResultKB = matrix;
                }
                else if (matrix.GroupDep == DepartmentGroupDep.DEPARTMENT_OZM && matrix.Type == HrmMatrixType.TYPE_ALLOC_RESULT &&
                    matrix.Status == HrmMatrixStatus.MATRIX_ACCEPTED) {
                    task_provision_matrix_reduction.AllocResultOZM = os.CreateObject<HrmMatrix>();
                    task_provision_matrix_reduction.AllocResultOZM = matrix;
                }
            }

            // Get Planned Matrixs
            foreach (HrmMatrix matrix in period.Matrixs) {
                if (matrix.GroupDep == DepartmentGroupDep.DEPARTMENT_KB && matrix.Type == HrmMatrixType.TYPE_MATIX &&
                    matrix.Status == HrmMatrixStatus.MATRIX_ACCEPTED && matrix.TypeMatrix==HrmMatrixTypeMatrix.MATRIX_PLANNED) {
                    task_provision_matrix_reduction.MatrixplanKB = os.CreateObject<HrmMatrix>();
                    task_provision_matrix_reduction.MatrixplanKB = matrix;
                }
                else if (matrix.GroupDep == DepartmentGroupDep.DEPARTMENT_OZM && matrix.Type == HrmMatrixType.TYPE_MATIX &&
                    matrix.Status == HrmMatrixStatus.MATRIX_ACCEPTED && matrix.TypeMatrix == HrmMatrixTypeMatrix.MATRIX_PLANNED) {
                    task_provision_matrix_reduction.MatrixPlanOZM = os.CreateObject<HrmMatrix>();
                    task_provision_matrix_reduction.MatrixPlanOZM = matrix;
                }
            }

            return task_provision_matrix_reduction;
        }


        // Merge planned matrix
        public static HrmMatrix mergePlanMatrixes(IObjectSpace os, HrmMatrix kb_plan_matrix, HrmMatrix ozm_plan_matrix) {
            HrmMatrix result_plan_matrix = os.CreateObject<HrmMatrix>();

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
        public static HrmMatrix mergeCorcedMatrixs(IObjectSpace os, HrmMatrix kb_corced_matrix, HrmMatrix ozm_coerced_matrix) {
            HrmMatrix result_coerced_matrix = os.CreateObject<HrmMatrix>();

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
        public static HrmMatrix createMoneyMatrix(IObjectSpace os, HrmMatrix plan_matrix, HrmPeriodAllocParameter alloc_parameters) {
            HrmMatrix money_matrix = os.CreateObject<HrmMatrix>();
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
                                    plan_cell.Money = control_order.NormKB * (plan_cell.Time + plan_cell.TravelTime);

                                }
                                else if (plan_cell.Column.Department.GroupDep == DepartmentGroupDep.DEPARTMENT_OZM) {
                                    plan_cell.Money = control_order.NormOZM * (plan_cell.Time + plan_cell.TravelTime);
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







        
        public static HrmMatrix calculateProvisionMatrix() { return null; }



        public HrmSalaryTaskProvisionMatrixReductionLogic(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }
}