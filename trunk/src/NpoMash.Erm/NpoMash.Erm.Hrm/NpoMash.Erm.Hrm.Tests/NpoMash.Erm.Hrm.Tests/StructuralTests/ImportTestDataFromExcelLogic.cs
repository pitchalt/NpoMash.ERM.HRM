using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DevExpress;
using DevExpress.Spreadsheet;
using DevExpress.Office;



namespace NpoMash.Erm.Hrm.Tests.StructuralTests {


    static class ImportTestDataFromExcelLogic {

        static MatrixFromExcel GetData(String path_to_file, int itogs_in_row = 1, int itogs_in_col = 1) {
            const String end_string = "Итого";
            //List<CellImportedFormExcel> result = new List<CellImportedFormExcel>();
            Workbook wb = new Workbook();
            wb.LoadDocument(path_to_file, DocumentFormat.Xls);
            Worksheet ws = wb.Worksheets.First();
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
                            List<CellValue> col_info = new List<CellValue>();
                            for (int i = 0; i < data_angle_row; i++)
                                col_info.Add(ws.Cells[i, current_col].Value);
                            List<CellValue> itog_col_info = new List<CellValue>();
                            for (int i = 0; i < itogs_in_col; i++)
                                itog_col_info.Add(ws.Cells[end_row + i, current_col].Value);
                            result.columns_info[index_col] = col_info;
                            result.itog_columns_info[index_col] = col_info;
                        }

                        if (result.rows_info[index_row] == null) {
                            List<CellValue> row_info = new List<CellValue>();
                            for (int i = 0; i < data_angle_col; i++)
                                row_info.Add(ws.Cells[current_row, i].Value);
                            current_col += col_merge_size;
                            List<CellValue> itog_row_info = new List<CellValue>();
                            for (int i = 0; i < itogs_in_row; i++)
                                itog_row_info.Add(ws.Cells[current_row, end_col + i].Value);

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





    }
}
