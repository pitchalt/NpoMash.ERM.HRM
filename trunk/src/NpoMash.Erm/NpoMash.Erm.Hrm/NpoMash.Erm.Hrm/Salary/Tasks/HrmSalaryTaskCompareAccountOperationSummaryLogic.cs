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
using IntecoAG.ERM.HRM.Organization;

namespace NpoMash.Erm.Hrm.Salary {
    public static class HrmSalaryTaskCompareAccountOperationSummaryLogic {

        public static void InitObjects(IObjectSpace local_object_space, HrmSalaryTaskCompareAccountOperationSummary task)
        {
            task.ProvisionMatrix = task.Period.CurrentMatrixProvision;
            if (task.Period.CurrentMatrixAllocPlanSummary != null) {
                task.MatrixAllocPlanSummary = task.Period.CurrentMatrixAllocPlanSummary;
            }
            if (task.Period.CurrentMatrixAllocResultSummary != null)
            {
                task.MatrixAllocResultSummary = task.Period.CurrentMatrixAllocResultSummary;
            }      
        
        }


        public static void CompareSummaryMatrix(IObjectSpace local_object_space, HrmSalaryTaskCompareAccountOperationSummary local_task) {
            local_task.GroupDep = IntecoAG.ERM.HRM.Organization.DepartmentGroupDep.DEPARTMENT_KB_OZM;
            local_task.MatrixAllocPlanSummary = local_task.Period.CurrentMatrixAllocPlanSummary;
            local_task.MatrixAllocResultSummary = local_task.Period.CurrentMatrixAllocResultSummary;
            if (local_task.Period.CurrentProvisionMatrix.ReserveMatrixEvristic.Status == HrmMatrixStatus.MATRIX_EXPORTED) {
                local_task.Period.CurrentProvisionMatrix.ReserveMatrixEvristic.Status = HrmMatrixStatus.MATRIX_ACCEPTED;
            }
            else {
                local_task.Period.CurrentProvisionMatrix.ReserveMatrixSimplex.Status = HrmMatrixStatus.MATRIX_ACCEPTED;
            }
        }
    }
}