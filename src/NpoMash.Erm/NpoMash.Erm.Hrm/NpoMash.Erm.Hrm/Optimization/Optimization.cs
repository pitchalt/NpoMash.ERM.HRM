using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization
{
    public abstract class Optimization
    {
        private float _Precision;
        public float Precision { get { return _Precision; } set { _Precision = value; } }
        public ValuesVector CurrentState;
        private Int32 _Iterations;
        public Int32 Iterations { get { return _Iterations; } set { _Iterations = value; } }
        /// <summary>
        /// Ограничения
        /// </summary>
        public List<LinearRestriction> Restrictions;
        /// <summary>
        /// Критерии останова
        /// </summary>
        public List<StopCriteria> StopCriterias;
        private Function _OptimCriteria;
        /// <summary>
        /// Критерий оптимальности решения (целевая функция)
        /// </summary>
        public Function OptimCriteria { get { return _OptimCriteria; } set { _OptimCriteria = value; } }
        public abstract void NextIteration();
        public void Optimize()
        {
            while (!IsComplete(CurrentState))
            {
                NextIteration();
            }
        }
        public bool IsComplete(ValuesVector values)
        {
            bool result = true;
            foreach (StopCriteria crit in StopCriterias)
                if (!crit.IsComplete(values)) result = false;
            return result;
        }
    }
}
