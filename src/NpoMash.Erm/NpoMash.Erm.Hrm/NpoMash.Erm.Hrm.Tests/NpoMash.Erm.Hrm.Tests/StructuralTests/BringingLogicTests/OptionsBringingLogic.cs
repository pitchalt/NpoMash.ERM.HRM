﻿using System;
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

        private void CreateTimeSheet(IObjectSpace os, HrmSalaryTaskImportSourceData task) {
            Random rand = new Random();
            HrmTimeSheetLogic.TaskSheetInit(os, task);
            foreach (Department current_department in os.GetObjects<Department>()) {
                HrmTimeSheetDep sheet_dep = os.CreateObject<HrmTimeSheetDep>();
                sheet_dep.Department = current_department;
                sheet_dep.BaseWorkTime = 100;
                sheet_dep.AdditionWorkTime = 0;
                if (current_department.GroupDep == DepartmentGroupDep.DEPARTMENT_KB) {
                    task.TimeSheetKB.TimeSheetDeps.Add(sheet_dep);
                }
                if (current_department.GroupDep == DepartmentGroupDep.DEPARTMENT_OZM) {
                    task.TimeSheetOZM.TimeSheetDeps.Add(sheet_dep);
                }
            }
        }

       protected virtual HrmMatrixAllocPlan CreateMatrixAllocPlan(IObjectSpace object_space, HrmPeriod current_period, DepartmentGroupDep group, 
            Int32 micro_department_count, Int32 small_department_count, Int32 big_department_count, Int32 uncontrolled_orders_count, Int32 probability) {
            Random random = new Random();
            IDictionary<String, DepartmentType> departments = new Dictionary<String, DepartmentType>();
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
                int type_control = random.Next(1,3);
                fmCOrder order = object_space.CreateObject<fmCOrder>();
                order.Code = code;
                order.TypeConstancy = FmCOrderTypeConstancy.CONST_ORDER_TYPE;
                if (type_control == 1) { order.TypeControl = FmCOrderTypeControl.FOT; }
                else { order.TypeControl = FmCOrderTypeControl.TRUDEMK_FOT; }
            }
            object_space.CommitChanges();
            HrmMatrixAllocPlan plan_matrix = object_space.CreateObject<HrmMatrixAllocPlan>();
            List<Department> departments_in_database = object_space.GetObjects<Department>()
                .Where<Department>(x => x.GroupDep == group)
                .ToList<Department>();
            List<fmCOrder> orders_in_database = object_space.GetObjects<fmCOrder>()
                .ToList<fmCOrder>();
            Dictionary<String, HrmMatrixRow> existing_rows = new Dictionary<String, HrmMatrixRow>();
            foreach (Department current_department in departments_in_database) {
                HrmMatrixColumn current_column = object_space.CreateObject<HrmMatrixColumn>();
                HrmMatrixRow current_row = null;
                current_column.Department = current_department;
                current_column.Matrix = plan_matrix;
                plan_matrix.Columns.Add(current_column);
                current_column.Department = current_department;
                foreach (fmCOrder current_order in orders_in_database) {
                    
                    if (existing_rows.ContainsKey(current_order.Code))
                        current_row = existing_rows[current_order.Code];
                    else {
                        current_row = object_space.CreateObject<HrmMatrixRow>();
                        current_row.Order = current_order;
                        plan_matrix.Rows.Add(current_row);
                        current_row.Matrix = plan_matrix;
                        existing_rows.Add(current_order.Code, current_row);
                    }
                    HrmMatrixCell new_cell = object_space.CreateObject<HrmMatrixCell>();
                    new_cell.Time = Convert.ToInt16(random.Next(100));
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
            HrmSalaryTaskImportSourceData task = prepare_object_space.CreateObject<HrmSalaryTaskImportSourceData>();
            current_period.PeriodTasks.Add(task);
            task.MatrixPlanKB = CreateMatrixAllocPlan(prepare_object_space, current_period, DepartmentGroupDep.DEPARTMENT_KB, micro_depaprtment_count, small_department_count, big_department_count, uncontrolled_orders_count, probability);
            task.MatrixPlanKB.Status = HrmMatrixStatus.MATRIX_ACCEPTED;
            task.MatrixPlanOZM = CreateMatrixAllocPlan(prepare_object_space, current_period, DepartmentGroupDep.DEPARTMENT_OZM, micro_depaprtment_count, small_department_count, big_department_count, uncontrolled_orders_count, probability);
            task.MatrixPlanOZM.Status = HrmMatrixStatus.MATRIX_ACCEPTED;
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