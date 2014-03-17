using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.FM.Order;


namespace NpoMash.Erm.Hrm.Salary {
    public enum RecordType{
        /// <summary>
        /// Запись с уведомлением
        /// </summary>
        INFO = 1,
        /// <summary>
        /// Запись с предупреждением
        /// </summary>
        WARNING = 2,
        /// <summary>
        /// Запись с ошибкой
        /// </summary>
        ERROR = 3,
    }

    [NavigationItem("A1 Integration")]
    [DefaultClassOptions]
    public class Log : BaseObject {
        [Association("Log-Records")]
        public XPCollection<LogRecord> Records {
            get { return GetCollection<LogRecord>("Records"); }
        }
        public Log(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }


    }

    [DefaultClassOptions]
    public class LogRecord : BaseObject {
        private Log _Log;
        [Association("Log-Records")]
        public Log Log { get { return _Log; } set { SetPropertyValue<Log>("Log", ref _Log, value); } }
        private RecordType _RecordType;
        public RecordType RecordType { get { return _RecordType; } set { SetPropertyValue<RecordType>("RecordType", ref _RecordType, value); } }
        private DateTime _TimeOfCreation;
        public DateTime TimeOfCreation { get { return _TimeOfCreation; } set { SetPropertyValue<DateTime>("TimeOfCreation", ref _TimeOfCreation, value); } }
        private String _RecordText;
        public String RecordText { get { return _RecordText; } set { SetPropertyValue<String>("RecordText", ref _RecordText, value); } }
        private fmCOrder _Order;
        public fmCOrder Order { get { return _Order; } set { SetPropertyValue<fmCOrder>("Order", ref _Order, value); } }
        private Department _Department;
        public Department Department { get { return _Department; } set { SetPropertyValue<Department>("Department", ref _Department, value); } }

        public LogRecord(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
            TimeOfCreation = DateTime.Now;
        }


    }
}
