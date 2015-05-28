using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization
{
    public abstract class Optimization
    {
        public List<LinearRestriction> Restrictions;
        public List<StopCriteria> stopCriterias;
        public abstract void nextIteration();
        public abstract void optimize();
        public bool isComplete()
        {
            bool result = true;
            foreach (StopCriteria crit in stopCriterias)
                if (!crit.isComplete()) result = false;
            return result;
        }
    }
}
