using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization
{
    public class StopCriteriaStepLength: StopCriteria
    {
        public override bool Check(ValuesVector values)
        {
            return (VectorLogic.Length(PreviousState,values)<Precision);
        }

        public StopCriteriaStepLength(float prec): base(prec)
        {

        }


    }
}
