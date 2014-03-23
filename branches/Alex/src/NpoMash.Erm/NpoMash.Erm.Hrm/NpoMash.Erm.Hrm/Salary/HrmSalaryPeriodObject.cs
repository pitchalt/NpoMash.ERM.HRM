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


    abstract public class HrmSalaryPeriodObject : BaseObject {

       
        private HrmSalaryPeriodObjectStatus _Status;
        public virtual HrmSalaryPeriodObjectStatus Status {
            get { return _Status; }
            set { SetPropertyValue<HrmSalaryPeriodObjectStatus>("Status", ref _Status, value); }
        }

        public virtual Type Type {
            get { return typeof(HrmSalaryPeriodObject); }
        }

        private DepartmentGroupDep _GroupDep;
        public DepartmentGroupDep GroupDep {
            get { return _GroupDep; }
            set { SetPropertyValue<DepartmentGroupDep>("GroupDep", ref _GroupDep, value); }
        }




        private HrmPeriod _Period; // Ссылка на HrmPeriod
        [Association("Period-SalaryObject")]
        public HrmPeriod Period {
            get { return _Period; }
            set { SetPropertyValue<HrmPeriod>("Period", ref _Period, value); }
        }

        
        
        
        
        
        public HrmSalaryPeriodObject(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

    public abstract class HrmSalaryPeriodObjectBase : HrmSalaryPeriodObject {

        [Association("ObjectBase-ObjectSlice")] //Коллекция ObjectSlice
        public XPCollection<HrmSalaryPeriodObjectSlice> ObjectSlice {
            get { return GetCollection<HrmSalaryPeriodObjectSlice>("ObjectSlice"); }
        }

        public HrmSalaryPeriodObjectBase(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

    public abstract class HrmSalaryPeriodObjectSlice : HrmSalaryPeriodObject {


        private HrmSalaryPeriodObjectBase _ObjectBase;
        [Association("ObjectBase-ObjectSlice")]
        public HrmSalaryPeriodObjectBase ObjectBase {
            get { return _ObjectBase; }
            set { SetPropertyValue<HrmSalaryPeriodObjectBase>("ObjectBase", ref _ObjectBase, value); }
        
        }

        public HrmSalaryPeriodObjectSlice(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }






}
