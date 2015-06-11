using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization {
    public class SingleVarElemDichotomous : SingleVarFunctionElem {
        private Function<ValuesVector> _MultiVarFunc;
        public Function<ValuesVector> MultiVarFunc { get { return _MultiVarFunc; } set { _MultiVarFunc = value; } }
        public ValuesVector FirstPoint;
        public ValuesVector SecondPoint;

        public override double Calculate(double value) {
            ValuesVector point = VectorLogic.PointOnSegment(FirstPoint, SecondPoint, value);
            return MultiVarFunc.Calculate(point);
        }

        public override double PartialDerivate(double value) {
            return MultiVarFunc.Calculate(SecondPoint) - MultiVarFunc.Calculate(FirstPoint);
        }

        public SingleVarElemDichotomous(Variable vr, Function<ValuesVector> func, ValuesVector first_pnt, ValuesVector second_pnt)
            : base(vr) {
            _MultiVarFunc = func;
            FirstPoint = first_pnt;
            SecondPoint = second_pnt;
        }
    }
}
