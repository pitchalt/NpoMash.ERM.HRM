using System;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;
//
using DevExpress.Xpo;
using DevExpress.ExpressApp;
//using DevExpress.ExpressApp.DC;
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
    
    [Persistent("HrmSalaryPeriodObject")]
    abstract public class HrmSalaryPeriodObject : BaseObject { //, ITreeNode  {

       
        private HrmSalaryPeriodObjectStatus _ObjectStatus;
        public virtual HrmSalaryPeriodObjectStatus ObjectStatus {
            get { return _ObjectStatus; }
            set { SetPropertyValue<HrmSalaryPeriodObjectStatus>("ObjectStatus", ref _ObjectStatus, value); }
        }

        public virtual Type PeriodObjectType {
            get { return typeof(HrmSalaryPeriodObject); }
        }

        private DepartmentGroupDep _GroupDep;
        public DepartmentGroupDep GroupDep {
            get { return _GroupDep; }
            set { SetPropertyValue<DepartmentGroupDep>("GroupDep", ref _GroupDep, value); }
        }




        private HrmPeriod _PeriodBase; // Ссылка на HrmPeriod
        [Association("HrmPeriod-HrmPeriodSalaryObject")]
        public HrmPeriod PeriodBase {
            get { return _PeriodBase; }
            set { SetPropertyValue<HrmPeriod>("PeriodBase", ref _PeriodBase, value); }
        }

        
        public HrmSalaryPeriodObject(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

/*
        public virtual IBindingList Children {
            get { return new BindingList<HrmSalaryPeriodObject>(); }
        }

        public virtual string Name {
            get { return PeriodObjectType.FullName; }
        }

        public virtual ITreeNode Parent {
            get { return null; }
        }
*/

    }

    //// abstract//
    //[MapInheritance(MapInheritanceType.ParentTable)]
    //public  class HrmSalaryPeriodObjectBase : HrmSalaryPeriodObject {

    //    [Association("HrmSalaryPeriodObjectBase-HrmSalaryPeriodObjectSlice")] //Коллекция ObjectSlice
    //    public XPCollection<HrmSalaryPeriodObjectSlice> ObjectSlices {
    //        get { return GetCollection<HrmSalaryPeriodObjectSlice>("ObjectSlices"); }
    //    }


    //    //public override IBindingList Children {
    //    //    get {
    //    //       return new BindingList<HrmSalaryPeriodObjectSlice>(ObjectSlices);
    //    //    }
    //    //}


    //    /*[Association("SalaryObject-ObjectSlice"), Aggregated] //Коллекция HrmSalaryObjectSlice
    //    public XPCollection<HrmSalaryPeriodObjectSlice> ObjectSlice {
    //        get { return GetCollection<HrmSalaryPeriodObjectSlice>("ObjectSlice"); }
    //    }*/

    //    [Association("SalaryObject-Column"), Aggregated] //Коллекция HrmMatrixColumn
    //    public XPCollection<HrmMatrixColumn> Column {
    //        get { return GetCollection<HrmMatrixColumn>("Column"); }
    //    }

    //    [Association("SalaryObject-Row"), Aggregated] //Коллекция HrmMatrixRow
    //    public XPCollection<HrmMatrixRow> Row {
    //        get { return GetCollection<HrmMatrixRow>("Row"); }
    //    }

    //    public HrmSalaryPeriodObjectBase(Session session) : base(session) { }
    //    public override void AfterConstruction() { base.AfterConstruction(); }
    //}


    ////abstract
    //[MapInheritance(MapInheritanceType.ParentTable)]
    //public class HrmSalaryPeriodObjectSlice : HrmSalaryPeriodObject {

    //    private HrmSalaryPeriodObjectBase _ObjectBase;
    //    [Association("HrmSalaryPeriodObjectBase-HrmSalaryPeriodObjectSlice")]
    //    public HrmSalaryPeriodObjectBase ObjectBase {
    //        get { return _ObjectBase; }
    //        set { SetPropertyValue<HrmSalaryPeriodObjectBase>("ObjectBase", ref _ObjectBase, value); }
        
    //    }


    //    //public override ITreeNode Parent {
    //    //    get {
    //    //        return ObjectBase;
    //    //    }
    //    //}



    //    public HrmSalaryPeriodObjectSlice(Session session) : base(session) { }
    //    public override void AfterConstruction() { base.AfterConstruction(); }
    //}


}
