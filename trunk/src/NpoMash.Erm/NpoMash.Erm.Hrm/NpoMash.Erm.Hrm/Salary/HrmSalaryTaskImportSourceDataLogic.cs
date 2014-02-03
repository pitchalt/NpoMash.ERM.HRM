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
            var engine = new FileHelperEngine<ImportMatrixTimeSheet>();
            ImportMatrixTimeSheet[] stream = engine.ReadFile("../../../../../../../var/Matrix_TimeSheet.dat");
            IList<Department> deps = os.GetObjects<Department>();
            foreach (var each in stream) {
                String code = Convert.ToString(Convert.ToInt32(each.Code.Trim()));
                Department dep = deps.FirstOrDefault(x => x.Code == code);
                HrmTimeSheetDep sheet_dep = os.CreateObject<HrmTimeSheetDep>();
                sheet_dep.Department = dep;
                sheet_dep.BaseWorkTime = each.MatrixWorkTime;
                sheet_dep.AdditionWorkTime = 0;
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
            kb_plan_matrix.Status = HrmMatrixStatus.MATRIX_SAVED;
            //            kb_plan_matrix.Period = period;
            kb_plan_matrix.TypeMatrix = HrmMatrixTypeMatrix.MATRIX_PLANNED;
            kb_plan_matrix.Type = HrmMatrixType.TYPE_MATIX;
            kb_plan_matrix.GroupDep = DepartmentGroupDep.DEPARTMENT_KB;
            kb_plan_matrix.IterationNumber = 1;
            task.Period.Matrixs.Add(kb_plan_matrix);
            HrmMatrixAllocPlan ozm_plan_matrix = os.CreateObject<HrmMatrixAllocPlan>();
            ozm_plan_matrix.Status = HrmMatrixStatus.MATRIX_SAVED;
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
                    String file_ord_code = each.OrderCode.Trim();
                    //if (file_ord_code.Length == 8) continue; //это пока в базе нет заказов с восьмизначным кодом!
                    String file_dep_code = Convert.ToString(Convert.ToInt32(each.Department.Trim()));
                    if (!orders_in_database.ContainsKey(file_dep_code) || !departments_in_database.ContainsKey(file_ord_code)) {
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
                    else throw new Exception("There is no department in database with code " + each.Department.Trim());
                    /*foreach (Department dep in os.GetObjects<Department>()) {
                        if (String.Compare(Convert.ToString(Convert.ToInt32(each.Department.Trim())), dep.Code) == 0)
                            if (dep.GroupDep == DepartmentGroupDep.DEPARTMENT_KB)
                                plan_matrix = kb_plan_matrix;
                            else plan_matrix = ozm_plan_matrix;
                        //теперь мы знаем с какой матрицей работаем
                    }*/
                    //если не нашли такого подразделения - все плохо
                    //if (plan_matrix == null)
                    //    throw new Exception("There is no department with code " + each.Department.Trim());
                    //иначе - создаем ячейку и начинаем ее заполнять
                    HrmMatrixCell cell = os.CreateObject<HrmMatrixCell>();
                    cell.Time = each.Norm;
                    cell.Sum = 1;
                    //разбираемся с колонкой
                    HrmMatrixColumn current_column = null;
                    /*foreach (HrmMatrixColumn col in plan_matrix.Columns)
                        if (col.Department.Code == each.Department.Trim())
                            current_column = col;*/
                    if (plan_matrix_columns.ContainsKey(file_dep_code))
                        current_column = plan_matrix_columns[file_dep_code];
                    //если колонки еще не было - то создаем и инициализируем новую
                    else {
                        current_column = os.CreateObject<HrmMatrixColumn>();
                        current_column.Matrix = plan_matrix;
                        plan_matrix.Columns.Add(current_column);
                        current_column.Department = departments_in_database[file_dep_code];
                        plan_matrix_columns.Add(file_dep_code, current_column);
                        /*foreach (Department dep in os.GetObjects<Department>())
                            if (System.String.Compare(dep.Code, each.Department.Trim()) == 0)
                                current_column.Department = dep;
                        */
                    }
                    //теперь связываем колонку с ячейкой, больше с колонкой делать нечего
                    cell.Column = current_column;
                    current_column.Cells.Add(cell);
                    //теперь разбираемся со строчкой

                    HrmMatrixRow current_row = null;
                    if (plan_matrix_rows.ContainsKey(file_ord_code))
                        current_row = plan_matrix_rows[file_ord_code];
                    else {

                        //throw new Exception("There is now order in database wiht code " + file_ord_code);
                    /*foreach (HrmMatrixRow row in plan_matrix.Rows)
                        if (System.String.Compare(row.Order.Code, each.OrderCode.Trim()) == 0)
                            current_row = row;*/
                    //если строчки еще не было - тогда создаем и инициализируем новую
                    //if (current_row == null) {
                        current_row = os.CreateObject<HrmMatrixRow>();
                        current_row.Matrix = plan_matrix;
                        plan_matrix.Rows.Add(current_row);
                        plan_matrix_rows.Add(file_ord_code, current_row);
                        if (orders_in_database.ContainsKey(file_ord_code))
                            current_row.Order = orders_in_database[file_ord_code];
                        else throw new Exception("There is now order in database with code " + file_ord_code);
                        /*foreach (fmCOrder order in os.GetObjects<fmCOrder>())
                            if (System.String.Compare(order.Code, each.OrderCode.Trim()) == 0)
                                current_row.Order = order;*/
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