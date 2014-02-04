using System;

using DevExpress.ExpressApp;

using NpoMash.Erm.Hrm.Salary;
using NpoMash.Erm.Hrm.Salary.BringingStructure;

using NUnit.Framework;

namespace NpoMash.Erm.Hrm.Tests.StructuralTests.BringingLogicTests {


    [TestFixture]
    public class SmallDepartmentsBringingLogic : OptionsBringingLogic {

//контролируемые, неконтролируемые, микро, малые, большие, степень неразреженности
        [Test]
        public void BringUncontrolledOrders_1SmallDepClassic_PlanFactEqualExpect() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            Matrix matrix = BringingLogic.PrepareBringingStructure(PrepareTestData(test_object_space, 1, 1, 0, 0, 1, 100));
            BringingLogic.BringUncontrolledOrders(matrix);
            BringingLogic.RestoreInitialFact(matrix);
            foreach (var department in matrix.deps.Values) {
                Assert.AreEqual(department.plan, department.fact);
            }
        }

        [Test]
        public void BringUncontrolledOrders_2SmallDepClassic_PlanFactEqualExpect() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            Matrix matrix = BringingLogic.PrepareBringingStructure(PrepareTestData(test_object_space, 2, 5, 0, 0, 3, 50));
            BringingLogic.BringUncontrolledOrders(matrix);
            BringingLogic.RestoreInitialFact(matrix);
            foreach (var department in matrix.deps.Values) {
                Assert.AreEqual(department.plan, department.fact);
            }
        }
    }
}