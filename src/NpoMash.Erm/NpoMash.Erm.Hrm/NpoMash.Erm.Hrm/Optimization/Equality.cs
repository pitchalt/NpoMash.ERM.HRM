using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization {
    public class Equality : LinearRestriction {
        private double _EqualTo;
        public double EqualTo { get { return _EqualTo; } set { _EqualTo = value; } }
        public override bool Satisfies(ValuesVector values) {
            return Calculate(values) == EqualTo;
        }
    }
}
