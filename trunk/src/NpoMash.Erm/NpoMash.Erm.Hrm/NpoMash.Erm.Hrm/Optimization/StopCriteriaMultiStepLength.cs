using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization {
    public class StopCriteriaMultiStepLength : StopCriteria {
        private Optimization<ValuesVector> _Optimization;
        private ValuesVector _PreviousState;
        public override bool Check() {
            return VectorLogic.Length(_PreviousState, _Optimization.CurrentState) < Precision;
        }

        public override void UpdateState() {
            _PreviousState = _Optimization.CurrentState;
        }

        public StopCriteriaMultiStepLength(double prec, Optimization<ValuesVector> opt)
            : base(prec) {
            _Optimization = opt;
        }


    }
}
