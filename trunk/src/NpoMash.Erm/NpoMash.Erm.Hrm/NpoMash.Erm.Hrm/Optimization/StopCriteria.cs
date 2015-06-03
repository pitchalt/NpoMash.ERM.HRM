using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization
{
    /// <summary>
    /// Критерий останова алгоритма
    /// </summary>
    public abstract class StopCriteria
    {
        private float _Precision;
        /// <summary>
        /// Точность
        /// </summary>
        public float Precision { get { return _Precision; } }
        private bool _IsFirstIter;
        /// <summary>
        /// Признак того, что проверка осуществляется в первый раз
        /// </summary>
        public bool IsFirstIter { get { return _IsFirstIter; } }
        /// <summary>
        /// Предыдущее состояние
        /// </summary>
        public ValuesVector PreviousState;
        /// <summary>
        /// Проверка выполнения условия останова в текущей точке
        /// </summary>
        /// <param name="values">Текущая точка</param>
        /// <returns></returns>
        public abstract bool Check(ValuesVector values);

        public bool IsComplete(ValuesVector values)
        {
            bool result;
            if (IsFirstIter)
            {
                _IsFirstIter = false;
                result = false;
            }
            else
            {
                result = Check(values);
            }
            PreviousState = values;
            return result;
        }
        public StopCriteria(float prec)
        {
            _Precision = prec;
            _IsFirstIter = true;
        }
    }
}
