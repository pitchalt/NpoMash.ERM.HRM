using System;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;
//
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Utils;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
//
using IntecoAG.ERM.HRM.Organization;
//
namespace NpoMash.Erm.Hrm.Salary {

    public enum HrmTimeSheetStatus {
        DOWNLOADED = 1,
        ACCEPTED = 2,
        ARCHIVE = 3,
        NOTDOWNLOADED = 4 
    }

    [Persistent("HrmTimeSheet")]
    [Appearance("", AppearanceItemType = "Action", TargetItems = "Delete, New", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, TargetItems = "*", Context = "Any", Enabled = false)]
    [DefaultProperty("Name")]
    public class HrmTimeSheet : BaseObject, ITimeSheet {

        // Cсылка на HrmPeriodTimeSheetBaseObject
        [Aggregated]
        [Persistent]
        private HrmTimeSheetPeriodObject _TimeSheetPeriodObject;
        //public HrmTimeSheetPeriodObject _PeriodObject {
        //    get { return _TimeSheetPeriodObject; }
        //    set { SetPropertyValue<HrmTimeSheetPeriodObject>("TimeSheetPeriodObject", ref _TimeSheetPeriodObject, value); }
        //}
        //
        [Association("TimeSheet-TimeSheetDeps"), Aggregated] // Коллекция TimeSheetDeps
        public XPCollection<HrmTimeSheetDep> TimeSheetDeps {
            get { return GetCollection<HrmTimeSheetDep>("TimeSheetDeps"); }
        }

        private HrmPeriod _Period; // Ссылка на HrmPeriod
        [Association("Period-TimeSheets")]
        public HrmPeriod Period {
            get { return _Period; }
            set { 
                SetPropertyValue<HrmPeriod>("Period", ref _Period, value);
                if (!IsLoading) {
                    _TimeSheetPeriodObject.Period = value;
                   // PeriodBase = value;
                }
            }
        }

        private DepartmentGroupDep _GroupDep;
        public DepartmentGroupDep GroupDep {
            get { return _GroupDep; }
            set { SetPropertyValue<DepartmentGroupDep>("GroupDep", ref _GroupDep, value); }
        }

        [Persistent("Status")]
        private HrmTimeSheetStatus _Status;
        [RuleRequiredField(DefaultContexts.Save)]
        [PersistentAlias("_Status")]
        public HrmTimeSheetStatus Status {
            get { return _Status; }
        }

        public void SetStatus(HrmTimeSheetStatus stat) {
            SetPropertyValue<HrmTimeSheetStatus>("Status", ref _Status, stat);
        }


        public HrmTimeSheet(Session session): base(session) { }
        public override void AfterConstruction() { 
            base.AfterConstruction();
            _TimeSheetPeriodObject = new HrmTimeSheetPeriodObject(this);
            SetStatus(HrmTimeSheetStatus.DOWNLOADED);
        }

        //public HrmSalaryPeriodObjectStatus PeriodObjectStatus {
        //    get { return _TimeSheetPeriodObject.Status; }
        //}
        public String PeriodObjectStatus {
            get {
                EnumDescriptor ed = new EnumDescriptor(typeof(HrmTimeSheetStatus));
                return ed.GetCaption(Status);
            }
        }

        public Type PeriodObjectType {
            get { return typeof(HrmTimeSheet); }
        }

        public Type TaskObjectType {
            get { return PeriodObjectType; }
        }

        public String TaskObjectName {
            get { return Name; }
        }

        public String Name {
            get {
                EnumDescriptor ed = new EnumDescriptor(typeof(HrmTimeSheetStatus));
                return ed.GetCaption(Status) + " " + (Period.Year * 100 + Period.Month).ToString() + " " + PeriodObjectType.Name;
            }
        }


        public string TaskObjectStatus {
            get { return PeriodObjectStatus; }
        }
    }
}