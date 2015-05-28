using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization
{
    public abstract class StopCriteria
    {
        public ValuesVector previousState;
        public ValuesVector currentState;
        public abstract bool isComplete();
    }
}
