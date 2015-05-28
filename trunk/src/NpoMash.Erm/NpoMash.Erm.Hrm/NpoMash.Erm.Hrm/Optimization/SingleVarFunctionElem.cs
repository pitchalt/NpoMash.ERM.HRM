using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization
{
    /// <summary>
    /// Элемент функции, содержащий одну переменную
    /// </summary>
    public abstract class SingleVarFunctionElem
    {
        private Variable _ElemVar;
        /// <summary>
        /// переменная элемента функции
        /// </summary>
        public Variable ElemVar { get { return _ElemVar; } set { _ElemVar = value; } }

        private float _Coefficient;
        public float Coefficient { get { return _Coefficient; } set { _Coefficient = value; } }

        public abstract float Calculate(float value);
        public abstract float PartialDerivate(float value);
    }
}
