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

        private Department _Department; //—сылка на Department
        public Department Department {
            get { return _Department; }
            set { SetPropertyValue<Department>("Department", ref _Department, value); }
        }

        private HrmTimeSheet _TimeSheet; //—сылка на HrmTimeSheet
        [Association("TimeSheet-TimeSheetDeps")]
        public HrmTimeSheet TimeSheet {
            get { return _TimeSheet; }
            set { SetPropertyValue<HrmTimeSheet>("TimeSheet", ref _TimeSheet, value); }
        }

        private Int32 _MatrixWorkTime;
        public Int32 MatrixWorkTime {
            get { return _MatrixWorkTime; }
            set { SetPropertyValue<Int32>("MatrixWorkTime", ref _MatrixWorkTime, value); }
        }

        private Int32 _OtherWorkTime;
        public Int32 OtherWorkTime {
            get { return _OtherWorkTime; }
            set { SetPropertyValue<Int32>("OtherWorkTime", ref _MatrixWorkTime, value); }
        }

        public HrmTimeSheetDep(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction();}
    }
}
