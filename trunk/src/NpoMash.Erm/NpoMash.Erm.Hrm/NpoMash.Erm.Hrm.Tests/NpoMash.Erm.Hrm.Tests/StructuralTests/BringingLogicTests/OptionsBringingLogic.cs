using System;
using System.Linq;
using System.Collections.Generic;

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
        private IDictionary<String, DepartmentType> departments;
        private enum DepartmentType {
            MICRO_DEPARTMENT = 0,
            SMALL_DEPARTMENT = 1,
            BIG_DEPARTMENT = 2
        }

        [SetUp]
        protected void SetUp() {
            IObjectSpaceProvider object_space_provider = new XPObjectSpaceProvider(new MemoryDataStoreProvider());
            application = new TestApplication();
            ModuleBase test_module = new ModuleBase();
            test_module.AdditionalExportedTypes.Add(typeof(HrmSalaryTaskMatrixReduction));
            application.Modules.Add(test_module);
            application.Setup("BringingApp", object_space_provider);
        }

        protected virtual void CreateTimeSheet(IObjectSpace object_space, HrmSalaryTaskImportSourceData task) {
            var random = new Random();
            HrmTimeSheetLogic.TaskSheetInit(object_space, task);
            foreach (var column in object_space.GetObjects<HrmMatrixColumn>()) {
                Int64 dep_sum = 0;
                Int64 controlled_dep_sum = 0;
                foreach (var cell in column.Cells) {
                    dep_sum += cell.Time;
                    if (cell.Row.Order.TypeControl == FmCOrderTypeControl.TRUDEMK_FOT) {
                        controlled_dep_sum += cell.Time;
                    }
                }
                if (departments[column.Department.Code] == DepartmentType.MICRO_DEPARTMENT) {
                    HrmTimeSheetDep sheet_dep = object_space.CreateObject<HrmTimeSheetDep>();
                    sheet_dep.Department = column.Department;
                    sheet_dep.BaseWorkTime = Convert.ToInt32(2 * dep_sum);
                    sheet_dep.AdditionWorkTime = 0;
                    if (column.Department.GroupDep == DepartmentGroupDep.DEPARTMENT_KB) {
                        task.TimeSheetKB.TimeSheetDeps.Add(sheet_dep);
                    }
                    if (column.Department.GroupDep == DepartmentGroupDep.DEPARTMENT_OZM) {
                        task.TimeSheetOZM.TimeSheetDeps.Add(sheet_dep);
                    }
                }
                if (departments[column.Department.Code] == DepartmentType.SMALL_DEPARTMENT) {
                    HrmTimeSheetDep sheet_dep = object_space.CreateObject<HrmTimeSheetDep>();
                    sheet_dep.Department = column.Department;
                    sheet_dep.BaseWorkTime = Convert.ToInt32(2 * controlled_dep_sum + dep_sum);
                    sheet_dep.AdditionWorkTime = 0;
                    if (column.Department.GroupDep == DepartmentGroupDep.DEPARTMENT_KB) {
                        task.TimeSheetKB.TimeSheetDeps.Add(sheet_dep);
                    }
                    if (column.Department.GroupDep == DepartmentGroupDep.DEPARTMENT_OZM) {
                        task.TimeSheetOZM.TimeSheetDeps.Add(sheet_dep);
                    }
                }
                if (departments[column.Department.Code] == DepartmentType.BIG_DEPARTMENT) {
                    HrmTimeSheetDep sheet_dep = object_space.CreateObject<HrmTimeSheetDep>();
                    sheet_dep.Department = column.Department;
                    sheet_dep.BaseWorkTime = Convert.ToInt32(Math.Round(Convert.ToDouble(dep_sum) / 1.5, 0));
                    sheet_dep.AdditionWorkTime = 0;
                    if (column.Department.GroupDep == DepartmentGroupDep.DEPARTMENT_KB) {
                        task.TimeSheetKB.TimeSheetDeps.Add(sheet_dep);
                    }
                    if (column.Department.GroupDep == DepartmentGroupDep.DEPARTMENT_OZM) {
                        task.TimeSheetOZM.TimeSheetDeps.Add(sheet_dep);
                    }
                }
            }
        }

        private void CreateDepartments(IObjectSpace object_space, Int32 micro_department_count, Int32 small_department_count, Int32 big_department_count) {
            var random = new Random();
            departments = new Dictionary<String, DepartmentType>();
            for (int i = 0 ; i < micro_department_count ; i++) {
                String department_code = Convert.ToString(random.Next(1, 10000));
                if (!departments.ContainsKey(department_code)) { departments.Add(department_code, DepartmentType.MICRO_DEPARTMENT); }
                else { micro_department_count += 1; }
            }
            for (int i = 0 ; i < small_department_count ; i++) {
                String department_code = Convert.ToString(random.Next(1, 10000));
                if (!departments.ContainsKey(department_code)) { departments.Add(department_code, DepartmentType.SMALL_DEPARTMENT); }
                else { small_department_count += 1; }
            }
            for (int i = 0 ; i < big_department_count ; i++) {
                String department_code = Convert.ToString(random.Next(1, 10000));
                if (!departments.ContainsKey(department_code)) { departments.Add(department_code, DepartmentType.BIG_DEPARTMENT); }
                else { big_department_count += 1; }
            }
            foreach (var code in departments.Keys) {
                int group_dep = random.Next(1, 3);
                Department department = object_space.CreateObject<Department>();
                department.Code = code;
                if (group_dep == 1) { department.GroupDep = DepartmentGroupDep.DEPARTMENT_KB; }
                else { department.GroupDep = DepartmentGroupDep.DEPARTMENT_OZM; }
            }
        }

        private void CreateUncontrolledOrders(IObjectSpace object_space, Int32 uncontrolled_orders_count) {
            var random = new Random();
            var orders_list = object_space.GetObjects<fmCOrder>();
            IList<String> exist_orders_code_list = new List<String>();
            IList<String> new_orders_code_list = new List<String>();
            foreach (var order in orders_list) {
                exist_orders_code_list.Add(order.Code);
            }
            for (int i = 0 ; i < uncontrolled_orders_count ; i++) {
                String order_code = Convert.ToString(random.Next(100000, 100000000));
                if (!exist_orders_code_list.Contains(order_code)) { new_orders_code_list.Add(order_code); }
                else { uncontrolled_orders_count += 1; }
            }
            foreach (var code in new_orders_code_list) {
                int type_control = random.Next(1, 3);
                fmCOrder order = object_space.CreateObject<fmCOrder>();
                order.Code = code;
                order.TypeConstancy = FmCOrderTypeConstancy.CONST_ORDER_TYPE;
                if (type_control == 1) { order.TypeControl = FmCOrderTypeControl.FOT; }
                else { order.TypeControl = FmCOrderTypeControl.TRUDEMK_FOT; }
            }
        }

        protected virtual HrmMatrixAllocPlan CreateMatrixAllocPlan(IObjectSpace object_space, HrmPeriod current_period, DepartmentGroupDep group,
            Int32 probability) {
            var random = new Random();
            HrmMatrixAllocPlan plan_matrix = object_space.CreateObject<HrmMatrixAllocPlan>();
            IList<Department> departments_in_object_space = object_space.GetObjects<Department>()
                .Where<Department>(x => x.GroupDep == group)
                .ToList<Department>();
            IList<fmCOrder> orders_in_database = object_space.GetObjects<fmCOrder>()
                .ToList<fmCOrder>();
            Dictionary<String, HrmMatrixRow> existing_rows = new Dictionary<String, HrmMatrixRow>();
            foreach (Department current_department in departments_in_object_space) {
                switch (departments[current_department.Code]) {
                    case DepartmentType.MICRO_DEPARTMENT: {
                            HrmMatrixRow current_row = null;
                            HrmMatrixColumn current_column = object_space.CreateObject<HrmMatrixColumn>();
                            current_column.Department = current_department;
                            current_column.Matrix = plan_matrix;
                            plan_matrix.Columns.Add(current_column);
                            foreach (var current_order in orders_in_database) {
                                int span = random.Next(100);
                                if ((current_order.TypeControl == FmCOrderTypeControl.TRUDEMK_FOT)&&(span <= probability)) {
                                    if (existing_rows.ContainsKey(current_order.Code)) { current_row = existing_rows[current_order.Code]; }
                                    else {
                                        current_row = object_space.CreateObject<HrmMatrixRow>();
                                        current_row.Order = current_order;
                                        plan_matrix.Rows.Add(current_row);
                                        current_row.Matrix = plan_matrix;
                                        existing_rows.Add(current_order.Code, current_row);
                                    }
                                    HrmMatrixCell new_cell = object_space.CreateObject<HrmMatrixCell>();
                                    new_cell.Time = random.Next(750);
                                    new_cell.Sum = 0;
                                    new_cell.Column = current_column;
                                    new_cell.Row = current_row;
                                    current_column.Cells.Add(new_cell);
                                    current_row.Cells.Add(new_cell);
                                }
                            }
                            break;
                        }
                    case DepartmentType.SMALL_DEPARTMENT: {
                            HrmMatrixRow current_row = null;
                            HrmMatrixColumn current_column = object_space.CreateObject<HrmMatrixColumn>();
                            current_column.Department = current_department;
                            current_column.Matrix = plan_matrix;
                            plan_matrix.Columns.Add(current_column);
                            foreach (var current_order in orders_in_database) {
                                int span = random.Next(100);
                                if (span <= probability) {
                                    if (existing_rows.ContainsKey(current_order.Code)) { current_row = existing_rows[current_order.Code]; }
                                    else {
                                        current_row = object_space.CreateObject<HrmMatrixRow>();
                                        current_row.Order = current_order;
                                        plan_matrix.Rows.Add(current_row);
                                        current_row.Matrix = plan_matrix;
                                        existing_rows.Add(current_order.Code, current_row);
                                    }
                                    HrmMatrixCell new_cell = object_space.CreateObject<HrmMatrixCell>();
                                    new_cell.Time = random.Next(750);
                                    new_cell.Sum = 0;
                                    new_cell.Column = current_column;
                                    new_cell.Row = current_row;
                                    current_column.Cells.Add(new_cell);
                                    current_row.Cells.Add(new_cell);
                                }
                            }
                            break;
                        }
                    case DepartmentType.BIG_DEPARTMENT: {
                            HrmMatrixRow current_row = null;
                            HrmMatrixColumn current_column = object_space.CreateObject<HrmMatrixColumn>();
                            current_column.Department = current_department;
                            current_column.Matrix = plan_matrix;
                            plan_matrix.Columns.Add(current_column);
                            foreach (var current_order in orders_in_database) {
                                int span = random.Next(100);
                                if (span <= probability) {
                                    if (existing_rows.ContainsKey(current_order.Code)) { current_row = existing_rows[current_order.Code]; }
                                    else {
                                        current_row = object_space.CreateObject<HrmMatrixRow>();
                                        current_row.Order = current_order;
                                        plan_matrix.Rows.Add(current_row);
                                        current_row.Matrix = plan_matrix;
                                        existing_rows.Add(current_order.Code, current_row);
                                    }
                                    HrmMatrixCell new_cell = object_space.CreateObject<HrmMatrixCell>();
                                    new_cell.Time = random.Next(750);
                                    new_cell.Sum = 0;
                                    new_cell.Column = current_column;
                                    new_cell.Row = current_row;
                                    current_column.Cells.Add(new_cell);
                                    current_row.Cells.Add(new_cell);
                                }
                            }
                            break;
                        }
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

        protected HrmSalaryTaskMatrixReduction PrepareTestData(IObjectSpace prepare_object_space, Int32 controlled_orders_count,
            Int32 uncontrolled_orders_count, Int32 micro_depaprtment_count, Int32 big_department_count, Int32 small_department_count, Int32 probability) {
            TestWCLogic.AllocparameterCount = 1;
            TestWCLogic.AddControlledOrders(prepare_object_space, controlled_orders_count);
            HrmPeriod period = null;
            TestWCLogic.addTestData(prepare_object_space);
            prepare_object_space.CommitChanges();
            foreach (var periods in prepare_object_space.GetObjects<HrmPeriod>()) {
                periods.setStatus(HrmPeriodStatus.OPENED);
                period = periods;
            }
            HrmPeriod current_period = period;
            CreateDepartments(prepare_object_space, micro_depaprtment_count, small_department_count, big_department_count);
            CreateUncontrolledOrders(prepare_object_space, uncontrolled_orders_count);
            prepare_object_space.CommitChanges();
            HrmSalaryTaskImportSourceData task = prepare_object_space.CreateObject<HrmSalaryTaskImportSourceData>();
            current_period.PeriodTasks.Add(task);
            task.MatrixPlanKB = CreateMatrixAllocPlan(prepare_object_space, current_period, DepartmentGroupDep.DEPARTMENT_KB, probability);
            task.MatrixPlanKB.Status = HrmMatrixStatus.MATRIX_ACCEPTED;
            task.MatrixPlanOZM = CreateMatrixAllocPlan(prepare_object_space, current_period, DepartmentGroupDep.DEPARTMENT_OZM, probability);
            task.MatrixPlanOZM.Status = HrmMatrixStatus.MATRIX_ACCEPTED;
            prepare_object_space.CommitChanges();
            CreateTimeSheet(prepare_object_space, task);
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
            if (period.CurrentOZMmatrixReduction == null) {
                reduc = HrmSalaryTaskMatrixReductionLogic.initTaskMatrixReduction(prepare_object_space, period, group_dep, bringing_method);
            }
            else {
                reduc = prepare_object_space.GetObject<HrmSalaryTaskMatrixReduction>(period.CurrentKBmatrixReduction);
            }
            prepare_object_space.CommitChanges();
            return reduc;
        }
    }
}