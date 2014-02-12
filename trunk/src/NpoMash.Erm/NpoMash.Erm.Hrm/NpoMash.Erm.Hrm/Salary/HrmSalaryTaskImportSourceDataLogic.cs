using System;
using System.Linq;
using System.Text;
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
            HrmTimeSheetLogic.TaskSheetInit(os, task);
            var timesheet_data = new FileHelperEngine<ImportMatrixTimeSheet>();
            ImportMatrixTimeSheet[] timesheet_list = timesheet_data.ReadFile("../../../../../../../var/Matrix_TimeSheet.dat");
            IList<Department> deps = os.GetObjects<Department>();
            foreach (var each in timesheet_list) {
                String code = each.Department_Code;
                Department dep = deps.FirstOrDefault(x => x.Code == code);
                if (dep == null) continue;
                HrmTimeSheetDep sheet_dep = os.CreateObject<HrmTimeSheetDep>();
                sheet_dep.Department = dep;
                sheet_dep.BaseWorkTime = each.BaseWorkTime / 100;
                sheet_dep.ConstantWorkTime = each.ConstantWorkTime / 100;
                sheet_dep.AdditionWorkTime = 0;
                sheet_dep.TravelWorkTime = each.TravelWorkTime / 100;
                if (dep.GroupDep == DepartmentGroupDep.DEPARTMENT_KB) {
                    task.TimeSheetKB.TimeSheetDeps.Add(sheet_dep);
                }
                if (dep.GroupDep == DepartmentGroupDep.DEPARTMENT_OZM) {
                    task.TimeSheetOZM.TimeSheetDeps.Add(sheet_dep);
                }
            }
        }

        public static void ImportPlanMatrixes(IObjectSpace os, HrmSalaryTaskImportSourceData task) {
            //            HrmPeriod period, out HrmMatrixAllocPlan KBMatrix, out HrmMatrixAllocPlan OZMMatrix) {
            var plan_data = new FixedFileEngine<ImportMatrixPlan>();
            ImportMatrixPlan[] plan_list = plan_data.ReadFile("../../../../../../../var/Matrix_Plan.dat");
            //Инициализируем плановые матрицы кб и озм
            HrmMatrixAllocPlan kb_plan_matrix = os.CreateObject<HrmMatrixAllocPlan>();
            kb_plan_matrix.Status = HrmMatrixStatus.MATRIX_OPENED;
            //            kb_plan_matrix.Period = period;
            kb_plan_matrix.TypeMatrix = HrmMatrixTypeMatrix.MATRIX_PLANNED;
            kb_plan_matrix.Type = HrmMatrixType.TYPE_MATIX;
            kb_plan_matrix.GroupDep = DepartmentGroupDep.DEPARTMENT_KB;
            kb_plan_matrix.IterationNumber = 1;
            task.Period.Matrixs.Add(kb_plan_matrix);
            HrmMatrixAllocPlan ozm_plan_matrix = os.CreateObject<HrmMatrixAllocPlan>();
            ozm_plan_matrix.Status = HrmMatrixStatus.MATRIX_OPENED;
            //            ozm_plan_matrix.Period = period;
            ozm_plan_matrix.TypeMatrix = HrmMatrixTypeMatrix.MATRIX_PLANNED;
            ozm_plan_matrix.Type = HrmMatrixType.TYPE_MATIX;
            ozm_plan_matrix.GroupDep = DepartmentGroupDep.DEPARTMENT_OZM;
            ozm_plan_matrix.IterationNumber = 1;
            task.Period.Matrixs.Add(ozm_plan_matrix);

            Int16 current_year = task.Period.Year;
            Int16 current_month = task.Period.Month;
            //создаем необходимые словари, чтобы не наматывать круги в форычах при поиске
            Dictionary<String, HrmMatrixColumn> ozm_columns = new Dictionary<string, HrmMatrixColumn>();
            Dictionary<String, HrmMatrixRow> ozm_rows = new Dictionary<string, HrmMatrixRow>();
            Dictionary<String, HrmMatrixColumn> kb_columns = new Dictionary<string, HrmMatrixColumn>();
            Dictionary<String, HrmMatrixRow> kb_rows = new Dictionary<string, HrmMatrixRow>();
            Dictionary<String, HrmMatrixColumn> plan_matrix_columns = null;
            Dictionary<String, HrmMatrixRow> plan_matrix_rows = null;
            Dictionary<String, Department> departments_in_database = os.GetObjects<Department>()
                .ToDictionary<Department, String>(x => x.Code);
            Dictionary<String, fmCOrder> orders_in_database = os.GetObjects<fmCOrder>()
                .ToDictionary<fmCOrder, String>(x => x.Code);
            Int32 how_many_mismatches = 0;
            //начинаем перебирать строки в файле
            foreach (var each in plan_list) {
                //если запись относится к нашему периоду то начинаем обработку
                if (each.Year == current_year && each.Month == current_month) {
                    HrmMatrix plan_matrix = null;
                    String file_ord_code = each.OrderCode;
                    //if (file_ord_code.Length == 8) continue; //это пока в базе нет заказов с восьмизначным кодом!
                    String file_dep_code = each.Department_Code;
                    if (!orders_in_database.ContainsKey(file_ord_code) || !departments_in_database.ContainsKey(file_dep_code)) {
                        how_many_mismatches++;
                        continue;
                    }
                    //определяем к какой группе подразделений относится запись
                    if (departments_in_database.ContainsKey(file_dep_code))
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
                    else throw new Exception("There is no department in database with code " + each.Department_Code.Trim());
                    //иначе - создаем ячейку и начинаем ее заполнять
                    HrmMatrixCell cell = os.CreateObject<HrmMatrixCell>();
                    cell.Time = each.Time / 100;
                    cell.Sum = 0;
                    //разбираемся с колонкой
                    HrmMatrixColumn current_column = null;
                    if (plan_matrix_columns.ContainsKey(file_dep_code))
                        current_column = plan_matrix_columns[file_dep_code];
                    //если колонки еще не было - то создаем и инициализируем новую
                    else {
                        current_column = os.CreateObject<HrmMatrixColumn>();
                        current_column.Matrix = plan_matrix;
                        plan_matrix.Columns.Add(current_column);
                        current_column.Department = departments_in_database[file_dep_code];
                        plan_matrix_columns.Add(file_dep_code, current_column);
                    }
                    //теперь связываем колонку с ячейкой, больше с колонкой делать нечего
                    cell.Column = current_column;
                    current_column.Cells.Add(cell);
                    //теперь разбираемся со строчкой
                    HrmMatrixRow current_row = null;
                    if (plan_matrix_rows.ContainsKey(file_ord_code))
                        current_row = plan_matrix_rows[file_ord_code];
                    else {
                        current_row = os.CreateObject<HrmMatrixRow>();
                        current_row.Matrix = plan_matrix;
                        plan_matrix.Rows.Add(current_row);
                        plan_matrix_rows.Add(file_ord_code, current_row);
                        if (orders_in_database.ContainsKey(file_ord_code))
                            current_row.Order = orders_in_database[file_ord_code];
                        else throw new Exception("There is now order in database with code " + file_ord_code);
                    }
                    //теперь связываем строчку с ячейкой, больше со строчкой делать нечего
                    cell.Row = current_row;
                    current_row.Cells.Add(cell);
                }
            }
            task.MatrixPlanKB = kb_plan_matrix;
            task.MatrixPlanOZM = ozm_plan_matrix;
        }
    }
}