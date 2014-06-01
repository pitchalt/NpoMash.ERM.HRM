using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Configuration;
using System.ComponentModel;
using System.Collections.Generic;

using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Templates;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Model.NodeGenerators;

using FileHelpers;

using NpoMash.Erm.Hrm.Salary;
using NpoMash.Erm.Hrm.Exchange;

using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.HRM.Organization;

namespace NpoMash.Erm.Hrm.Salary {


    public static class HrmSalaryTaskImportSourceDataLogic {

        public static HrmSalaryTaskImportSourceData InitTaskImportSourceData(IObjectSpace object_space, HrmPeriod period, DepartmentGroupDep group_dep) {
            var task_import_source_data = object_space.CreateObject<HrmSalaryTaskImportSourceData>();
            task_import_source_data.GroupDep = group_dep;
            period.PeriodTasks.Add(task_import_source_data);
            task_import_source_data.Period = period;
            if (group_dep == DepartmentGroupDep.DEPARTMENT_KB) {
                task_import_source_data.TimeSheetKB = period.CurrentTimeSheetKB;
            }
            return task_import_source_data;
        }

        public static void ImportTimeSheet(IObjectSpace os, HrmSalaryTaskImportSourceData task) {
            if (task.State != HrmSalaryTaskState.HRM_SALARY_TASK_ABORTED) {
                HrmTimeSheetLogic.TaskSheetInit(os, task);
                ExchangeMatrixTimeSheet[] timesheet_list = null;
                var timesheet_data = new FileHelperEngine<ExchangeMatrixTimeSheet>();
                try {
                    timesheet_list = timesheet_data.ReadFile(ConfigurationManager.AppSettings["FileExchangePath.ROOT"] + Convert.ToString(task.Period.CurrentAllocParameter.Year * 100 + task.Period.CurrentAllocParameter.Month) +"/Matrix_TimeSheet.ncd");
                }
                catch (FileNotFoundException) {
                    task.Abort();
                    task.LogRecord(LogRecordType.ERROR, null, null, "Не найден файл 'Matrix_TimeSheet.ncd'");
                    task.TimeSheetKB.SetStatus(HrmTimeSheetStatus.NOTDOWNLOADED);
                    task.TimeSheetOZM.SetStatus(HrmTimeSheetStatus.NOTDOWNLOADED);
                    return;
                }
                catch (BadUsageException) {
                    task.Abort();
                    task.LogRecord(LogRecordType.ERROR, null, null, "Файл 'Matrix_TimeSheet.ncd' имеет неправильную размерность");
                    task.TimeSheetKB.SetStatus(HrmTimeSheetStatus.NOTDOWNLOADED);
                    task.TimeSheetOZM.SetStatus(HrmTimeSheetStatus.NOTDOWNLOADED);
                    return;
                }
                if (timesheet_list == null) {
                    task.Abort();
                    task.LogRecord(LogRecordType.ERROR, null, null, "Нельзя импортировать пустой файл 'Matrix_TimeSheet.ncd'");
                    task.TimeSheetKB.SetStatus(HrmTimeSheetStatus.NOTDOWNLOADED);
                    task.TimeSheetOZM.SetStatus(HrmTimeSheetStatus.NOTDOWNLOADED);
                    return;
                }
                IList<Department> deps = os.GetObjects<Department>();
                foreach (var each in timesheet_list) {
                    if (each.Year != task.Period.Year || each.Month != task.Period.Month) {
                        task.Abort();
                        task.LogRecord(LogRecordType.ERROR, null, null, "Дата в файле 'Matrix_TimeSheet.ncd' не соответствует дате текущего периода");
                        task.TimeSheetKB.SetStatus(HrmTimeSheetStatus.NOTDOWNLOADED);
                        task.TimeSheetOZM.SetStatus(HrmTimeSheetStatus.NOTDOWNLOADED);
                        return;
                    }
                    else {
                        String code = each.Department_Code;
                        if (String.IsNullOrEmpty(each.Department_Code) && each.Department_Code != "") {
                            task.LogRecord(LogRecordType.WARNING, null, null, "Пустой код подразделения в файле");
                        }
                        else {
                            Department dep = deps.FirstOrDefault(x => x.BuhCode == code);
                            if (dep == null) {
                                task.LogRecord(LogRecordType.ERROR, null, null, "В спрачониках не найдено подразделения с кодом " + code.Trim());
                            }
                            else {
                                HrmTimeSheetDep sheet_dep = os.CreateObject<HrmTimeSheetDep>();
                                sheet_dep.BuhCode = dep.BuhCode;
                                sheet_dep.Department = dep;
                                sheet_dep.BaseWorkTime = each.BaseWorkTime;
                                sheet_dep.ConstantWorkTime = each.ConstantWorkTime;
                                sheet_dep.AdditionWorkTime = 0;
                                sheet_dep.TravelWorkTime = each.TravelWorkTime;
                                if (dep.GroupDep == DepartmentGroupDep.DEPARTMENT_KB) {
                                    task.TimeSheetKB.TimeSheetDeps.Add(sheet_dep);
                                }
                                if (dep.GroupDep == DepartmentGroupDep.DEPARTMENT_OZM) {
                                    task.TimeSheetOZM.TimeSheetDeps.Add(sheet_dep);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void ImportPlanMatrixes(IObjectSpace object_space, HrmSalaryTaskImportSourceData task) {
            if (task.State != HrmSalaryTaskState.HRM_SALARY_TASK_ABORTED) {
                //            HrmPeriod period, out HrmMatrixAllocPlan KBMatrix, out HrmMatrixAllocPlan OZMMatrix) {
                //Общая плановая матрица
                HrmMatrixAllocPlan matrix_alloc_plan_summary = object_space.CreateObject<HrmMatrixAllocPlan>();
                matrix_alloc_plan_summary.Status = HrmMatrixStatus.MATRIX_DOWNLOADED;
                matrix_alloc_plan_summary.Type = HrmMatrixType.TYPE_MATIX;
                matrix_alloc_plan_summary.TypeMatrix = HrmMatrixTypeMatrix.MATRIX_PLANNED;
                matrix_alloc_plan_summary.GroupDep = DepartmentGroupDep.DEPARTMENT_KB_OZM;
                task.Period.CurrentMatrixAllocPlanSummary = matrix_alloc_plan_summary;
                matrix_alloc_plan_summary.IterationNumber = 1;
                //Инициализируем плановые матрицы кб и озм
                HrmMatrixAllocPlan kb_plan_matrix = object_space.CreateObject<HrmMatrixAllocPlan>();
                kb_plan_matrix.Status = HrmMatrixStatus.MATRIX_DOWNLOADED;
                //            kb_plan_matrix.Period = period;
                kb_plan_matrix.TypeMatrix = HrmMatrixTypeMatrix.MATRIX_PLANNED;
                kb_plan_matrix.Type = HrmMatrixType.TYPE_MATIX;
                kb_plan_matrix.GroupDep = DepartmentGroupDep.DEPARTMENT_KB;
                kb_plan_matrix.IterationNumber = 1;
                task.Period.Matrixs.Add(kb_plan_matrix);
                task.Period.CurrentMatrixAllocPlanKB = kb_plan_matrix;
                HrmMatrixAllocPlan ozm_plan_matrix = object_space.CreateObject<HrmMatrixAllocPlan>();
                ozm_plan_matrix.Status = HrmMatrixStatus.MATRIX_DOWNLOADED;
                //            ozm_plan_matrix.Period = period;
                ozm_plan_matrix.TypeMatrix = HrmMatrixTypeMatrix.MATRIX_PLANNED;
                ozm_plan_matrix.Type = HrmMatrixType.TYPE_MATIX;
                ozm_plan_matrix.GroupDep = DepartmentGroupDep.DEPARTMENT_OZM;
                ozm_plan_matrix.IterationNumber = 1;
                task.Period.Matrixs.Add(ozm_plan_matrix);
                task.Period.CurrentMatrixAllocPlanOZM = ozm_plan_matrix;
                Int16 current_year = task.Period.Year;
                Int16 current_month = task.Period.Month;
                FixedFileEngine<ExchangeMatrixPlan> plan_data = new FixedFileEngine<ExchangeMatrixPlan>();
                FixedFileEngine<ExchangeMatrixTravelTime> travel_data = new FixedFileEngine<ExchangeMatrixTravelTime>();
                FixedFileEngine<ExchangeConstOrderTime> const_order_data = new FixedFileEngine<ExchangeConstOrderTime>();
                ExchangeMatrixPlan[] plan_list = null;
                ExchangeMatrixTravelTime[] travel_list = null;
                ExchangeConstOrderTime[] const_order_list = null;
                try {
                    plan_list = plan_data.ReadFile(ConfigurationManager.AppSettings["FileExchangePath.ROOT"] + Convert.ToString(task.Period.CurrentAllocParameter.Year * 100 + task.Period.CurrentAllocParameter.Month) + "/Matrix_Plan.ncd");
                }
                catch (FileNotFoundException) {
                    task.Abort();
                    task.LogRecord(LogRecordType.ERROR, null, null, "Не найден файл 'Matrix_Plan.ncd'");
                    matrix_alloc_plan_summary.Status = HrmMatrixStatus.NOTDOWNLOADED;
                    kb_plan_matrix.Status = HrmMatrixStatus.NOTDOWNLOADED;
                    ozm_plan_matrix.Status = HrmMatrixStatus.NOTDOWNLOADED;
                    return;
                }
                catch (BadUsageException) {
                    task.Abort();
                    task.LogRecord(LogRecordType.ERROR, null, null, "Файл 'Matrix_Plan.ncd' имеет неправильную размерность");
                    matrix_alloc_plan_summary.Status = HrmMatrixStatus.NOTDOWNLOADED;
                    kb_plan_matrix.Status = HrmMatrixStatus.NOTDOWNLOADED;
                    ozm_plan_matrix.Status = HrmMatrixStatus.NOTDOWNLOADED;
                    return;
                }
                try {
                    travel_list = travel_data.ReadFile(ConfigurationManager.AppSettings["FileExchangePath.ROOT"] + Convert.ToString(task.Period.CurrentAllocParameter.Year * 100 + task.Period.CurrentAllocParameter.Month) + "/Matrix_TravelTimePlan.ncd");
                }
                catch (FileNotFoundException) {
                    task.Abort();
                    task.LogRecord(LogRecordType.ERROR, null, null, "Не найден файл 'Matrix_TravelTimePlan.ncd'");
                    matrix_alloc_plan_summary.Status = HrmMatrixStatus.NOTDOWNLOADED;
                    kb_plan_matrix.Status = HrmMatrixStatus.NOTDOWNLOADED;
                    ozm_plan_matrix.Status = HrmMatrixStatus.NOTDOWNLOADED;
                    return;
                }
                catch (BadUsageException) {
                    task.Abort();
                    task.LogRecord(LogRecordType.ERROR, null, null, "Файл 'Matrix_TravelTimePlan.ncd' имеет неправильную размерность");
                    matrix_alloc_plan_summary.Status = HrmMatrixStatus.NOTDOWNLOADED;
                    kb_plan_matrix.Status = HrmMatrixStatus.NOTDOWNLOADED;
                    ozm_plan_matrix.Status = HrmMatrixStatus.NOTDOWNLOADED;
                    return;
                }
                try {
                    const_order_list = const_order_data.ReadFile(ConfigurationManager.AppSettings["FileExchangePath.ROOT"] + Convert.ToString(task.Period.CurrentAllocParameter.Year * 100 + task.Period.CurrentAllocParameter.Month) + "/Const_OrderTime.ncd");
                }
                catch (FileNotFoundException) {
                    task.Abort();
                    task.LogRecord(LogRecordType.ERROR, null, null, "Не найден файл 'Const_OrderTime.ncd'");
                    matrix_alloc_plan_summary.Status = HrmMatrixStatus.NOTDOWNLOADED;
                    kb_plan_matrix.Status = HrmMatrixStatus.NOTDOWNLOADED;
                    ozm_plan_matrix.Status = HrmMatrixStatus.NOTDOWNLOADED;
                    return;
                }
                catch (BadUsageException) {
                    task.Abort();
                    task.LogRecord(LogRecordType.ERROR, null, null, "Файл 'Const_OrderTime.ncd' имеет неправильную размерность");
                    matrix_alloc_plan_summary.Status = HrmMatrixStatus.NOTDOWNLOADED;
                    kb_plan_matrix.Status = HrmMatrixStatus.NOTDOWNLOADED;
                    ozm_plan_matrix.Status = HrmMatrixStatus.NOTDOWNLOADED;
                    return;
                }
                if (plan_list == null) {
                    task.Abort();
                    task.LogRecord(LogRecordType.ERROR, null, null, "Нельзя импортировать пустой файл 'Matrix_Plan.ncd'");
                    matrix_alloc_plan_summary.Status = HrmMatrixStatus.NOTDOWNLOADED;
                    kb_plan_matrix.Status = HrmMatrixStatus.NOTDOWNLOADED;
                    ozm_plan_matrix.Status = HrmMatrixStatus.NOTDOWNLOADED;
                    return;
                }
                if (travel_list == null) {
                    task.Abort();
                    task.LogRecord(LogRecordType.ERROR, null, null, "Нельзя импортировать пустой файл 'Matrix_TravelTimePlan.ncd'");
                    matrix_alloc_plan_summary.Status = HrmMatrixStatus.NOTDOWNLOADED;
                    kb_plan_matrix.Status = HrmMatrixStatus.NOTDOWNLOADED;
                    ozm_plan_matrix.Status = HrmMatrixStatus.NOTDOWNLOADED;
                    return;
                }
                if (const_order_list == null) {
                    task.Abort();
                    task.LogRecord(LogRecordType.ERROR, null, null, "Нельзя импортировать пустой файл 'Const_OrderTime.ncd'");
                    matrix_alloc_plan_summary.Status = HrmMatrixStatus.NOTDOWNLOADED;
                    kb_plan_matrix.Status = HrmMatrixStatus.NOTDOWNLOADED;
                    ozm_plan_matrix.Status = HrmMatrixStatus.NOTDOWNLOADED;
                    return;
                }
                //создаем необходимые словари, чтобы не наматывать круги в форычах при поиске
                IDictionary<String, HrmMatrixCell> cells_in_matrix = new Dictionary<String, HrmMatrixCell>();
                IDictionary<String, HrmMatrixColumn> ozm_columns = new Dictionary<string, HrmMatrixColumn>();
                IDictionary<String, HrmMatrixRow> ozm_rows = new Dictionary<string, HrmMatrixRow>();
                IDictionary<String, HrmMatrixColumn> kb_columns = new Dictionary<string, HrmMatrixColumn>();
                IDictionary<String, HrmMatrixRow> kb_rows = new Dictionary<string, HrmMatrixRow>();
                IDictionary<String, HrmMatrixColumn> plan_matrix_columns = null;
                IDictionary<String, HrmMatrixRow> plan_matrix_rows = null;
                IDictionary<String, Department> departments_in_database = object_space.GetObjects<Department>()
                    .ToDictionary<Department, String>(x => x.BuhCode);
                IDictionary<String, fmCOrder> orders_in_database = object_space.GetObjects<fmCOrder>()
                    .ToDictionary<fmCOrder, String>(x => x.Code);
                //начинаем перебирать строки в файле
                foreach (var each in plan_list) {
                    //если запись относится к нашему периоду то начинаем обработку
                    if (each.Year != current_year || each.Month != current_month) {
                        task.Abort();
                        task.LogRecord(LogRecordType.ERROR, null, null, "Дата в файле 'Matrix_Plan.ncd' не соответствует дате текущего периода");
                        matrix_alloc_plan_summary.Status = HrmMatrixStatus.NOTDOWNLOADED;
                        kb_plan_matrix.Status = HrmMatrixStatus.NOTDOWNLOADED;
                        ozm_plan_matrix.Status = HrmMatrixStatus.NOTDOWNLOADED;
                        return;
                    }
                    else {
                        HrmMatrix plan_matrix = null;
                        String file_ord_code = each.OrderCode;
                        if (String.IsNullOrEmpty(file_ord_code) && file_ord_code != "") {
                            task.LogRecord(LogRecordType.WARNING, null, null, "Пустой код заказа в файле");
                        }
                        //if (file_ord_code.Length == 8) continue; //это пока в базе нет заказов с восьмизначным кодом!
                        String file_dep_code = each.DepartmentCode;
                        if (String.IsNullOrEmpty(file_dep_code) && file_dep_code != "") {
                            task.LogRecord(LogRecordType.WARNING, null, null, "Пустой код подразделения в файле");
                        }
                        /*
                        if (!orders_in_database.ContainsKey(file_ord_code) || !departments_in_database.ContainsKey(file_dep_code)) {
                            how_many_mismatches++;
                            continue;
                        }
                        */
                        //определяем к какой группе подразделений относится запись
                        HrmMatrixColumn current_column = null;
                        if (departments_in_database.ContainsKey(file_dep_code)) {
                            if (departments_in_database[file_dep_code].GroupDep == DepartmentGroupDep.DEPARTMENT_KB) {
                                plan_matrix = kb_plan_matrix;
                                plan_matrix_columns = kb_columns;
                                plan_matrix_rows = kb_rows;
                            }
                            else {
                                plan_matrix = ozm_plan_matrix;
                                plan_matrix_columns = ozm_columns;
                                plan_matrix_rows = ozm_rows;
                            }
                            if (plan_matrix_columns.ContainsKey(file_dep_code))
                                current_column = plan_matrix_columns[file_dep_code];
                            //если колонки еще не было - то создаем и инициализируем новую
                            else {
                                current_column = object_space.CreateObject<HrmMatrixColumn>();
                                current_column.Matrix = plan_matrix;
                                plan_matrix.Columns.Add(current_column);
                                current_column.Department = departments_in_database[file_dep_code];
                                plan_matrix_columns.Add(file_dep_code, current_column);
                            }
                        }
                        else {
                            task.LogRecord(LogRecordType.ERROR, departments_in_database[file_dep_code], orders_in_database[file_ord_code], "В справочниках не найдено подразделения с кодом " + each.DepartmentCode.Trim());
                        }
                        //теперь разбираемся со строчкой
                        HrmMatrixRow current_row = null;
                        if (plan_matrix_rows.ContainsKey(file_ord_code))
                            current_row = plan_matrix_rows[file_ord_code];
                        else {
                            current_row = object_space.CreateObject<HrmMatrixRow>();
                            current_row.Matrix = plan_matrix;
                            plan_matrix.Rows.Add(current_row);
                            plan_matrix_rows.Add(file_ord_code, current_row);
                            if (orders_in_database.ContainsKey(file_ord_code))
                                current_row.Order = orders_in_database[file_ord_code];
                            else {
                                task.LogRecord(LogRecordType.ERROR, departments_in_database[file_dep_code], orders_in_database[file_ord_code], "В справочниках не найдено заказа с кодом " + file_ord_code);
                            }
                        }
                        if (current_row != null && current_column != null) {
                            String cell_key = current_column.Department.BuhCode + "|" + current_row.Order.Code;
                            if (!cells_in_matrix.ContainsKey(cell_key)) {
                                HrmMatrixCell cell = object_space.CreateObject<HrmMatrixCell>();
                                cell.Time = each.Time;
                                cell.MoneyAllSumm = 0;
                                cell.Row = current_row;
                                cell.Column = current_column;
                                current_row.Cells.Add(cell);
                                current_column.Cells.Add(cell);
                                cells_in_matrix.Add(cell_key, cell);
                            }
                            else {
                                HrmMatrixCell cell = cells_in_matrix[cell_key];
                                cell.Time += each.Time;
                            }
                        }
                        else {
                            task.LogRecord(LogRecordType.WARNING, current_column.Department, current_row.Order, "Не удалось создать ячейку матрицы из-за отсутствия подразделения и/или заказа");
                        }
                    }
                }
                foreach (var data in const_order_list) {
                    if (data.Year != current_year) {
                        task.Abort();
                        task.LogRecord(LogRecordType.ERROR, null, null, "Дата в файле 'Const_OrderTime.ncd' не соответствует дате текущего периода");
                        matrix_alloc_plan_summary.Status = HrmMatrixStatus.NOTDOWNLOADED;
                        kb_plan_matrix.Status = HrmMatrixStatus.NOTDOWNLOADED;
                        ozm_plan_matrix.Status = HrmMatrixStatus.NOTDOWNLOADED;
                        return;
                    }
                    else {
                        String file_order_code = data.OrderCode;
                        String file_dep_code = data.DepartmentCode;
                        if (String.IsNullOrEmpty(file_order_code)) {
                            task.LogRecord(LogRecordType.WARNING, null, null, "Пустой код заказа в файле");
                        }
                        if (String.IsNullOrEmpty(file_dep_code)) {
                            task.LogRecord(LogRecordType.WARNING, null, null, "Пустой код подразделения в файле");
                        }
                        if (!String.IsNullOrEmpty(file_dep_code) && !String.IsNullOrEmpty(file_order_code)) {
                            String cell_key = file_dep_code + "|" + file_order_code;
                            try {
                                cells_in_matrix[cell_key].ConstOrderTime += data.Time;
                            }
                            catch (KeyNotFoundException) {
                                task.LogRecord(LogRecordType.ERROR, null, null, "В матрице нет такой ячейки и/или код подразделения и/или заказа в файле пустые");
                            }
                        }
                        else {
                            task.LogRecord(LogRecordType.WARNING, null, null, "Не удалось заполнить поле 'Постоянное время заказа'");
                        }
                    }
                }
                foreach (var travel in travel_list) {
                    if (travel.Year != current_year || travel.Month != current_month) {
                        task.Abort();
                        task.LogRecord(LogRecordType.ERROR, null, null, "Дата в файле 'Matrix_TravelTimePlan.ncd' не соответствует дате текущего периода");
                        matrix_alloc_plan_summary.Status = HrmMatrixStatus.NOTDOWNLOADED;
                        kb_plan_matrix.Status = HrmMatrixStatus.NOTDOWNLOADED;
                        ozm_plan_matrix.Status = HrmMatrixStatus.NOTDOWNLOADED;
                        return;
                    }
                    else {
                        String file_order_code = travel.OrderCode;
                        String file_dep_code = travel.DepartmentCode;
                        if (String.IsNullOrEmpty(file_order_code)) {
                            task.LogRecord(LogRecordType.WARNING, null, null, "Пустой код заказа в файле");
                        }
                        if (String.IsNullOrEmpty(file_dep_code)) {
                            task.LogRecord(LogRecordType.WARNING, null, null, "Пустой код подразделения в файле");
                        }
                        if (!String.IsNullOrEmpty(file_dep_code) && !String.IsNullOrEmpty(file_order_code)) {
                            String cell_key = file_dep_code + "|" + file_order_code;
                            try {
                                cells_in_matrix[cell_key].TravelTime += travel.TravelTime;
                            }
                            catch (KeyNotFoundException) {
                                task.LogRecord(LogRecordType.WARNING, departments_in_database[file_dep_code], orders_in_database[file_order_code], "В матрице нет такой ячейки и/или код подразделения и/или заказа в файле пустые");
                            }
                        }
                        else {
                            task.LogRecord(LogRecordType.WARNING, null, null, "Не удалось заполнить поле 'Постоянное время заказа'");
                        }
                    }
                }
            }
            else {
                HrmMatrixAllocPlan matrix_alloc_plan_summary = object_space.CreateObject<HrmMatrixAllocPlan>();
                matrix_alloc_plan_summary.Status = HrmMatrixStatus.NOTDOWNLOADED;
                matrix_alloc_plan_summary.Type = HrmMatrixType.TYPE_MATIX;
                matrix_alloc_plan_summary.TypeMatrix = HrmMatrixTypeMatrix.MATRIX_PLANNED;
                matrix_alloc_plan_summary.GroupDep = DepartmentGroupDep.DEPARTMENT_KB_OZM;
                task.Period.CurrentMatrixAllocPlanSummary = matrix_alloc_plan_summary;
                matrix_alloc_plan_summary.IterationNumber = 1;
                //Инициализируем плановые матрицы кб и озм
                HrmMatrixAllocPlan kb_plan_matrix = object_space.CreateObject<HrmMatrixAllocPlan>();
                kb_plan_matrix.Status = HrmMatrixStatus.NOTDOWNLOADED;
                //            kb_plan_matrix.Period = period;
                kb_plan_matrix.TypeMatrix = HrmMatrixTypeMatrix.MATRIX_PLANNED;
                kb_plan_matrix.Type = HrmMatrixType.TYPE_MATIX;
                kb_plan_matrix.GroupDep = DepartmentGroupDep.DEPARTMENT_KB;
                kb_plan_matrix.IterationNumber = 1;
                task.Period.Matrixs.Add(kb_plan_matrix);
                task.Period.CurrentMatrixAllocPlanKB = kb_plan_matrix;
                HrmMatrixAllocPlan ozm_plan_matrix = object_space.CreateObject<HrmMatrixAllocPlan>();
                ozm_plan_matrix.Status = HrmMatrixStatus.NOTDOWNLOADED;
                //            ozm_plan_matrix.Period = period;
                ozm_plan_matrix.TypeMatrix = HrmMatrixTypeMatrix.MATRIX_PLANNED;
                ozm_plan_matrix.Type = HrmMatrixType.TYPE_MATIX;
                ozm_plan_matrix.GroupDep = DepartmentGroupDep.DEPARTMENT_OZM;
                ozm_plan_matrix.IterationNumber = 1;
                task.Period.Matrixs.Add(ozm_plan_matrix);
                task.Period.CurrentMatrixAllocPlanOZM = ozm_plan_matrix;
                task.MatrixPlanKB = kb_plan_matrix;
                task.MatrixPlanOZM = ozm_plan_matrix;
            }
        }
    }
}