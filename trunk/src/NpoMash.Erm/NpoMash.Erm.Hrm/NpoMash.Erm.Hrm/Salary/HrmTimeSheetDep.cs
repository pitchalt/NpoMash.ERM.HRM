using System;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;
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
using IntecoAG.ERM.HRM.Organization;

namespace NpoMash.Erm.Hrm.Salary {

    [Persistent("HrmTimeSheetDep")]
    public class HrmTimeSheetDep : BaseObject {

        private Department _Department; //������ �� Department
        public Department Department {
            get { return _Department; }
            set { SetPropertyValue<Department>("Department", ref _Department, value); }
        }

        private HrmTimeSheet _TimeSheet; //������ �� HrmTimeSheet
        [Association("TimeSheet-TimeSheetDeps")]
        public HrmTimeSheet TimeSheet {
            get { return _TimeSheet; }
            set { SetPropertyValue<HrmTimeSheet>("TimeSheet", ref _TimeSheet, value); }
        }

        private HrmTimeSheetGroup _TimeSheetGroup;
        [Association("TimeSheetDeps-TimeSheetGroup")]
        public HrmTimeSheetGroup TimeSheetGroup {
            get { return _TimeSheetGroup; }
            set { SetPropertyValue<HrmTimeSheetGroup>("TimeSheetGroup", ref _TimeSheetGroup, value); }
        }


        private Int32 _MatrixWorkTime;
        public Int32 MatrixWorkTime {
            get { return _AdditionWorkTime; }
            set { SetPropertyValue<Int32>("MatrixWorkTime", ref _AdditionWorkTime, value); }
        }

        private Int32 _BaseWorkTime;
        public Int32 BaseWorkTime {
            get { return _BaseWorkTime; }
            set { SetPropertyValue<Int32>("BaseWorkTime", ref _BaseWorkTime, value); }
        }

        private Int32 _AdditionWorkTime;
        public Int32 AdditionWorkTime {
            get { return _AdditionWorkTime; }
            set { SetPropertyValue<Int32>("AdditionWorkTime", ref _AdditionWorkTime, value); }
        }
        public HrmTimeSheetDep(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction();}
    }
}
