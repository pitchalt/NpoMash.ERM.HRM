using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization {
    public abstract class Optimization<VAL> {
        private double _Precision;
        /// <summary>
        /// Точность поиска
        /// </summary>
        public double Precision { get { return _Precision; } }
        public VAL CurrentState;
        private Int32 _Iterations;
        /// <summary>
        /// Число выполненных итераций
        /// </summary>
        public Int32 Iterations { get { return _Iterations; } set { _Iterations = value; } }
        /// <summary>
        /// Критерии останова
        /// </summary>
        public DateTime StartTime;
        public List<StopCriteria> StopCriterias;

        public virtual void NextIteration() {
            Iterations++;
        }
        public VAL Optimize() {
            StartTime = DateTime.Now;
            while (!IsComplete(CurrentState)) {
                NextIteration();
            }
            return CurrentState;
        }
        public bool IsComplete(VAL values) {
            bool result = true;
            foreach (StopCriteria crit in StopCriterias)
                if (!crit.IsComplete()) result = false;
            return result;
        }

        public Optimization(double prec) {
            StopCriterias = new List<StopCriteria>();
            _Precision = prec;
        }
    }
}
