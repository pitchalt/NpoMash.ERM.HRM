using System;
using System.Collections.Generic;
using System.Linq;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;

using IntecoAG.ERM.FM.Order;
using NpoMash.Erm.Hrm.Salary;
using IntecoAG.ERM.HRM.Organization;
using NpoMash.Erm.Hrm.Tests.Controllers;

using NUnit.Framework;

namespace NpoMash.Erm.Hrm.Tests.StructuralTests.BringingLogicTests {


    [TestFixture]
    public class OptionsBringingLogic {

        protected TestApplication application;

        [SetUp]
        protected void SetUp() {
            IObjectSpaceProvider object_space_provider = new XPObjectSpaceProvider(new MemoryDataStoreProvider());
            application = new TestApplication();
            ModuleBase test_module = new ModuleBase();
            test_module.AdditionalExportedTypes.Add(typeof(HrmSalaryTaskMatrixReduction));
            application.Modules.Add(test_module);
            application.Setup("BringingApp", object_space_provider);
        }

        private void CreateTimeSheet(IObjectSpace os, HrmSalaryTaskImportSourceData task, Int32 time_sheet_work_time) {
            Random rand = new Random();
            HrmTimeSheetLogic.TaskSheetInit(os, task);
            foreach (Department current_department in os.GetObjects<Department>()) {
                HrmTimeSheetDep sheet_dep = os.CreateObject<HrmTimeSheetDep>();
                sheet_dep.Department = current_department;
                sheet_dep.BaseWorkTime = time_sheet_work_time;
                sheet_dep.AdditionWorkTime = 0;
                if (current_department.GroupDep == DepartmentGroupDep.DEPARTMENT_KB) {
                    task.TimeSheetKB.TimeSheetDeps.Add(sheet_dep);
                }
                if (current_department.GroupDep == DepartmentGroupDep.DEPARTMENT_OZM) {
                    task.TimeSheetOZM.TimeSheetDeps.Add(sheet_dep);
                }
            }
        }

        private HrmMatrixAllocPlan CreateMatrixAllocPlan(IObjectSpace os, HrmPeriod current_period, DepartmentGroupDep group, Int32 plan_work_time_min, Int32 plan_work_time_max, Boolean pairs) {
            Random random = new Random();
            //List<HrmMatrixColumn> columns = new List<HrmMatrixColumn>();
            //List<HrmMatrixRow> rows = new List<HrmMatrixRow>();
            HrmMatrixAllocPlan plan_matrix = os.CreateObject<HrmMatrixAllocPlan>();
            List<Department> departments_in_database = os.GetObjects<Department>()
                .Where<Department>(x => x.GroupDep == group)
                .ToList<Department>();
            List<fmCOrder> orders_in_database = os.GetObjects<fmCOrder>()
                .ToList<fmCOrder>();
            Dictionary<String, HrmMatrixRow> existing_rows = new Dictionary<String ,HrmMatrixRow>();
            
            foreach (Department current_department in departments_in_database){
                HrmMatrixColumn current_column = os.CreateObject<HrmMatrixColumn>();
                HrmMatrixRow current_row = null;
                current_column.Department = current_department;
                current_column.Matrix = plan_matrix;
                plan_matrix.Columns.Add(current_column);
                current_column.Department = current_department;
                foreach (fmCOrder current_order in orders_in_database) {
                    // сюды надо вставить логику выброса ячейки и continue
                    if (existing_rows.ContainsKey(current_order.Code))
                        current_row = existing_rows[current_order.Code];
                    else {
                        current_row = os.CreateObject<HrmMatrixRow>();
                        current_row.Order = current_order;
                        plan_matrix.Rows.Add(current_row);
                        current_row.Matrix = plan_matrix;
                        existing_rows.Add(current_order.Code, current_row);
                    }
                    HrmMatrixCell new_cell = os.CreateObject<HrmMatrixCell>();
                    new_cell.Time = Convert.ToInt16(random.Next(plan_work_time_min, plan_work_time_max));
                    new_cell.Sum = 0;
                    new_cell.Column = current_column;
                    new_cell.Row = current_row;
                    current_row.Cells.Add(new_cell);
                    current_column.Cells.Add(new_cell);
                }

            }
            plan_matrix.Type = HrmMatrixType.TYPE_MATIX;
            plan_matrix.TypeMatrix = HrmMatrixTypeMatrix.MATRIX_PLANNED;
            plan_matrix.GroupDep = group;
            plan_matrix.Status = HrmMatrixStatus.MATRIX_OPENED;
            plan_matrix.IterationNumber = 1;
            plan_matrix.Variant = HrmMatrixVariant.PROPORTIONS_METHOD_VARIANT;
            plan_matrix.Period = current_period;
            current_period.Matrixs.Add(plan_matrix);
            return plan_matrix;
        }

