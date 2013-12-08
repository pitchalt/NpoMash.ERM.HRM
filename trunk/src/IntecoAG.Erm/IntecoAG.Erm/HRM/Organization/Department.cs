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

namespace IntecoAG.Erm.HRM.Organization
{
    [Persistent("Department")]
    public class Department : BaseObject
    {
        private String _Code;
        public String Code {
            get { return _Code; }
            set { SetPropertyValue<String>("Code", ref _Code, value); } }


        public enum DepartmentGroupDep { KB = 1, OZM = 2 }
        private DepartmentGroupDep _GroupDep;
        public DepartmentGroupDep GroupDep {
            get { return _GroupDep; }
            set { SetPropertyValue<DepartmentGroupDep>("GroupDep", ref _GroupDep, value); } }
 
        public Department(Session session) : base(session){ }
        public override void AfterConstruction()
        { base.AfterConstruction(); GroupDep = DepartmentGroupDep.KB; }
        
    }
}