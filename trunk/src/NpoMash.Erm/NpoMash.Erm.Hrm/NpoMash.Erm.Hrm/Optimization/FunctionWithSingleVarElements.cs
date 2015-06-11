using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization {
    public class FunctionWithSingleVarElements : Function<ValuesVector> {
        /// <summary>
        /// Элементы функции с одной переменной
        /// </summary>
        public Dictionary<Variable, SingleVarFunctionElem> SingleVarElems;

        public LinearFunction ToLinearView(ValuesVector values) {
            LinearFunction result = new LinearFunction();
            foreach (Variable vr in FunctionVariables) {
                SimpleFunctionElem elem = new SimpleFunctionElem(vr, PartialDerivate(vr, values));
                result.FunctionVariables.Add(vr);
                result.SimpleElements.Add(vr, elem);
            }
            return result;
        }

        /// <summary>
        /// Возвращает градиент функции в данной точке
        /// </summary>
        /// <param name="values">Точка, в которой производится расчет</param>
        /// <returns></returns>
        public ValuesVector Gradient(ValuesVector values) {
            ValuesVector result = new ValuesVector();
            foreach (Variable var in FunctionVariables) result.Add(var, PartialDerivate(var, values));
            return result;
        }

        public virtual double PartialDerivate(Variable var, ValuesVector values) {
            return SingleVarElems[var].PartialDerivate(values[var]);
        }

        public override double Calculate(ValuesVector values) {
            double result = 0;
            foreach (Variable vr in FunctionVariables) result += SingleVarElems[vr].Calculate(values[vr]);
            return result;
        }

        public FunctionWithSingleVarElements() {
            SingleVarElems = new Dictionary<Variable, SingleVarFunctionElem>();
        }

        public void AddElement(SingleVarFunctionElem elem) {
            if (SingleVarElems.ContainsKey(elem.ElemVar))
                throw new InvalidOperationException("В функции уже содержится элемент с такой переменной!");
            SingleVarElems.Add(elem.ElemVar, elem);
            if (!FunctionVariables.Contains(elem.ElemVar))
                FunctionVariables.Add(elem.ElemVar);
        }

    }
}