        protected HrmSalaryTaskMatrixReduction PrepareTestData(IObjectSpace prepare_object_space, Int32 department_count, Int32 alloc_parameter_count, Int32 referential_data_count, Int32 time_sheet_work_time,
            Int32 plan_work_time_min, Int32 plan_work_time_max, Boolean dinamyc_gen) {
            TestWCLogic.AllocparameterCount = alloc_parameter_count;
            TestWCLogic.DepartmentCount = department_count;
            TestWCLogic.ReferenceCount = referential_data_count;
            if (dinamyc_gen) { TestWCLogic.referenceClassesGenerate(prepare_object_space); }
            else { TestWCLogic.referenceClassesGenerate(prepare_object_space, !dinamyc_gen); }
            HrmPeriod period = null;
            TestWCLogic.addTestData(prepare_object_space);
            prepare_object_space.CommitChanges();
            foreach (var periods in prepare_object_space.GetObjects<HrmPeriod>()) {
                periods.setStatus(HrmPeriodStatus.OPENED);
                period = periods;
            }
            HrmPeriod current_period = period;
            HrmSalaryTaskImportSourceData task = prepare_object_space.CreateObject<HrmSalaryTaskImportSourceData>();
            current_period.PeriodTasks.Add(task);
            task.MatrixPlanKB = CreateMatrixAllocPlan(prepare_object_space, current_period, DepartmentGroupDep.DEPARTMENT_KB, plan_work_time_min, plan_work_time_max, false);
            task.MatrixPlanKB.Status = HrmMatrixStatus.MATRIX_ACCEPTED;
            task.MatrixPlanOZM = CreateMatrixAllocPlan(prepare_object_space, current_period, DepartmentGroupDep.DEPARTMENT_OZM, plan_work_time_min, plan_work_time_max, false);
            task.MatrixPlanOZM.Status = HrmMatrixStatus.MATRIX_ACCEPTED;
            CreateTimeSheet(prepare_object_space, task, time_sheet_work_time);
            DepartmentGroupDep group_dep = DepartmentGroupDep.DEPARTMENT_KB;
            period.setStatus(HrmPeriodStatus.READY_TO_CALCULATE_COERCED_MATRIXS);
            HrmSalaryTaskMatrixReduction reduc = null;
            HrmMatrixVariant bringing_method = HrmMatrixVariant.MINIMIZE_NUMBER_OF_DEVIATIONS_VARIANT;
            if (period.CurrentKBmatrixReduction == null) {
                reduc = HrmSalaryTaskMatrixReductionLogic.initTaskMatrixReduction(prepare_object_space, period, group_dep, bringing_method);
            }
            else {
                reduc = prepare_object_space.GetObject<HrmSalaryTaskMatrixReduction>(period.CurrentKBmatrixReduction);
            }
            /*
            if (period.CurrentOZMmatrixReduction == null) {
                reduc = HrmSalaryTaskMatrixReductionLogic.initTaskMatrixReduction(prepare_object_space, period, group_dep, bringing_method);
            }
            else {
                reduc = prepare_object_space.GetObject<HrmSalaryTaskMatrixReduction>(period.CurrentKBmatrixReduction);
            }
             * */
            prepare_object_space.CommitChanges();
            return reduc;
        }

