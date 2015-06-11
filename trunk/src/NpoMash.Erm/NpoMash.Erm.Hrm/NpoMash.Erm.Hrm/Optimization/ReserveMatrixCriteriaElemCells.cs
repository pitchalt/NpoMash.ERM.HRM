using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization {
    /// <summary>
    /// Элемент функции критерия матрицы резерва для представления поячеечного отклонения
    /// </summary>
    public class ReserveMatrixCriteriaElemCells : SingleVarFunctionElem {

        private double _Coefficient;
        ///// <summary>
        ///// Коэффициент при элементе функции
        ///// </summary>
        public double Coefficient { get { return _Coefficient; } }

        private double _FreePart;
        /// <summary>
        /// Свободный член
        /// </summary>
        public double FreePart { get { return _FreePart; } }

        public override double Calculate(double value) {
            double x = FreePart - value;
            return Coefficient * x * x;
        }

        public override double PartialDerivate(double value) {
            return 2 * (value - FreePart) * Coefficient;
        }

        public ReserveMatrixCriteriaElemCells(Variable vr, double coef, double free_prt)
            : base(vr) {
            _FreePart = free_prt;
            _Coefficient = coef;
        }

    }
}
