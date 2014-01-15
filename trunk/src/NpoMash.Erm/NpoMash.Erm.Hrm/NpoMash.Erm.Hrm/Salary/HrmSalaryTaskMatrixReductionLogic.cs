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
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Model.NodeGenerators;

using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.FM.Order;

namespace NpoMash.Erm.Hrm.Salary {
    public static class HrmSalaryTaskMatrixReductionLogic {

        public static HrmSalaryTaskMatrixReduction initTaskMatrixReduction(HrmPeriod Period, IObjectSpace os,
            DEPARTMENT_GROUP_DEP group_dep, HRM_MATRIX_VARIANT bringing_method) {
            HrmSalaryTaskMatrixReduction MatrixReduction = os.CreateObject<HrmSalaryTaskMatrixReduction>();
            MatrixReduction.GroupDep = group_dep;
            MatrixReduction.Period = Period;
            Period.MatrixReduction.Add(MatrixReduction);
            MatrixReduction.AllocParameters = Period.CurrentAllocParameter;
            if (group_dep == DEPARTMENT_GROUP_DEP.KB) {
                MatrixReduction.TimeSheetGroup = Period.CurrentTimeSheet.KB;
                MatrixReduction.Period.CurrentKBmatrixReduction = MatrixReduction;
            }
            else {
                MatrixReduction.TimeSheetGroup = Period.CurrentTimeSheet.OZM;
                MatrixReduction.Period.CurrentOZMmatrixReduction = MatrixReduction; 
            }

            foreach (HrmMatrix matrix in Period.Matrixs) {
                if (matrix.TypeMatrix == HRM_MATRIX_TYPE_MATRIX.Planned &&
                    matrix.GroupDep == group_dep) {
                    MatrixReduction.MatrixPlan = matrix;
                }
            }
            /*
            if (bringing_method == HRM_MATRIX_VARIANT.MinimizeMaximumDeviations) {
                MatrixReduction.MinimizeMaximumDeviationsMatrix = 
                    HrmMatrixLogic.makeAllocMatrix(MatrixReduction, os, group_dep, bringing_method, Period);
            }
            if (bringing_method == HRM_MATRIX_VARIANT.MinimizeNumberOfDeviations) {
                MatrixReduction.MinimizeNumberOfDeviationsMatrix = 
                    HrmMatrixLogic.makeAllocMatrix(MatrixReduction, os, group_dep, bringing_method, Period); 
            }
            if (bringing_method == HRM_MATRIX_VARIANT.ProportionsMethod) {
                MatrixReduction.ProportionsMethodMatrix = 
                    HrmMatrixLogic.makeAllocMatrix(MatrixReduction, os, group_dep, bringing_method, Period);
            }*/
            /*if (group_dep == DEPARTMENT_GROUP_DEP.KB) {
                MatrixReduction.Period.CurrentKBmatrixReduction = MatrixReduction;
            }
            else {
                MatrixReduction.Period.CurrentOZMmatrixReduction = MatrixReduction; 
            }*/

            return MatrixReduction;
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
                if (m.TypeMatrix == HRM_MATRIX_TYPE_MATRIX.Coerced && m.Status == HRM_MATRIX_STATUS.Accepted)
                    m.Status = HRM_MATRIX_STATUS.Exported;
        }

        public static bool AllCoercedMatrixesAccepted(HrmMatrix matrix_to_accept, HrmPeriod current_period) {
            bool kb_accepted = false;
            bool ozm_accepted = false;
            if (matrix_to_accept.GroupDep == DEPARTMENT_GROUP_DEP.KB)
                kb_accepted = true;
            else ozm_accepted = true;
            foreach (HrmMatrix m in current_period.Matrixs) {
                if (m.TypeMatrix == HRM_MATRIX_TYPE_MATRIX.Coerced && m.Status == HRM_MATRIX_STATUS.Accepted)
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
                reduc.MinimizeMaximumDeviationsMatrix.Status = HRM_MATRIX_STATUS.Closed;
            if (reduc.MinimizeNumberOfDeviationsMatrix != null)
                reduc.MinimizeNumberOfDeviationsMatrix.Status = HRM_MATRIX_STATUS.Closed;
            if (reduc.ProportionsMethodMatrix != null)
                reduc.ProportionsMethodMatrix.Status = HRM_MATRIX_STATUS.Closed;
            matrix_to_accept.Status = HRM_MATRIX_STATUS.Accepted;
        }

    }
}
