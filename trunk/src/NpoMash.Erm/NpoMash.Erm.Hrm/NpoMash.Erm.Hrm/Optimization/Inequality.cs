using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization
{
    /// <summary>
    /// Линейное неравенство
    /// </summary>
    public class Inequality: LinearRestriction
    {
        private float _GreaterThan;
        /// <summary>
        /// Должно быть больше чем это значение
        /// </summary>
        public float GreaterThan { get { return _GreaterThan; } set { _GreaterThan = value; } }
        private float _LowerThan;
        /// <summary>
        /// Должно быть меньше чем это значение
        /// </summary>
        public float LowerThan { get { return _LowerThan; } set { _LowerThan = value; } }

        /// <summary>
        /// Выполняется в заданной точке
        /// </summary>
        /// <param name="values">Вектор значений</param>
        /// <returns></returns>
        public override bool Satisfies(ValuesVector values)
        {
            float value = Calculate(values);
            return (value > GreaterThan) && (value < LowerThan);
        }

    }
}
