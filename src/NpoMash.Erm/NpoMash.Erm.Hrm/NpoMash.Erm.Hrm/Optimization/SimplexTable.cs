using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization
{
    /// <summary>
    /// Симплекс-таблица, с которой работает симплекс-метод
    /// </summary>
    public class SimplexTable
    {
        public List<SimplexRow> Rows;
        public Dictionary<Variable,SimplexColumn> Columns;
        
        public SimplexTable(OptimCriteria criteria, List<Equality> restrictions)
        {
            Columns = new Dictionary<Variable, SimplexColumn>();
            Rows = new List<SimplexRow>();
            // для каждого ограничения из списка создается строка
            foreach (Equality eq in restrictions)
            {
                SimplexRow row = new SimplexRow(eq,this);
                // для каждой переменной в ограничении
                foreach (Variable var_col in eq.Variables)
                {
                    SimplexColumn col = null;
                    // если столбца с данной переменной еще нет, то он создается
                    if (!Columns.ContainsKey(var_col))
                        col = new SimplexColumn(this,var_col);
                    else col = Columns[var_col];

                    // создание ячейки
                    SimplexCell cell = new SimplexCell(col,row);
                    cell.CellVal = eq.SimpleElements[var_col].Coefficient;
                }
            }


        }
    }
}
