using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization
{
    /// <summary>
    /// Элемент функции критерия матрицы резерва для представления позаказного отклонения
    /// </summary>
    public class ReserveMatrixCriteriaElemOrds: MultiVarFunctionElem
    {

        private float _FreePart;
        public float FreePart { get { return _FreePart; } set { _FreePart = value; } }

        private float _Coefficent;
        public float Coefficient { get { return _Coefficent; } set { _Coefficent = value; } }

        public override float Calculate(ValuesVector values)
        {
            float sum = 0;
            foreach (Variable vr in ElemVars) sum += values[vr];
            float x = FreePart - sum;
            return Coefficient * x * x;
        }

        public override float PartialDerivate(ValuesVector values, Variable der_variable)
        {
            float sum = 0;
            foreach (Variable vr in ElemVars) sum += values[vr];
            return 2 * Coefficient * (sum - FreePart);
        }
    }
}
