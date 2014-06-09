using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
//
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
//
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.HRM.Organization;

namespace NpoMash.Erm.Hrm.Salary {
    /// <summary>
    /// Состояние задачи
    /// </summary>
    public enum HrmSalaryTaskState {
        /// <summary>
        /// Задача создана
        /// </summary>
        HRM_SALARY_TASK_CREATED = 0,
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
    [Appearance("", AppearanceItemType = "Action", TargetItems = "Delete, New", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, TargetItems = "*", Context = "Any", Enabled = false)]
    public abstract class HrmSalaryTask : BaseObject, ITask {
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

        private DepartmentGroupDep _GroupDep;
        /// <summary>
        /// Для задач которые имеют разделение по КБ / ОЗМ
        /// </summary>
        public DepartmentGroupDep GroupDep {
            get { return _GroupDep; }
            set { SetPropertyValue<DepartmentGroupDep>("GroupDep", ref _GroupDep, value); }
        }

        [Persistent("CreateTime")]
        private DateTime _CreateTime;
        /// <summary>
        /// Время создания
        /// </summary>
        [PersistentAlias("_CreateTime")]
        //[ModelDefault("Format", "(000)-00")]
        [ModelDefault("DisplayFormat", "{0:F}")]
        public DateTime CreateTime {
            get { return _CreateTime; }
        }
        [Persistent("FinishTime")]
        private DateTime _FinishTime;
        /// <summary>
        /// Время завершения
        /// </summary>
        [PersistentAlias("_FinishTime")]
        [ModelDefault("DisplayFormat", "{0:F}")]
        public DateTime FinishTime {
            get { return _FinishTime; }
        }

        [Association("HrmSalaryTask-HrmSalaryLogRecord")]
        [Browsable(false)]
        public XPCollection<HrmLogRecord> LogRecordCol {
            get {
                return GetCollection<HrmLogRecord>("LogRecordCol");
            }
        }

        public HrmSalaryTask(Session session)
            : base(session) {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here or place it only when the IsLoading property is false.
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            _CreateTime = DateTime.Now;
            StateSet(HrmSalaryTaskState.HRM_SALARY_TASK_CREATED);
            LogRecord(LogRecordType.INFO, null, null, "Задача создана");
        }

        public virtual void Activate() {
            StateSet(HrmSalaryTaskState.HRM_SALARY_TASK_ACTIVED);
        }

        public virtual void Complete() {
            SetPropertyValue<DateTime>("FinishTime", ref _FinishTime, DateTime.Now);
            StateSet(HrmSalaryTaskState.HRM_SALARY_TASK_COMPLETED);
            LogRecord(LogRecordType.INFO, null, null, "Задача выполнена успешно");
        }

        public virtual void Abort() {
            SetPropertyValue<DateTime>("FinishTime", ref _FinishTime, DateTime.Now);
            StateSet(HrmSalaryTaskState.HRM_SALARY_TASK_ABORTED);
            LogRecord(LogRecordType.INFO, null, null, "Задача отклонена");
        }

        [Aggregated]
        public IList<ILogRecord> LogRecords {
            get { return new ListConverter<ILogRecord, HrmLogRecord>(LogRecordCol); }
        }

        private XPCollection<AuditDataItemPersistent> _AuditTrail;
        public XPCollection<AuditDataItemPersistent> AuditTrail {
            get {
                if (_AuditTrail == null) {
                    _AuditTrail = AuditedObjectWeakReference.GetAuditTrail(Session, this);
                }
                return _AuditTrail;
            }
        }

        public void LogRecord(LogRecordType type, Department department, fmCOrder order, String text) {
            HrmLogRecord record = new HrmLogRecord(this.Session);
            record.Init(type, text, this.Period, this, department, order);
        }
        protected IList<ITaskObject> _InObjects;
        public IList<ITaskObject> InObjects {
            get {
                if (_InObjects == null) {
                    _InObjects = new List<ITaskObject>();
                    InObjectsLoad();
                }
                return _InObjects;
            }
        }
        protected abstract void InObjectsLoad();
    }
}