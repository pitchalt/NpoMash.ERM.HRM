using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization {
    public class StopCriteriaSingleStepLength : StopCriteria {
        private Optimization<double> _Optimization;
        private double _PreviousState;
        public override bool Check() {
            return Math.Abs(_PreviousState - _Optimization.CurrentState) < Precision;
        }

        public override void UpdateState() {
            _PreviousState = _Optimization.CurrentState;
        }

        public StopCriteriaSingleStepLength(double prec, Optimization<double> opt)
            : base(prec) {
            _Optimization = opt;
        }
    }
}
