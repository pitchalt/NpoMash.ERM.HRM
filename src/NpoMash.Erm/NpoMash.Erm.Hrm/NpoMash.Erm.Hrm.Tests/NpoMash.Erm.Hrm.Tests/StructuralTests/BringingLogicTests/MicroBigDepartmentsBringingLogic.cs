using System;

using DevExpress.ExpressApp;

using NpoMash.Erm.Hrm.Salary;
using NpoMash.Erm.Hrm.Salary.BringingStructure;

using NUnit.Framework;

namespace NpoMash.Erm.Hrm.Tests.StructuralTests.BringingLogicTests {


    [TestFixture]
    public class MicroBigDepartmentsBringingLogic : OptionsBringingLogic {

        [Test]
        public void BringMicroBringBig_1on1Classic_PlanFactAreEqualExpect() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            Matrix matrix = BringingLogic.PrepareBringingStructure(PrepareTestData(test_object_space, 2, 2, 1, 1, 0, 100));
            BringingLogic.BringMicroDepartments(matrix);
            BringingLogic.BringBigDepartments(matrix);
            BringingLogic.RestoreInitialFact(matrix);
            foreach (var department in matrix.deps.Values) {
                Assert.AreEqual(department.plan, department.fact);
            }
        }
    }
}