        protected HrmSalaryTaskMatrixReduction PrepareTestData(IObjectSpace prepare_object_space, Int32 department_count, Int32 alloc_parameter_count, Int32 referential_data_count, Int32 time_sheet_work_time,
            Int32 plan_work_time_min, Int32 plan_work_time_max, Boolean dinamyc_gen, Boolean in_pairs) {
            TestWCLogic.AllocparameterCount = alloc_parameter_count;
            TestWCLogic.DepartmentCount = department_count;
            TestWCLogic.ReferenceCount = referential_data_count;
            if (dinamyc_gen) { TestWCLogic.referenceClassesGenerate(prepare_object_space); }
            else { TestWCLogic.referenceClassesGenerate(prepare_object_space, !dinamyc_gen); }
            HrmPeriod period = null;
            TestWCLogic.addTestData(prepare_object_space);
            prepare_object_space.CommitChanges();
            foreach (var periods in prepare_object_space.GetObjects<HrmPeriod>()) {
                periods.setStatus(HrmPeriodStatus.OPENED);
                period = periods;
            }
            HrmPeriod current_period = period;
            HrmSalaryTaskImportSourceData task = prepare_object_space.CreateObject<HrmSalaryTaskImportSourceData>();
            current_period.PeriodTasks.Add(task);
            if (in_pairs) {
                task.MatrixPlanKB = CreateMatrixAllocPlan(prepare_object_space, current_period, DepartmentGroupDep.DEPARTMENT_KB, plan_work_time_min, plan_work_time_max, true);
                task.MatrixPlanKB.Status = HrmMatrixStatus.MATRIX_ACCEPTED;
                task.MatrixPlanOZM = CreateMatrixAllocPlan(prepare_object_space, current_period, DepartmentGroupDep.DEPARTMENT_OZM, plan_work_time_min, plan_work_time_max, true);
                task.MatrixPlanOZM.Status = HrmMatrixStatus.MATRIX_ACCEPTED;
            }
            CreateTimeSheet(prepare_object_space, task, time_sheet_work_time);
            DepartmentGroupDep group_dep = DepartmentGroupDep.DEPARTMENT_KB;
            period.setStatus(HrmPeriodStatus.READY_TO_CALCULATE_COERCED_MATRIXS);
            HrmSalaryTaskMatrixReduction reduc = null;
            HrmMatrixVariant bringing_method = HrmMatrixVariant.MINIMIZE_NUMBER_OF_DEVIATIONS_VARIANT;
            if (period.CurrentKBmatrixReduction == null) {
                reduc = HrmSalaryTaskMatrixReductionLogic.initTaskMatrixReduction(prepare_object_space, period, group_dep, bringing_method);
            }
            else {
                reduc = prepare_object_space.GetObject<HrmSalaryTaskMatrixReduction>(period.CurrentKBmatrixReduction);
            }
            /*
            if (period.CurrentOZMmatrixReduction == null) {
                reduc = HrmSalaryTaskMatrixReductionLogic.initTaskMatrixReduction(prepare_object_space, period, group_dep, bringing_method);
            }
            else {
                reduc = prepare_object_space.GetObject<HrmSalaryTaskMatrixReduction>(period.CurrentKBmatrixReduction);
            }
             * */
            prepare_object_space.CommitChanges();
            return reduc;
        }
    }
}


/*private HrmMatrixAllocPlan CreateMatrixAllocPlan(IObjectSpace os, HrmPeriod current_period, DepartmentGroupDep group, Int32 plan_work_time_min, Int32 plan_work_time_max, Boolean pairs) {
            Random random = new Random();
            List<HrmMatrixColumn> columns = new List<HrmMatrixColumn>();
            List<HrmMatrixRow> rows = new List<HrmMatrixRow>();
            HrmMatrixAllocPlan plan_matrix = os.CreateObject<HrmMatrixAllocPlan>();
            Dictionary<String, Department> departments_in_database = os.GetObjects<Department>()
                .ToDictionary<Department, String>(x => x.Code);
            Dictionary<String, fmCOrder> orders_in_database = os.GetObjects<fmCOrder>()
                .ToDictionary<fmCOrder, String>(x => x.Code);
            //foreach (fmCOrder current_order in os.GetObjects<fmCOrder>()) {
            foreach (Department current_department in departments_in_database.Values) {

                HrmMatrixRow current_row = os.CreateObject<HrmMatrixRow>();
                current_row.Matrix = plan_matrix;
                plan_matrix.Rows.Add(current_row);
                current_row.Order = current_order;
                HrmMatrixColumn current_column = null;
                
                foreach (Department current_department in os.GetObjects<Department>()) {
                    if (current_department.GroupDep == group) {
                        foreach (HrmMatrixColumn col in plan_matrix.Columns)
                            if (col.Department == current_department) current_column = col;
                        if (current_column == null) {
                            current_column = os.CreateObject<HrmMatrixColumn>();
                            current_column.Department = current_department;
                            current_column.Matrix = plan_matrix;
                            plan_matrix.Columns.Add(current_column);
                        }
                        HrmMatrixCell new_cell = os.CreateObject<HrmMatrixCell>();
                        new_cell.Time = Convert.ToInt16(random.Next(plan_work_time_min, plan_work_time_max));
                        new_cell.Sum = 0;
                        new_cell.Column = current_column;
                        new_cell.Row = current_row;
                        current_row.Cells.Add(new_cell);
                        current_column.Cells.Add(new_cell);

                    }
                    current_column = null;
                }
            }
            plan_matrix.Type = HrmMatrixType.TYPE_MATIX;
            plan_matrix.TypeMatrix = HrmMatrixTypeMatrix.MATRIX_PLANNED;
            plan_matrix.GroupDep = group;
            plan_matrix.Status = HrmMatrixStatus.MATRIX_OPENED;
            plan_matrix.IterationNumber = 1;
            plan_matrix.Variant = HrmMatrixVariant.PROPORTIONS_METHOD_VARIANT; ;
            plan_matrix.Period = current_period;
            current_period.Matrixs.Add(plan_matrix);
            return plan_matrix;
        }*/