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

using IntecoAG.ERM.HRM.Organization;
using NpoMash.Erm.Hrm.Salary;

namespace NpoMash.Erm.Hrm {
    public class OpenPeriodExistsException : Exception {
        public OpenPeriodExistsException() : base() { }
        public OpenPeriodExistsException(string message) : base(message) { }
    }


    public static class HrmPeriodLogic {

        public const Int16 INIT_YEAR = 2014;
        public const Int16 INIT_MONTH = 11;

        public static HrmPeriod findLastPeriod(IObjectSpace os) {
            var period_list = os.GetObjects<HrmPeriod>(null, true);
            HrmPeriod last_period=null;
            if (period_list.Count() != 0) {
                var maxYear = period_list.Max(Period => Period.Year);
                List<HrmPeriod> HrmPeriodMaxYearsCollection = new List<HrmPeriod>(); //������ �������� � ������������ �����
                //��������� ���� ����
                foreach (HrmPeriod a in period_list) {
                    if (a.Year == maxYear) {
                        HrmPeriodMaxYearsCollection.Add(a);
                    }
                }
                Int16 maxMonth = HrmPeriodMaxYearsCollection.Max(myProd => myProd.Month); //������������ ����� � ���� ���������
                foreach (HrmPeriod a in period_list) {
                    if ((a.Year == maxYear) && (a.Month == maxMonth)) {
                        last_period = a;
                    }
                }
            }
            return last_period; // ���������� ��������� ������
        }

        public static void addMonth(HrmPeriod period_with_next_month, Int16 y, Int16 m) {
            m++;
            if (m > 12) {
                m = 1;
                y++;
            }
            period_with_next_month.Init(y, m);
        }

        public static HrmPeriod createPeriod(IObjectSpace os) {
            HrmPeriod last_period = findLastPeriod(os);
            if (last_period != null && last_period.Status != HrmPeriodStatus.CLOSED) {
                throw new OpenPeriodExistsException("���� ���������� ������");
            }
            HrmPeriod new_period = os.CreateObject<HrmPeriod>();
            if (last_period == null) {
                new_period.PeriodPrevious = new_period;
                new_period.Init(INIT_YEAR, INIT_MONTH);
            }
            else {
                addMonth(new_period, last_period.Year, last_period.Month);
                new_period.PeriodPrevious = last_period;
            }
            new_period.setStatus(HrmPeriodStatus.OPENED);
            return new_period;
        }

        public static HrmPeriodStatus SetReadyToCalculateCoercedMatrixesStatus(HrmPeriod period) {
            bool ok = false;
            HrmPeriodStatus stat = HrmPeriodStatus.READY_TO_CALCULATE_COERCED_MATRIXS;
            if ((period.CurrentAllocParameter.Status == Salary.HrmPeriodAllocParameterStatus.LIST_OF_ORDER_ACCEPTED ||
                period.CurrentAllocParameter.Status == Salary.HrmPeriodAllocParameterStatus.ALLOC_PARAMETERS_ACCEPTED)) {
                ok = SourceDataIsAccepted(period);
            }
            if (ok) return stat;
            else throw new InvalidOperationException();
        }

        public static bool SourceDataIsLoaded(HrmPeriod period) {
            bool kb_plan_mat_imported = false;
            bool ozm_plan_mat_imported = false;
            bool kb_time_sheet_imported = false;
            bool ozm_time_sheet_imported = false;
            foreach (HrmMatrix mat in period.Matrixs) {
                if (mat.TypeMatrix == HrmMatrixTypeMatrix.MATRIX_PLANNED && mat.GroupDep == DepartmentGroupDep.DEPARTMENT_KB &&
                    mat.Status != HrmMatrixStatus.MATRIX_ARCHIVE && mat.Status != HrmMatrixStatus.NOTDOWNLOADED)
                    kb_plan_mat_imported = true;
                if (mat.TypeMatrix == HrmMatrixTypeMatrix.MATRIX_PLANNED && mat.GroupDep == DepartmentGroupDep.DEPARTMENT_OZM &&
                    mat.Status != HrmMatrixStatus.MATRIX_ARCHIVE && mat.Status != HrmMatrixStatus.NOTDOWNLOADED)
                    ozm_plan_mat_imported = true;
            }
            foreach (HrmTimeSheet ts in period.TimeSheets) {
                if (ts.GroupDep == DepartmentGroupDep.DEPARTMENT_KB && ts.Status != HrmTimeSheetStatus.ARCHIVE && ts.Status != HrmTimeSheetStatus.NOTDOWNLOADED)
                    kb_time_sheet_imported = true;
                if (ts.GroupDep == DepartmentGroupDep.DEPARTMENT_OZM && ts.Status != HrmTimeSheetStatus.ARCHIVE && ts.Status != HrmTimeSheetStatus.NOTDOWNLOADED)
                    ozm_time_sheet_imported = true;
            }
            return kb_plan_mat_imported && ozm_plan_mat_imported && kb_time_sheet_imported && ozm_time_sheet_imported;
        }

