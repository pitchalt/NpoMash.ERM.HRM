using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
//
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
//
using IntecoAG.ERM.HRM.Organization;
// With XPO, the data model is declared by classes (so-called Persistent Objects) that will define the database structure, and consequently, the user interface (http://documentation.devexpress.com/#Xaf/CustomDocument2600).
namespace NpoMash.Erm.Hrm.Salary {
    /// <summary>
    /// Состояние задачи
    /// </summary>
    public enum HrmSalaryTaskState { 
        /// <summary>
        /// Задача активна
        /// </summary>
        HRM_SALARY_TASK_ACTIVED = 1,
        /// <summary>
        /// Задача выполнена успешно
        /// </summary>
        HRM_SALARY_TASK_COMPLETED = 2,
        /// <summary>
        /// Задача отклонена
        /// </summary>
        HRM_SALARY_TASK_ABORTED = 3,
    }
    /// <summary>
    /// Абстарктный класс для всех задач  периода
    /// </summary>
    [Persistent("HrmSalaryTask")]
    public abstract class HrmSalaryTask : BaseObject { // You can use a different base persistent class based on your requirements (http://documentation.devexpress.com/#Xaf/CustomDocument3146).
        private HrmPeriod _Period; 
        /// <summary>
        /// Период к которому относиться задача
        /// </summary>
        [Association("HrmPeriod-HrmSalaryTask")]
        public HrmPeriod Period {
            get { return _Period; }
            set { SetPropertyValue<HrmPeriod>("Period", ref _Period, value); }
        }
        /// <summary>
        /// Тип задачи
        /// </summary>
        public Type TaskType {
            get { return this.GetType(); }
        }

        [Persistent("State")]
        private HrmSalaryTaskState _State;
        /// <summary>
        /// Текущее состояние задачи
        /// </summary>
        [PersistentAlias("_State")]
        public HrmSalaryTaskState State {
            get { return _State; }
        }
        protected void StateSet(HrmSalaryTaskState state) {
            SetPropertyValue<HrmSalaryTaskState>("State", ref _State, state);
        }

        private DEPARTMENT_GROUP_DEP _GroupDep;
        /// <summary>
        /// Для задач которые имеют разделение по КБ / ОЗМ
        /// </summary>
        public DEPARTMENT_GROUP_DEP GroupDep {
            get { return _GroupDep; }
            set { SetPropertyValue<DEPARTMENT_GROUP_DEP>("GroupDep", ref _GroupDep, value); }
        }

        [Persistent("CreateTime")]
        private DateTime _CreateTime;
        /// <summary>
        /// Время создания
        /// </summary>
        [PersistentAlias("_CreateTime")]
        [ModelDefault("Format", "(000)-00")]
        public DateTime CreateTime {
            get { return _CreateTime; }
        }
        [Persistent("FinishTime")]
        private DateTime _FinishTime;
        /// <summary>
        /// Время завершения
        /// </summary>
        [PersistentAlias("_FinishTime")]
        [ModelDefault("Format", "(000)-00")]
        public DateTime FinishTime {
            get { return _FinishTime; }
        }

        public HrmSalaryTask(Session session)
            : base(session) {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here or place it only when the IsLoading property is false.
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            StateSet(HrmSalaryTaskState.HRM_SALARY_TASK_ACTIVED);
            _CreateTime = DateTime.Now;
       }

        public virtual void Complete() { 
            SetPropertyValue<DateTime>("FinishTime", ref _FinishTime, DateTime.Now);
            StateSet(HrmSalaryTaskState.HRM_SALARY_TASK_COMPLETED);
        }

        public virtual void Abort() {
            SetPropertyValue<DateTime>("FinishTime", ref _FinishTime, DateTime.Now);
            StateSet(HrmSalaryTaskState.HRM_SALARY_TASK_ABORTED);
        }

    }
}
