using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization
{
    public abstract class MultiDimOptim : Optimization<ValuesVector>
    {
        /// <summary>
        /// Ограничения
        /// </summary>
        public List<Equality> Restrictions;

        public MultiDimOptim(double prec) : base(prec) { }
    }
}
