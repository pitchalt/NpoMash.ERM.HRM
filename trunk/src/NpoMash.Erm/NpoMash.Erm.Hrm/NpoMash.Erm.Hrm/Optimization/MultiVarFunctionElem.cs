using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization {
    /// <summary>
    /// Элемент функции с несколькими переменными
    /// </summary>
    public abstract class MultiVarFunctionElem {
        /// <summary>
        /// переменные составного элемента функции
        /// </summary>
        public List<Variable> ElemVars;

        private double _Coefficent;
        /// <summary>
        /// Коэффициент
        /// </summary>
        public double Coefficient { get { return _Coefficent; } set { _Coefficent = value; } }

        public abstract double Calculate(ValuesVector values);
        public abstract double PartialDerivate(ValuesVector values, Variable der_variable);

        public MultiVarFunctionElem(List<Variable> vars, double coef) {
            ElemVars = vars;
            _Coefficent = coef;
        }
    }
}