        public static bool SourceDataIsAccepted(HrmPeriod period) {
            bool kb_plan_mat_accepted = false;
            bool ozm_plan_mat_accepted = false;
            bool kb_time_sheet_accepted = false;
            bool ozm_time_sheet_accepted = false;
            foreach (HrmMatrix mat in period.Matrixs) {
                if (mat.TypeMatrix == HrmMatrixTypeMatrix.MATRIX_PLANNED &&
                    mat.GroupDep == DepartmentGroupDep.DEPARTMENT_KB &&
                    mat.Status == HrmMatrixStatus.MATRIX_ACCEPTED)
                    kb_plan_mat_accepted = true;
                if (mat.TypeMatrix == HrmMatrixTypeMatrix.MATRIX_PLANNED &&
                    mat.GroupDep == DepartmentGroupDep.DEPARTMENT_OZM &&
                    mat.Status == HrmMatrixStatus.MATRIX_ACCEPTED)
                    ozm_plan_mat_accepted = true;
            }
            foreach (HrmTimeSheet ts in period.TimeSheets) {
                if (ts.GroupDep == DepartmentGroupDep.DEPARTMENT_KB &&
                    ts.Status == HrmTimeSheetStatus.ACCEPTED)
                    kb_time_sheet_accepted = true;
                if (ts.GroupDep == DepartmentGroupDep.DEPARTMENT_OZM &&
                    ts.Status == HrmTimeSheetStatus.ACCEPTED)
                    ozm_time_sheet_accepted = true;
            }
            return kb_plan_mat_accepted && ozm_plan_mat_accepted && kb_time_sheet_accepted && ozm_time_sheet_accepted;
        }

        public static bool AccountOperationCompared(HrmPeriod period) {
            bool kb_coerced_matrix_accepted = false;
            bool ozm_coerced_matrix_accepted = false;
            bool kb_alloc_result_accepted = false;
            bool ozm_alloc_result_accepted = false;

            foreach (HrmMatrix mat in period.Matrixs) {
                if (mat.TypeMatrix == HrmMatrixTypeMatrix.MATRIX_COERCED &&
                    mat.GroupDep == DepartmentGroupDep.DEPARTMENT_KB &&
                    mat.Status == HrmMatrixStatus.MATRIX_ACCEPTED)
                    kb_coerced_matrix_accepted = true;
                if (mat.TypeMatrix == HrmMatrixTypeMatrix.MATRIX_COERCED &&
                    mat.GroupDep == DepartmentGroupDep.DEPARTMENT_OZM &&
                    mat.Status == HrmMatrixStatus.MATRIX_ACCEPTED)
                    ozm_coerced_matrix_accepted = true;
                if (mat.Type==HrmMatrixType.TYPE_ALLOC_RESULT&&
                    mat.GroupDep == DepartmentGroupDep.DEPARTMENT_OZM &&
                    mat.Status == HrmMatrixStatus.MATRIX_ACCEPTED)
                    ozm_alloc_result_accepted = true;
                if (mat.Type == HrmMatrixType.TYPE_ALLOC_RESULT &&
                    mat.GroupDep == DepartmentGroupDep.DEPARTMENT_KB &&
                    mat.Status == HrmMatrixStatus.MATRIX_ACCEPTED)
                    kb_alloc_result_accepted = true;
            }

            if ((kb_coerced_matrix_accepted && ozm_coerced_matrix_accepted && kb_alloc_result_accepted && ozm_alloc_result_accepted && period.CurrentAllocParameter.Status == HrmPeriodAllocParameterStatus.ALLOC_PARAMETERS_ACCEPTED)) { period.setStatus(HrmPeriodStatus.READY_TO_RESERVE_MATRIX_CREATE); }
            return kb_coerced_matrix_accepted && ozm_coerced_matrix_accepted && kb_alloc_result_accepted && ozm_alloc_result_accepted;
        }

        public static bool KBAccountOperationCompared(HrmPeriod period) {
            bool kb_alloc_result_accepted = false;

            foreach (HrmMatrix mat in period.Matrixs) {
                if (
                           mat.GroupDep == DepartmentGroupDep.DEPARTMENT_KB &&
                           mat.Status == HrmMatrixStatus.MATRIX_EXPORTED && mat.TypeMatrix==HrmMatrixTypeMatrix.MATRIX_COERCED)
                    kb_alloc_result_accepted = true;
            }

            return kb_alloc_result_accepted;
        }

        public static bool OZMAccountOperationCompared(HrmPeriod period) {
            bool ozm_alloc_result_accepted = false;

            foreach (HrmMatrix mat in period.Matrixs) {

                if (mat.GroupDep == DepartmentGroupDep.DEPARTMENT_OZM &&
                           mat.Status == HrmMatrixStatus.MATRIX_EXPORTED && mat.TypeMatrix == HrmMatrixTypeMatrix.MATRIX_COERCED)
                    ozm_alloc_result_accepted = true;
            }


            return ozm_alloc_result_accepted;
        }



    }
}