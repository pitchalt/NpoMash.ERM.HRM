using System;

using DevExpress.ExpressApp;
using NpoMash.Erm.Hrm.Salary;
using NpoMash.Erm.Hrm.Salary.BringingStructure;

using NUnit.Framework;

namespace NpoMash.Erm.Hrm.Tests.StructuralTests.BringingLogicTests {


    [TestFixture]
    public class ReadyForBringingLogic : OptionsBringingLogic {

        [Test]
        public void Ready_CheckZeroValues_ExeptionExpect() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            try { var reduction = PrepareTestData(test_object_space, 0, 0, 0, 0, 0, 0); }
            catch (NullReferenceException) { }
        }

        [Test]
        public void Ready_CheckPlanRef_NotNullExpect() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var reduction = PrepareTestData(test_object_space, 4, 10, 2, 1, 5, 100);
            Assert.IsNotNull(reduction.MatrixPlan);
        }

        [Test]
        public void Ready_CheckAllocParametersRef_NotNullExpect() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var reduction = PrepareTestData(test_object_space, 2, 10, 1, 5, 6, 100);
            Assert.IsNotNull(reduction.AllocParameters);
        }

        [Test]
        public void Ready_CheckCreateTime_NotNullExpect() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var reduction = PrepareTestData(test_object_space, 2, 10, 1, 5, 6, 100);
            Assert.IsNotNull(reduction.CreateTime);
        }

        [Test]
        public void Ready_CheckDepartmentRef_NotNullExpect() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var reduction = PrepareTestData(test_object_space, 2, 10, 1, 5, 6, 100);
            Assert.IsNotNull(reduction.Department);
        }

        [Test]
        public void Ready_CheckFinishTime_NotNullExpect() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var reduction = PrepareTestData(test_object_space, 2, 10, 1, 5, 6, 100);
            Assert.IsNotNull(reduction.FinishTime);
        }

        [Test]
        public void Ready_CheckMatrixPlanRef_NotNullExpect() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var reduction = PrepareTestData(test_object_space, 2, 10, 1, 5, 6, 100);
            Assert.IsNotNull(reduction.MatrixPlan);
        }

        [Test]
        public void Ready_CheckMinimizeNumberOfDeviationsMatrixRef_NullExpect() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var reduction = PrepareTestData(test_object_space, 2, 10, 1, 5, 6, 100);
            Assert.IsNull(reduction.MinimizeNumberOfDeviationsMatrix);
        }

        [Test]
        public void Ready_CheckMinimizeMaximumDeviationsMatrix_NullExpect() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var reduction = PrepareTestData(test_object_space, 2, 10, 1, 5, 6, 100);
            Assert.IsNull(reduction.MinimizeMaximumDeviationsMatrix);
        }

        [Test]
        public void Ready_CheckProportionsMethodMatrix_NullExpect() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var reduction = PrepareTestData(test_object_space, 2, 10, 1, 5, 6, 100);
            Assert.IsNull(reduction.ProportionsMethodMatrix);
        }

        [Test]
        public void Ready_CheckOrderRef_NotNullExpect() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var reduction = PrepareTestData(test_object_space, 2, 10, 1, 5, 6, 100);
            Assert.IsNotNull(reduction.Order);
        }

        [Test]
        public void Ready_CheckPeriodRef_NotNullExpect() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var reduction = PrepareTestData(test_object_space, 2, 10, 1, 5, 6, 100);
            Assert.IsNotNull(reduction.Period);
        }

        [Test]
        public void Ready_CheckTimeSheetRef_NotNullExpect() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            var reduction = PrepareTestData(test_object_space, 2, 10, 1, 5, 6, 100);
            Assert.IsNotNull(reduction.TimeSheet);
        }

        [Test]
        public void ReadyMatrix_CheckZeroValues_ExeptionExpect() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            try { Matrix matrix = BringingLogic.PrepareBringingStructure(PrepareTestData(test_object_space, 0, 0, 0, 0, 0, 0)); }
            catch (NullReferenceException) { }
        }

        [Test]
        public void ReadyMatrix_CheckCellsInDictionary_NotNullExpect() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            Matrix matrix = BringingLogic.PrepareBringingStructure(PrepareTestData(test_object_space, 2, 10, 1, 5, 6, 100));
            Assert.IsNotNull(matrix.cellsInDictionary);
        }

        [Test]
        public void ReadyMatrix_CheckCellsInDictionaryKeys_NotNullExpect() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            Matrix matrix = BringingLogic.PrepareBringingStructure(PrepareTestData(test_object_space, 2, 10, 1, 5, 6, 100));
            Assert.IsNotNull(matrix.cellsInDictionary.Keys);
        }

        [Test]
        public void ReadyMatrix_CheckCellsInDictionaryValues_NotNullExpect() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            Matrix matrix = BringingLogic.PrepareBringingStructure(PrepareTestData(test_object_space, 2, 10, 1, 5, 6, 100));
            Assert.IsNotNull(matrix.cellsInDictionary.Values);
        }

        [Test]
        public void ReadyMatrix_CheckDeps_NotNullExpect() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            Matrix matrix = BringingLogic.PrepareBringingStructure(PrepareTestData(test_object_space, 2, 10, 1, 5, 6, 100));
            Assert.IsNotNull(matrix.deps);
        }

        [Test]
        public void ReadyMatrix_CheckDepsKeys_NotNullExpect() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            Matrix matrix = BringingLogic.PrepareBringingStructure(PrepareTestData(test_object_space, 2, 10, 1, 5, 6, 100));
            Assert.IsNotNull(matrix.deps.Keys);
        }

        [Test]
        public void ReadyMatrix_CheckDepsValues_NotNullExpect() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            Matrix matrix = BringingLogic.PrepareBringingStructure(PrepareTestData(test_object_space, 2, 10, 1, 5, 6, 100));
            Assert.IsNotNull(matrix.deps.Values);
        }

        [Test]
        public void ReadyMatrix_CheckOrders_NotNullExpect() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            Matrix matrix = BringingLogic.PrepareBringingStructure(PrepareTestData(test_object_space, 2, 10, 1, 5, 6, 100));
            Assert.IsNotNull(matrix.orders);
        }

        [Test]
        public void ReadyMatrix_CheckOrdersKeys_NotNullexpect() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            Matrix matrix = BringingLogic.PrepareBringingStructure(PrepareTestData(test_object_space, 2, 10, 1, 5, 6, 100));
            Assert.IsNotNull(matrix.orders.Keys);
        }

        [Test]
        public void ReadyMatrix_CheckOrersValues_NotNullExpect() {
            IObjectSpace test_object_space = application.CreateObjectSpace();
            Matrix matrix = BringingLogic.PrepareBringingStructure(PrepareTestData(test_object_space, 2, 10, 1, 5, 6, 100));
            Assert.IsNotNull(matrix.orders.Values);
        }
    }
}