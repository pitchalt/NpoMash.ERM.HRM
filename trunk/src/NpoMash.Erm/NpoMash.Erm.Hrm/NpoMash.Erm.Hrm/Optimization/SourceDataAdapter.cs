using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization {

    /// <summary>
    /// Адаптер исходных данных для оптимизации
    /// </summary>
    public abstract class SourceDataAdapter {
        /// <summary>
        /// Получение исходных данных и формирование необходимых структур
        /// </summary>
        protected abstract void GetData();
        /// <summary>
        /// Создание критерия оптимизации
        /// </summary>
        protected abstract void CreateCriteria();
        /// <summary>
        /// Создание системы ограничений
        /// </summary>
        protected abstract void CreateRestrictions();
        /// <summary>
        /// Выполнение оптимизации
        /// </summary>
        protected abstract void RunOptimisation();
        /// <summary>
        /// Дополнительные действия после оптимизации (например, округление)
        /// </summary>
        protected abstract void AfterOptimisation();
        /// <summary>
        /// Возвращение результатов в исходную структуру данных
        /// </summary>
        protected abstract void ReturnData();
        /// <summary>
        /// Выполнение всех необходимых действий
        /// </summary>
        public void Execute() {
            GetData();
            CreateCriteria();
            CreateRestrictions();
            RunOptimisation();
            AfterOptimisation();
            ReturnData();
        }
    }
}
