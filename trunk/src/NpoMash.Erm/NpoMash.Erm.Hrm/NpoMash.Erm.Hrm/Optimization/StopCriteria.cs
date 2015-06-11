using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization {
    /// <summary>
    /// Критерий останова алгоритма
    /// </summary>
    public abstract class StopCriteria {
        private double _Precision;
        /// <summary>
        /// Точность
        /// </summary>
        public double Precision { get { return _Precision; } }
        private bool _IsFirstIter;
        /// <summary>
        /// Признак того, что проверка осуществляется в первый раз
        /// </summary>
        public bool IsFirstIter { get { return _IsFirstIter; } }
        public abstract bool Check();

        public bool IsComplete() {
            bool result;
            if (IsFirstIter) {
                _IsFirstIter = false;
                result = false;
            }
            else {
                result = Check();
            }
            UpdateState();
            return result;
        }

        public abstract void UpdateState();
        public StopCriteria(double prec) {
            _Precision = prec;
            _IsFirstIter = true;
        }
    }
}
