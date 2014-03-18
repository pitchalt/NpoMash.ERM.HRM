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
            const String end_string = "Итого:";
            Worksheet ws = wb.Worksheets.First(x => x.Name == worksheet_name);
            int data_angle_row = 0;
            int data_angle_col = 0;
            int end_row = 0;
            int end_col = 0;
            while (ws.Cells[data_angle_row,0].Value.IsEmpty && data_angle_row <20)
                data_angle_row++;
            while (ws.Cells[0, data_angle_col].Value.IsEmpty && data_angle_col < 20)
                data_angle_col++;
            while (ws.Cells[0,end_col].Value != end_string && end_col<1000)
                end_col++;
            while (ws.Cells[end_row,0].Value != end_string && end_row<1000)
                end_row++;
            if (data_angle_row == 20 || data_angle_col == 20 ||
                end_col == 1000 || end_row == 1000)
                throw new Exception("The input file is invalid");
            int current_row = data_angle_row;
            int current_col = data_angle_col;
            /*throw new Exception("Contents[0,2]: " + ws.Cells[0, 2].Value.ToString()+
            "Contents[0,3]: " + ws.Cells[0, 3].Value.ToString()+
            "Contents[0,4]: " + ws.Cells[0, 4].Value.ToString()+
            "Contents[0,5]: " + ws.Cells[0, 5].Value.ToString()+
            "Contents[0,6]: " + ws.Cells[0, 6].Value.ToString()
            );*/
            //throw new Exception("Angle row: " + data_angle_row.ToString() +
            //    "angle col: " + data_angle_col.ToString());
            MatrixFromExcel result = new MatrixFromExcel((end_row - data_angle_row)/2, (end_col - data_angle_col)/2);
            int index_row = 0;
            while (ws.Cells[current_row, 0].Value != end_string) {
                int row_merge_size = 2; // ws.Cells[current_row, 0].BottomRowIndex - ws.Cells[current_row, 0].TopRowIndex + 1;//.GetMergedRanges().Count();
                //if(row_merge_size != 1)
                    //throw new Exception("Row_merge_size = " + row_merge_size.ToString());
                int index_col = 0;
                current_col = data_angle_col;
                while (ws.Cells[0, current_col].Value != end_string) {
                    int col_merge_size = 2;// ws.Cells[0, current_col].ColumnCount;//.GetMergedRanges().Count();
                    List<Decimal> cell_info = new List<Decimal>();
                    int current_cell_col = current_col;
                    int current_cell_row = current_row;
                    bool is_empty_cells = false;
                    bool all_cells_are_empty = true;
                    while (current_cell_row < current_row + row_merge_size && !is_empty_cells) {
                        current_cell_col = current_col;
                        while (current_cell_col < current_col + col_merge_size && !is_empty_cells) {
                            CellValue cv = ws.Cells[current_cell_row, current_cell_col].Value;
                            if (cv.IsEmpty)
                                is_empty_cells = true;
                            else {
                                cell_info.Add((decimal)cv.NumericValue);
                                all_cells_are_empty = false;
                            }
                            current_cell_col++;
                        }
                        current_cell_row++;
                    }
                    if (!all_cells_are_empty) {
                        //try {
                            
                        /*}
                        catch (Exception) {
                            throw new Exception(result.NumberOfColumns.ToString() +
                                " is max index of this massiv, but was index = " + 
                                index_col.ToString() +
                                "and current_col = " +
                                ws.Cells[0,current_col].Value.TextValue +
                                "("+current_col.ToString()+") "+
                                "and current_row = " +
                                ws.Cells[current_row,0].Value.TextValue +
                                "("+current_row.ToString()+") "
                                );
                        }*/

                        
                        result.mat[index_row, index_col] = cell_info;
                    }

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
                    current_col += col_merge_size;
                    index_col++;
                }

                if (result.rows_info[index_row] == null) {
                    List<String> row_info = new List<String>();
                    for (int i = 0; i < data_angle_col; i++)
                        row_info.Add(ws.Cells[current_row, i].Value.TextValue);

                    List<Decimal> itog_row_info = new List<Decimal>();
                    for (int i = 0; i < itogs_in_row; i++)
                        itog_row_info.Add((Decimal)ws.Cells[current_row, end_col + i].Value.NumericValue);
                    result.rows_info[index_row] = row_info;
                    result.itog_rows_info[index_row] = itog_row_info;
                }

                current_row += row_merge_size;
                index_row++;
            }
            return result;
        }


        public static void CreateDepartmentsFromExcelTab(IObjectSpace os, MatrixFromExcel mat) {
            int i = 0;
            String created_deps = "";
            //throw new Exception("Number of rows: " + mat.NumberOfRows.ToString() +
            //    " number of columns: " + mat.NumberOfColumns.ToString());
            foreach (List<String> str_info in mat.columns_info) {
                try {
                    String str = str_info[0];
                    i++;
                    created_deps += "<" + str_info[0] + ">";
                    Department dep = os.CreateObject<Department>();
                    dep.BuhCode = str;
                    str = str_info[1];
                    switch (str) {
                    case "КБ": {
                            dep.GroupDep = DepartmentGroupDep.DEPARTMENT_KB;
                            break;
                        }
                    case "ОЗМ": {
                            dep.GroupDep = DepartmentGroupDep.DEPARTMENT_OZM;
                            break;
                        }
                    default: {
                            throw new Exception("Invalid group_dep from excel");
                        }
                    }
                }
                catch (NullReferenceException) {
                    throw new Exception("Error in iteration " + i.ToString() +
                    "read deps: " + created_deps);
                }
            }
        }

        public static void CreateOrdersFromExcelTab(IObjectSpace os, MatrixFromExcel mat) {
            String ex_str = "";
                int iter = 0;
                try {
            foreach (List<String> str_info in mat.rows_info) {
                
                    String str = str_info[0];
                    ex_str += "<" + str + ">";
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
                    iter++;
                }
                
            }
                catch (NullReferenceException) {
                    throw new Exception("Read orders:" + ex_str
                        + " number or iter = " + iter.ToString());
                }
        }

        public static HrmPeriodAllocParameter CreateAllocParametersFromExcelTab(IObjectSpace os) {
            HrmPeriodAllocParameter ap = os.CreateObject<HrmPeriodAllocParameter>();
            ap.StatusSet(HrmPeriodAllocParameterStatus.ALLOC_PARAMETERS_ACCEPTED);
            ap.NormNoControlKB = 100;
            ap.NormNoControlOZM = 200;
            foreach(fmCOrder ord in os.GetObjects<fmCOrder>(null,true)){
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
            Dictionary<String, fmCOrder> orders_in_database = os.GetObjects<fmCOrder>(null, true).ToDictionary(x => x.Code);
            Dictionary<String, Department> departments_in_database = os.GetObjects<Department>(null, true).ToDictionary(x => x.BuhCode);
            for (int i = 0; i < mat.NumberOfRows; i++) {
                String row_code = mat.rows_info[i][0];
                HrmMatrixRow current_row = os.CreateObject<HrmMatrixRow>();
                current_row.Matrix = result;
                result.Rows.Add(current_row);
                try {
                    current_row.Order = orders_in_database[row_code];
                }
                catch (KeyNotFoundException){
                    throw new Exception("The key which wasn't found: " + row_code);
                }
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
                        try {
                            current_cell.PlanMoney = mat.mat[i, j][0];
                            current_cell.MoneyNoReserve = mat.mat[i, j][1];
                            current_cell.MoneyReserve = mat.mat[i, j][2];
                        }
                        catch (Exception) {
                            String values = "";
                            foreach (int val in mat.mat[i, j]) {
                                values += "<" + val.ToString() + ">";
                            }
                            throw new Exception("Error in cell [" + i.ToString() + "," + j.ToString() + "], " +
                                "number of values in this cell: " + mat.mat[i, j].Count.ToString() +
                            " and values there are: " + values);
                        }

                    }
                }
            }
            return result;
        }

        

    }
}
