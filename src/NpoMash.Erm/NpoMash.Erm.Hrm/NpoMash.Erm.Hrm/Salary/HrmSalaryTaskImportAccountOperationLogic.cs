using System;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;
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
using FileHelpers;
using IntecoAG.ERM.HRM;
using IntecoAG.ERM.FM.Order;
using NpoMash.Erm.Hrm.Exchange;
using IntecoAG.ERM.HRM.Organization;


namespace NpoMash.Erm.Hrm.Salary {
    public static class HrmSalaryTaskImportAccountOperationLogic {

        public static void CreateAllocResultFromPlan(IObjectSpace local_object_space, HrmMatrixAllocResult kb_result, HrmMatrixAllocResult ozm_result, HrmSalaryTaskImportAccountOperation local_task) {
            IDictionary<DepartmentGroupDep, HrmMatrixAllocPlan> plan_matrixes = local_object_space.GetObjects<HrmMatrixAllocPlan>()
                .ToDictionary<HrmMatrixAllocPlan, DepartmentGroupDep>(x => x.GroupDep);
            foreach (var department_group in plan_matrixes.Keys) {
                if (department_group == DepartmentGroupDep.DEPARTMENT_KB) {
                    foreach (var column in plan_matrixes[department_group].Columns) {
                        HrmMatrixColumn new_column = local_object_space.CreateObject<HrmMatrixColumn>();
                        new_column.Department = column.Department;
                        new_column.Matrix = column.Matrix;
                        kb_result.Columns.Add(new_column);
                        foreach (var cell in column.Cells) {
                            HrmMatrixCell new_cell = local_object_space.CreateObject<HrmMatrixCell>();
                            HrmMatrixRow new_row = local_object_space.CreateObject<HrmMatrixRow>();
                            new_cell.Column = new_column;
                            new_cell.Row = new_row;
                            new_cell.Row.Order = cell.Row.Order;
                            new_row.Matrix = cell.Row.Matrix;
                            new_row.Cells.Add(new_cell);
                            new_column.Cells.Add(new_cell);
                            kb_result.Rows.Add(new_row);
                        }
                    }
                }
                else {
                    foreach (var column in plan_matrixes[department_group].Columns) {
                        HrmMatrixColumn new_column = local_object_space.CreateObject<HrmMatrixColumn>();
                        new_column.Department = column.Department;
                        new_column.Matrix = column.Matrix;
                        kb_result.Columns.Add(new_column);
                        foreach (var cell in column.Cells) {
                            HrmMatrixCell new_cell = local_object_space.CreateObject<HrmMatrixCell>();
                            HrmMatrixRow new_row = local_object_space.CreateObject<HrmMatrixRow>();
                            new_cell.Column = new_column;
                            new_cell.Row = new_row;
                            new_cell.Row.Order = cell.Row.Order;
                            new_row.Matrix = cell.Row.Matrix;
                            new_row.Cells.Add(new_cell);
                            new_column.Cells.Add(new_cell);
                            kb_result.Rows.Add(new_row);
                        }
                    }
                }
            }
            local_object_space.CommitChanges();
            CreateTestAllocResultMatrix(local_object_space, kb_result, DepartmentGroupDep.DEPARTMENT_KB,
                CreateReservePayType(local_object_space), CreateNoReservePayType(local_object_space), null);
            CreateTestAllocResultMatrix(local_object_space, ozm_result, DepartmentGroupDep.DEPARTMENT_OZM,
                CreateReservePayType(local_object_space), CreateNoReservePayType(local_object_space), null);
        }

        public static HrmSalaryPayType CreateReservePayType(IObjectSpace local_object_space) {
            HrmSalaryPayType paytype_in_db = null;
            foreach (var paytype in local_object_space.GetObjects<HrmSalaryPayType>()) {
                paytype_in_db = paytype;
            }
            return paytype_in_db;
        }

        public static HrmSalaryPayType CreateNoReservePayType(IObjectSpace local_object_space) {
            HrmSalaryPayType paytype_to_db =  local_object_space.CreateObject<HrmSalaryPayType>();
            paytype_to_db.Code = "100";
            paytype_to_db.Name = "Код оплаты, не входящий в резерв";
            local_object_space.CommitChanges();
            return paytype_to_db;
        }

