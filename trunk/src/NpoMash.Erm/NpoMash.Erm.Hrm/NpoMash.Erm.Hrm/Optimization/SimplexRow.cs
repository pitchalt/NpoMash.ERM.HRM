using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization
{
    public class SimplexRow
    {
        private Variable _BasisVar;
        /// <summary>
        /// Базисная переменная
        /// </summary>
        public Variable BasisVar { get { return _BasisVar; } set { _BasisVar = value; } }
        private float _VarValue;
        /// <summary>
        /// Значение
        /// </summary>
        public float VarValue { get { return _VarValue; } set { _VarValue = value; } }
        /// <summary>
        /// Переменные в строке
        /// </summary>
        public Dictionary<SimplexColumn, SimplexCell> Cells;
        public SimplexRow(Equality eq, SimplexTable table)
        {
            Cells = new Dictionary<SimplexColumn, SimplexCell>();
            VarValue = eq.EqualTo;
            table.Rows.Add(this);
        }

    }
}
