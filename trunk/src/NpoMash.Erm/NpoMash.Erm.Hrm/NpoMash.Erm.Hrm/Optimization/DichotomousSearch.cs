using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization
{
    /// <summary>
    /// Одномерная оптимизация методом дихотомического поиска
    /// </summary>
    public class DichotomousSearch: SingleDimOptim
    {

        public override void NextIteration()
        {
            base.NextIteration();
            double center = (_LeftBorder + _RightBorder) / 2;
            double left_point = center - Precision;
            double right_point = center + Precision;
            if (OptimCriteria.Calculate(left_point) > OptimCriteria.Calculate(right_point)) {
                _LeftBorder = left_point;
                CurrentState = right_point;
            }
            else {
                _RightBorder = right_point;
                CurrentState = left_point;
            }
        }

        public DichotomousSearch(double prec, FunctionWithSingleVar opt, double left_border, double right_border) : base(prec, opt, left_border, right_border) { }
    }
}
