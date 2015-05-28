using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization
{
    public class FrankWulfMethod: MultiDimOptim
    {
        private SingleDimOptim _sngDimOptim;
        public SingleDimOptim sngDimOptim { get { return _sngDimOptim; } set { _sngDimOptim = value; } }
        public override void optimize()
        {
            throw new NotImplementedException();
        }

        public override void nextIteration() {
            throw new NotImplementedException();
        }

    }
}
