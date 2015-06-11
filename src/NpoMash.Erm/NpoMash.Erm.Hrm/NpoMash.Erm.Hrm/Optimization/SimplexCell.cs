using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization {
    public class SimplexCell {
        private double _CellVal;
        /// <summary>
        /// Значение в ячейке
        /// </summary>
        public double CellVal { get { return _CellVal; } set { _CellVal = value; } }

        private SimplexColumn _Column;
        /// <summary>
        /// Столбец таблицы
        /// </summary>
        public SimplexColumn Column { get { return _Column; } set { _Column = value; } }

        private SimplexRow _Row;
        /// <summary>
        /// Строка таблицы
        /// </summary>
        public SimplexRow Row { get { return _Row; } set { _Row = value; } }

        public SimplexCell(SimplexColumn column, SimplexRow row, double val) {
            Column = column;
            column.Cells.Add(row, this);
            Row = row;
            row.Cells.Add(column, this);
            CellVal = val;
        }

    }
}
