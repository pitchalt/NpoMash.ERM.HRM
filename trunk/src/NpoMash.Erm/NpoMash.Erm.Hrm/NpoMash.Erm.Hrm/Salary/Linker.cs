using System;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;
//
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
//
using IntecoAG.Erm.HRM;
namespace NpoMash.Erm.Hrm.Salary {
    
    [Persistent("Linker")]
    public class Linker : BaseObject {
        private HrmPeriodAllocParameter _HrmPeriodAllocParameter;
        [Association("PeriodAllocParameters-HrmPeriodPayType")]// связь с HrmPeriodAllocParameter
        public HrmPeriodAllocParameter PeriodAllocParameters {
            get { return _HrmPeriodAllocParameter; }
            set { SetPropertyValue<HrmPeriodAllocParameter>("PeriodAllocParameters", ref _HrmPeriodAllocParameter, value); }
        }


        private HrmSalaryPayType _PeriodPayType;  //Связь с HrmSalaryPayType
        public HrmSalaryPayType HrmSalaryPayType {
            get { return _PeriodPayType; }
            set { SetPropertyValue<HrmSalaryPayType>("HrmSalaryPayType", ref _PeriodPayType, value); }
        }


        public Linker(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }

    }
}
