using System;
using System.Collections.Generic;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;

using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.FM.Order;
using NpoMash.Erm.Hrm.Salary;
using NpoMash.Erm.Hrm.Tests.Controllers;

using NUnit.Framework;

namespace NpoMash.Erm.Hrm.Tests.StructuralTests.BringingLogicTests {


    [TestFixture]
    public class SmallDepartmentsBringinLogic : OptionsBringingLogic {

        public void loadTimeSheetIntoPeriod(IObjectSpace os, HrmSalaryTaskImportSourceData task) {
            Random rand = new Random();
            HrmTimeSheetLogic.TaskSheetInit(os, task);
            foreach (Department current_department in os.GetObjects<Department>()) {
                HrmTimeSheetDep sheet_dep = os.CreateObject<HrmTimeSheetDep>();
                sheet_dep.Department = current_department;
                sheet_dep.BaseWorkTime = 10000;
                sheet_dep.AdditionWorkTime = 0;
                if (current_department.GroupDep == DepartmentGroupDep.DEPARTMENT_KB) {
                    task.TimeSheetKB.TimeSheetDeps.Add(sheet_dep);
                }
                if (current_department.GroupDep == DepartmentGroupDep.DEPARTMENT_OZM) {
                    task.TimeSheetOZM.TimeSheetDeps.Add(sheet_dep);
                }
            }
        }

        public HrmMatrixAllocPlan setTestData(IObjectSpace os, HrmPeriod current_period, DepartmentGroupDep group) {
            Random random = new Random();
            List<HrmMatrixColumn> columns = new List<HrmMatrixColumn>();
            List<HrmMatrixRow> rows = new List<HrmMatrixRow>();
            HrmMatrixAllocPlan plan_matrix = os.CreateObject<HrmMatrixAllocPlan>();
            foreach (fmCOrder current_order in os.GetObjects<fmCOrder>()) {
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
                        new_cell.Time = Convert.ToInt16(random.Next(100, 500));
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
            plan_matrix.Variant = HrmMatrixVariant.PROPORTIONS_METHOD_VARIANT;
            plan_matrix.Period = current_period;
            current_period.Matrixs.Add(plan_matrix);
            return plan_matrix;
        }

        [Test]
        public void BringUncontrolledOrders_Uncontrolledorders_SuccessAssert() {
            IObjectSpace prepare_object_space = application.CreateObjectSpace();
            TestWCLogic.AllocparameterCount = 1;
            TestWCLogic.DepartmentCount = 1;
            TestWCLogic.ReferenceCount = 5;
            TestWCLogic.referenceClassesGenerate(prepare_object_space);
            HrmPeriod period = null;
            TestWCLogic.addTestData(prepare_object_space);
            foreach (var periods in prepare_object_space.GetObjects<HrmPeriod>(null, true)) {
                periods.setStatus(HrmPeriodStatus.OPENED);
                period = periods;
            }
            HrmPeriod current_period = prepare_object_space.GetObject<HrmPeriod>(period);
            if (current_period.Status == HrmPeriodStatus.OPENED || current_period.Status == HrmPeriodStatus.LIST_OF_CONTROLLED_ORDERS_ACCEPTED) {
                HrmSalaryTaskImportSourceData task = prepare_object_space.CreateObject<HrmSalaryTaskImportSourceData>();
                current_period.PeriodTasks.Add(task);
                task.MatrixPlanKB = setTestData(prepare_object_space, current_period, DepartmentGroupDep.DEPARTMENT_KB);
                task.MatrixPlanKB.Status = HrmMatrixStatus.MATRIX_OPENED;
                task.MatrixPlanOZM = setTestData(prepare_object_space, current_period, DepartmentGroupDep.DEPARTMENT_OZM);
                task.MatrixPlanOZM.Status = HrmMatrixStatus.MATRIX_OPENED;
                loadTimeSheetIntoPeriod(prepare_object_space, task);
            }
            DepartmentGroupDep group_dep = DepartmentGroupDep.DEPARTMENT_KB;
            if (period.Status == HrmPeriodStatus.READY_TO_CALCULATE_COERCED_MATRIXS) {
                HrmMatrixVariant bringing_method = HrmMatrixVariant.MINIMIZE_NUMBER_OF_DEVIATIONS_VARIANT;
                HrmSalaryTaskMatrixReduction reduc = null;
                if (period.CurrentKBmatrixReduction == null)
                    reduc = HrmSalaryTaskMatrixReductionLogic.initTaskMatrixReduction(prepare_object_space, period,
                        group_dep, bringing_method);
                else reduc = prepare_object_space.GetObject<HrmSalaryTaskMatrixReduction>(period.CurrentKBmatrixReduction);
                HrmSalaryTaskMatrixReductionLogic.CreateMatrixInReduc(reduc, prepare_object_space, group_dep, bringing_method, period);
            }
            prepare_object_space.CommitChanges();
            var matrix_list = prepare_object_space.GetObjects<HrmSalaryTaskMatrixReduction>();
            foreach (var matrix in matrix_list) {
                foreach (var data in matrix.Department) {
                    Assert.AreEqual(data.DepartmentFact, data.DepartmentPlan);
                }
            }
        }
    }
}