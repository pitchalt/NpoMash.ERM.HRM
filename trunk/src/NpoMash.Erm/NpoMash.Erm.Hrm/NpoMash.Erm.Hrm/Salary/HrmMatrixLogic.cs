using System;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;
//
using FileHelpers;
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
using IntecoAG.ERM.FM.Order;
using NpoMash.Erm.Hrm.Exchange;

namespace NpoMash.Erm.Hrm.Salary {


    public static class HrmMatrixLogic {

        public static void ImportPlanMatrixes(IObjectSpace os, HrmSalaryTaskImportSourceData task) {
//            HrmPeriod period, out HrmMatrixAllocPlan KBMatrix, out HrmMatrixAllocPlan OZMMatrix) {
            var plan_data = new FixedFileEngine<ImportMatrixPlan>();
            ImportMatrixPlan[] plan_list = plan_data.ReadFile("../../../../../../../var/Matrix_Plan.dat");
            //�������������� �������� ������� �� � ���
            HrmMatrixAllocPlan kb_plan_matrix = os.CreateObject<HrmMatrixAllocPlan>();
            kb_plan_matrix.Status = HrmMatrixStatus.MATRIX_ACCEPTED;
//            kb_plan_matrix.Period = period;
            kb_plan_matrix.TypeMatrix = HrmMatrixTypeMatrix.MATRIX_PLANNED;
            kb_plan_matrix.Type = HrmMatrixType.TYPE_MATIX;
            kb_plan_matrix.GroupDep = DepartmentGroupDep.DEPARTMENT_KB;
            kb_plan_matrix.IterationNumber = 1;
            task.Period.Matrixs.Add(kb_plan_matrix);
            HrmMatrixAllocPlan ozm_plan_matrix = os.CreateObject<HrmMatrixAllocPlan>();
            ozm_plan_matrix.Status = HrmMatrixStatus.MATRIX_ACCEPTED;
//            ozm_plan_matrix.Period = period;
            ozm_plan_matrix.TypeMatrix = HrmMatrixTypeMatrix.MATRIX_PLANNED;
            ozm_plan_matrix.Type = HrmMatrixType.TYPE_MATIX;
            ozm_plan_matrix.GroupDep = DepartmentGroupDep.DEPARTMENT_OZM;
            ozm_plan_matrix.IterationNumber = 1;
            task.Period.Matrixs.Add(ozm_plan_matrix);

            Int16 current_year = task.Period.Year;
            Int16 current_month = task.Period.Month;
            //�������� ���������� ������ � �����
            foreach (var each in plan_list) {
                //���� ������ ��������� � ������ ������� �� �������� ���������
                if (each.Year == current_year && each.Month == current_month) {
                    HrmMatrix plan_matrix = null;
                    //���������� � ����� ������ ������������� ��������� ������
                    foreach (Department dep in os.GetObjects<Department>()) {
                        if (String.Compare(each.Department.Trim(), dep.Code) == 0)
                            if (dep.GroupDep == DepartmentGroupDep.DEPARTMENT_KB)
                                plan_matrix = kb_plan_matrix;
                            else plan_matrix = ozm_plan_matrix;
                        //������ �� ����� � ����� �������� ��������
                    }
                    //���� �� ����� ������ ������������� - ��� �����
                    if (plan_matrix == null) 
                        throw new Exception("There is no department with code " + each.Department.Trim());
                    //����� - ������� ������ � �������� �� ���������
                    HrmMatrixCell cell = os.CreateObject<HrmMatrixCell>();
                    cell.Time = each.Norm;
                    cell.Sum = 1;
                    //����������� � ��������
                    HrmMatrixColumn current_column = null;
                    foreach (HrmMatrixColumn col in plan_matrix.Columns)
                        if (col.Department.Code == each.Department.Trim())
                            current_column = col;
                    //���� ������� ��� �� ���� - �� ������� � �������������� �����
                    if (current_column == null) {
                        current_column = os.CreateObject<HrmMatrixColumn>();
                        current_column.Matrix = plan_matrix;
                        plan_matrix.Columns.Add(current_column);
                        foreach (Department dep in os.GetObjects<Department>())
                            if (System.String.Compare(dep.Code, each.Department.Trim()) == 0)
                                current_column.Department = dep;
                    }
                    //������ ��������� ������� � �������, ������ � �������� ������ ������
                    cell.Column = current_column;
                    current_column.Cells.Add(cell);
                    //������ ����������� �� ��������
                    HrmMatrixRow current_row = null;
                    foreach (HrmMatrixRow row in plan_matrix.Rows)
                        if (System.String.Compare(row.Order.Code, each.OrderCode.Trim()) == 0)
                            current_row = row;
                    //���� ������� ��� �� ���� - ����� ������� � �������������� �����
                    if (current_row == null) {
                        current_row = os.CreateObject<HrmMatrixRow>();
                        current_row.Matrix = plan_matrix;
                        plan_matrix.Rows.Add(current_row);
                        foreach (fmCOrder order in os.GetObjects<fmCOrder>())
                            if (System.String.Compare(order.Code, each.OrderCode.Trim()) == 0)
                                current_row.Order = order;
                    }
                    //������ ��������� ������� � �������, ������ �� �������� ������ ������
                    cell.Row = current_row;
                    current_row.Cells.Add(cell);
                }
            }
            task.MatrixPlanKB = kb_plan_matrix;
            task.MatrixPlanOZM = ozm_plan_matrix;
        }

