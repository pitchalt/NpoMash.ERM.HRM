using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization {
    /// <summary>
    /// Элемент функции критерия матрицы резерва для представления позаказного отклонения
    /// </summary>
    public class ReserveMatrixCriteriaElemOrds : MultiVarFunctionElem {

        private double _FreePart;
        /// <summary>
        /// Свободный член
        /// </summary>
        public double FreePart { get { return _FreePart; } }

        public override double Calculate(ValuesVector values) {
            double sum = 0;
            foreach (Variable vr in ElemVars) sum += values[vr];
            double x = FreePart - sum;
            return Coefficient * x * x;
        }

        public override double PartialDerivate(ValuesVector values, Variable der_variable) {
            double sum = 0;
            foreach (Variable vr in ElemVars) sum += values[vr];
            return 2 * Coefficient * (sum - FreePart);
        }

        public ReserveMatrixCriteriaElemOrds(List<Variable> vars, double coef, double free_prt)
            : base(vars, coef) {
            _FreePart = free_prt;
        }
    }
}
