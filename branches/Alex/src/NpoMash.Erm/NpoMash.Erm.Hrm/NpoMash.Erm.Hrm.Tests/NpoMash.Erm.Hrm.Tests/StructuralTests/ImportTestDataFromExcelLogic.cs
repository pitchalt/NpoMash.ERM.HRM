using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DevExpress;
using DevExpress.Spreadsheet;
using DevExpress.Office;

using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
//
using NpoMash.Erm.Hrm.Salary;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.HRM.Organization;



namespace NpoMash.Erm.Hrm.Tests.StructuralTests {


    static class ImportTestDataFromExcelLogic {

        public static Workbook getWorkbook(String path_to_file) {
            Workbook wb = new Workbook();
            wb.LoadDocument(path_to_file, DocumentFormat.Xls);
            return wb;
        }

        public static MatrixFromExcel GetData(Workbook wb, String worksheet_name, int itogs_in_row = 1, int itogs_in_col = 1) {
            const String end_string = "Итого";
            Worksheet ws = wb.Worksheets.First(x => x.Name == worksheet_name);
            int data_angle_row = 0;
            int data_angle_col = 0;
            int end_row = 0;
            int end_col = 0;
            while (ws.Cells[data_angle_row,0].Value.IsEmpty && data_angle_row <20)
                data_angle_row++;
            while (ws.Cells[data_angle_row, 0].Value.IsEmpty && data_angle_row < 20)
                data_angle_row++;
            while (ws.Cells[0,end_col].Value != end_string && end_col<1000)
                end_col++;
            while (ws.Cells[end_row,0].Value != end_string && end_row<1000)
                end_row++;
            if (data_angle_row == 20 || data_angle_col == 20 ||
                end_col == 1000 || end_row == 1000)
                throw new Exception("The input file is invalid");
            int current_row = data_angle_row;
            int current_col = data_angle_col;
            MatrixFromExcel result = new MatrixFromExcel(end_row - data_angle_row, end_col - data_angle_col);
            int index_row = 0;
            while (ws.Cells[current_row, 0].Value != end_string) {
                int row_merge_size = ws.Cells[current_row, 0].GetMergedRanges().Count();
                int index_col = 0;
                while (ws.Cells[0, current_col].Value != end_string) {
                    int col_merge_size = ws.Cells[0, current_col].GetMergedRanges().Count();
                    List<Decimal> cell_info = new List<Decimal>();
                    int current_cell_col = current_col;
                    int current_cell_row = current_row;
                    bool is_empty_cells = false;
                    bool all_cells_are_empty = true;
                    while (current_cell_row < current_row + row_merge_size && !is_empty_cells) {
                        while (current_cell_col < current_col + col_merge_size && !is_empty_cells) {
                            CellValue cv = ws.Cells[current_cell_row, current_cell_col].Value;
                            if (cv.IsEmpty)
                                is_empty_cells = true;
                            else {
                                cell_info.Add((decimal)cv.NumericValue);
                                all_cells_are_empty = false;
                            }
                            current_cell_row++;
                        }
                        current_cell_col++;
                    }
                    if (!all_cells_are_empty) {
                        if (result.columns_info[index_col] == null) {
                            List<String> col_info = new List<String>();
                            for (int i = 0; i < data_angle_row; i++)
                                col_info.Add(ws.Cells[i, current_col].Value.TextValue);
                            List<Decimal> itog_col_info = new List<Decimal>();
                            for (int i = 0; i < itogs_in_col; i++)
                                itog_col_info.Add((Decimal)ws.Cells[end_row + i, current_col].Value.NumericValue);
                            result.columns_info[index_col] = col_info;
                            result.itog_columns_info[index_col] = itog_col_info;
                        }

                        if (result.rows_info[index_row] == null) {
                            List<String> row_info = new List<String>();
                            for (int i = 0; i < data_angle_col; i++)
                                row_info.Add(ws.Cells[current_row, i].Value.TextValue);
                            current_col += col_merge_size;
                            List<Decimal> itog_row_info = new List<Decimal>();
                            for (int i = 0; i < itogs_in_row; i++)
                                itog_row_info.Add((Decimal)ws.Cells[current_row, end_col + i].Value.NumericValue);

                            result.rows_info[index_row] = row_info;
                            result.itog_rows_info[index_row] = itog_row_info;
                        }
                        result.mat[index_row, index_col] = cell_info;
                    }
                    index_col++;
                }
                current_row += row_merge_size;
                index_row++;
            }
            return result;
        }


