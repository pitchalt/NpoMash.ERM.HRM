using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization {
    public abstract class Function<VAL> {
        public List<Variable> FunctionVariables;
        public abstract double Calculate(VAL values);
        public Function() {
            FunctionVariables = new List<Variable>();
        }
    }
}
