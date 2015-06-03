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
        private float _LeftBorder;
        public float LeftBorder { get { return _LeftBorder; } set { LeftBorder = value; } }
        private float _RightBorder;
        public float RightBorder { get { return _RightBorder; } set { _RightBorder = value; } }
        public override void NextIteration()
        {
            Iterations++;
            float center = (LeftBorder + RightBorder) / 2;
            float left_point = center - Precision;
            float right_point = center + Precision;            
        }
    }
}
