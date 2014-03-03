using System;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;
//
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
//
using FileHelpers;
using IntecoAG.ERM.HRM;
using IntecoAG.ERM.FM.Order;
using NpoMash.Erm.Hrm.Exchange;
using IntecoAG.ERM.HRM.Organization;


namespace NpoMash.Erm.Hrm.Salary {
    public static class HrmSalaryTaskImportAccountOperationLogic {

        public static void CreateTestAllocResultMatrix(IObjectSpace local_object_space, HrmMatrixAllocResult matrix_alloc_result, DepartmentGroupDep group_dep) {
            var random = new Random();
            //int account_operation_count = local_object_space.GetObjects<fmCOrder>().Count<fmCOrder>() * local_object_space.GetObjects<Department>().Count<Department>();
            //ImportAccountOperation[] account_list = new ImportAccountOperation[account_operation_count];
            IDictionary<String, fmCOrder> orders_in_db = local_object_space.GetObjects<fmCOrder>()
                .ToDictionary<fmCOrder, String>(x => x.Code);
            IDictionary<String, Department> departments_in_db = local_object_space.GetObjects<Department>()
                .ToDictionary<Department, String>(x => x.BuhCode);
            foreach (var order in orders_in_db.Keys) {
                HrmAccountOperation account_to_db = local_object_space.CreateObject<HrmAccountOperation>();
                account_to_db.AllocResult = matrix_alloc_result;
                account_to_db.Order = orders_in_db[order];
                account_to_db.Order.Code = order;
                account_to_db.Time = random.Next(1, 1000);
                account_to_db.Money = random.Next(1,1000);
                matrix_alloc_result.AccountOperations.Add(account_to_db);
            }
            foreach (var department in departments_in_db.Keys) {
                if (departments_in_db[department].GroupDep == group_dep) {
                }
            }
        }

        public static void ImportAccountOperation(IObjectSpace local_object_space, HrmSalaryTaskImportAccountOperation local_task) {
            FileHelperEngine<ImportAccountOperation> account_operation_data = new FileHelperEngine<ImportAccountOperation>();
            ImportAccountOperation[] account_list = account_operation_data.ReadFile("../../../../../../../var/AccountOperation_First.ncd");
            HrmMatrixAllocResult matrix_alloc_result_kb = local_object_space.CreateObject<HrmMatrixAllocResult>();
            HrmMatrixAllocResult matrix_alloc_result_ozm = local_object_space.CreateObject<HrmMatrixAllocResult>();
            matrix_alloc_result_kb.IterationNumber = 1;
            matrix_alloc_result_ozm.IterationNumber = 1;
            matrix_alloc_result_kb.Type = HrmMatrixType.TYPE_ALLOC_RESULT;
            matrix_alloc_result_ozm.Type = HrmMatrixType.TYPE_ALLOC_RESULT;
            matrix_alloc_result_kb.Status = HrmMatrixStatus.MATRIX_OPENED;
            matrix_alloc_result_ozm.Status = HrmMatrixStatus.MATRIX_OPENED;
            local_task.MatrixAllocResultKB = matrix_alloc_result_kb;
            local_task.MatrixAllocResultOZM = matrix_alloc_result_ozm;
            local_task.GroupDep = IntecoAG.ERM.HRM.Organization.DepartmentGroupDep.DEPARTMENT_KB;
            local_task.MatrixAllocResultKB.GroupDep = IntecoAG.ERM.HRM.Organization.DepartmentGroupDep.DEPARTMENT_KB;
            local_task.MatrixAllocResultOZM.GroupDep = IntecoAG.ERM.HRM.Organization.DepartmentGroupDep.DEPARTMENT_OZM;
            local_task.Period.CurrentMatrixAllocResultKB = matrix_alloc_result_kb;
            local_task.Period.CurrentMatrixAllocResultOZM = matrix_alloc_result_ozm;
            local_task.Period.Matrixs.Add(matrix_alloc_result_kb);
            local_task.Period.Matrixs.Add(matrix_alloc_result_ozm);
        }
    }
}