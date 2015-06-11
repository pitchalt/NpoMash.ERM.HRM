using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization {
    /// <summary>
    /// Функция, содержащая элементы с несколькими переменными
    /// </summary>
    public class FunctionWithMultiVarElements : FunctionWithSingleVarElements {
        public List<MultiVarFunctionElem> MultiVarElems;
        public Dictionary<Variable, List<MultiVarFunctionElem>> MultiVarElemsByVariables;

        public override double PartialDerivate(Variable var, ValuesVector values) {
            double result = base.PartialDerivate(var, values);
            foreach (MultiVarFunctionElem elem in MultiVarElemsByVariables[var]) result += elem.PartialDerivate(values, var);
            return result;
        }

        public override double Calculate(ValuesVector values) {
            double result = base.Calculate(values);
            foreach (MultiVarFunctionElem elem in MultiVarElems) result += elem.Calculate(values);
            return result;
        }

        public FunctionWithMultiVarElements() {
            MultiVarElems = new List<MultiVarFunctionElem>();
            MultiVarElemsByVariables = new Dictionary<Variable, List<MultiVarFunctionElem>>();
        }

        public void AddElement(MultiVarFunctionElem elem) {
            MultiVarElems.Add(elem);
            foreach (Variable vr in elem.ElemVars) {
                if (!MultiVarElemsByVariables.ContainsKey(vr))
                    MultiVarElemsByVariables.Add(vr, new List<MultiVarFunctionElem>());
                MultiVarElemsByVariables[vr].Add(elem);
                if (!FunctionVariables.Contains(vr))
                    FunctionVariables.Add(vr);
            }
        }

    }
}
