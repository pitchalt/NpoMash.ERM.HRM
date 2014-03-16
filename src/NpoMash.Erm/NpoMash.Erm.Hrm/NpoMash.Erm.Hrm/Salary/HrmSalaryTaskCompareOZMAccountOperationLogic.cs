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

    public static class HrmSalaryTaskCompareOZMAccountOperationLogic {

        public static void CompareOZMMatrix(IObjectSpace local_object_space, HrmSalaryTaskCompareOZMAccountOperation local_task) {
            local_task.GroupDep = IntecoAG.ERM.HRM.Organization.DepartmentGroupDep.DEPARTMENT_OZM;
            local_task.MatrixAllocPlanOZM = local_task.Period.CurrentMatrixAllocPlanOZM;
            local_task.ReducMatrixOZM = local_task.Period.CurrentOZMmatrixReduction.MinimizeNumberOfDeviationsMatrix;
            local_task.MatrixAllocResultOZM = local_task.Period.CurrentMatrixAllocResultOZM;
        }
    }
}