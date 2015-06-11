using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization {
    public class StopCriteriaFunctionDelta<VAL> : StopCriteria {
        private Optimization<VAL> _Optimization;
        private VAL _PreviousState;
        private Function<VAL> _CriteriaFuncton;
        /// <summary>
        /// Целевая функция оптимизации
        /// </summary>
        public Function<VAL> CriteriaFunction { get { return _CriteriaFuncton; } }
        public override bool Check() {
            bool result = CriteriaFunction.Calculate(_PreviousState) - CriteriaFunction.Calculate(_Optimization.CurrentState) < Precision;
            return result;
        }

        public override void UpdateState() {
            _PreviousState = _Optimization.CurrentState;
        }
        public StopCriteriaFunctionDelta(double prec, Function<VAL> func, Optimization<VAL> opt)
            : base(prec) {
            _CriteriaFuncton = func;
            _Optimization = opt;
        }
    }
}
