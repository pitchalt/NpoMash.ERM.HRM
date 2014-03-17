using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
//
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Editors;
//
using IntecoAG.ERM.HRM.Organization;

namespace NpoMash.Erm.Hrm.Salary {
    [DefaultProperty("GroupDep")]
    public class HrmTimeSheetGroup : HrmTimeSheetBase {

        private HrmTimeSheet _TimeSheet;
        public HrmTimeSheet TimeSheet {
            get { return _TimeSheet; }
            set { SetPropertyValue<HrmTimeSheet>("TimeSheet", ref _TimeSheet, value); }
        }

        private DEPARTMENT_GROUP_DEP _GroupDep;
        public DEPARTMENT_GROUP_DEP GroupDep {
            get { return _GroupDep; }
            set { SetPropertyValue<DEPARTMENT_GROUP_DEP>("GroupDep", ref _GroupDep, value); } }
 
        [Association("TimeSheetDeps-TimeSheetGroup"),Aggregated]
        public XPCollection<HrmTimeSheetDep> TimeSheetDeps {
            get { return GetCollection<HrmTimeSheetDep>("TimeSheetDeps"); }
        }


        public HrmTimeSheetGroup(Session session) : base(session) { }
        public override void AfterConstruction() {  base.AfterConstruction(); }
    }
}
