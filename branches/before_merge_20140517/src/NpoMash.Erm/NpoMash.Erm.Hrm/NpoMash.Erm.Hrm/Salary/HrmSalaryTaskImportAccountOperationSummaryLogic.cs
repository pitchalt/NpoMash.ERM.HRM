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
    public static class HrmSalaryTaskImportAccountOperationSummaryLogic {

        public static void ImportAccountOperationSummary(IObjectSpace local_object_space, HrmSalaryTaskImportAccountOperationSummary local_task) {
            HrmMatrixAllocResult matrix_alloc_result_summary = local_object_space.CreateObject<HrmMatrixAllocResult>();
            local_task.GroupDep = IntecoAG.ERM.HRM.Organization.DepartmentGroupDep.DEPARTMENT_KB_OZM;
            matrix_alloc_result_summary.Type = HrmMatrixType.TYPE_ALLOC_RESULT;
            matrix_alloc_result_summary.Status = HrmMatrixStatus.MATRIX_DOWNLOADED;
            local_task.MatrixAllocResultSummary = matrix_alloc_result_summary;
            local_task.MatrixAllocResultSummary.GroupDep = IntecoAG.ERM.HRM.Organization.DepartmentGroupDep.DEPARTMENT_KB_OZM;
            local_task.Period.CurrentMatrixAllocResultSummary = matrix_alloc_result_summary;
            local_task.Period.Matrixs.Add(matrix_alloc_result_summary);
        }
    }
}