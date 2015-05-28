using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization
{
    public class SimplexColumn
    {
        private float _Сoefficient;
        /// <summary>
        /// коэффициент целевой функции для данного столбца
        /// </summary>
        public float Сoefficient { get { return _Сoefficient; } set { _Сoefficient = value; } }
        private Variable _СolumnVar;
        /// <summary>
        /// переменная данного столбца
        /// </summary>
        public Variable СolumnVar { get { return _СolumnVar; } set { _СolumnVar = value; } }
        /// <summary>
        /// Переменные в столбце
        /// </summary>
        public Dictionary<SimplexRow, SimplexCell> Cells;

        private bool _IsFictive;
        /// <summary>
        /// является фиктивной переменной, необходимой для выбора начального опорного плана 
        /// </summary>
        public bool IsFictive { get { return _IsFictive; } set { _IsFictive = value; } }
        public SimplexColumn(SimplexTable table, Variable vr)
        {
            
            table.Columns.Add(vr,this);
            Cells = new Dictionary<SimplexRow, SimplexCell>();
            IsFictive = false;
        }
    }
}
