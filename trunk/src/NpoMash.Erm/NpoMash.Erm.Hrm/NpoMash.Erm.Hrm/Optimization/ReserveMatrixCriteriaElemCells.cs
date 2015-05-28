using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization
{
    /// <summary>
    /// Элемент функции критерия матрицы резерва для представления поячеечного отклонения
    /// </summary>
    public class ReserveMatrixCriteriaElemCells : SingleVarFunctionElem
    {
        /// <summary>
        /// Свободный член
        /// </summary>
        private float _FreePart;
        public float FreePart { get { return _FreePart; } set { _FreePart = value; } }

        ///// <summary>
        ///// Коэффициент при элементе функции
        ///// </summary>
        //private float _Coefficient;
        //public float Coefficient { get { return _Coefficient; } set { _Coefficient = value; } }

        public override float Calculate(float value)
        {
            float x = FreePart - value;
            return Coefficient * x * x;
        }

        public override float PartialDerivate(float value)
        {
            return 2*(value - FreePart) * Coefficient;
        }
    }
}
