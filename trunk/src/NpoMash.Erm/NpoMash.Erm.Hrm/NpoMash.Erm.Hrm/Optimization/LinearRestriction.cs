using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization
{
    public abstract class LinearRestriction : LinearFunction
    {
        public abstract bool Satisfies(ValuesVector values);
    }
}
