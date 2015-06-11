using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization {
    public class FunctionWithSingleVar : Function<double> {
        private SingleVarFunctionElem _FunctionElem;
        public SingleVarFunctionElem FunctionElem { get { return _FunctionElem; } }

        public void SetElement(SingleVarFunctionElem elem) {
            _FunctionElem = elem;
            FunctionVariables.Add(elem.ElemVar);
        }

        public override double Calculate(double x) {
            return FunctionElem.Calculate(x);
        }

        public FunctionWithSingleVar(SingleVarFunctionElem elem) {
            _FunctionElem = elem;
        }

    }
}
