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
using DevExpress.Persistent.Base.General;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
//
using IntecoAG.ERM.HRM.Organization;

namespace NpoMash.Erm.Hrm.Salary {


    public enum HrmSalaryPeriodObjectStatus { }

  
    abstract public class HrmSalaryPeriodObject : BaseObject, ITreeNode {

       
        private HrmSalaryPeriodObjectStatus _ObjectStatus;
        public virtual HrmSalaryPeriodObjectStatus ObjectStatus {
            get { return _ObjectStatus; }
            set { SetPropertyValue<HrmSalaryPeriodObjectStatus>("ObjectStatus", ref _ObjectStatus, value); }
        }

        public virtual Type ObjectType {
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

        public virtual IBindingList Children {
            get { return new BindingList<HrmSalaryPeriodObject>(); }
        }

        public virtual string Name {
            get { return ObjectType.FullName; }
        }

        public virtual ITreeNode Parent {
            get { return null; }
        }
    }

    // abstract//
    public  class HrmSalaryPeriodObjectBase : HrmSalaryPeriodObject {

        [Association("ObjectBase-ObjectSlices")] //Коллекция ObjectSlice
        public XPCollection<HrmSalaryPeriodObjectSlice> ObjectSlices {
            get { return GetCollection<HrmSalaryPeriodObjectSlice>("ObjectSlices"); }
        }


        public override IBindingList Children {
            get {
                return new BindingList<HrmSalaryPeriodObjectSlice>(ObjectSlices);
            }
        }

        public HrmSalaryPeriodObjectBase(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }


    //abstract
    public  class HrmSalaryPeriodObjectSlice : HrmSalaryPeriodObject {

        private HrmSalaryPeriodObjectBase _ObjectBase;
        [Association("ObjectBase-ObjectSlices")]
        public HrmSalaryPeriodObjectBase ObjectBase {
            get { return _ObjectBase; }
            set { SetPropertyValue<HrmSalaryPeriodObjectBase>("ObjectBase", ref _ObjectBase, value); }
        
        }


        public override ITreeNode Parent {
            get {
                return ObjectBase;
            }
        }



        public HrmSalaryPeriodObjectSlice(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }






}
