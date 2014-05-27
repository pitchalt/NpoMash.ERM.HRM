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
    public static class HrmSalaryTaskCompareAccountOperationSummaryLogic {

        public static void CompareSummaryMatrix(IObjectSpace local_object_space, HrmSalaryTaskCompareAccountOperationSummary local_task) {
            local_task.GroupDep = IntecoAG.ERM.HRM.Organization.DepartmentGroupDep.DEPARTMENT_KB_OZM;
            local_task.MatrixAllocPlanSummary = local_task.Period.CurrentMatrixAllocPlanSummary;
            local_task.MatrixAllocResultSummary = local_task.Period.CurrentMatrixAllocResultSummary;
        }
    }
}