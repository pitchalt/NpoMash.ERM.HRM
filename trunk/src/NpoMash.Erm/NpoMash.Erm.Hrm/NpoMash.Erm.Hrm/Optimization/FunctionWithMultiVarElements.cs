using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization
{
    /// <summary>
    /// Функция, содержащая элементы с несколькими переменными
    /// </summary>
    public class FunctionWithMultiVarElements: FunctionWithSingleVarElements
    {
        public List<MultiVarFunctionElem> MultiVarElems;
        public Dictionary<Variable, List<MultiVarFunctionElem>> MultiVarElemsByVariables;

        public override float PartialDerivate(Variable var, ValuesVector values)
        {
            float result = base.PartialDerivate(var, values);
            foreach (MultiVarFunctionElem elem in MultiVarElemsByVariables[var]) result += elem.PartialDerivate(values, var);
            return result;
        }

        public override float Calculate(ValuesVector values)
        {
            float result = base.Calculate(values);
            foreach (MultiVarFunctionElem elem in MultiVarElems) result += elem.Calculate(values);
            return result;
        }

    }
}
