using System;

using DevExpress.ExpressApp;

using IntecoAG.ERM.FM.Order;
using NpoMash.Erm.Hrm.Salary;
using NpoMash.Erm.Hrm.Tests.Controllers;
using NpoMash.Erm.Hrm.Salary.BringingStructure;

using NUnit.Framework;

namespace NpoMash.Erm.Hrm.Tests.StructuralTests.BringingLogicTests {


    [TestFixture]
    public class MicroDepartmentsBringingLogic : OptionsBringingLogic {

        

        [Test]
        public void BringingMicroDepartments_deps_PlanFactEqualExpect() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            HrmMatrixCell oldcell = null;
            HrmMatrixRow row = null;
            var task = PrepareTestData(test_object_space, 2, 1, 5, 6000, 100, 150, false);
            foreach (var column in task.MatrixPlan.Columns) {
                foreach (var cell in column.Cells) {
                    cell.Time = cell.Time*10;
                    row = cell.Row;
                    oldcell = cell;
                }
                break;
            }
            Matrix matrix = BringingLogic.PrepareBringingStructure(task);
            BringingLogic.BringMicroDepartments(matrix);
            BringingLogic.BringBigDepartments(matrix);
            BringingLogic.RestoreInitialFact(matrix);
            foreach (var department in matrix.deps.Values) {
                Assert.AreEqual(department.fact, department.plan);
            }
        }

    }
}