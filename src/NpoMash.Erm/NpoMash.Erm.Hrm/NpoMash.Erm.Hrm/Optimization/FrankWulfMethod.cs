using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization {
    public class FrankWulfMethod : MultiDimOptim {

        private SimplexMethod _SMethod;
        private FunctionWithSingleVarElements _OptimCriteria;

        public override void NextIteration() {
            base.NextIteration();
            if (_SMethod == null) {
                _SMethod = new SimplexMethod(0, _OptimCriteria.ToLinearView(CurrentState), Restrictions);
            }
            else {
                _SMethod.Table.ReplaceCriteria(_OptimCriteria.ToLinearView(CurrentState));
            }
            ValuesVector new_point = _SMethod.Optimize();
            FunctionWithSingleVar single_var_func = 
                new FunctionWithSingleVar(new SingleVarElemDichotomous(new Variable(), _OptimCriteria, CurrentState, new_point));
            DichotomousSearch ds = new DichotomousSearch(Precision/_OptimCriteria.FunctionVariables.Count(), single_var_func, 0,1);
            ds.Optimize();
            CurrentState = VectorLogic.PointOnSegment(CurrentState, new_point, ds.CurrentState);
        }

        public FrankWulfMethod(double prec, FunctionWithSingleVarElements crit, List<Equality> equalities, ValuesVector start_vector)
            : base(prec) {
                CurrentState = start_vector;
                Restrictions = equalities;
                _OptimCriteria = crit;
            StopCriterias.Add(new StopCriteriaMultiStepLength(prec, this));
            StopCriterias.Add(new StopCriteriaFunctionDelta<ValuesVector>(prec, crit,this));
        }


    }
}
