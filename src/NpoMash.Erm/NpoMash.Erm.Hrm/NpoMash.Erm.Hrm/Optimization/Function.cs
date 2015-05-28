using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization
{
    public abstract class Function
    {
        public List<Variable> Variables;
        public abstract float Calculate(ValuesVector values);
    }
}
