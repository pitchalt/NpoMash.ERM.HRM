﻿using System;
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
//
using NpoMash.Erm.Hrm.Salary;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.HRM.Organization;

using DevExpress.ExpressApp.Xpo;
using NpoMash.Erm.Hrm.Tests.Controllers;
using DevExpress.Spreadsheet;
using DevExpress.Office;

using NpoMash.Erm.Hrm.Salary.ProvisionMatrixBringingStructure;


namespace NpoMash.Erm.Hrm.Tests.StructuralTests {
    [TestFixture]
    class ReserveMatrixFromExcelTest {

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

        [TestCase("C:\\ExcelTests\\Provisions_matrix_for_tests.xls","Test1")]
        public void BringFromExcelTest(String path_to_file, String worksheet_name) {
            IObjectSpace os = application.CreateObjectSpace();
            HrmSalaryTaskProvisionMatrixReduction task = os.CreateObject<HrmSalaryTaskProvisionMatrixReduction>();
            Workbook wb = ImportTestDataFromExcelLogic.getWorkbook(path_to_file);
            MatrixFromExcel mat = ImportTestDataFromExcelLogic.GetData(wb, worksheet_name);
            ImportTestDataFromExcelLogic.CreateDepartmentsFromExcelTab(os, mat);
            ImportTestDataFromExcelLogic.CreateOrdersFromExcelTab(os, mat);
            task.AllocParameters = ImportTestDataFromExcelLogic.CreateAllocParametersFromExcelTab(os);
            task.ProvisionMatrix = ImportTestDataFromExcelLogic.CreateMatrixFromExcel(os, mat);
            /*String test_str = "";
            foreach (HrmMatrixColumn col in task.ProvisionMatrix.Columns) {
                test_str +="<" +col.Department.BuhCode + ">" +"NumberOfCells: " + col.Cells.Count.ToString() + ";";
            }
            throw new Exception(test_str);*/
            ProvMat m = ProvBringLogic.CreateProvBringStructure(task);
            ProvBringLogic.BringVeryEasyDeps(m);
            ProvBringLogic.BringEasyDeps(m);
            ProvBringLogic.BringDifficultDeps(m);
            ProvBringLogic.LoadProvBringResultInTask(m);
            //HrmMatrix result_matrix = HrmSalaryTaskProvisionMatrixReductionLogic.calculateProvisionMatrix(os, task);
            HrmMatrix result_matrix = task.ProvisionMatrix;
            Dictionary<String,HrmMatrixColumn> dictionary_of_columns = result_matrix.Columns.ToDictionary(x => x.Department.BuhCode);
            for (int i = 0; i < mat.NumberOfColumns; i++) {
                Decimal expected_value = mat.itog_columns_info[i][0];
                String dep_code = mat.columns_info[i][0];
                Decimal result_value = dictionary_of_columns[dep_code].Cells.Sum(x => x.SourceProvision);
                Assert.AreEqual(expected_value, result_value);
            }
        }


    }
}