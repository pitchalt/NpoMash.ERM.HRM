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

namespace NpoMash.Erm.Hrm.Salary {
    public static class HrmSalaryTaskImportAccountOperationLogic {

        public static void ImportAccountOperation(IObjectSpace local_object_space, HrmSalaryTaskImportAccountOperation local_task) {
            HrmMatrixAllocResult matrix_alloc_result_kb = local_object_space.CreateObject<HrmMatrixAllocResult>();
            HrmMatrixAllocResult matrix_alloc_result_ozm = local_object_space.CreateObject<HrmMatrixAllocResult>();
            local_task.GroupDep = IntecoAG.ERM.HRM.Organization.DepartmentGroupDep.DEPARTMENT_KB;
            matrix_alloc_result_kb.Type = HrmMatrixType.TYPE_ALLOC_RESULT;
            matrix_alloc_result_ozm.Type = HrmMatrixType.TYPE_ALLOC_RESULT;
            matrix_alloc_result_kb.Status = HrmMatrixStatus.MATRIX_OPENED;
            matrix_alloc_result_ozm.Status = HrmMatrixStatus.MATRIX_OPENED;
            local_task.MatrixAllocResultKB = matrix_alloc_result_kb;
            local_task.MatrixAllocResultOZM = matrix_alloc_result_ozm;
            local_task.MatrixAllocResultKB.GroupDep = IntecoAG.ERM.HRM.Organization.DepartmentGroupDep.DEPARTMENT_KB;
            local_task.MatrixAllocResultOZM.GroupDep = IntecoAG.ERM.HRM.Organization.DepartmentGroupDep.DEPARTMENT_OZM;
            local_task.Period.CurrentMatrixAllocResultKB = matrix_alloc_result_kb;
            local_task.Period.CurrentMatrixAllocResultOZM = matrix_alloc_result_ozm;
            local_task.Period.Matrixs.Add(matrix_alloc_result_kb);
            local_task.Period.Matrixs.Add(matrix_alloc_result_ozm);
        }
    }
}