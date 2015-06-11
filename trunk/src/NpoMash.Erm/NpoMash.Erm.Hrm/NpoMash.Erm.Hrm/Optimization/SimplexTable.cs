using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization {
    /// <summary>
    /// Симплекс-таблица, с которой работает симплекс-метод
    /// </summary>
    public class SimplexTable {
        public List<SimplexRow> Rows;
        public Dictionary<Variable, SimplexColumn> Columns;

        public SimplexTable(LinearFunction criteria, List<Equality> restrictions) {
            Columns = new Dictionary<Variable, SimplexColumn>();
            Rows = new List<SimplexRow>();
            // для каждого ограничения из списка создается строка
            foreach (Equality eq in restrictions) {
                SimplexRow row = new SimplexRow(eq, this);
                // для каждой переменной в ограничении
                foreach (Variable var_col in eq.FunctionVariables) {
                    SimplexColumn col = null;
                    // если столбца с данной переменной еще нет, то он создается
                    if (!Columns.ContainsKey(var_col)) {
                        double coef = 0;
                        if (criteria.SimpleElements.ContainsKey(var_col))
                            coef = criteria.SimpleElements[var_col].Coefficient;
                        col = new SimplexColumn(this, var_col, coef, false);
                    }
                    else col = Columns[var_col];
                    // создание ячейки
                    SimplexCell cell = new SimplexCell(col, row, eq.SimpleElements[var_col].Coefficient);
                }
            }
            // выбор начального опорного плана
            foreach (SimplexRow row in Rows) {
                SimplexCell basis_cell = null;
                try {
                    basis_cell = row.Cells.Values.Where(x => x.CellVal == 1 &&
                    x.Column.Cells.Values.Count(y => y != x && y.CellVal != 0) == 0).First();
                }
                catch (InvalidOperationException) { }
                if (basis_cell != null) {
                    row.BasisVar = basis_cell.Column.ColumnVar;
                }
                else {
                    AddFictiveVar(row);
                }
            }
            // создание нулевых ячеек
            foreach (SimplexRow row in Rows) {
                foreach (SimplexColumn col in Columns.Values) {
                    if (!row.Cells.ContainsKey(col)) {
                        new SimplexCell(col, row, 0);
                    }
                }
            }
            UpdateDeltas();
        }

        /// <summary>
        /// Добавляет в таблицу фиктивную переменную для формирования начального опорного плана
        /// </summary>
        /// <param name="row">Строка симплексной таблицы</param>
        public void AddFictiveVar(SimplexRow row) {
            Variable fict_var = new Variable();
            row.BasisVar = fict_var;
            SimplexColumn col = new SimplexColumn(this, fict_var, 0, true);
            new SimplexCell(col, row, 1);
        }
        /// <summary>
        /// Убрать фиктивную переменную из таблицы
        /// </summary>
        /// <param name="col">Столбец с фиктивной переменной</param>
        public void RemoveFictiveVar(SimplexColumn col) {
            Columns.Remove(col.ColumnVar);
            foreach (SimplexCell cell in col.Cells.Values)
                cell.Row.Cells.Remove(col);
        }

        public void UpdateDeltas() {
            foreach (SimplexColumn col in Columns.Values)
                col.UpdateDelta();
        }

        /// <summary>
        /// Замена критерия в симплексной таблице
        /// </summary>
        /// <param name="crit">Новый линейный критерий</param>
        public void ReplaceCriteria(LinearFunction crit) {
            foreach (SimplexColumn col in Columns.Values) {
                Variable vr = col.ColumnVar;
                if (crit.SimpleElements.ContainsKey(vr))
                    col.Coefficient = crit.SimpleElements[vr].Coefficient;
                else col.Coefficient = 0;
            }
            UpdateDeltas();
        }
        /// <summary>
        /// Введение переменной в базис
        /// </summary>
        /// <param name="basis_col">Опорный столбец</param>
        /// <param name="basis_row">Опорная строка</param>
        public void IntoBasis(SimplexColumn basis_col, SimplexRow basis_row) {
            // если заменяется фиктивная переменная - удалить столбец с этой переменной
            if (Columns[basis_row.BasisVar].IsFictive)
                RemoveFictiveVar(Columns[basis_row.BasisVar]);
            basis_row.BasisVar = basis_col.ColumnVar;
            double coef = basis_col.Cells[basis_row].CellVal;
            // если коэффициент не равен единице, то осуществить деление опорной строки
            if (coef != 1) {
                foreach (SimplexCell cell in basis_row.Cells.Values)
                    cell.CellVal /= coef;
                basis_row.VarValue /= coef;
            }
            // на пересечении базисной строки и базисного столбца - единица
            basis_row.Cells[basis_col].CellVal = 1;
            // пересчитывается каждая строка таблицы по правилу треугольника (если необходимо)
            foreach (SimplexRow row in Rows.Where(x => x != basis_row && x.Cells[basis_col].CellVal != 0)) {
                double row_coef = row.Cells[basis_col].CellVal;
                row.Cells[basis_col].CellVal = 0;
                row.VarValue -= row_coef*basis_row.VarValue;
                foreach (SimplexCell cell in row.Cells.Values.Where(x=> x.Column != basis_col && x.Column.Cells[basis_row].CellVal != 0)) {
                    cell.CellVal -= row_coef * cell.Column.Cells[basis_row].CellVal;
                }

            }

        //    // по правилу треугольника обновляются ячейки всей остальной таблицы, где необходимо
        //    foreach (SimplexColumn column in Columns.Values.Where(x => x.Cells[basis_row].CellVal != 0 && x != basis_col)) {
        //        foreach (SimplexCell cell in column.Cells.Values.Where(x => x.Row != basis_row && x.Row.Cells[basis_col].CellVal != 0))
        //            cell.CellVal -= basis_col.Cells[cell.Row].CellVal * column.Cells[basis_row].CellVal;
        //    }
        //    foreach (SimplexRow row in Rows.Where(x => x != basis_row)) {
        //        row.VarValue -= basis_col.Cells[row].CellVal * basis_row.VarValue;
        //    }

        //    // столбец с базисной переменной - единичный
        //    foreach(SimplexCell cell in basis_col.Cells.Values.Where(x=> x != basis_col.Cells[basis_row]))
        //        cell.CellVal = 0;
            UpdateDeltas();
        }
    }
}
