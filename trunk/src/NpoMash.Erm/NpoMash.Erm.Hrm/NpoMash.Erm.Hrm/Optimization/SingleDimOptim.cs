using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization {
    public abstract class SingleDimOptim : Optimization<double> {
        /// <summary>
        /// Текущая левая граница
        /// </summary>
        protected double _LeftBorder;
        /// <summary>
        /// Текущая правая граница
        /// </summary>
        protected double _RightBorder;
        private FunctionWithSingleVar _OptimCriteria;
        /// <summary>
        /// Критерий оптимальности решения (целевая функция)
        /// </summary>
        public FunctionWithSingleVar OptimCriteria { get { return _OptimCriteria; } set { _OptimCriteria = value; } }
        //private Inequality _Borders;
        ///// <summary>
        ///// Отрезок, на котором ведется поиск
        ///// </summary>
        //public Inequality Borders { get { return _Borders; } set { _Borders = value; } }

        public SingleDimOptim(double prec, FunctionWithSingleVar opt, double left_border, double right_border)
            : base(prec) {
            StopCriterias.Add(new StopCriteriaSingleStepLength(prec / 2, this));
            OptimCriteria = opt;
            if (left_border > right_border) throw new InvalidOperationException("Левая граница должна быть меньше правой!");
            _LeftBorder = left_border;
            _RightBorder = right_border;
        }
    }
}
