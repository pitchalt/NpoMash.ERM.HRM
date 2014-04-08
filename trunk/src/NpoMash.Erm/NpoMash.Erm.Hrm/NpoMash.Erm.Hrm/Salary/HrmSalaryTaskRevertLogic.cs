using System;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;
//
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;


namespace NpoMash.Erm.Hrm.Salary {
 

    public static class HrmSalaryTaskRevertLogic {

        public static void InitObjects(IObjectSpace local_object_space, HrmSalaryTaskRevert local_task) {
            switch (local_task.Period.Status) {
                case HrmPeriodStatus.READY_TO_CALCULATE_COERCED_MATRIXS:
                    local_task.AllocParameter = local_task.Period.CurrentAllocParameter;
                    local_task.TimeSheetKB = local_task.Period.CurrentTimeSheetKB;
                    local_task.TimeSheetOZM = local_task.Period.CurrentTimeSheetOZM;
                    local_task.MatrixPlanKB = local_task.Period.CurrentMatrixAllocPlanKB;;
                    local_task.MatrixPlanOZM = local_task.Period.CurrentMatrixAllocPlanOZM;
                    break;
                case HrmPeriodStatus.READY_TO_EXPORT_CORCED_MATRIXS:
                    local_task.AllocParameter = local_task.Period.CurrentAllocParameter;
                    local_task.TimeSheetKB = local_task.Period.CurrentTimeSheetKB;
                    local_task.TimeSheetOZM = local_task.Period.CurrentTimeSheetOZM;
                    local_task.MatrixPlanKB = local_task.Period.CurrentMatrixAllocPlanKB;;
                    local_task.MatrixPlanOZM = local_task.Period.CurrentMatrixAllocPlanOZM;
                    if (local_task.Period.CurrentKBmatrixReduction.MinimizeMaximumDeviationsMatrix != null) { local_task.MatrixReductionKB = local_task.Period.CurrentKBmatrixReduction.MinimizeMaximumDeviationsMatrix; }
                    if (local_task.Period.CurrentKBmatrixReduction.ProportionsMethodMatrix != null) { local_task.MatrixReductionKB = local_task.Period.CurrentKBmatrixReduction.ProportionsMethodMatrix; }
                    if (local_task.Period.CurrentKBmatrixReduction.MinimizeNumberOfDeviationsMatrix != null) { local_task.MatrixReductionKB = local_task.Period.CurrentKBmatrixReduction.MinimizeNumberOfDeviationsMatrix; }
                    if (local_task.Period.CurrentOZMmatrixReduction.MinimizeMaximumDeviationsMatrix != null) { local_task.MatrixReductionOZM = local_task.Period.CurrentOZMmatrixReduction.MinimizeMaximumDeviationsMatrix; }
                    if (local_task.Period.CurrentOZMmatrixReduction.MinimizeNumberOfDeviationsMatrix != null) { local_task.MatrixReductionOZM = local_task.Period.CurrentOZMmatrixReduction.MinimizeNumberOfDeviationsMatrix; }
                    if (local_task.Period.CurrentOZMmatrixReduction.ProportionsMethodMatrix != null) { local_task.MatrixReductionOZM = local_task.Period.CurrentOZMmatrixReduction.ProportionsMethodMatrix; }
                    break;
                case HrmPeriodStatus.COERCED_MATRIXES_EXPORTED:
                    local_task.AllocParameter = local_task.Period.CurrentAllocParameter;
                    local_task.TimeSheetKB = local_task.Period.CurrentTimeSheetKB;
                    local_task.TimeSheetOZM = local_task.Period.CurrentTimeSheetOZM;
                    local_task.MatrixPlanKB = local_task.Period.CurrentMatrixAllocPlanKB;;
                    local_task.MatrixPlanOZM = local_task.Period.CurrentMatrixAllocPlanOZM;
                    if (local_task.Period.CurrentKBmatrixReduction.MinimizeMaximumDeviationsMatrix != null) { local_task.MatrixReductionKB = local_task.Period.CurrentKBmatrixReduction.MinimizeMaximumDeviationsMatrix; }
                    if (local_task.Period.CurrentKBmatrixReduction.ProportionsMethodMatrix != null) { local_task.MatrixReductionKB = local_task.Period.CurrentKBmatrixReduction.ProportionsMethodMatrix; }
                    if (local_task.Period.CurrentKBmatrixReduction.MinimizeNumberOfDeviationsMatrix != null) { local_task.MatrixReductionKB = local_task.Period.CurrentKBmatrixReduction.MinimizeNumberOfDeviationsMatrix; }
                    if (local_task.Period.CurrentOZMmatrixReduction.MinimizeMaximumDeviationsMatrix != null) { local_task.MatrixReductionOZM = local_task.Period.CurrentOZMmatrixReduction.MinimizeMaximumDeviationsMatrix; }
                    if (local_task.Period.CurrentOZMmatrixReduction.MinimizeNumberOfDeviationsMatrix != null) { local_task.MatrixReductionOZM = local_task.Period.CurrentOZMmatrixReduction.MinimizeNumberOfDeviationsMatrix; }
                    if (local_task.Period.CurrentOZMmatrixReduction.ProportionsMethodMatrix != null) { local_task.MatrixReductionOZM = local_task.Period.CurrentOZMmatrixReduction.ProportionsMethodMatrix; }
                    break;
                case HrmPeriodStatus.ACCOUNT_OPERATION_FIRST_IMPORTED:
                    local_task.AllocParameter = local_task.Period.CurrentAllocParameter;
                    local_task.TimeSheetKB = local_task.Period.CurrentTimeSheetKB;
                    local_task.TimeSheetOZM = local_task.Period.CurrentTimeSheetOZM;
                    local_task.MatrixPlanKB = local_task.Period.CurrentMatrixAllocPlanKB;;
                    local_task.MatrixPlanOZM = local_task.Period.CurrentMatrixAllocPlanOZM;
                    if (local_task.Period.CurrentKBmatrixReduction.MinimizeMaximumDeviationsMatrix != null) { local_task.MatrixReductionKB = local_task.Period.CurrentKBmatrixReduction.MinimizeMaximumDeviationsMatrix; }
                    if (local_task.Period.CurrentKBmatrixReduction.ProportionsMethodMatrix != null) { local_task.MatrixReductionKB = local_task.Period.CurrentKBmatrixReduction.ProportionsMethodMatrix; }
                    if (local_task.Period.CurrentKBmatrixReduction.MinimizeNumberOfDeviationsMatrix != null) { local_task.MatrixReductionKB = local_task.Period.CurrentKBmatrixReduction.MinimizeNumberOfDeviationsMatrix; }
                    if (local_task.Period.CurrentOZMmatrixReduction.MinimizeMaximumDeviationsMatrix != null) { local_task.MatrixReductionOZM = local_task.Period.CurrentOZMmatrixReduction.MinimizeMaximumDeviationsMatrix; }
                    if (local_task.Period.CurrentOZMmatrixReduction.MinimizeNumberOfDeviationsMatrix != null) { local_task.MatrixReductionOZM = local_task.Period.CurrentOZMmatrixReduction.MinimizeNumberOfDeviationsMatrix; }
                    if (local_task.Period.CurrentOZMmatrixReduction.ProportionsMethodMatrix != null) { local_task.MatrixReductionOZM = local_task.Period.CurrentOZMmatrixReduction.ProportionsMethodMatrix; }
                    local_task.MatrixAllocResultKB = local_task.Period.CurrentMatrixAllocResultKB;
                    local_task.MatrixAllocResultOZM = local_task.Period.CurrentMatrixAllocResultOZM;
                    break;
                case HrmPeriodStatus.READY_TO_RESERVE_MATRIX_CREATE:
                    local_task.AllocParameter = local_task.Period.CurrentAllocParameter;
                    local_task.TimeSheetKB = local_task.Period.CurrentTimeSheetKB;
                    local_task.TimeSheetOZM = local_task.Period.CurrentTimeSheetOZM;
                    local_task.MatrixPlanKB = local_task.Period.CurrentMatrixAllocPlanKB;;
                    local_task.MatrixPlanOZM = local_task.Period.CurrentMatrixAllocPlanOZM;
                    if (local_task.Period.CurrentKBmatrixReduction.MinimizeMaximumDeviationsMatrix != null) { local_task.MatrixReductionKB = local_task.Period.CurrentKBmatrixReduction.MinimizeMaximumDeviationsMatrix; }
                    if (local_task.Period.CurrentKBmatrixReduction.ProportionsMethodMatrix != null) { local_task.MatrixReductionKB = local_task.Period.CurrentKBmatrixReduction.ProportionsMethodMatrix; }
                    if (local_task.Period.CurrentKBmatrixReduction.MinimizeNumberOfDeviationsMatrix != null) { local_task.MatrixReductionKB = local_task.Period.CurrentKBmatrixReduction.MinimizeNumberOfDeviationsMatrix; }
                    if (local_task.Period.CurrentOZMmatrixReduction.MinimizeMaximumDeviationsMatrix != null) { local_task.MatrixReductionOZM = local_task.Period.CurrentOZMmatrixReduction.MinimizeMaximumDeviationsMatrix; }
                    if (local_task.Period.CurrentOZMmatrixReduction.MinimizeNumberOfDeviationsMatrix != null) { local_task.MatrixReductionOZM = local_task.Period.CurrentOZMmatrixReduction.MinimizeNumberOfDeviationsMatrix; }
                    if (local_task.Period.CurrentOZMmatrixReduction.ProportionsMethodMatrix != null) { local_task.MatrixReductionOZM = local_task.Period.CurrentOZMmatrixReduction.ProportionsMethodMatrix; }
                    local_task.MatrixAllocResultKB = local_task.Period.CurrentMatrixAllocResultKB;
                    local_task.MatrixAllocResultOZM = local_task.Period.CurrentMatrixAllocResultOZM;
                    break;
                case HrmPeriodStatus.READY_TO_RESERVE_MATRIX_UPLOAD:
                    local_task.AllocParameter = local_task.Period.CurrentAllocParameter;
                    local_task.TimeSheetKB = local_task.Period.CurrentTimeSheetKB;
                    local_task.TimeSheetOZM = local_task.Period.CurrentTimeSheetOZM;
                    local_task.MatrixPlanKB = local_task.Period.CurrentMatrixAllocPlanKB;;
                    local_task.MatrixPlanOZM = local_task.Period.CurrentMatrixAllocPlanOZM;
                    if (local_task.Period.CurrentKBmatrixReduction.MinimizeMaximumDeviationsMatrix != null) { local_task.MatrixReductionKB = local_task.Period.CurrentKBmatrixReduction.MinimizeMaximumDeviationsMatrix; }
                    if (local_task.Period.CurrentKBmatrixReduction.ProportionsMethodMatrix != null) { local_task.MatrixReductionKB = local_task.Period.CurrentKBmatrixReduction.ProportionsMethodMatrix; }
                    if (local_task.Period.CurrentKBmatrixReduction.MinimizeNumberOfDeviationsMatrix != null) { local_task.MatrixReductionKB = local_task.Period.CurrentKBmatrixReduction.MinimizeNumberOfDeviationsMatrix; }
                    if (local_task.Period.CurrentOZMmatrixReduction.MinimizeMaximumDeviationsMatrix != null) { local_task.MatrixReductionOZM = local_task.Period.CurrentOZMmatrixReduction.MinimizeMaximumDeviationsMatrix; }
                    if (local_task.Period.CurrentOZMmatrixReduction.MinimizeNumberOfDeviationsMatrix != null) { local_task.MatrixReductionOZM = local_task.Period.CurrentOZMmatrixReduction.MinimizeNumberOfDeviationsMatrix; }
                    if (local_task.Period.CurrentOZMmatrixReduction.ProportionsMethodMatrix != null) { local_task.MatrixReductionOZM = local_task.Period.CurrentOZMmatrixReduction.ProportionsMethodMatrix; }
                    local_task.MatrixAllocResultKB = local_task.Period.CurrentMatrixAllocResultKB;
                    local_task.MatrixAllocResultOZM = local_task.Period.CurrentMatrixAllocResultOZM;
                    local_task.MatrixProvision = local_task.Period.CurrentProvisionMatrix.ProvisionMatrix;
                    break;
                case HrmPeriodStatus.RESERVE_MATRIX_UPLOADED:
                    local_task.AllocParameter = local_task.Period.CurrentAllocParameter;
                    local_task.TimeSheetKB = local_task.Period.CurrentTimeSheetKB;
                    local_task.TimeSheetOZM = local_task.Period.CurrentTimeSheetOZM;
                    local_task.MatrixPlanKB = local_task.Period.CurrentMatrixAllocPlanKB;;
                    local_task.MatrixPlanOZM = local_task.Period.CurrentMatrixAllocPlanOZM;
                    if (local_task.Period.CurrentKBmatrixReduction.MinimizeMaximumDeviationsMatrix != null) { local_task.MatrixReductionKB = local_task.Period.CurrentKBmatrixReduction.MinimizeMaximumDeviationsMatrix; }
                    if (local_task.Period.CurrentKBmatrixReduction.ProportionsMethodMatrix != null) { local_task.MatrixReductionKB = local_task.Period.CurrentKBmatrixReduction.ProportionsMethodMatrix; }
                    if (local_task.Period.CurrentKBmatrixReduction.MinimizeNumberOfDeviationsMatrix != null) { local_task.MatrixReductionKB = local_task.Period.CurrentKBmatrixReduction.MinimizeNumberOfDeviationsMatrix; }
                    if (local_task.Period.CurrentOZMmatrixReduction.MinimizeMaximumDeviationsMatrix != null) { local_task.MatrixReductionOZM = local_task.Period.CurrentOZMmatrixReduction.MinimizeMaximumDeviationsMatrix; }
                    if (local_task.Period.CurrentOZMmatrixReduction.MinimizeNumberOfDeviationsMatrix != null) { local_task.MatrixReductionOZM = local_task.Period.CurrentOZMmatrixReduction.MinimizeNumberOfDeviationsMatrix; }
                    if (local_task.Period.CurrentOZMmatrixReduction.ProportionsMethodMatrix != null) { local_task.MatrixReductionOZM = local_task.Period.CurrentOZMmatrixReduction.ProportionsMethodMatrix; }
                    local_task.MatrixAllocResultKB = local_task.Period.CurrentMatrixAllocResultKB;
                    local_task.MatrixAllocResultOZM = local_task.Period.CurrentMatrixAllocResultOZM;
                    local_task.MatrixProvision = local_task.Period.CurrentProvisionMatrix.ProvisionMatrix;
                    break;
                case HrmPeriodStatus.ACCOUNT_OPERATION_LAST_IMPORTED: local_task.AllocParameter = local_task.Period.CurrentAllocParameter;
                    local_task.TimeSheetKB = local_task.Period.CurrentTimeSheetKB;
                    local_task.TimeSheetOZM = local_task.Period.CurrentTimeSheetOZM;
                    local_task.MatrixPlanKB = local_task.Period.CurrentMatrixAllocPlanKB; ;
                    local_task.MatrixPlanOZM = local_task.Period.CurrentMatrixAllocPlanOZM;
                    if (local_task.Period.CurrentKBmatrixReduction.MinimizeMaximumDeviationsMatrix != null) { local_task.MatrixReductionKB = local_task.Period.CurrentKBmatrixReduction.MinimizeMaximumDeviationsMatrix; }
                    if (local_task.Period.CurrentKBmatrixReduction.ProportionsMethodMatrix != null) { local_task.MatrixReductionKB = local_task.Period.CurrentKBmatrixReduction.ProportionsMethodMatrix; }
                    if (local_task.Period.CurrentKBmatrixReduction.MinimizeNumberOfDeviationsMatrix != null) { local_task.MatrixReductionKB = local_task.Period.CurrentKBmatrixReduction.MinimizeNumberOfDeviationsMatrix; }
                    if (local_task.Period.CurrentOZMmatrixReduction.MinimizeMaximumDeviationsMatrix != null) { local_task.MatrixReductionOZM = local_task.Period.CurrentOZMmatrixReduction.MinimizeMaximumDeviationsMatrix; }
                    if (local_task.Period.CurrentOZMmatrixReduction.MinimizeNumberOfDeviationsMatrix != null) { local_task.MatrixReductionOZM = local_task.Period.CurrentOZMmatrixReduction.MinimizeNumberOfDeviationsMatrix; }
                    if (local_task.Period.CurrentOZMmatrixReduction.ProportionsMethodMatrix != null) { local_task.MatrixReductionOZM = local_task.Period.CurrentOZMmatrixReduction.ProportionsMethodMatrix; }
                    local_task.MatrixAllocResultKB = local_task.Period.CurrentMatrixAllocResultKB;
                    local_task.MatrixAllocResultOZM = local_task.Period.CurrentMatrixAllocResultOZM;
                    local_task.MatrixProvision = local_task.Period.CurrentProvisionMatrix.ProvisionMatrix;
                    break;
            }
        }
    } 
}