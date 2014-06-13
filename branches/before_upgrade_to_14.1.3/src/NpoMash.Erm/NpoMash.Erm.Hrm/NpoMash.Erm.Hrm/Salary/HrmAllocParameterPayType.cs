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

    public enum HrmPayTypes {
        PROVISION_CODE = 0,
        TRAVEL_CODE = 1,
        BASE_CODE = 2
    }

    [RuleCombinationOfPropertiesIsUnique("", DefaultContexts.Save, "AllocParameter, PayType")]

    [Persistent("HrmSalaryAllocParameterPayType")]
    public class HrmAllocParameterPayType : XPObject {

        private HrmAllocParameter _AllocParameter;
        [Association("HrmPeriodAllocParameter-HrmPeriodPayType")]// ����� � HrmPeriodAllocParameter
        public HrmAllocParameter AllocParameter {
            get { return _AllocParameter; }
            set { SetPropertyValue<HrmAllocParameter>("AllocParameter", ref _AllocParameter, value); }
        }

        private HrmSalaryPayType _PayType;  //����� � HrmSalaryPayType
        [Indexed("AllocParameter", Unique = true)]
        [RuleRequiredField(DefaultContexts.Save)]
        public HrmSalaryPayType PayType {
            get { return _PayType; }
            set { SetPropertyValue<HrmSalaryPayType>("PayType", ref _PayType, value); }
        }

        private HrmPayTypes _Type;
        public HrmPayTypes Type {
            get { return _Type; }
            set { SetPropertyValue<HrmPayTypes>("Type", ref _Type, value); }
        }

        public HrmAllocParameterPayType(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            SetPropertyValue<HrmPayTypes>("Type", ref _Type, HrmPayTypes.PROVISION_CODE);
        }
    }
}