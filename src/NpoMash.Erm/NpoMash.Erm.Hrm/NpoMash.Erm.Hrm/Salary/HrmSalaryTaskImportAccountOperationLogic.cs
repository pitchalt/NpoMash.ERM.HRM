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
            HrmMatrixAllocResult matrix_alloc_result = local_object_space.CreateObject<HrmMatrixAllocResult>();
            matrix_alloc_result.Type = HrmMatrixType.TYPE_ALLOC_RESULT;
            local_task.MatrixAllocResult = matrix_alloc_result;
            local_task.Period.Matrixs.Add(matrix_alloc_result);
            matrix_alloc_result.Status = HrmMatrixStatus.MATRIX_OPENED;
        }
    }
}