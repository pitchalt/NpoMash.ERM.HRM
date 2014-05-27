using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.ExpressApp.DC;
//
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.FM.Order;
//
namespace NpoMash.Erm.Hrm.Salary {

    public enum LogRecordType {
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

    [DomainComponent]
    public interface ILogRecord {
        LogRecordType RecordType { get; }
        DateTime TimeOfCreation { get; }
        HrmPeriod Period { get; }
        HrmSalaryTask Task { get; }
        Department Department { get; }
        fmCOrder Order { get; }
        String RecordText { get; }
    }

//    [DomainComponent]
    public interface ILogSupport {
        IList<ILogRecord> LogRecords { get; }
        void LogRecord(LogRecordType type, Department department, fmCOrder order, String text);
    }

}