        public static void CreateDepartmentsFromExcelTab(IObjectSpace os, MatrixFromExcel mat) {
            foreach (List<String> str_info in mat.columns_info) {
                String str = str_info[0];
                Department dep = os.CreateObject<Department>();
                dep.Code = str;
                str = str_info[1];
                if (str == "КБ")
                    dep.GroupDep = DepartmentGroupDep.DEPARTMENT_KB;
                if (str == "ОЗМ")
                    dep.GroupDep = DepartmentGroupDep.DEPARTMENT_OZM;
            }
        }

        public static void CreateOrdersFromExcelTab(IObjectSpace os, MatrixFromExcel mat) {
            foreach (List<String> str_info in mat.rows_info) {
                String str = str_info[0];
                fmCOrder order = os.CreateObject<fmCOrder>();
                order.Code = str;
                str = str_info[1];
                switch (str) {
                case "ТФ": {
                        order.TypeControl = FmCOrderTypeControl.TRUDEMK_FOT;
                        break;
                    }
                case "Ф": {
                        order.TypeControl = FmCOrderTypeControl.FOT;
                        break;
                    }
                case "Н": {
                        order.TypeControl = FmCOrderTypeControl.NO_ORDERED;
                        break;
                    }
                default: {
                        throw new Exception("Invalid type control in excel file!");
                    }
                }
            }
        }

        public static HrmPeriodAllocParameter CreateAllocParametersFromExcelTab(IObjectSpace os) {
            HrmPeriodAllocParameter ap = os.CreateObject<HrmPeriodAllocParameter>();
            ap.StatusSet(HrmPeriodAllocParameterStatus.ALLOC_PARAMETERS_ACCEPTED);
            ap.NormNoControlKB = 100;
            ap.NormNoControlOZM = 200;
            foreach(fmCOrder ord in os.GetObjects<fmCOrder>()){
                HrmPeriodOrderControl oc = os.CreateObject<HrmPeriodOrderControl>();
                oc.Order = ord;
                oc.NormKB = 100;
                oc.NormOZM = 200;
                oc.TypeControl = ord.TypeControl;
                oc.AllocParameter = ap;
                ap.OrderControls.Add(oc);
            }
            return ap;
        }

        public static HrmMatrix CreateMatrixFromExcel(IObjectSpace os, MatrixFromExcel mat) {
            HrmMatrix result = os.CreateObject<HrmMatrix>();
            result.Status = HrmMatrixStatus.MATRIX_SAVED;
            result.Type = HrmMatrixType.TYPE_MATIX;
            result.TypeMatrix = HrmMatrixTypeMatrix.MATRIX_RESERVE;
            Dictionary<String, HrmMatrixColumn> created_columns = new Dictionary<string,HrmMatrixColumn>();
            Dictionary<String, fmCOrder> orders_in_database = os.GetObjects<fmCOrder>().ToDictionary(x => x.Code);
            Dictionary<String, Department> departments_in_database = os.GetObjects<Department>().ToDictionary(x => x.Code);
            for (int i = 0; i < mat.NumberOfRows; i++) {
                String row_code = mat.rows_info[i][0];
                HrmMatrixRow current_row = os.CreateObject<HrmMatrixRow>();
                current_row.Matrix = result;
                result.Rows.Add(current_row);
                current_row.Order = orders_in_database[row_code];
                for (int j = 0; j < mat.NumberOfColumns; j++) {
                    if (mat.mat[i, j] != null) {
                        String col_code = mat.columns_info[j][0];
                        HrmMatrixColumn current_column = null;
                        if (created_columns.ContainsKey(col_code)){
                            current_column = created_columns[col_code];
                        }
                        else{
                            current_column = os.CreateObject<HrmMatrixColumn>();
                            current_column.Department = departments_in_database[col_code];
                            current_column.Matrix = result;
                            result.Columns.Add(current_column);
                            created_columns.Add(col_code, current_column);
                        }

                        HrmMatrixCell current_cell = os.CreateObject<HrmMatrixCell>();
                        current_cell.Row = current_row;
                        current_row.Cells.Add(current_cell);
                        current_cell.Column = current_column;
                        current_column.Cells.Add(current_cell);
                        current_cell.PlanMoney = mat.mat[i, j][0];
                        current_cell.MoneyNoReserve = mat.mat[i, j][1];
                        current_cell.MoneyReserve = mat.mat[i, j][2];

                    }
                }
            }
            return result;
        }

        

    }
}
