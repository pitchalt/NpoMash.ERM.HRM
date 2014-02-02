using System;

using DevExpress.ExpressApp;

using NpoMash.Erm.Hrm.Salary;
using NpoMash.Erm.Hrm.Tests.Controllers;
using NpoMash.Erm.Hrm.Salary.BringingStructure;

using NUnit.Framework;

namespace NpoMash.Erm.Hrm.Tests.StructuralTests.BringingLogicTests {


    [TestFixture]
    public class SmallDepartmentsBringinLogic : OptionsBringingLogic {
// dep, alloc, orders, timesheet, plan_min, plan_max
        [Test]
        public void BringUncontrolledOrders_1SmallDep_PlanFactEqualExpect() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            Matrix matrix = BringingLogic.PrepareBringingStructure(PrepareTestData(test_object_space, 1, 1, 5, 1000, 100, 150, true));
            BringingLogic.BringUncontrolledOrders(matrix);
            BringingLogic.RestoreInitialFact(matrix);
            foreach (var department in matrix.deps.Values) {
                Assert.AreEqual(department.plan, department.fact);
            }
        }

        [Test]
        public void BringUncontrolledOrders_2SmallDep_PlanFactEqualExpect() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            Matrix matrix = BringingLogic.PrepareBringingStructure(PrepareTestData(test_object_space, 2, 1, 5, 1000, 100, 150, true));
            BringingLogic.BringUncontrolledOrders(matrix);
            BringingLogic.RestoreInitialFact(matrix);
            foreach (var department in matrix.deps.Values) {
                Assert.AreEqual(department.plan, department.fact);
            }
        }

        [Test]
        public void BringUncontrolledOrders_3SmallDep_PlanFactEqualExpect() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            Matrix matrix = BringingLogic.PrepareBringingStructure(PrepareTestData(test_object_space, 3, 1, 50, 1000, 10, 15, true));
            BringingLogic.BringUncontrolledOrders(matrix);
            BringingLogic.RestoreInitialFact(matrix);
            foreach (var department in matrix.deps.Values) {
                Assert.AreEqual(department.plan, department.fact);
            }
        }

        [Test]
        public void BringUncontrolledOrders_10SmallDep_PlanFactEqualExpect() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            Matrix matrix = BringingLogic.PrepareBringingStructure(PrepareTestData(test_object_space, 10, 1, 5, 100, 10, 20, true));
            BringingLogic.BringUncontrolledOrders(matrix);
            BringingLogic.RestoreInitialFact(matrix);
            foreach (var department in matrix.deps.Values) {
                Assert.AreEqual(department.plan, department.fact);
            }
        }

        [Test]
        public void BringUncontrolledOrders_2SmallDep_PlanFactNotEqualExpect() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            TestWCLogic.AddOrder(test_object_space, IntecoAG.ERM.FM.Order.FmCOrderTypeControl.TRUDEMK_FOT);
            Matrix matrix = BringingLogic.PrepareBringingStructure(PrepareTestData(test_object_space, 2, 1, 20, 10, 300, 400, true));
            BringingLogic.BringUncontrolledOrders(matrix);
            BringingLogic.RestoreInitialFact(matrix);
            foreach (var department in matrix.deps.Values) {
                Assert.AreNotEqual(department.plan, department.fact);
            }
        }

        [Test]
        public void BringUncontrolledOrders_2SmallDepSingleValues_PlanFactNotEqualExpect() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            TestWCLogic.AddOrder(test_object_space, IntecoAG.ERM.FM.Order.FmCOrderTypeControl.TRUDEMK_FOT);
            Matrix matrix = BringingLogic.PrepareBringingStructure(PrepareTestData(test_object_space, 2, 1, 100, 100000, 0, 10, true));
            BringingLogic.BringUncontrolledOrders(matrix);
            BringingLogic.RestoreInitialFact(matrix);
            foreach (var department in matrix.deps.Values) {
                Assert.AreEqual(department.plan, department.fact);
            }
        }

    }
}