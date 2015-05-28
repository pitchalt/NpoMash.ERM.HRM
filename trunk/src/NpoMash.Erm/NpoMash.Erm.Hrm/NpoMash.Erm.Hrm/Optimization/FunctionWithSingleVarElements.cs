using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization
{
    public class FunctionWithSingleVarElements: Function
    {
        /// <summary>
        /// Элементы функции с одной переменной
        /// </summary>
        public Dictionary<Variable,SingleVarFunctionElem> SingleVarElems;

        public override float Calculate(ValuesVector values)
        {
            float result = 0;
            foreach (Variable vr in Variables) result += SingleVarElems[vr].Calculate(values[vr]);
            return result;
        }
        
    }
}
