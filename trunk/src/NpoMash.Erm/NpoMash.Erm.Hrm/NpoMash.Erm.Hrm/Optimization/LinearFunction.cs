using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization {
    public class LinearFunction : Function<ValuesVector> {
        public Dictionary<Variable, SimpleFunctionElem> SimpleElements;
        public override double Calculate(ValuesVector values) {
            double result = 0;
            foreach (Variable vr in FunctionVariables) result += SimpleElements[vr].Calculate(values[vr]);
            return result;
        }

        public void AddElement(SimpleFunctionElem elem) {
            if (SimpleElements.ContainsKey(elem.ElemVar))
                throw new InvalidOperationException("Элемент с такой переменной в этой линейной функции уже есть!");
            SimpleElements.Add(elem.ElemVar, elem);
            FunctionVariables.Add(elem.ElemVar);
        }

        public LinearFunction() {
            SimpleElements = new Dictionary<Variable, SimpleFunctionElem>();
        }

    }
}
