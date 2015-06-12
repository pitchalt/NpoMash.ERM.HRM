using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization {
    /// <summary>
    /// Критерий останова оптимизации
    /// </summary>
    public abstract class StopCriteria {
        private double _Precision;
        /// <summary>
        /// Точность
        /// </summary>
        public double Precision { get { return _Precision; } }
        /// <summary>
        /// Признак того, что проверка осуществляется в первый раз
        /// </summary>
        private bool _IsFirstIter;
        
        /// <summary>
        /// Проверка выполнения критерия останова
        /// </summary>
        public abstract bool Check();
        /// <summary>
        /// Общий механизм проверки выполнения критерия останова
        /// </summary>
        public bool IsComplete() {
            bool result;
            if (_IsFirstIter) {
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
