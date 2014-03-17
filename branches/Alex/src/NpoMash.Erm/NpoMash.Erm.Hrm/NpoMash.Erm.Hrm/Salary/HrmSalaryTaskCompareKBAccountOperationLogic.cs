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

    public static class HrmSalaryTaskCompareKBAccountOperationLogic {

        public static void CompareKBMatrix(IObjectSpace local_object_space, HrmSalaryTaskCompareKBAccountOperation local_task) {
            local_task.GroupDep = IntecoAG.ERM.HRM.Organization.DepartmentGroupDep.DEPARTMENT_KB;
            local_task.MatrixPlanKB = local_task.Period.CurrentMatrixAllocPlanKB;
            local_task.ReducMatrixKB = local_task.Period.CurrentKBmatrixReduction.MinimizeNumberOfDeviationsMatrix;
            local_task.MatrixAllocResultKB = local_task.Period.CurrentMatrixAllocResultKB;
        }
    }
}