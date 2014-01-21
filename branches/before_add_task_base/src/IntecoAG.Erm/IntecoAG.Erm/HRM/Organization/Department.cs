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
    public enum DEPARTMENT_GROUP_DEP { KB = 0, OZM = 1 }

    [Persistent("Department")]
    [NavigationItem("ERM")]
    [DefaultProperty("Code")]
    public class Department : BaseObject
    {
        private String _Code;
        public String Code {
            get { return _Code; }
            set { SetPropertyValue<String>("Code", ref _Code, value); } }

        private DEPARTMENT_GROUP_DEP _GroupDep;
        public DEPARTMENT_GROUP_DEP GroupDep {
            get { return _GroupDep; }
            set { SetPropertyValue<DEPARTMENT_GROUP_DEP>("GroupDep", ref _GroupDep, value); } }
 
        public Department(Session session) : base(session){ }
        public override void AfterConstruction()
        { base.AfterConstruction(); GroupDep = DEPARTMENT_GROUP_DEP.KB; }
        
    }
}
