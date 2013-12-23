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



namespace NpoMash.Erm.Hrm.Salary {
    [Persistent("HrmSalaryTaskMatrixReduction")]
    [NavigationItem("A1 Integration")] 
     
    public class HrmSalaryTaskMatrixReduction : BaseObject {  public HrmSalaryTaskMatrixReduction(Session session) : base(session) { }

    private HrmPeriodAllocParameter _AllocParameter;
    public HrmPeriodAllocParameter AllocParameter {
        get { return _AllocParameter; }
        set { SetPropertyValue<HrmPeriodAllocParameter>("AllocParameter", ref _AllocParameter, value); }
    }

    private HrmMatrix _Matrix;
    public HrmMatrix Matrix {
        get { return _Matrix; }
        set { SetPropertyValue<HrmMatrix>("Matrix", ref _Matrix, value); }
    }
    private HrmPeriod _Period;
    public HrmPeriod Period {
        get { return _Period; }
        set { SetPropertyValue<HrmPeriod>("Period", ref _Period, value);}
    }

    [NonPersistent]
    public class ReducingCardCollection : XPCustomObject {
        private String _A;
        public String A {
            get { return _A; }
        }
    }

    IList<ReducingCardCollection> collection {
        get {
            return new List<ReducingCardCollection>();
        }
    }

    public override void AfterConstruction() {  base.AfterConstruction();  }
    }
}
