using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DevExpress;
using DevExpress.Spreadsheet;
using DevExpress.Office;



namespace NpoMash.Erm.Hrm.Tests.StructuralTests {
    class MatrixFromExcel {
        public List<Decimal>[,] mat;
        public List<String>[] rows_info;
        public List<String>[] columns_info;
        public List<Decimal>[] itog_columns_info;
        public List<Decimal>[] itog_rows_info;
        private int _NumberOfRows;
        public int NumberOfRows { get { return _NumberOfRows; } }
        private int _NumberOfColumns;
        public int NumberOfColumns { get { return _NumberOfColumns; } }

        public MatrixFromExcel(int rows_number, int columns_number) {
            _NumberOfColumns = columns_number;
            _NumberOfRows = rows_number;
            mat = new List<decimal>[rows_number, columns_number];
            rows_info = new List<String>[rows_number];
            columns_info = new List<String>[columns_number];
            itog_columns_info = new List<Decimal>[columns_number];
            itog_rows_info = new List<Decimal>[rows_number];
        }

    }

}
