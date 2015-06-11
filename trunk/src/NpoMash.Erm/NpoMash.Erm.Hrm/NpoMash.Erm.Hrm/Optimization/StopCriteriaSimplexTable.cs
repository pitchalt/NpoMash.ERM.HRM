using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization {
    /// <summary>
    /// Критерий останова, используемый в методе симплекса
    /// </summary>
    public class StopCriteriaSimplexTable : StopCriteria {
        /// <summary>
        /// Симплексная таблица
        /// </summary>
        private SimplexTable _Table;

        public override bool Check() {
            // есть возможность дальнейшей оптимизации
            bool has_cols_to_optimize = (_Table.Columns.Values.Count(x => x.Delta > 0 && x.Cells.Values.Count(y=> y.CellVal > 0) > 0) > 0);
            // если дальнейшая оптимизация невозможна, проверить на наличие фиктивных переменных
            if (!has_cols_to_optimize && (_Table.Columns.Values.Count(x => x.IsFictive) > 0 || _Table.Columns.Values.Count(x=> x.Delta > 0 ) > 0 ))
                throw new InvalidOperationException("Данная задача не имеет решения симплекс-методом");
            return !has_cols_to_optimize;

        }
        public override void UpdateState() { }

        public StopCriteriaSimplexTable(double prec, SimplexTable tab)
            : base(prec) {
                _Table = tab;
        }

    }
}
