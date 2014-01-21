using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Model.NodeGenerators;

using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.FM.Order;

namespace NpoMash.Erm.Hrm.Salary {
    public static class HrmSalaryTaskMatrixReductionLogic {

        public static HrmSalaryTaskMatrixReduction initTaskMatrixReduction(IObjectSpace os, HrmPeriod period, 
            DEPARTMENT_GROUP_DEP group_dep, HRM_MATRIX_VARIANT bringing_method) {
            HrmSalaryTaskMatrixReduction task_matrix_reduction = os.CreateObject<HrmSalaryTaskMatrixReduction>();
            task_matrix_reduction.GroupDep = group_dep;
            //!!!Поскольку это ассоциация, то этот оператор дублирует следующий 
            //task_matrix_reduction.Period = period;
            period.PeriodTasks.Add(task_matrix_reduction);
            task_matrix_reduction.AllocParameters = period.CurrentAllocParameter;
            if (group_dep == DEPARTMENT_GROUP_DEP.KB) {
                task_matrix_reduction.TimeSheet = period.CurrentTimeSheetKB;
                period.CurrentKBmatrixReduction = task_matrix_reduction;
            }
            else if (group_dep == DEPARTMENT_GROUP_DEP.OZM) {
                task_matrix_reduction.TimeSheet = period.CurrentTimeSheetOZM;
                period.CurrentOZMmatrixReduction = task_matrix_reduction; 
            }

            foreach (HrmMatrix matrix in period.Matrixs) {
                if (matrix.TypeMatrix == HRM_MATRIX_TYPE_MATRIX.Planned &&
                    matrix.Status == HRM_MATRIX_STATUS.ACCEPTED &&
                    matrix.GroupDep == group_dep) {
                    task_matrix_reduction.MatrixPlan = matrix;
                }
            }
            return task_matrix_reduction;
        }

        public static void CreateMatrixInReduc(HrmSalaryTaskMatrixReduction reduc, IObjectSpace os, DEPARTMENT_GROUP_DEP group_dep,
            HRM_MATRIX_VARIANT bringing_method, HrmPeriod period){
            if (reduc.MinimizeMaximumDeviationsMatrix == null && bringing_method == HRM_MATRIX_VARIANT.MinimizeMaximumDeviations)
                HrmMatrixLogic.makeAllocMatrix(reduc, os, group_dep, bringing_method, period);
            if (reduc.MinimizeNumberOfDeviationsMatrix == null && bringing_method == HRM_MATRIX_VARIANT.MinimizeNumberOfDeviations)
                HrmMatrixLogic.makeAllocMatrix(reduc, os, group_dep, bringing_method, period);
            if (reduc.ProportionsMethodMatrix == null && bringing_method == HRM_MATRIX_VARIANT.ProportionsMethod)
                HrmMatrixLogic.makeAllocMatrix(reduc, os, group_dep, bringing_method, period);
        }

        public static HRM_MATRIX_VARIANT DetermineSelectedBringingMethod(SingleChoiceActionExecuteEventArgs e) {
            HRM_MATRIX_VARIANT bringing_method = HRM_MATRIX_VARIANT.ProportionsMethod;
            if (e.SelectedChoiceActionItem.Id == "ProportionsMethod")
                bringing_method = HRM_MATRIX_VARIANT.ProportionsMethod;
            if (e.SelectedChoiceActionItem.Id == "MinimizeDifferenceNumber")
                bringing_method = HRM_MATRIX_VARIANT.MinimizeNumberOfDeviations;
            if (e.SelectedChoiceActionItem.Id == "MinimizeMaxDifference")
                bringing_method = HRM_MATRIX_VARIANT.MinimizeMaximumDeviations;
            return bringing_method;
        }

        public static HrmMatrix DetermineSelectedMatrixToAccept(SingleChoiceActionExecuteEventArgs e,
            HrmSalaryTaskMatrixReduction reduc) {
            HrmMatrix matrix_to_accept = null;
            if (e.SelectedChoiceActionItem.Id == "AcceptProportionsMethod")
                matrix_to_accept = reduc.ProportionsMethodMatrix;
            if (e.SelectedChoiceActionItem.Id == "AcceptMinimizeNumberOfDeviationsMethod")
                matrix_to_accept = reduc.MinimizeNumberOfDeviationsMatrix;
            if (e.SelectedChoiceActionItem.Id == "AcceptMinimizeDeviationsMethod")
                matrix_to_accept = reduc.MinimizeMaximumDeviationsMatrix;
            return matrix_to_accept;
        }

        public static void ExportMatrixes(HrmPeriod current_period){
            foreach (HrmMatrix m in current_period.Matrixs)
                if (m.TypeMatrix == HRM_MATRIX_TYPE_MATRIX.Coerced && m.Status == HRM_MATRIX_STATUS.ACCEPTED)
                    m.Status = HRM_MATRIX_STATUS.EXPORTED;
        }

        public static bool AllCoercedMatrixesAccepted(HrmMatrix matrix_to_accept, HrmPeriod current_period) {
            bool kb_accepted = false;
            bool ozm_accepted = false;
            if (matrix_to_accept.GroupDep == DEPARTMENT_GROUP_DEP.KB)
                kb_accepted = true;
            else ozm_accepted = true;
            foreach (HrmMatrix m in current_period.Matrixs) {
                if (m.TypeMatrix == HRM_MATRIX_TYPE_MATRIX.Coerced && m.Status == HRM_MATRIX_STATUS.ACCEPTED)
                    if (m.GroupDep == DEPARTMENT_GROUP_DEP.KB)
                        kb_accepted = true;
                    else ozm_accepted = true;
            }
            if (kb_accepted && ozm_accepted)
                return true;
            else return false;
        }

        public static void AcceptSelectedMatrix(HrmSalaryTaskMatrixReduction reduc, HrmMatrix matrix_to_accept) {
            if (reduc.MinimizeMaximumDeviationsMatrix != null)
                reduc.MinimizeMaximumDeviationsMatrix.Status = HRM_MATRIX_STATUS.CLOSED;
            if (reduc.MinimizeNumberOfDeviationsMatrix != null)
                reduc.MinimizeNumberOfDeviationsMatrix.Status = HRM_MATRIX_STATUS.CLOSED;
            if (reduc.ProportionsMethodMatrix != null)
                reduc.ProportionsMethodMatrix.Status = HRM_MATRIX_STATUS.CLOSED;
            matrix_to_accept.Status = HRM_MATRIX_STATUS.ACCEPTED;
        }

    }
}