        static public HrmMatrixAllocPlan setTestData(IObjectSpace os, HrmPeriod current_period, DepartmentGroupDep group) {
            Random random = new Random();
            List<HrmMatrixColumn> columns = new List<HrmMatrixColumn>();
            List<HrmMatrixRow> rows = new List<HrmMatrixRow>();
            HrmMatrixAllocPlan plan_matrix = os.CreateObject<HrmMatrixAllocPlan>();

            foreach (fmCOrder current_order in os.GetObjects<fmCOrder>()) {
                HrmMatrixRow current_row = os.CreateObject<HrmMatrixRow>();
                current_row.Matrix = plan_matrix;
                plan_matrix.Rows.Add(current_row);
                current_row.Order = current_order;
                HrmMatrixColumn current_column = null;
                foreach (Department current_department in os.GetObjects<Department>()) {
                    if (current_department.GroupDep == group) {
                        foreach (HrmMatrixColumn col in plan_matrix.Columns)
                            if (col.Department == current_department) current_column = col;
                        if (current_column == null) {
                            current_column = os.CreateObject<HrmMatrixColumn>();
                            current_column.Department = current_department;
                            current_column.Matrix = plan_matrix;
                            plan_matrix.Columns.Add(current_column);
                        }
                        HrmMatrixCell new_cell = os.CreateObject<HrmMatrixCell>();
                        new_cell.Time = Convert.ToInt16(random.Next(100, 500));
                        new_cell.Sum = Convert.ToInt16(random.Next(100, 500));
                        new_cell.Column = current_column;
                        new_cell.Row = current_row;
                        current_row.Cells.Add(new_cell);
                        current_column.Cells.Add(new_cell);

                    }
                    current_column = null;
                }
            }

            plan_matrix.Type = HrmMatrixType.TYPE_MATIX;
            plan_matrix.TypeMatrix = HrmMatrixTypeMatrix.MATRIX_PLANNED;
            plan_matrix.GroupDep = group;
            plan_matrix.Status = HrmMatrixStatus.MATRIX_OPENED;
            plan_matrix.IterationNumber = 1;
            plan_matrix.Variant = HrmMatrixVariant.PROPORTIONS_METHOD_VARIANT;
            plan_matrix.Period = current_period;
            current_period.Matrixs.Add(plan_matrix);
            return plan_matrix;
        }

        static public HrmMatrix makeAllocMatrix(HrmSalaryTaskMatrixReduction AllocMatrix, IObjectSpace os,
            DepartmentGroupDep group_dep, HrmMatrixVariant bringing_method, HrmPeriod period) {

            var result_matrix = os.CreateObject<HrmMatrix>();
            foreach (HrmMatrixColumn col in AllocMatrix.MatrixPlan.Columns) {
                HrmMatrixColumn new_col = os.CreateObject<HrmMatrixColumn>();
                new_col.Department = col.Department;
                result_matrix.Columns.Add(new_col);
            }

            foreach (HrmMatrixRow row in AllocMatrix.MatrixPlan.Rows) {
                HrmMatrixRow new_row = os.CreateObject<HrmMatrixRow>();
                new_row.Order = row.Order;
                result_matrix.Rows.Add(new_row);

                foreach (HrmMatrixCell cell in row.Cells) {
                    HrmMatrixColumn new_col = result_matrix.Columns.FirstOrDefault(x => x.Department == cell.Column.Department);
                    HrmMatrixCell new_cell = os.CreateObject<HrmMatrixCell>();
                    new_row.Cells.Add(new_cell);
                    new_col.Cells.Add(new_cell);
                    Int16 coefficient = 0;
                    switch (bringing_method) {
                        case HrmMatrixVariant.MINIMIZE_MAXIMUM_DEVIATIONS_VARIANT: {
                                coefficient = 2;
                                AllocMatrix.MinimizeMaximumDeviationsMatrix = result_matrix;
                                break;
                            }
                        case HrmMatrixVariant.MINIMIZE_NUMBER_OF_DEVIATIONS_VARIANT: {
                                coefficient = 3;
                                AllocMatrix.MinimizeNumberOfDeviationsMatrix = result_matrix;
                                break;
                            }
                        case HrmMatrixVariant.PROPORTIONS_METHOD_VARIANT: {
                                coefficient = 4;
                                AllocMatrix.ProportionsMethodMatrix = result_matrix;
                                break;
                            }
                    }
                    new_cell.Time = (Int16)(cell.Time * coefficient);
                    new_cell.Sum = cell.Sum * coefficient;

                    result_matrix.Type = HrmMatrixType.TYPE_MATIX;
                    result_matrix.TypeMatrix = HrmMatrixTypeMatrix.MATRIX_COERCED;
                    result_matrix.GroupDep = group_dep;
                    result_matrix.Status = HrmMatrixStatus.MATRIX_SAVED;
                    result_matrix.IterationNumber = 2;
                    result_matrix.Variant = bringing_method;
                    result_matrix.Period = period;

                }
            }
            return result_matrix;
        }
    }
}


