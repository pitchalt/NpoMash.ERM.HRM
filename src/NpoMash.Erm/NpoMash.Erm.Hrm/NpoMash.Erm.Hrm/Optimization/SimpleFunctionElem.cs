using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization
{
    /// <summary>
    /// Представляет простейший элемент функции - переменная с коэффициентом
    /// </summary>
    public class SimpleFunctionElem : SingleVarFunctionElem
    {
        public override float PartialDerivate(float value)
        {
            return Coefficient;
        }

        public override float Calculate(float value)
        {
            return value*Coefficient;
        }

    }
}