        public static void CreateTestAllocResultMatrix(IObjectSpace local_object_space, HrmMatrixAllocResult matrix_alloc_result, DepartmentGroupDep group_dep,
            HrmSalaryPayType reserve_paytype, HrmSalaryPayType no_reserve_paytype, HrmSalaryPayType travel_paytype) {
            var random = new Random();
            foreach (var column in matrix_alloc_result.Columns) {
                if (column.Department.GroupDep == group_dep) {
                    foreach (var cell in column.Cells) {
                        int how_many_accounts = random.Next(1, 4);
                        switch (how_many_accounts) {
                            case 1:
                                HrmAccountOperation reserve_account_first = local_object_space.CreateObject<HrmAccountOperation>();
                                reserve_account_first.Department = cell.Column.Department;
                                reserve_account_first.PayType = reserve_paytype;
                                reserve_account_first.Order = cell.Row.Order;
                                reserve_account_first.Money = random.Next(100, 1000);
                                cell.AccountOperations.Add(reserve_account_first);
                                matrix_alloc_result.AccountOperations.Add(reserve_account_first);
                                break;
                            case 2:
                                HrmAccountOperation reserve_account_second = local_object_space.CreateObject<HrmAccountOperation>();
                                HrmAccountOperation no_reserve_account_second = local_object_space.CreateObject<HrmAccountOperation>();
                                reserve_account_second.Department = cell.Column.Department;
                                reserve_account_second.PayType = reserve_paytype;
                                reserve_account_second.Order = cell.Row.Order;
                                reserve_account_second.Money = random.Next(1, 1000);
                                no_reserve_account_second.Department = cell.Column.Department;
                                no_reserve_account_second.PayType = no_reserve_paytype;
                                no_reserve_account_second.Order = cell.Row.Order;
                                no_reserve_account_second.Money = random.Next(1, 1000);
                                cell.AccountOperations.Add(reserve_account_second);
                                cell.AccountOperations.Add(no_reserve_account_second);
                                matrix_alloc_result.AccountOperations.Add(reserve_account_second);
                                matrix_alloc_result.AccountOperations.Add(no_reserve_account_second);
                                break;
                            case 3:
                                HrmAccountOperation reserve_account_third = local_object_space.CreateObject<HrmAccountOperation>();
                                HrmAccountOperation no_reserve_account_third = local_object_space.CreateObject<HrmAccountOperation>();
                                //HrmAccountOperation travel_account_third = local_object_space.CreateObject<HrmAccountOperation>();
                                reserve_account_third.Department = cell.Column.Department;
                                reserve_account_third.PayType = reserve_paytype;
                                reserve_account_third.Order = cell.Row.Order;
                                reserve_account_third.Money = random.Next(1, 1000);
                                no_reserve_account_third.Department = cell.Column.Department;
                                no_reserve_account_third.PayType = no_reserve_paytype;
                                no_reserve_account_third.Order = cell.Row.Order;
                                no_reserve_account_third.Money = random.Next(1, 1000);
                                //travel_account_third.Department = cell.Column.Department;
                                //travel_account_third.PayType = travel_paytype;
                                //travel_account_third.Order = cell.Row.Order;
                                //travel_account_third.Money = random.Next(1, 1000);
                                cell.AccountOperations.Add(reserve_account_third);
                                cell.AccountOperations.Add(no_reserve_account_third);
                                matrix_alloc_result.AccountOperations.Add(reserve_account_third);
                                matrix_alloc_result.AccountOperations.Add(no_reserve_account_third);
                                //matrix_alloc_result.AccountOperations.Add(travel_account_third);
                                break;
                        }
                    }
                }
            }
        }
        public static void ImportAccountOperationTestData(IObjectSpace local_object_space, HrmSalaryTaskImportAccountOperation local_task) {
            FileHelperEngine<ImportAccountOperation> account_operation_data = new FileHelperEngine<ImportAccountOperation>();
            HrmMatrixAllocResult matrix_alloc_result_kb = local_object_space.CreateObject<HrmMatrixAllocResult>();
            HrmMatrixAllocResult matrix_alloc_result_ozm = local_object_space.CreateObject<HrmMatrixAllocResult>();
            matrix_alloc_result_kb.IterationNumber = 1;
            matrix_alloc_result_ozm.IterationNumber = 1;
            matrix_alloc_result_kb.Period = local_task.Period;
            matrix_alloc_result_ozm.Period = local_task.Period;
            matrix_alloc_result_kb.GroupDep = DepartmentGroupDep.DEPARTMENT_KB;
            matrix_alloc_result_ozm.GroupDep = DepartmentGroupDep.DEPARTMENT_OZM;
            matrix_alloc_result_kb.Type = HrmMatrixType.TYPE_ALLOC_RESULT;
            matrix_alloc_result_ozm.Type = HrmMatrixType.TYPE_ALLOC_RESULT;
            matrix_alloc_result_kb.Status = HrmMatrixStatus.MATRIX_OPENED;
            matrix_alloc_result_ozm.Status = HrmMatrixStatus.MATRIX_OPENED;
            local_task.MatrixAllocResultKB = matrix_alloc_result_kb;
            local_task.MatrixAllocResultOZM = matrix_alloc_result_ozm;
            local_task.GroupDep = IntecoAG.ERM.HRM.Organization.DepartmentGroupDep.DEPARTMENT_KB;
            local_task.MatrixAllocResultKB.GroupDep = IntecoAG.ERM.HRM.Organization.DepartmentGroupDep.DEPARTMENT_KB;
            local_task.MatrixAllocResultOZM.GroupDep = IntecoAG.ERM.HRM.Organization.DepartmentGroupDep.DEPARTMENT_OZM;
            local_task.Period.CurrentMatrixAllocResultKB = matrix_alloc_result_kb;
            local_task.Period.CurrentMatrixAllocResultOZM = matrix_alloc_result_ozm;
            local_task.Period.Matrixs.Add(matrix_alloc_result_kb);
            local_task.Period.Matrixs.Add(matrix_alloc_result_ozm);
            CreateAllocResultFromPlan(local_object_space, matrix_alloc_result_kb, matrix_alloc_result_ozm, local_task);
        }
        public static void ImportAccountOperation(IObjectSpace local_object_space, HrmSalaryTaskImportAccountOperation local_task) {
            FileHelperEngine<ImportAccountOperation> account_operation_data = new FileHelperEngine<ImportAccountOperation>();
            ImportAccountOperation[] account_list = account_operation_data.ReadFile("../../../../../../../var/AccountOperation_First.ncd");
            HrmMatrixAllocResult matrix_alloc_result_kb = local_object_space.CreateObject<HrmMatrixAllocResult>();
            HrmMatrixAllocResult matrix_alloc_result_ozm = local_object_space.CreateObject<HrmMatrixAllocResult>();
            matrix_alloc_result_kb.IterationNumber = 1;
            matrix_alloc_result_ozm.IterationNumber = 1;
            matrix_alloc_result_kb.Period = local_task.Period;
            matrix_alloc_result_ozm.Period = local_task.Period;
            matrix_alloc_result_kb.GroupDep = DepartmentGroupDep.DEPARTMENT_KB;
            matrix_alloc_result_ozm.GroupDep = DepartmentGroupDep.DEPARTMENT_OZM;
            matrix_alloc_result_kb.Type = HrmMatrixType.TYPE_ALLOC_RESULT;
            matrix_alloc_result_ozm.Type = HrmMatrixType.TYPE_ALLOC_RESULT;
            matrix_alloc_result_kb.Status = HrmMatrixStatus.MATRIX_OPENED;
            matrix_alloc_result_ozm.Status = HrmMatrixStatus.MATRIX_OPENED;
            local_task.MatrixAllocResultKB = matrix_alloc_result_kb;
            local_task.MatrixAllocResultOZM = matrix_alloc_result_ozm;
            local_task.GroupDep = IntecoAG.ERM.HRM.Organization.DepartmentGroupDep.DEPARTMENT_KB;
            local_task.MatrixAllocResultKB.GroupDep = IntecoAG.ERM.HRM.Organization.DepartmentGroupDep.DEPARTMENT_KB;
            local_task.MatrixAllocResultOZM.GroupDep = IntecoAG.ERM.HRM.Organization.DepartmentGroupDep.DEPARTMENT_OZM;
            local_task.Period.CurrentMatrixAllocResultKB = matrix_alloc_result_kb;
            local_task.Period.CurrentMatrixAllocResultOZM = matrix_alloc_result_ozm;
            local_task.Period.Matrixs.Add(matrix_alloc_result_kb);
            local_task.Period.Matrixs.Add(matrix_alloc_result_ozm);
            IDictionary<String, HrmMatrixColumn> ozm_columns = new Dictionary<string, HrmMatrixColumn>();
            IDictionary<String, HrmMatrixRow> ozm_rows = new Dictionary<string, HrmMatrixRow>();
            IDictionary<String, HrmMatrixColumn> kb_columns = new Dictionary<string, HrmMatrixColumn>();
            IDictionary<String, HrmMatrixRow> kb_rows = new Dictionary<string, HrmMatrixRow>();
            IDictionary<String, HrmMatrixColumn> alloc_result_columns = null;
            IDictionary<String, HrmMatrixRow> alloc_result_rows = null;
            IDictionary<fmCOrder, HrmPeriodOrderControl> reserve_orders = local_task.Period.CurrentAllocParameter.OrderControls
                .ToDictionary<HrmPeriodOrderControl, fmCOrder>(x => x.Order);
            IDictionary<String, HrmSalaryPayType> paytypes_in_database = local_object_space.GetObjects<HrmSalaryPayType>()
                .ToDictionary<HrmSalaryPayType, String>(x => x.Code);
            IDictionary<String, Department> departments_in_database = local_object_space.GetObjects<Department>()
                .ToDictionary<Department, String>(x => x.BuhCode);
            IDictionary<String, fmCOrder> orders_in_database = local_object_space.GetObjects<fmCOrder>()
                .ToDictionary<fmCOrder, String>(x => x.Code);
            foreach (var account in account_list) {
                HrmMatrix alloc_result_matrix = null;
                String file_order_code = account.OrderCode;
                String file_department_code = account.DepartmentCode;
                if (departments_in_database.ContainsKey(file_department_code)) {
                    if (departments_in_database[file_department_code].GroupDep == DepartmentGroupDep.DEPARTMENT_KB) {
                        alloc_result_matrix = matrix_alloc_result_kb;
                        alloc_result_columns = kb_columns;
                        alloc_result_rows = kb_rows;
                    }
                    else {
                        alloc_result_matrix = matrix_alloc_result_ozm;
                        alloc_result_columns = ozm_columns;
                        alloc_result_rows = ozm_rows;
                    }
                }
                else throw new Exception("There is no department in database with code " + account.DepartmentCode);
                HrmMatrixCell cell = local_object_space.CreateObject<HrmMatrixCell>();
                HrmMatrixColumn current_column = null;
                if (alloc_result_columns.ContainsKey(file_department_code)) { current_column = alloc_result_columns[file_department_code]; }
                else {
                    current_column = local_object_space.CreateObject<HrmMatrixColumn>();
                    current_column.Matrix = alloc_result_matrix;
                    alloc_result_matrix.Columns.Add(current_column);
                    current_column.Department = departments_in_database[file_department_code];
                    alloc_result_columns.Add(file_department_code, current_column);
                }
                cell.Column = current_column;
                current_column.Cells.Add(cell);
                HrmMatrixRow current_row = null;
                if (alloc_result_rows.ContainsKey(file_order_code)) {
                    current_row = alloc_result_rows[file_order_code];
                }
                else {
                    current_row = local_object_space.CreateObject<HrmMatrixRow>();
                    current_row.Matrix = alloc_result_matrix;
                    alloc_result_matrix.Rows.Add(current_row);
                    alloc_result_rows.Add(file_order_code, current_row);
                    if (orders_in_database.ContainsKey(file_order_code)) {
                        current_row.Order = orders_in_database[file_order_code];
                    }
                    else throw new Exception("There is no order with code " + file_order_code);
                }
                cell.Row = current_row;
                current_row.Cells.Add(cell);
            }
            foreach (var account in account_list) {
                    HrmAccountOperation account_operation = local_object_space.CreateObject<HrmAccountOperation>();
                    account_operation.Sign = account.Sign;
                    account_operation.Debit = account.Debit;
                    account_operation.Money = account.Money;
                    account_operation.Credit = account.Credit;
                    account_operation.Order = orders_in_database[account.OrderCode];
                    account_operation.PayType = paytypes_in_database[account.PayTypeCode];
                    account_operation.Department = departments_in_database[account.DepartmentCode];
                    if (account_operation.Department.GroupDep == DepartmentGroupDep.DEPARTMENT_KB) {
                        IDictionary<Department, HrmMatrixColumn> column_in_matrix = matrix_alloc_result_kb.Columns.ToDictionary<HrmMatrixColumn, Department>(x => x.Department);
                        foreach (var cell in column_in_matrix[account_operation.Department].Cells) {
                            if (cell.Row.Order == account_operation.Order) {
                                cell.AccountOperations.Add(account_operation);
                                if (reserve_orders.ContainsKey(account_operation.Order)) {
                                    cell.MoneyReserve += account_operation.Money;
                                    cell.Time += account.Time;
                                }
                                else {
                                    cell.MoneyNoReserve += account_operation.Money;
                                    cell.Time += account.Time;
                                }
                                break;
                            }
                        }
                        account_operation.AllocResult = matrix_alloc_result_kb;
                        matrix_alloc_result_kb.AccountOperations.Add(account_operation);
                    }
                    else {
                        IDictionary<Department, HrmMatrixColumn> column_in_matrix = matrix_alloc_result_ozm.Columns.ToDictionary<HrmMatrixColumn, Department>(x => x.Department);
                        foreach (var cell in column_in_matrix[account_operation.Department].Cells) {
                            if (cell.Row.Order == account_operation.Order) {
                                cell.AccountOperations.Add(account_operation);
                                if (reserve_orders.ContainsKey(account_operation.Order)) {
                                    cell.MoneyReserve += account_operation.Money;
                                    cell.Time += account.Time;
                                }
                                else {
                                    cell.MoneyNoReserve += account_operation.Money;
                                    cell.Time += account.Time;
                                }
                                break;
                            }
                        }
                        account_operation.AllocResult = matrix_alloc_result_ozm;
                        matrix_alloc_result_ozm.AccountOperations.Add(account_operation);
                    }
                }
            }
        }
    }