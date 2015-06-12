using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NpoMash.Erm.Hrm;
using NpoMash.Erm.Hrm.Salary;
using NpoMash.Erm.Hrm.Optimization;
using IntecoAG.ERM.FM.Order;


namespace NpoMash.Erm.Hrm.Simplex {
    public class ReserveMatrixOptimizationAdapter : SourceDataAdapter {
        private HrmSalaryTaskProvisionMatrixReduction _Card;
        private double _CellsPriority;
        private double _OrdersPriority;
        private double _Precision;
        private Dictionary<HrmMatrixCell,Variable> _ControlledReserveInCells;
        private Dictionary<HrmMatrixColumn,Variable> _UncontroledReserveInDepartment;
        private Dictionary<fmCOrder, bool> _OrderTypeControl;
        private Dictionary<HrmMatrixRow, bool> _RowTypeControl;
        private ValuesVector _CurrentState;
        private FunctionWithMultiVarElements _OptimCriteria;
        private List<Equality> _Restrictions;



        /// <summary>
        /// Создание необходимых структур данных
        /// </summary>
        protected override void GetData() {
            _ControlledReserveInCells = new Dictionary<HrmMatrixCell,Variable>();
            _UncontroledReserveInDepartment = new Dictionary<HrmMatrixColumn, Variable>();
            _OrderTypeControl = new Dictionary<fmCOrder,bool>();
            _RowTypeControl = new Dictionary<HrmMatrixRow,bool>();
            _CurrentState = new ValuesVector();
            foreach(HrmAllocParameterOrderControl ord in _Card.AllocParameters.OrderControls){
                _OrderTypeControl.Add(ord.Order,ord.TypeControl == FmCOrderTypeControl.TRUDEMK_FOT || ord.TypeControl == FmCOrderTypeControl.FOT);
            }
            foreach (HrmMatrixRow row in _Card.ReserveMatrixSimplex.Rows)
                _RowTypeControl.Add(row, _OrderTypeControl[row.Order]);
            foreach (HrmMatrixColumn col in _Card.ReserveMatrixSimplex.Columns) {
                if (col.Cells.Count(x => !_RowTypeControl[x.Row]) > 0) {
                    Variable vr = new Variable();
                    _UncontroledReserveInDepartment.Add(col, vr);
                    _CurrentState.Add(vr, 0);
                }
                foreach (HrmMatrixCell cell in col.Cells.Where(x => _RowTypeControl[x.Row])) {
                    Variable vr = new Variable(); 
                    _ControlledReserveInCells.Add(cell, vr);
                    _CurrentState.Add(vr, 0);
                }
            }
            // Начальная точка, удовлетворяющая системе ограничений
            foreach (HrmMatrixColumn col in _Card.ReserveMatrixSimplex.Columns) {
                HrmMatrixCell cell = col.Cells.First(x => _RowTypeControl[x.Row]);
                _CurrentState[_ControlledReserveInCells[cell]] = (double)col.Cells.Sum(x => x.SourceProvision);
            }


        }

        /// <summary>
        /// Создание критерия оптимизации
        /// </summary>
        protected override void CreateCriteria() {
            _OptimCriteria = new FunctionWithMultiVarElements();
            foreach (HrmMatrixCell cell in _ControlledReserveInCells.Keys) {
                _OptimCriteria.AddElement(
                    new ReserveMatrixCriteriaElemCells(_ControlledReserveInCells[cell],
                        _CellsPriority / (double)cell.PlanMoney,
                        (double)(cell.PlanMoney - cell.MoneyNoReserve)));
            }
            foreach (HrmMatrixRow row in _Card.ReserveMatrixSimplex.Rows.Where(x => _RowTypeControl[x])) {
                List<Variable> vars_list = new List<Variable>();
                double base_part = 0;
                double plan_part = 0;
                foreach (HrmMatrixCell cell in row.Cells) {
                    vars_list.Add(_ControlledReserveInCells[cell]);
                    base_part += (double)cell.MoneyNoReserve;
                    plan_part += (double)cell.PlanMoney;
                }
                _OptimCriteria.AddElement(
                    new ReserveMatrixCriteriaElemOrds(vars_list,
                        _OrdersPriority / plan_part, plan_part - base_part));                
            }
        }

        /// <summary>
        /// Создание системы ограничений
        /// </summary>
        protected override void CreateRestrictions() {
            _Restrictions = new List<Equality>();
            foreach (HrmMatrixColumn col in _Card.ReserveMatrixSimplex.Columns) {
                Equality eq = new Equality();
                foreach (HrmMatrixCell cell in col.Cells.Where(x => _RowTypeControl[x.Row])) {
                    eq.AddElement(new SimpleFunctionElem(_ControlledReserveInCells[cell], 1));
                }
                if (_UncontroledReserveInDepartment.ContainsKey(col))
                    eq.AddElement(new SimpleFunctionElem(_UncontroledReserveInDepartment[col], 1));
                eq.EqualTo = (double)col.Cells.Sum(x => x.SourceProvision);
            }

        }

        /// <summary>
        /// Выполнение оптимизации
        /// </summary>
        protected override void RunOptimisation() {
            FrankWulfMethod fwm = new FrankWulfMethod(_Precision, _OptimCriteria, _Restrictions, _CurrentState);
            _CurrentState = fwm.Optimize();
        }

        /// <summary>
        /// Выполнение дополнительных действий после оптимизации:
        /// приведение данных к целому типу
        /// </summary>
        protected override void AfterOptimisation() {
            foreach (Variable vr in _CurrentState.Keys.ToList())
                _CurrentState[vr] = Math.Round(_CurrentState[vr]);
            foreach (Equality eq in _Restrictions) {
                double diff = 0;
                do{
                    diff = eq.Calculate(_CurrentState) - eq.EqualTo;
                    if (diff > 0) {
                        _CurrentState[eq.FunctionVariables
                            .Where(x => _CurrentState[x] >= 1)
                            .OrderByDescending(x => _OptimCriteria.PartialDerivate(x, _CurrentState)).First()]--;
                    }
                    if (diff < 0) {
                        _CurrentState[eq.FunctionVariables
                        .OrderBy(x => _OptimCriteria.PartialDerivate(x, _CurrentState)).First()]++;
                    }
                }while(diff != 0);
            }
        }

        /// <summary>
        /// Запись результатов в матрицу резерва и распределение резерва неконтролируемых заказов
        /// </summary>
        protected override void ReturnData() {
            foreach (HrmMatrixCell cell in _ControlledReserveInCells.Keys.ToList()) {
                cell.NewProvision = (Decimal)Math.Round(_CurrentState[_ControlledReserveInCells[cell]]);
            }
            foreach (HrmMatrixColumn col in _UncontroledReserveInDepartment.Keys) {
                decimal res = Math.Round((decimal)_CurrentState[_UncontroledReserveInDepartment[col]]);
                List<HrmMatrixCell> uncontrolled_cells = col.Cells.Where(x => !_RowTypeControl[x.Row]).ToList();
                int remaining_cells = uncontrolled_cells.Count();
                foreach (HrmMatrixCell cell in uncontrolled_cells) {
                    decimal to_take = Math.Round(res / remaining_cells);
                    cell.NewProvision = to_take;
                    res -= to_take;
                    remaining_cells--;
                }
            }
        }

        public ReserveMatrixOptimizationAdapter(HrmSalaryTaskProvisionMatrixReduction card, double cells_priority, double orders_priority, double precision ) {
            _Card = card;
            _CellsPriority = cells_priority;
            _OrdersPriority = orders_priority;
            _Precision = precision;
        }
    }
}
