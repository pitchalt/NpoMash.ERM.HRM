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
            period.CurrentProvisionMatrix = task_provision_matrix_reduction;
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
                   // ����������� �������� ������� � ��������� ������
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
                   // ����������� �������� ������� � ��������� ������
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
                    // ����������� �������� ������� � ��������� ������
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
                    // ����������� �������� ������� � ��������� ������
                }
            }

            return result_coerced_matrix;
        }
        // Create money matrix
        public static HrmMatrix createMoneyMatrix(IObjectSpace os, HrmSalaryTaskProvisionMatrixReduction card) {
            HrmMatrix money_matrix = os.CreateObject<HrmMatrix>();
            var alloc_parameters = card.AllocParameters;
            var plan_matrix = HrmSalaryTaskProvisionMatrixReductionLogic.mergePlanMatrixes(os, card);
            //������ �� ������� ��������(����) �������
            foreach (var plan_orders in plan_matrix.Rows) {

                // ������� � ������� �� ���� ������� � ��������� �������
                foreach (var control_order in alloc_parameters.OrderControls) {
                    //���� ���_������ � ���_�������� ������ �� �������� ������� ������� � ����� � ����� �� ���������� �������
                    if (plan_orders.Order.Code == control_order.Order.Code && plan_orders.Order.TypeControl == control_order.Order.TypeControl) {
                        // ��������� ��� ��� ����+���
                        if (plan_orders.Order.TypeControl == FmCOrderTypeControl.FOT || plan_orders.Order.TypeControl == FmCOrderTypeControl.TRUDEMK_FOT) {
                            // ���� ��, �� ��������� ���� �� ������� � ����������������������� � �������������;
                            foreach (var plan_cell in plan_orders.Cells) {
                                //���� ����� ��
                                if (plan_cell.Column.Department.GroupDep == DepartmentGroupDep.DEPARTMENT_KB) {
                                    plan_cell.PlanMoney = control_order.NormKB * (plan_cell.Time);

                                }
                                else if (plan_cell.Column.Department.GroupDep == DepartmentGroupDep.DEPARTMENT_OZM) {
                                    plan_cell.PlanMoney = control_order.NormOZM * (plan_cell.Time);
                                }


                            }
                        } // ���� ��������� ����� ��� ����� ����������������
                        else if (plan_orders.Order.TypeControl == FmCOrderTypeControl.NO_ORDERED) {
                            foreach (var plan_cell in plan_orders.Cells) {
                                // HrmMatrixRow new_row = os.CreateObject<HrmMatrixRow>();
                                // HrmMatrixColumn new_column = os.CreateObject<HrmMatrixColumn>();
                                //���� ����� ��
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
                    // ����������� �������� ������� � ��������� ������
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
                    // ����������� �������� ������� � ��������� ������
                }
            }

            return alloc_result_kb_ozm;
        }










        public static HrmMatrix calculateProvisionMatrix(IObjectSpace os, HrmSalaryTaskProvisionMatrixReduction card) {
            var plan_matrix = card.MatrixPlan;
            var alloc_result = card.AllocResultKBOZM;

            foreach (var column in alloc_result.Columns) {
                int a_key = column.Cells.Count;
                int b_key = 0;
                Decimal department_provision = 0;
                Decimal department_needs = 0;
                // ��������� ��������� ������� ��� ���
                foreach (var cell in column.Cells) {
                    if (cell.Row.Order.TypeControl == FmCOrderTypeControl.FOT || cell.Row.Order.TypeControl == FmCOrderTypeControl.TRUDEMK_FOT) {
                        b_key++;
                    }
                }

                //��������� ������
                foreach (var cell in column.Cells) {
                    department_provision += cell.MoneyReserve;
                }

                //���� ��������� ��������������
                if (b_key == a_key) {

                    foreach (var plan_column in plan_matrix.Columns) {
                        //���� ������� � ����� � � �������� �������
                        if (plan_column == column) {

                            //��������� ������� ������ ���� ��� ������ ����� �� �����
                            foreach (var plan_cell in plan_column.Cells) {
                                foreach (var alloc_cell in column.Cells) {
                                    if (plan_cell == alloc_cell) {
                                        Decimal difference = plan_cell.PlanMoney - alloc_cell.MoneyNoReserve;
                                        if (difference > 0) { department_needs += difference; }
                                    }
                                }
                            }
                            //���� ������ ������ ������������ 
                            if (department_provision >= department_needs) {
                                List<Decimal> diffs_list = new List<Decimal>();
                                int unzero_orders_count = 0;
                                //�� ������������ � ������ ������� �� ������ ��� �-�(����)>0
                                foreach (var plan_cell in plan_column.Cells) {
                                    foreach (var alloc_cell in column.Cells) {
                                        if (plan_cell == alloc_cell) {
                                            Decimal difference = plan_cell.PlanMoney - alloc_cell.MoneyNoReserve;
                                            //���� �������� ������ 0
                                            if (difference > 0) {
                                                alloc_cell.MoneyNoReserve += department_provision * (difference / department_provision);
                                                department_provision -= department_provision * (difference / department_provision);
                                                unzero_orders_count++;
                                            }
                                            //���� �������� ������ 0
                                            else if (difference < 0) {
                                                diffs_list.Add(Math.Abs(difference));
                                            }
                                        }
                                    }
                                }
                                // ��������� �� ������ ��� ������� ������ ��� ����������, �� ������ ����� ������ �������� �� �������, � ������ ��� ���� ������������
                                // ������� ������� ������ ����������� ���������� ����� ��� ����� ��� �-�<0, � ������� ��� ���������� � ������� ������ ��� �-�=0 

                                Decimal min_waste = diffs_list.Min(); // ������ ���������� ����������

                                //����� ���� ���������, ���� ������� ������� ������ ����� �� ���� ������� ���� �������

                                //���� ������/ ���� ������ �� ��������
                                while (department_provision != 0) {
                                    if (department_provision - (min_waste * unzero_orders_count) >= 0) {
                                        foreach (var plan_cell in plan_column.Cells) {
                                            foreach (var alloc_cell in column.Cells) {
                                                if (plan_cell == alloc_cell) {
                                                    Decimal difference = plan_cell.PlanMoney - alloc_cell.MoneyNoReserve;
                                                    if (difference == 0) {
                                                        alloc_cell.MoneyNoReserve += min_waste;
                                                        department_provision -= min_waste;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    //���� �� ������, �� ����������� ��������������� ���-�� �������
                                    else if (department_provision - (min_waste * unzero_orders_count) < 0 || min_waste == null) {
                                        foreach (var plan_cell in plan_column.Cells) {
                                            foreach (var alloc_cell in column.Cells) {
                                                if (plan_cell == alloc_cell) {
                                                    Decimal difference = plan_cell.PlanMoney - alloc_cell.MoneyNoReserve;
                                                    if (difference == 0) {
                                                        alloc_cell.MoneyNoReserve += department_provision / unzero_orders_count;
                                                        department_provision -= department_provision / unzero_orders_count;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    diffs_list.Remove(min_waste);
                                    min_waste = diffs_list.Min();
                                }

                            }
                            //���� ������� �� �������, �� ��������� ������������ ���� ���� ������
                            else if (department_provision <= department_needs) {
                                //�� ������������ �� ������ ��� �-�(����)>0
                                foreach (var plan_cell in plan_column.Cells) {
                                    if (department_provision != 0) {
                                        foreach (var alloc_cell in column.Cells) {
                                            if (plan_cell == alloc_cell) {
                                                Decimal difference = plan_cell.PlanMoney - alloc_cell.MoneyNoReserve;
                                                //���� �������� ������ 0
                                                // if (department_provision != 0) {
                                                if (difference > 0) {
                                                    alloc_cell.MoneyNoReserve += department_provision * (difference / department_provision);
                                                    department_provision -= department_provision * (difference / department_provision);
                                                }
                                                // }
                                                // else { break; }
                                            }
                                        }
                                    }
                                    else { break; }
                                }
                            }

                        }


                    }
                }
                //��������� ������������� 
                else if (b_key != 0 && b_key != a_key) {

                    int un_controled_orders_count = 0;

                    foreach (var plan_column in plan_matrix.Columns) {
                        //���� ������� � ����� � � �������� �������
                        if (plan_column == column) {
                            //��������� ������� ������ ���� ��� ������ ����� �� �����
                            foreach (var plan_cell in plan_column.Cells) {
                                foreach (var alloc_cell in column.Cells) {
                                    if (plan_cell == alloc_cell) {
                                        if (alloc_cell.Row.Order.TypeControl == FmCOrderTypeControl.FOT || alloc_cell.Row.Order.TypeControl == FmCOrderTypeControl.TRUDEMK_FOT) {
                                            Decimal difference = plan_cell.PlanMoney - alloc_cell.MoneyNoReserve;
                                            if (difference > 0) { department_needs += difference; }
                                        }
                                        else un_controled_orders_count++;
                                    }
                                }
                            }
                            //���� ������ ������ ������������ 
                            if (department_provision >= department_needs) {

                                //�� ������������ � ������ ������� �� ������ ��� �-�(����)>0
                                foreach (var plan_cell in plan_column.Cells) {
                                    foreach (var alloc_cell in column.Cells) {
                                        if (plan_cell == alloc_cell) {
                                            Decimal difference = plan_cell.PlanMoney - alloc_cell.MoneyNoReserve;
                                            //���� �������� ������ 0
                                            if (difference > 0) {
                                                alloc_cell.MoneyNoReserve += department_provision * (difference / department_provision);
                                                department_provision -= department_provision * (difference / department_provision);
                                                un_controled_orders_count++;
                                            }
                                            //���� �������� ������ 0, �� ������ �� ������
                                            else if (difference < 0) { }
                                        }
                                    }
                                }
                            }

                            //��������� ������ ������� ���� ����� ��� ���������� ��� �������� ������ �������������� �������, ��������� ������� ������ ������ ��
                            //���������������� ��������������� �� ����������

                                foreach (var plan_cell in plan_column.Cells) {
                                    foreach (var alloc_cell in column.Cells) {
                                        if (plan_cell == alloc_cell) {
                                            Decimal difference = plan_cell.PlanMoney - alloc_cell.MoneyNoReserve;

                                            if (alloc_cell.Row.Order.TypeControl == FmCOrderTypeControl.NO_ORDERED && difference > 0) {

                                                alloc_cell.MoneyNoReserve += department_provision / un_controled_orders_count;

                                            }
                                        }
                                    }
                                }



                        }
                        
                    }
                }
                    //��������� ����������������
                else if (b_key == 0) {
                    foreach (var plan_column in plan_matrix.Columns) {
                        //���� ������� � ����� � � �������� �������
                        if (plan_column == column) {

                            //��������� ������� ������ ���� ��� ������ ����� �� �����
                            foreach (var plan_cell in plan_column.Cells) {
                                foreach (var alloc_cell in column.Cells) {
                                    if (plan_cell == alloc_cell) {
                                        Decimal difference = plan_cell.PlanMoney - alloc_cell.MoneyNoReserve;
                                        if (difference > 0) { department_needs += difference; }
                                    }
                                }
                            }
                            //���� ������ ������ ������������ 
                            if (department_provision >= department_needs) {
                                List<Decimal> diffs_list = new List<Decimal>();
                                int unzero_orders_count = 0;
                                //�� ������������ � ������ ������� �� ������ ��� �-�(����)>0
                                foreach (var plan_cell in plan_column.Cells) {
                                    foreach (var alloc_cell in column.Cells) {
                                        if (plan_cell == alloc_cell) {
                                            Decimal difference = plan_cell.PlanMoney - alloc_cell.MoneyNoReserve;
                                            //���� �������� ������ 0
                                            if (difference > 0) {
                                                alloc_cell.MoneyNoReserve += department_provision * (difference / department_provision);
                                                department_provision -= department_provision * (difference / department_provision);
                                                unzero_orders_count++;
                                            }
                                            //���� �������� ������ 0
                                            else if (difference < 0) {
                                                diffs_list.Add(Math.Abs(difference));
                                            }
                                        }
                                    }
                                }
                                // ��������� �� ������ ��� ������� ������ ��� ����������, �� ������ ����� ������ �������� �� �������, � ������ ��� ���� ������������
                                // ������� ������� ������ ����������� ���������� ����� ��� ����� ��� �-�<0, � ������� ��� ���������� � ������� ������ ��� �-�=0 

                                Decimal min_waste = diffs_list.Min(); // ������ ���������� ����������

                                //����� ���� ���������, ���� ������� ������� ������ ����� �� ���� ������� ���� �������

                                //���� ������/ ���� ������ �� ��������
                                while (department_provision != 0) {
                                    if (department_provision - (min_waste * unzero_orders_count) >= 0) {
                                        foreach (var plan_cell in plan_column.Cells) {
                                            foreach (var alloc_cell in column.Cells) {
                                                if (plan_cell == alloc_cell) {
                                                    Decimal difference = plan_cell.PlanMoney - alloc_cell.MoneyNoReserve;
                                                    if (difference == 0) {
                                                        alloc_cell.MoneyNoReserve += min_waste;
                                                        department_provision -= min_waste;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    //���� �� ������, �� ����������� ��������������� ���-�� �������
                                    else if (department_provision - (min_waste * unzero_orders_count) < 0 || min_waste == null) {
                                        foreach (var plan_cell in plan_column.Cells) {
                                            foreach (var alloc_cell in column.Cells) {
                                                if (plan_cell == alloc_cell) {
                                                    Decimal difference = plan_cell.PlanMoney - alloc_cell.MoneyNoReserve;
                                                    if (difference == 0) {
                                                        alloc_cell.MoneyNoReserve += department_provision / unzero_orders_count;
                                                        department_provision -= department_provision / unzero_orders_count;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    diffs_list.Remove(min_waste);
                                    min_waste = diffs_list.Min();
                                }

                            }
                            //���� ������� �� �������, �� ��������� ������������ ���� ���� ������
                            else if (department_provision <= department_needs) {
                                //�� ������������ �� ������ ��� �-�(����)>0
                                foreach (var plan_cell in plan_column.Cells) {
                                    if (department_provision != 0) {
                                        foreach (var alloc_cell in column.Cells) {
                                            if (plan_cell == alloc_cell) {
                                                Decimal difference = plan_cell.PlanMoney - alloc_cell.MoneyNoReserve;
                                                //���� �������� ������ 0
                                                // if (department_provision != 0) {
                                                if (difference > 0) {
                                                    alloc_cell.MoneyNoReserve += department_provision * (difference / department_provision);
                                                    department_provision -= department_provision * (difference / department_provision);
                                                }
                                                // }
                                                // else { break; }
                                            }
                                        }
                                    }
                                    else { break; }
                                }
                            }

                        }


                    }

                }



            }
            return alloc_result;
        }
            
        



        public HrmSalaryTaskProvisionMatrixReductionLogic(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }
}