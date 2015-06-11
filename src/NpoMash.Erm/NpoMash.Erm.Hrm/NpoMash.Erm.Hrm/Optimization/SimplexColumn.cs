using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization {
    public class SimplexColumn {
        private SimplexTable _Table;
        public SimplexTable Table { get { return _Table; } }
        private double _Сoefficient;
        /// <summary>
        /// коэффициент целевой функции для данного столбца
        /// </summary>
        public double Coefficient { get { return _Сoefficient; } set { _Сoefficient = value; } }
        private Variable _СolumnVar;
        /// <summary>
        /// переменная данного столбца
        /// </summary>
        public Variable ColumnVar { get { return _СolumnVar; } set { _СolumnVar = value; } }
        /// <summary>
        /// Переменные в столбце
        /// </summary>
        public Dictionary<SimplexRow, SimplexCell> Cells;

        private bool _IsFictive;
        /// <summary>
        /// является фиктивной переменной, необходимой для выбора начального опорного плана 
        /// </summary>
        public bool IsFictive { get { return _IsFictive; } }

        private double _Delta;
        /// <summary>
        /// Дельта для выбора опорного столбца
        /// </summary>
        public double Delta { get { return _Delta; } }
        public SimplexColumn(SimplexTable table, Variable vr, double coef, bool is_fictive) {
            Cells = new Dictionary<SimplexRow, SimplexCell>();
            _Table = table;
            table.Columns.Add(vr, this);
            _СolumnVar = vr;
            _Сoefficient = coef;
            _IsFictive = is_fictive;
        }
        public void UpdateDelta() {
            double result = 0;
            foreach (SimplexCell cell in Cells.Values) result += cell.CellVal * Table.Columns[cell.Row.BasisVar].Coefficient;
            result -= Coefficient;
            _Delta = result;
        }
    }
}
