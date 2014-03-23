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
//
using IntecoAG.ERM.HRM.Organization;

namespace NpoMash.Erm.Hrm.Salary {


    public enum HrmSalaryPeriodObjectStatus { }

    public enum HrmSalaryPeriodObjectType { }


    abstract public class HrmSalaryPeriodObject : BaseObject {

       
        private virtual HrmSalaryPeriodObjectStatus _Status;
        public virtual HrmSalaryPeriodObjectStatus Status {
            get { return _Status; }
            set { SetPropertyValue<HrmSalaryPeriodObjectStatus>("Status", ref _Status, value); }
        }

         [Persistent("Status")]
        private HrmSalaryPeriodObjectType _Type;
                [PersistentAlias("_Status")]
        private HrmSalaryPeriodObjectType Type {
            get { return _Type; }
            set { SetPropertyValue<HrmSalaryPeriodObjectType>("Type", ref _Type, value); }
        }

        private DepartmentGroupDep _GroupDep;
        public DepartmentGroupDep GroupDep {
            get { return _GroupDep; }
            set { SetPropertyValue<DepartmentGroupDep>("GroupDep", ref _GroupDep, value); }
        }




        private HrmPeriod _Period; // —сылка на HrmPeriod
        [Association("Period-SalaryObject")]
        public HrmPeriod Period {
            get { return _Period; }
            set { SetPropertyValue<HrmPeriod>("Period", ref _Period, value); }
        }

        
        
        
        
        
        public HrmSalaryPeriodObject(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }
}
