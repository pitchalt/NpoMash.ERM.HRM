using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization {
    public class SimplexMethod : MultiDimOptim {
        private SimplexTable _Table;
        public SimplexTable Table { get { return _Table; } set { _Table = value; } }
        /// <summary>
        /// Ищет опорные столбец и строку, и вводит соответствующую переменную в базис
        /// </summary>
        public override void NextIteration() {
            base.NextIteration();
            SimplexColumn basis_col = Table.Columns.Values.Where(x=> !x.IsFictive && x.Delta > 0 && x.Cells.Values.Count(y=>y.CellVal > 0) >0)
                .OrderByDescending(x=> x.Delta).First();
            SimplexRow basis_row = null;
            // в первую очередь из базиса исключаются фиктивные переменные (если возможно)
            if (basis_col.Cells.Values.Count(x => x.CellVal > 0 && Table.Columns[x.Row.BasisVar].IsFictive) > 0) {
                basis_row = basis_col.Cells.Values.Where(x => x.CellVal > 0 && Table.Columns[x.Row.BasisVar].IsFictive)
                    .OrderBy(x => x.Row.VarValue / x.CellVal).First().Row;
            }
            else {
                basis_row = basis_col.Cells.Values.Where(x => x.CellVal > 0)
                    .OrderBy(x=> x.Row.VarValue / x.CellVal).First().Row;
            }
            Table.IntoBasis(basis_col, basis_row);
            foreach (Variable vr in new List<Variable>(CurrentState.Keys)) CurrentState[vr] = 0;
            foreach (SimplexRow row in Table.Rows) {
                if (CurrentState.ContainsKey(row.BasisVar))
                    CurrentState[row.BasisVar] = row.VarValue;
            }
        }

        public SimplexMethod(double prec, LinearFunction fun, List<Equality> restrictions)
            : base(prec) {
                CurrentState = new ValuesVector();
                foreach (Variable vr in fun.FunctionVariables)
                    CurrentState.Add(vr, 0);
                foreach (Equality eq in restrictions) 
                    foreach (Variable vr in eq.FunctionVariables) 
                        if (!CurrentState.ContainsKey(vr))
                            CurrentState.Add(vr, 0);
                Table = new SimplexTable(fun, restrictions);
                Restrictions = restrictions;
                StopCriterias.Add(new StopCriteriaSimplexTable(prec, Table));
        }
    }
}
