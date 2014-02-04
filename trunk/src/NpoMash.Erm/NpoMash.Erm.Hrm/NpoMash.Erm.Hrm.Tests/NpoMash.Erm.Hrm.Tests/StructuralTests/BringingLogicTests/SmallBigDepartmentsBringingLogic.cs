using System;

using DevExpress.ExpressApp;

using NpoMash.Erm.Hrm.Salary;
using NpoMash.Erm.Hrm.Salary.BringingStructure;

using NUnit.Framework;

namespace NpoMash.Erm.Hrm.Tests.StructuralTests.BringingLogicTests {


    [TestFixture]
    public class SmallBigDepartmentsBringingLogic : OptionsBringingLogic {

        [Test]
        public void BringBigSmall_SuccessVariant_PlanFactEqualExpect() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            Matrix matrix = BringingLogic.PrepareBringingStructure(PrepareTestData(test_object_space, 4, 16, 0, 3, 5, 80));
            BringingLogic.BringMicroDepartments(matrix);
            BringingLogic.BringBigDepartments(matrix);
            BringingLogic.BringUncontrolledOrders(matrix);
            BringingLogic.RestoreInitialFact(matrix);
            foreach (var department in matrix.deps.Values) {
                Assert.AreEqual(department.plan, department.fact);
            }
        }
    }
}
