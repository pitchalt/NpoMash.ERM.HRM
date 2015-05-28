using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization
{
    public abstract class FunctionElem
    {
        private float _Coefficient;
        /// <summary>
        /// Коэффициент при данной переменной
        /// </summary>
        public float Coefficient { get { return _Coefficient; } set { _Coefficient = value; } }

        /// <summary>
        /// Переменные в данном элементе функции
        /// </summary>
        public List<Variable> Variables;
        /// <summary>
        /// Значение частной производной в заданной точке
        /// </summary>
        /// <param name="values">Вектор значений переменных</param>
        /// <param name="der_variable">По какой переменной берется частная производная</param>
        /// <returns></returns>
        public abstract float PartialDerivate(ValuesVector values, Variable der_variable);
        /// <summary>
        /// Вычислить значение элемента функции при заданном значении переменной
        /// </summary>
        /// <param name="value">Значение переменной</param>
        /// <returns></returns>
        public abstract float Calculate(ValuesVector values);
    }
}
