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

    [Persistent("HrmSalaryAllocParameterDepartmentControl")]
    public class HrmAllocParameterDepartmentControl : XPObject {


        private String _BuhCode;
        public String BuhCode {
            get { return _BuhCode; }
            set { SetPropertyValue<String>("BuhCode", ref _BuhCode, value); }
        }

        private DepartmentGroupDep _Group;
        public DepartmentGroupDep Group {
            get { return _Group; }
            set { SetPropertyValue<DepartmentGroupDep>("Group", ref _Group, value); }
        }


        private Department _Department;
        public Department Department {
            get { return _Department; }
            set { SetPropertyValue<Department>("Department", ref _Department, value); }
        }

        private HrmAllocParameter _AllocParameter;
        [Association("AllocParameter-DepartmentControl")]// סגÿחü ס HrmPeriodAllocParameter
        public HrmAllocParameter AllocParameter {
            get { return _AllocParameter; }
            set { SetPropertyValue<HrmAllocParameter>("AllocParameter", ref _AllocParameter, value); }
        }

        public HrmAllocParameterDepartmentControl(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }
}
