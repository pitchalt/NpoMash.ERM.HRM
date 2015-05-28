using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization
{
    public class SimplexCell
    {
        private float _CellVal;
        public float CellVal { get { return _CellVal; } set { _CellVal = value; } }

        private SimplexColumn _Column;
        public SimplexColumn Column { get { return _Column; } set { _Column = value; } }

        private SimplexRow _Row;
        public SimplexRow Row { get { return _Row; } set { _Row = value; } }

        public SimplexCell(SimplexColumn column, SimplexRow row)
        {
            Column = column;
            column.Cells.Add(row, this);
            Row = row;
            row.Cells.Add(column, this);
        }

    }
}
