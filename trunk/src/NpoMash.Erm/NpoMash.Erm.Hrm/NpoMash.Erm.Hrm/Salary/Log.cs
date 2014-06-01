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

    //    [NavigationItem("A1 Integration")]
    //[Persistent]
    //public class Log : BaseObject {
    //    [Association("Log-Records")]
    //    public XPCollection<LogRecord> Records {
    //        get { return GetCollection<LogRecord>("Records"); }
    //    }
    //    public Log(Session session)
    //        : base(session) {
    //    }
    //    public override void AfterConstruction() {
    //        base.AfterConstruction();
    //    }


    //}

    [Persistent]
    public class HrmSalaryLogRecord : XPObject, ILogRecord {
        private LogRecordType _RecordType;
        public LogRecordType RecordType {
            get { return _RecordType; }
            set { SetPropertyValue<LogRecordType>("RecordType", ref _RecordType, value); }
        }
        [Persistent("TimeOfCreation")]
        private DateTime _TimeOfCreation;
        [PersistentAlias("_TimeOfCreation")]
        public DateTime TimeOfCreation {
            get { return _TimeOfCreation; }
            //            set { SetPropertyValue<DateTime>("TimeOfCreation", ref _TimeOfCreation, value); } 
        }
        [Persistent("RecordText")]
        private String _RecordText;
        [PersistentAlias("_RecordText")]
        public String RecordText {
            get { return _RecordText; }
            //            set { SetPropertyValue<String>("RecordText", ref _RecordText, value); } 
        }
        [Persistent("Period")]
        [Association("HrmPeriod-HrmSalaryLogRecord")]
        private HrmPeriod _Period;
        [PersistentAlias("_Period")]
        public HrmPeriod Period {
            get { return _Period; }
            //            set { SetPropertyValue<fmCOrder>("Order", ref _Order, value); } 
        }
        [Persistent("Task")]
        [Association("HrmSalaryTask-HrmSalaryLogRecord")]
        private HrmSalaryTask _Task;
        [PersistentAlias("_Task")]
        public HrmSalaryTask Task {
            get { return _Task; }
            //            set { SetPropertyValue<fmCOrder>("Order", ref _Order, value); } 
        }
        [Persistent("Order")]
        private fmCOrder _Order;
        [PersistentAlias("_Order")]
        public fmCOrder Order {
            get { return _Order; }
            //            set { SetPropertyValue<fmCOrder>("Order", ref _Order, value); } 
        }
        [Persistent("Department")]
        private Department _Department;
        [PersistentAlias("_Department")]
        public Department Department {
            get { return _Department; }
            //            set { SetPropertyValue<Department>("Department", ref _Department, value); } 
        }

        public HrmSalaryLogRecord(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
            _TimeOfCreation = DateTime.Now;
        }

        public void Init(LogRecordType type, String text, HrmPeriod period, HrmSalaryTask task, Department department, fmCOrder order) {
            _RecordType = type;
            _RecordText = text;
            _Period = period;
            _Task = task;
            _Department = department;
            _Order = order;
        }

    }
}