using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization
{
    public abstract class SingleDimOptim : Optimization
    {
        private Variable _OptimVar;
        public Variable OptimVar;

        private Inequality _Borders;
        public Inequality Borders { get { return _Borders; } set { _Borders = value; } }
        

    }
}
