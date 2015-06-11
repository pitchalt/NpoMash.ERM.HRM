using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization {
    /// <summary>
    /// Представляет простейший элемент функции - переменная с коэффициентом
    /// </summary>
    public class SimpleFunctionElem : SingleVarFunctionElem {
        private double _Coefficient;
        public double Coefficient { get { return _Coefficient; } }

        public override double Calculate(double value) {
            return value * Coefficient;
        }

        public override double PartialDerivate(double value) {
            return Coefficient;
        }

        public SimpleFunctionElem(Variable vr, double coef)
            : base(vr) {
            _Coefficient = coef;
        }

    }
}
