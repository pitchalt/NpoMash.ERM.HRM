using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization
{
    public class StopCriteriaFunctionDelta: StopCriteria
    {
        private Function _CriteriaFuncton;
        /// <summary>
        /// Целевая функция оптимизации
        /// </summary>
        public Function CriteriaFunction { get { return _CriteriaFuncton; } }
        public override bool Check(ValuesVector values)
        {
            return (CriteriaFunction.Calculate(PreviousState)-CriteriaFunction.Calculate(values) < Precision);
        }

        public StopCriteriaFunctionDelta(float prec, Function func)
            : base(prec)
        {
            _CriteriaFuncton = func;
        }
    }
}
