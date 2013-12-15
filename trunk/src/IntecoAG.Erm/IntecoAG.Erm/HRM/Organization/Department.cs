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

namespace IntecoAG.Erm.HRM.Organization
{
    public enum DepartmentGroupDep { KB = 0, OZM = 1 }

    [Persistent("Department")]
    public class Department : BaseObject
    {
        private String _Code;
        public String Code {
            get { return _Code; }
            set { SetPropertyValue<String>("Code", ref _Code, value); } }

        private DepartmentGroupDep _GroupDep;
        public DepartmentGroupDep GroupDep {
            get { return _GroupDep; }
            set { SetPropertyValue<DepartmentGroupDep>("GroupDep", ref _GroupDep, value); } }
 
        public Department(Session session) : base(session){ }
        public override void AfterConstruction()
        { base.AfterConstruction(); GroupDep = DepartmentGroupDep.KB; }
        
    }
}
