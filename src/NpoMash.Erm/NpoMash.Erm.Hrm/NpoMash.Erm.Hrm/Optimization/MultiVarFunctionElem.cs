using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization
{
    /// <summary>
    /// Элемент функции с несколькими переменными
    /// </summary>
    public abstract class MultiVarFunctionElem
    {
        /// <summary>
        /// переменные составного элемента функции
        /// </summary>
        public List<Variable> ElemVars;

        public abstract float Calculate(ValuesVector values);
        public abstract float PartialDerivate(ValuesVector values, Variable der_variable);
    }
}
