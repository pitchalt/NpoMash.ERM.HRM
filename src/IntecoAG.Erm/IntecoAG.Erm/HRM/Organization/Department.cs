using System;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;
//
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace IntecoAG.ERM.HRM.Organization
{
    public enum DepartmentGroupDep { 
        DEPARTMENT_KB = 0, 
        DEPARTMENT_OZM = 1,
        DEPARTMENT_KB_OZM = 2
    }

    [Persistent("Department")]
    [NavigationItem("ERM")]
    [DefaultProperty("Code")]
    public class Department : BaseObject
    {
        private Boolean _IsClosed;
        [VisibleInLookupListView(false)]
        [VisibleInDetailView(false)]
        public Boolean IsClosed {
            get { return _IsClosed; }
            set { SetPropertyValue<Boolean>("IsClosed", ref _IsClosed, value); }
        }

        private String _Code;
        public String Code {
            get { return _Code; }
            set { SetPropertyValue<String>("Code", ref _Code, value); } 
        }

        private String _BuhCode;
        public String BuhCode {
            get { return _BuhCode; }
            set { SetPropertyValue<String>("BuhCode", ref _BuhCode, value); }
        }

        private DepartmentGroupDep _GroupDep;
        public DepartmentGroupDep GroupDep {
            get { return _GroupDep; }
            set { SetPropertyValue<DepartmentGroupDep>("GroupDep", ref _GroupDep, value); } 
        }
 
        public Department(Session session) : base(session){ }
        public override void AfterConstruction()
        { base.AfterConstruction(); GroupDep = DepartmentGroupDep.DEPARTMENT_KB; }        
    }
}