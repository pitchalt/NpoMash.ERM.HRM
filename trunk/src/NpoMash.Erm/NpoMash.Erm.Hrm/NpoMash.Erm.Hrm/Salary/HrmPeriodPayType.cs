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
using IntecoAG.ERM.HRM;
namespace NpoMash.Erm.Hrm.Salary {

    [RuleCombinationOfPropertiesIsUnique("", DefaultContexts.Save, "AllocParameter, PayType")]
    [Persistent("HrmPeriodPayType")]
    public class HrmPeriodPayType : BaseObject {

        private HrmPeriodAllocParameter _AllocParameter;
        [Association("AllocParameter-PeriodPayTypes")]// связь с HrmPeriodAllocParameter
        public HrmPeriodAllocParameter AllocParameter {
            get { return _AllocParameter; }
            set { SetPropertyValue<HrmPeriodAllocParameter>("AllocParameter", ref _AllocParameter, value); }
        }


        private HrmSalaryPayType _PayType;  //Связь с HrmSalaryPayType
        [Indexed("AllocParameter", Unique = true)]
        [RuleRequiredField(DefaultContexts.Save)]
        public HrmSalaryPayType PayType {
            get { return _PayType; }
            set { SetPropertyValue<HrmSalaryPayType>("PayType", ref _PayType, value); }
        }


        public HrmPeriodPayType(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }

    }
}
