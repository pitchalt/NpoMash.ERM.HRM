using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization
{
    public class LinearFunction: Function
    {
        public Dictionary<Variable, SimpleFunctionElem> SimpleElements;
        public override float Calculate(ValuesVector values)
        {
            float result = 0;
            foreach (Variable vr in Variables) result += SimpleElements[vr].Calculate(values[vr]);
            return result;
        }

    }
}
