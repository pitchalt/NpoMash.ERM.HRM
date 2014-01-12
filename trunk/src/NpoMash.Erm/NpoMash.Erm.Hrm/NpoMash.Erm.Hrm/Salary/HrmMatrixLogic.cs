using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace NpoMash.Erm.Hrm.Salary {


    public static class HrmMatrixLogic {

        static public HrmMatrixAllocPlan setTestData(IObjectSpace os, HrmPeriod current_period, DEPARTMENT_GROUP_DEP group) {
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

            plan_matrix.Type = HRM_MATRIX_TYPE.Matrix;
            plan_matrix.TypeMatrix = HRM_MATRIX_TYPE_MATRIX.Planned;
            plan_matrix.GroupDep = group;
            plan_matrix.Status = HRM_MATRIX_STATUS.Opened;
            plan_matrix.IterationNumber = 1;
            plan_matrix.Variant = HRM_MATRIX_VARIANT.ProportionsMethod;
            plan_matrix.Period = current_period;
            current_period.Matrixs.Add(plan_matrix);
            return plan_matrix;
        }

        static public HrmMatrix makeAllocMatrix(HrmSalaryTaskMatrixReduction AllocMatrix, IObjectSpace os,
            DEPARTMENT_GROUP_DEP group_dep, HRM_MATRIX_VARIANT bringing_method,HrmPeriod period) {

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
                    switch (bringing_method){
                    case HRM_MATRIX_VARIANT.MinimizeMaximumDeviations:
                        {
                            coefficient = 2;
                            break;
                        }
                    case HRM_MATRIX_VARIANT.MinimizeNumberOfDeviations:
                        {
                            coefficient = 3;
                            break;
                        }
                    case HRM_MATRIX_VARIANT.ProportionsMethod:
                        {
                            coefficient = 4;
                            break;
                        }
                    }
                    new_cell.Time = (Int16) (cell.Time * coefficient);
                    new_cell.Sum = cell.Sum * coefficient;
                }
            }
            result_matrix.Type = HRM_MATRIX_TYPE.Matrix;
            result_matrix.TypeMatrix = HRM_MATRIX_TYPE_MATRIX.Coerced;
            result_matrix.GroupDep = group_dep;
            result_matrix.Status = HRM_MATRIX_STATUS.Saved;
            result_matrix.IterationNumber = 2;
            result_matrix.Variant = bringing_method;
            return result_matrix;
        }
    }
}

