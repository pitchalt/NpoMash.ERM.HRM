using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization {
    public class Variable {
        private String _VarName;
        /// <summary>
        /// Название переменной (для удобства)
        /// </summary>
        public String VarName { get { return _VarName; } set { _VarName = value; } }
    }
}
