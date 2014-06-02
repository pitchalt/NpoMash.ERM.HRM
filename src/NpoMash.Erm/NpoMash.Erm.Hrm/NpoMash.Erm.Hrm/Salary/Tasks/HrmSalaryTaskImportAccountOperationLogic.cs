using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Configuration;
using System.ComponentModel;
using System.Collections.Generic;
//
using DevExpress.ExpressApp.Xpo;
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
            FileHelperEngine<ExchangeAccountOperation> account_operation_data = new FileHelperEngine<ExchangeAccountOperation>();
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
            matrix_alloc_result_kb.Status = HrmMatrixStatus.MATRIX_DOWNLOADED;
            matrix_alloc_result_ozm.Status = HrmMatrixStatus.MATRIX_DOWNLOADED;
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
            //local_object_space = local_object_space.CreateNestedObjectSpace();
            //local_task = local_object_space.GetObject<HrmSalaryTaskImportAccountOperation>(local_task);
            Session session = ((XPObjectSpace)local_object_space).Session;
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
            matrix_alloc_result_kb.Status = HrmMatrixStatus.MATRIX_DOWNLOADED;
            matrix_alloc_result_ozm.Status = HrmMatrixStatus.MATRIX_DOWNLOADED;
            local_task.MatrixAllocResultKB = matrix_alloc_result_kb;
            local_task.MatrixAllocResultOZM = matrix_alloc_result_ozm;
            local_task.GroupDep = IntecoAG.ERM.HRM.Organization.DepartmentGroupDep.DEPARTMENT_KB;
            local_task.MatrixAllocResultKB.GroupDep = IntecoAG.ERM.HRM.Organization.DepartmentGroupDep.DEPARTMENT_KB;
            local_task.MatrixAllocResultOZM.GroupDep = IntecoAG.ERM.HRM.Organization.DepartmentGroupDep.DEPARTMENT_OZM;
            local_task.Period.CurrentMatrixAllocResultKB = matrix_alloc_result_kb;
            local_task.Period.CurrentMatrixAllocResultOZM = matrix_alloc_result_ozm;
            local_task.Period.Matrixs.Add(matrix_alloc_result_kb);
            local_task.Period.Matrixs.Add(matrix_alloc_result_ozm);
            FileHelperEngine<ExchangeAccountOperation> account_operation_data = new FileHelperEngine<ExchangeAccountOperation>();
            ExchangeAccountOperation[] account_list = null;
            try {
                account_list = account_operation_data.ReadFile(ConfigurationManager.AppSettings["FileExchangePath.ROOT"] + Convert.ToString(local_task.Period.CurrentAllocParameter.Year * 100 + local_task.Period.CurrentAllocParameter.Month) + "/AccountOperation_First.ncd");
            }
            catch (FileNotFoundException) {
                local_task.Abort();
                local_task.LogRecord(LogRecordType.ERROR, null, null, "Не найден файл 'AccountOperation_First.ncd'");
                matrix_alloc_result_kb.Status = HrmMatrixStatus.NOTDOWNLOADED;
                matrix_alloc_result_ozm.Status = HrmMatrixStatus.NOTDOWNLOADED;
                return;
            }
            catch (BadUsageException) {
                local_task.Abort();
                local_task.LogRecord(LogRecordType.ERROR, null, null, "Файл 'AccountOperation_First.ncd' имеет неправильную размерность");
                matrix_alloc_result_kb.Status = HrmMatrixStatus.NOTDOWNLOADED;
                matrix_alloc_result_ozm.Status = HrmMatrixStatus.NOTDOWNLOADED;
                return;
            }
            if (account_list == null) {
                local_task.Abort();
                local_task.LogRecord(LogRecordType.ERROR, null, null, "Нельзя импортировать пустой файл 'AccountOperation_First.ncd'");
                matrix_alloc_result_kb.Status = HrmMatrixStatus.NOTDOWNLOADED;
                matrix_alloc_result_ozm.Status = HrmMatrixStatus.NOTDOWNLOADED;
                return;
            }
            IDictionary<HrmSalaryPayType, HrmPeriodPayType> paytypes_in_alloc_parameter = local_task.Period.CurrentAllocParameter.PeriodPayTypes
                    .ToDictionary<HrmPeriodPayType, HrmSalaryPayType>(x => x.PayType);
            IDictionary<String, HrmSalaryPayType> paytypes_in_database = local_object_space.GetObjects<HrmSalaryPayType>()
                    .ToDictionary<HrmSalaryPayType, String>(x => x.Code);
            IDictionary<String, Department> departments_in_database = local_object_space.GetObjects<Department>()
                    .ToDictionary<Department, String>(x => x.BuhCode);
            IDictionary<String, fmCOrder> orders_in_database = local_object_space.GetObjects<fmCOrder>()
                    .ToDictionary<fmCOrder, String>(x => x.Code);
            IDictionary<String, HrmMatrixCell> cells_in_matrix = new Dictionary<String, HrmMatrixCell>();
            IDictionary<String, HrmMatrixColumn> ozm_columns = new Dictionary<String, HrmMatrixColumn>();
            IDictionary<String, HrmMatrixColumn> kb_columns = new Dictionary<String, HrmMatrixColumn>();
            IDictionary<String, HrmMatrixRow> ozm_rows = new Dictionary<String, HrmMatrixRow>();
            IDictionary<String, HrmMatrixRow> kb_rows = new Dictionary<String, HrmMatrixRow>();
            IDictionary<String, HrmMatrixColumn> alloc_result_columns = null;
            IDictionary<String, HrmMatrixRow> alloc_result_rows = null;
            int count = 0;
            foreach (var account_operation in account_list) {
                count++;
                HrmMatrix alloc_result_matrix = null;
                String file_order_code = account_operation.OrderCode;
                if (String.IsNullOrEmpty(file_order_code) && file_order_code != "") {
                    local_task.LogRecord(LogRecordType.WARNING, null, null, "Пустой код заказа в файле");
                }
                String file_department_code = account_operation.DepartmentCode;
                if (String.IsNullOrEmpty(file_department_code) && file_department_code != "") {
                    local_task.LogRecord(LogRecordType.WARNING, null, null, "Пустой код подразделения в файле");
                }
                String file_payType = account_operation.PayTypeCode;
                if (String.IsNullOrEmpty(file_payType) && file_payType != "") {
                    local_task.LogRecord(LogRecordType.WARNING, null, null, "Пустой код оплаты в файле");
                }
                //HrmAccountOperation account_to_db = local_object_space.CreateObject<HrmAccountOperation>();
                HrmMatrixColumn current_column = null;
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
                    if (alloc_result_columns.ContainsKey(file_department_code)) { current_column = alloc_result_columns[file_department_code]; }
                    else {
                        current_column = local_object_space.CreateObject<HrmMatrixColumn>();
                        current_column.Matrix = alloc_result_matrix;
                        alloc_result_matrix.Columns.Add(current_column);
                        current_column.Department = departments_in_database[file_department_code];
                        alloc_result_columns.Add(file_department_code, current_column);
                    }
                }
                else {
                    local_task.LogRecord(LogRecordType.ERROR, null, null, "В справочниках не найдено подразделения с кодом " + account_operation.DepartmentCode);
                }
                HrmMatrixRow current_row = null;
                if (alloc_result_rows.ContainsKey(file_order_code)) { current_row = alloc_result_rows[file_order_code]; }
                else {
                    current_row = local_object_space.CreateObject<HrmMatrixRow>();
                    current_row.Matrix = alloc_result_matrix;
                    alloc_result_matrix.Rows.Add(current_row);
                    alloc_result_rows.Add(file_order_code, current_row);
                    if (orders_in_database.ContainsKey(file_order_code)) {
                        current_row.Order = orders_in_database[file_order_code];
                    }
                    else {
                        local_task.LogRecord(LogRecordType.ERROR, null, null, "В справочниках не найдено заказа с кодом " + account_operation.DepartmentCode);
                    }
                }
                HrmMatrixCell current_cell = null;
                if (current_row != null && current_column != null && !String.IsNullOrEmpty(file_payType) && current_row.Order != null && current_column.Department != null) {
                    HrmAccountOperation account_to_db = new HrmAccountOperation(session);
                    account_to_db.Sign = account_operation.Sign;
                    account_to_db.Debit = account_operation.Debit;
                    account_to_db.Money = account_operation.Money;
                    account_to_db.Credit = account_operation.Credit;
                    try {
                        account_to_db.Order = orders_in_database[account_operation.OrderCode];
                        account_to_db.PayType = paytypes_in_database[account_operation.PayTypeCode];
                        account_to_db.Department = departments_in_database[account_operation.DepartmentCode];
                    }
                    catch(KeyNotFoundException) {
                        local_task.LogRecord(LogRecordType.ERROR, null, null, "Не удалось связать проводку с заказом и/или кодом оплаты и/или подразделением");
                    }
                    if (account_to_db.Department.GroupDep == DepartmentGroupDep.DEPARTMENT_KB) { account_to_db.AllocResult = matrix_alloc_result_kb; }
                    else { account_to_db.AllocResult = matrix_alloc_result_ozm; }
                    String cell_key = current_column.Department.BuhCode + "|" + current_row.Order.Code;
                    if (!cells_in_matrix.ContainsKey(cell_key)) {
                        HrmMatrixCell cell = local_object_space.CreateObject<HrmMatrixCell>();
                        cells_in_matrix.Add(cell_key, cell);
                        cell.AccountOperations.Add(account_to_db);
                        cell.Column = current_column;
                        current_column.Cells.Add(cell);
                        cell.Row = current_row;
                        current_row.Cells.Add(cell);
                        current_cell = cell;
                    }
                    else {
                        HrmMatrixCell cell = cells_in_matrix[cell_key];
                        cell.AccountOperations.Add(account_to_db);
                        current_cell = cell;
                    }
                    if (paytypes_in_alloc_parameter[account_to_db.PayType].Type == HrmPayTypes.PROVISION_CODE) {
                        current_cell.SourceProvision += account_operation.Money;
                        current_cell.Time += account_operation.Time;
                    }
                    if (paytypes_in_alloc_parameter[account_to_db.PayType].Type == HrmPayTypes.TRAVEL_CODE) {
                        current_cell.TravelMoney += account_operation.Money;
                        current_cell.Time += account_operation.Time;
                    }
                    if (paytypes_in_alloc_parameter[account_to_db.PayType].Type == HrmPayTypes.BASE_CODE) {
                        current_cell.MoneyNoReserve += account_operation.Money;
                        current_cell.Time += account_operation.Time;
                    }
                }
                else {
                    local_task.LogRecord(LogRecordType.WARNING, null, null, "Не удалось создать ячейку матрицы из-за отсутствия подразделения и/или заказа и/или кода оплаты");
                }
            }
            //local_object_space.CommitChanges();
        }
    }
}