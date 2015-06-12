using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization {
    public static class VectorLogic {
        /// <summary>
        /// Длина отрезка
        /// </summary>
        /// <param name="vect1">Точка 1</param>
        /// <param name="vect2">Точка 2</param>
        /// <returns>Результат</returns>
        public static double Length(ValuesVector vect1, ValuesVector vect2) {
            double result = 0;
            foreach (Variable var in vect1.Keys) {
                double x = vect1[var] - vect2[var];
                result += x * x;
            }
            return Math.Sqrt(result);
        }

        /// <summary>
        /// Ищет точку на отрезке
        /// </summary>
        /// <param name="vect1">Начало отрезка</param>
        /// <param name="vect2">Конец отрезка</param>
        /// <param name="lambda">Доля длины отрезка от первой точки до искомой, значение от 0 до 1</param>
        /// <returns></returns>
        public static ValuesVector PointOnSegment(ValuesVector vect1, ValuesVector vect2, double lambda) {
            ValuesVector result = new ValuesVector();
            foreach (Variable x in vect1.Keys) result.Add(x, vect1[x] * (1 - lambda) + lambda * vect2[x]);
            return result;
        }
    }
}
