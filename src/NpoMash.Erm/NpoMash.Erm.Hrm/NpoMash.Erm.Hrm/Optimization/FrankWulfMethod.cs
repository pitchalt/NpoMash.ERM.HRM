using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization
{
    public class FrankWulfMethod: MultiDimOptim
    {
        private SingleDimOptim _sngDimOptim;
        public SingleDimOptim sngDimOptim { get { return _sngDimOptim; } set { _sngDimOptim = value; } }

        public override void NextIteration()
        {
            //приведение критерия к линейному виду

            // создание симплексной таблицы

            // оптимизация симплексом

            // поиск новой точки оптимума одномерной оптимизацией

            throw new NotImplementedException();
        }
    }
}
