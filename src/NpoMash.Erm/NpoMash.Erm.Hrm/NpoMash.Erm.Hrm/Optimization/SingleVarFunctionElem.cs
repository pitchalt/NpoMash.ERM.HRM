using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization {
    /// <summary>
    /// Элемент функции, содержащий одну переменную
    /// </summary>
    public abstract class SingleVarFunctionElem {
        private Variable _ElemVar;
        /// <summary>
        /// переменная элемента функции
        /// </summary>
        public Variable ElemVar { get { return _ElemVar; } }

        public abstract double Calculate(double value);
        public abstract double PartialDerivate(double value);

        public SingleVarFunctionElem(Variable vr) {
            _ElemVar = vr;
        }
    }
}
