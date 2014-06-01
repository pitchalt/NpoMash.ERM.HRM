using System;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;
//
using DevExpress.Xpo;
using DevExpress.Xpo.Helpers;
//
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

    [Persistent("HrmPeriodObject")]
    abstract public class HrmPeriodObject : BaseObject, IPersistentInterfaceData<IPeriodObject> { //, ITreeNode  {

        //private HrmSalaryPeriodObjectStatus _Status;
        //public HrmSalaryPeriodObjectStatus Status {
        //    get { return _Status; }
        //    set { SetPropertyValue<HrmSalaryPeriodObjectStatus>("Status", ref _Status, value); }
        //}

        public String PeriodObjectStatus = "";

        public abstract Type PeriodObjectType { get; }

        private DepartmentGroupDep _GroupDep;
        public DepartmentGroupDep GroupDep {
            get { return _GroupDep; }
            set { SetPropertyValue<DepartmentGroupDep>("GroupDep", ref _GroupDep, value); }
        }

        private HrmPeriod _Period; // Ссылка на HrmPeriod
        /// <summary>
        /// Ссылка на период
        /// </summary>
        [Association("HrmPeriod-HrmPeriodObject")]
        public HrmPeriod Period {
            get { return _Period; }
            set { SetPropertyValue<HrmPeriod>("Period", ref _Period, value); }
        }

        public HrmPeriodObject(Session session) : base(session) { }
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


        public abstract IPeriodObject Instance { get; }

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
