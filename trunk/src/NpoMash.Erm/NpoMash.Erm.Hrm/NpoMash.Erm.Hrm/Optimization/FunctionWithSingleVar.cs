using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization
{
    public class FunctionWithSingleVar: Function
    {
        private SingleVarFunctionElem _FunctionElem;
        public SingleVarFunctionElem FunctionElem { get { return _FunctionElem; } set { _FunctionElem = value; } }

        public override float Calculate(ValuesVector values)
        {
            if (values.ContainsKey(FunctionElem.ElemVar))
                return Calculate(values[FunctionElem.ElemVar]);
            else throw new InvalidOperationException("В переданном векторе значений отсутствует искомая переменная");
        }

        public float Calculate(float x)
        {
            return FunctionElem.Calculate(x);
        }

    }
}
