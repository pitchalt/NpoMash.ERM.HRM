using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
////
using NpoMash.Erm.Hrm.Salary;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.HRM.Organization;

using DevExpress.ExpressApp.Xpo;
using NpoMash.Erm.Hrm.Tests.Controllers;
using DevExpress.Spreadsheet;
using DevExpress.Office;

using NpoMash.Erm.Hrm.Simplex;

using NpoMash.Erm.Hrm.Salary.ProvisionMatrixBringingStructure;

namespace NpoMash.Erm.Hrm.Tests.StructuralTests {
    [TestFixture]
    class SimplexCoreTest {

        private TestApplication application;

        [SetUp]
        protected void SetUp() {
            IObjectSpaceProvider object_space_provider = new XPObjectSpaceProvider(new MemoryDataStoreProvider());
            application = new TestApplication();
            ModuleBase test_module = new ModuleBase();
            test_module.AdditionalExportedTypes.Add(typeof(HrmSalaryTaskProvisionMatrixReduction));
            application.Modules.Add(test_module);
            application.Setup("BringingApp", object_space_provider);
        }

        [Test]
        public void MaximizationTest() {
            Dictionary<int, double> func = new Dictionary<int, double>();
            func.Add(0, 9); func.Add(1, 10); func.Add(2, 16);

            List<SimplexLimitation> limits = new List<SimplexLimitation>();
            SimplexLimitation lim1 = new SimplexLimitation();
            lim1.coefficients = new Dictionary<int, double>();
            lim1.freeMember = 360;
            lim1.coefficients.Add(0, 18);
            lim1.coefficients.Add(1, 15);
            lim1.coefficients.Add(2, 12);
            lim1.coefficients.Add(3, 1);
            SimplexLimitation lim2 = new SimplexLimitation();
            lim2.coefficients = new Dictionary<int, double>();
            lim2.freeMember = 192;
            lim2.coefficients.Add(0, 6);
            lim2.coefficients.Add(1, 4);
            lim2.coefficients.Add(2, 8);
            lim2.coefficients.Add(4, 1);
            SimplexLimitation lim3 = new SimplexLimitation();
            lim3.coefficients = new Dictionary<int, double>();
            lim3.freeMember = 180;
            lim3.coefficients.Add(0, 5);
            lim3.coefficients.Add(1, 3);
            lim3.coefficients.Add(2, 3);
            lim3.coefficients.Add(5, 1);
            limits.Add(lim1);
            limits.Add(lim2);
            limits.Add(lim3);

            SimplexTab tablica = new SimplexTab(limits, func);
            double[] result = SimplexStructureLogic.Maximize(tablica);
            double[] expected_result = { 0, 8, 20, 0, 0, 96 };
            for (int i = 0; i < result.Count(); i++)
                Assert.AreEqual(expected_result[i], result[i]);
        }

        [Test]
        public void MinimizationTest() {
            Dictionary<int, double> func = new Dictionary<int, double>();
            func.Add(0, 9); func.Add(1, 10); func.Add(2, 16);

            List<SimplexLimitation> limits = new List<SimplexLimitation>();
            SimplexLimitation lim1 = new SimplexLimitation();
            lim1.coefficients = new Dictionary<int, double>();
            lim1.freeMember = 360;
            lim1.coefficients.Add(0, 18);
            lim1.coefficients.Add(1, 15);
            lim1.coefficients.Add(2, 12);
            lim1.coefficients.Add(3, 1);
            SimplexLimitation lim2 = new SimplexLimitation();
            lim2.coefficients = new Dictionary<int, double>();
            lim2.freeMember = 192;
            lim2.coefficients.Add(0, 6);
            lim2.coefficients.Add(1, 4);
            lim2.coefficients.Add(2, 8);
            lim2.coefficients.Add(4, 1);
            SimplexLimitation lim3 = new SimplexLimitation();
            lim3.coefficients = new Dictionary<int, double>();
            lim3.freeMember = 180;
            lim3.coefficients.Add(0, 5);
            lim3.coefficients.Add(1, 3);
            lim3.coefficients.Add(2, 3);
            lim3.coefficients.Add(5, 1);
            limits.Add(lim1);
            limits.Add(lim2);
            limits.Add(lim3);

            SimplexTab tablica = new SimplexTab(limits, func);
            double[] result = SimplexStructureLogic.Minimize(tablica);
            double[] expected_result = { 0, 0,0,360,192,180 };
            for (int i = 0; i < result.Count(); i++)
                Assert.AreEqual(expected_result[i], result[i]);
        }




    }
}
