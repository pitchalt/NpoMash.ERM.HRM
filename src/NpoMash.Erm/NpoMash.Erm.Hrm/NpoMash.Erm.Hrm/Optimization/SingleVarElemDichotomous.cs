using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization
{
    public class SingleVarElemDichotomous: SingleVarFunctionElem
    {
        private Function _MultiVarFunc;
        public Function MultiVarFunc { get { return _MultiVarFunc; } set { _MultiVarFunc = value; } }
        public ValuesVector FirstPoint;
        public ValuesVector SecondPoint;

        public override float Calculate(float value)
        {
            ValuesVector point = VectorLogic.PointOnSegment(FirstPoint, SecondPoint, value);
            return MultiVarFunc.Calculate(point);
        }

        public override float PartialDerivate(float value)
        {
            return MultiVarFunc.Calculate(SecondPoint) - MultiVarFunc.Calculate(FirstPoint);
        }
    }
}
