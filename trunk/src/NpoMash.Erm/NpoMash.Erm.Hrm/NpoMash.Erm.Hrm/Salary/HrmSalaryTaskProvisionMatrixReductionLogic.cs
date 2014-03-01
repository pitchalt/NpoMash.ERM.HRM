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

            //Initiate provision matrix
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
                if ( matrix.GroupDep == DepartmentGroupDep.DEPARTMENT_KB && matrix.Type==HrmMatrixType.TYPE_ALLOC_RESULT &&
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

               return task_provision_matrix_reduction;
        }

        public static HrmMatrix createMoneyMatrix(IObjectSpace os, HrmMatrix plan_matrix,HrmPeriodAllocParameter alloc_parameters) {
            HrmMatrix money_matrix = os.CreateObject<HrmMatrix>();
            //Шагаем по строкам плановой(труд) матрицы
            foreach (var plan_orders in plan_matrix.Rows) {

                // Заходим с заказом из этой строчки в параметры расчета
                foreach (var control_order in alloc_parameters.OrderControls) {
                    //Если Код_заказа и Тип_контроля заказа из плановой матрицы совпали с кодом и типом из параметров расчета
                    if (plan_orders.Order.Code == control_order.Order.Code && plan_orders.Order.TypeControl == control_order.Order.TypeControl) {
                       // Проверяем ФОТ или Труд+ФОТ
                        if (plan_orders.Order.TypeControl == FmCOrderTypeControl.FOT || plan_orders.Order.TypeControl==FmCOrderTypeControl.TRUDEMK_FOT) {
                            // Если да, то проверяем идем по ячейкам и проверяемпринадлежность к подразделению;
                            foreach (var plan_cell in plan_orders.Cells) {
                                HrmMatrixRow new_row = os.CreateObject<HrmMatrixRow>();
                                HrmMatrixColumn new_column = os.CreateObject<HrmMatrixColumn>();
                                //Если вдруг КБ
                                if (plan_cell.Column.Department.GroupDep == DepartmentGroupDep.DEPARTMENT_KB) {
                                    HrmMatrixCell new_cell = os.CreateObject<HrmMatrixCell>(); // Создали пустую ячейку
                                    
                                    new_cell.Money = Convert.ToInt64(plan_orders.Order.NormKB * (plan_cell.Time + plan_cell.TravelTime)); // Посчитали деньги
                                    new_cell.Time = plan_cell.Time;
                                    new_cell.TravelTime = plan_cell.TravelTime;

                                    new_row.Order = os.CreateObject<fmCOrder>();
                                    new_row.Order.Code = plan_orders.Order.Code;
                                    new_row.Order.TypeControl = plan_orders.Order.TypeControl;
                                    new_row.Order.TypeConstancy = plan_orders.Order.TypeConstancy;
                                    new_row.Order.NormKB = plan_orders.Order.NormKB;
                                    new_row.Order.NormOZM = plan_orders.Order.NormOZM;
                                    new_row.Cells.Add(new_cell);

                                    new_column.Department = os.CreateObject<Department>();
                                    new_column.Department.GroupDep = plan_cell.Column.Department.GroupDep;
                                    new_column.Department.Code = plan_cell.Column.Department.Code;
                                    new_column.Department.BuhCode = plan_cell.Column.Department.BuhCode;
                                    new_column.Cells.Add(new_cell);

                                    money_matrix.Columns.Add(new_column);
                                    money_matrix.Rows.Add(new_row);


                                }
                                else if (plan_cell.Column.Department.GroupDep == DepartmentGroupDep.DEPARTMENT_OZM) {

                                    HrmMatrixCell new_cell = os.CreateObject<HrmMatrixCell>(); // Создали пустую ячейку

                                    new_cell.Money = Convert.ToInt64(plan_orders.Order.NormKB * (plan_cell.Time + plan_cell.TravelTime)); // Посчитали деньги
                                    new_cell.Time = plan_cell.Time;
                                    new_cell.TravelTime = plan_cell.TravelTime;

                                    new_row.Order = os.CreateObject<fmCOrder>();
                                    new_row.Order.Code = plan_orders.Order.Code;
                                    new_row.Order.TypeControl = plan_orders.Order.TypeControl;
                                    new_row.Order.TypeConstancy = plan_orders.Order.TypeConstancy;
                                    new_row.Order.NormKB = plan_orders.Order.NormKB;
                                    new_row.Order.NormOZM = plan_orders.Order.NormOZM;
                                    new_row.Cells.Add(new_cell);

                                    new_column.Department = os.CreateObject<Department>();
                                    new_column.Department.GroupDep = plan_cell.Column.Department.GroupDep;
                                    new_column.Department.Code = plan_cell.Column.Department.Code;
                                    new_column.Department.BuhCode = plan_cell.Column.Department.BuhCode;
                                    new_column.Cells.Add(new_cell);

                                    money_matrix.Columns.Add(new_column);
                                    money_matrix.Rows.Add(new_row);
                                }
                            
                            
                            }
                        } // Если случилось такое что заказ неконтролируемый
                        else if (plan_orders.Order.TypeControl == FmCOrderTypeControl.NO_ORDERED) {//Пока ничего не делаем 
                        }
                }
                }
            }
            return money_matrix;
        }


      

        // Merge planned matrix
        public static HrmMatrix createPlanMatrix(HrmPeriod period, IObjectSpace os) {
            HrmMatrix result_plan_matrix = os.CreateObject<HrmMatrix>();
            HrmMatrix kb_plan_matrix = null;
            HrmMatrix ozm_plan_matrix = null;
            // find plan matrixs for KB and OZM
            foreach (HrmMatrix matrix in period.Matrixs) {
                if (matrix.TypeMatrix == HrmMatrixTypeMatrix.MATRIX_PLANNED &&
                    matrix.Status == HrmMatrixStatus.MATRIX_ACCEPTED &&
                    matrix.GroupDep == DepartmentGroupDep.DEPARTMENT_KB) {
                        kb_plan_matrix = matrix;
                }
                else if (matrix.TypeMatrix == HrmMatrixTypeMatrix.MATRIX_PLANNED &&
                    matrix.Status == HrmMatrixStatus.MATRIX_ACCEPTED &&
                    matrix.GroupDep == DepartmentGroupDep.DEPARTMENT_OZM &&
                    matrix.Type==HrmMatrixType.TYPE_MATIX) {
                        ozm_plan_matrix = matrix;
                }
            }

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
               foreach (var result_row in result_plan_matrix.Rows) {
                   foreach (var plan_cell in plan_row.Cells) {
                       if (plan_row.Order.Code == result_row.Order.Code) {
                           result_row.Cells.Add(plan_cell);
                       }
                   }
               }
           }

           foreach (var plan_col in kb_plan_matrix.Columns) {
               foreach (var result_col in result_plan_matrix.Columns) {
                   foreach (var plan_cell in plan_col.Cells) {
                       if (plan_col.Department.Code == result_col.Department.Code) {
                           result_col.Cells.Add(plan_cell);
                       }
                   }
               }
           }


            //+OZM
           foreach (var plan_row in ozm_plan_matrix.Rows) {
               bool flag = false; //Есть такой заказ в новой матрице или еще нет
               HrmMatrixRow new_row = os.CreateObject<HrmMatrixRow>();
               HrmMatrixColumn new_col = os.CreateObject<HrmMatrixColumn>();

               foreach (var row in result_plan_matrix.Rows) {
                   if (plan_row.Order.Code == row.Order.Code) { flag = true; }
               }

               //Если такого заказа нет
               if (flag == false) {
                   new_row.Order = os.CreateObject<fmCOrder>();
                   new_row.Order.Code = plan_row.Order.Code;
                   new_row.Order.TypeControl = plan_row.Order.TypeControl;
                   new_row.Order.TypeConstancy = plan_row.Order.TypeConstancy;
                   new_row.Order.NormKB = plan_row.Order.NormKB;
                   new_row.Order.NormOZM = plan_row.Order.NormOZM;

                   foreach (var plan_cell in plan_row.Cells) {
                       bool flag1 = false;
                       HrmMatrixCell new_cel = os.CreateObject<HrmMatrixCell>();
                       new_cel.Sum = plan_cell.Sum;
                       new_cel.Time = plan_cell.Time;
                       new_cel.TravelTime = plan_cell.TravelTime;
                       new_row.Cells.Add(new_cel);


                       new_col.Department = os.CreateObject<Department>();
                       new_col.Department.Code = plan_cell.Column.Department.Code;
                       new_col.Department.BuhCode = plan_cell.Column.Department.BuhCode;
                       new_col.Department.GroupDep = plan_cell.Column.Department.GroupDep;
                       new_col.Cells.Add(new_cel);
                   }

                   result_plan_matrix.Rows.Add(new_row);
                   result_plan_matrix.Columns.Add(new_col);
               }
               else if (flag == true) { 
               // Ничего не делаем
               }

           }
            return result_plan_matrix; 
        }

        // Merge corced matrix
        public static HrmMatrix createCorcedMatrix(IObjectSpace os, HrmMatrix kb_corced_matrix, HrmMatrix ozm_coerced_matrix) {
            HrmMatrix result_coerced_matrix = os.CreateObject<HrmMatrix>();

            //+KB
            foreach (var plan_row in kb_corced_matrix.Rows) {
                HrmMatrixRow new_row = os.CreateObject<HrmMatrixRow>();
                HrmMatrixColumn new_col = os.CreateObject<HrmMatrixColumn>();

                new_row.Order = os.CreateObject<fmCOrder>();
                new_row.Order.Code = plan_row.Order.Code;
                new_row.Order.TypeControl = plan_row.Order.TypeControl;
                new_row.Order.TypeConstancy = plan_row.Order.TypeConstancy;
                new_row.Order.NormKB = plan_row.Order.NormKB;
                new_row.Order.NormOZM = plan_row.Order.NormOZM;

                foreach (var plan_cell in plan_row.Cells) {
                    HrmMatrixCell new_cel = os.CreateObject<HrmMatrixCell>();
                    new_cel.Sum = plan_cell.Sum;
                    new_cel.Time = plan_cell.Time;
                    new_cel.TravelTime = plan_cell.TravelTime;
                    new_row.Cells.Add(new_cel);

                    new_col.Department = os.CreateObject<Department>();
                    new_col.Department.Code = plan_cell.Column.Department.Code;
                    new_col.Department.BuhCode = plan_cell.Column.Department.BuhCode;
                    new_col.Department.GroupDep = plan_cell.Column.Department.GroupDep;
                    new_col.Cells.Add(new_cel);
                }

                result_coerced_matrix.Rows.Add(new_row);
                result_coerced_matrix.Columns.Add(new_col);
            }

            //+OZM
            foreach (var plan_row in ozm_coerced_matrix.Rows) {
                bool flag = false; //Есть такой заказ в новой матрице или еще нет
                HrmMatrixRow new_row = os.CreateObject<HrmMatrixRow>();
                HrmMatrixColumn new_col = os.CreateObject<HrmMatrixColumn>();

                foreach (var row in result_coerced_matrix.Rows) {
                    if (plan_row == row) { flag = true; }
                }

                //Если такого заказа нет
                if (flag == false) {
                    new_row.Order = os.CreateObject<fmCOrder>();
                    new_row.Order.Code = plan_row.Order.Code;
                    new_row.Order.TypeControl = plan_row.Order.TypeControl;
                    new_row.Order.TypeConstancy = plan_row.Order.TypeConstancy;
                    new_row.Order.NormKB = plan_row.Order.NormKB;
                    new_row.Order.NormOZM = plan_row.Order.NormOZM;

                    foreach (var plan_cell in plan_row.Cells) {
                        HrmMatrixCell new_cel = os.CreateObject<HrmMatrixCell>();
                        new_cel.Sum = plan_cell.Sum;
                        new_cel.Time = plan_cell.Time;
                        new_cel.TravelTime = plan_cell.TravelTime;
                        new_row.Cells.Add(new_cel);

                        new_col.Department = os.CreateObject<Department>();
                        new_col.Department.Code = plan_cell.Column.Department.Code;
                        new_col.Department.BuhCode = plan_cell.Column.Department.BuhCode;
                        new_col.Department.GroupDep = plan_cell.Column.Department.GroupDep;
                        new_col.Cells.Add(new_cel);
                    }

                    result_coerced_matrix.Rows.Add(new_row);
                    result_coerced_matrix.Columns.Add(new_col);
                }
                else if (flag == true) {
                    // Ничего не делаем
                }

            }
            return result_coerced_matrix;
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