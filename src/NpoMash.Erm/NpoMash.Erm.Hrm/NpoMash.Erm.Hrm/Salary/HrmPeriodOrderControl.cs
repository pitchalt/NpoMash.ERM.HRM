using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
//
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
//
using IntecoAG.ERM.FM.Order;
//
namespace NpoMash.Erm.Hrm.Salary {

    [Persistent("HrmPeriodOrderControl")]
    [Appearance("Enable", TargetItems = "TypeControl", Criteria = "AllocParameter.Status=='ListOfOrderAccepted'", Context = "Any", Enabled = false)] //5
    [RuleCombinationOfPropertiesIsUnique("", DefaultContexts.Save, "Order, AllocParameter")]
    [Appearance("Visibility", AppearanceItemType = "Action", TargetItems = "Delete", Context = "Any", Criteria = "AllocParameter.Status == 'ListOfOrderAccepted' and TypeControl == 'TrudEmk_FOT'", Visibility = ViewItemVisibility.Hide)]
    [RuleCriteria("", DefaultContexts.Save, "NormKB>= 0 and NormOZM>= 0", CustomMessageTemplate="Значения НормаКБ или НормаОЗМ не должны быть меньше нуля.")]
    public class HrmPeriodOrderControl : BaseObject {


        private FmCOrderTypeControl _TypeControl;
        [RuleValueComparison(DefaultContexts.Save, ValueComparisonType.NotEquals, FmCOrderTypeControl.TRUDEMK_FOT, TargetCriteria = "RuleMethod")]
        public FmCOrderTypeControl TypeControl {
            get { return _TypeControl; }
            set {
                SetPropertyValue<FmCOrderTypeControl>("TypeControl", ref _TypeControl, value);
            }
        }

        private Decimal _NormKB;
        [RuleValueComparison(null, DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0, TargetCriteria = "TypeControl != 'No_Ordered'")]
        [RuleRequiredField(DefaultContexts.Save)]
        [ModelDefault("DisplayFormat", "{0:N}")]
        public Decimal NormKB {
            get { return _NormKB; }
            set { SetPropertyValue<Decimal>("NormKB", ref _NormKB, value); }
        }

        private Decimal _NormOZM;
        [RuleValueComparison(null, DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0, TargetCriteria = "TypeControl != 'No_Ordered'")]
        // [RuleValueComparison(null, DefaultContexts.Save, ValueComparisonType.GreaterThan, 0)]
        [ModelDefault("DisplayFormat", "{0:N}")]
        public Decimal NormOZM {
            get { return _NormOZM; }
            set { SetPropertyValue<Decimal>("NormOZM", ref _NormOZM, value); }
        }



        [Browsable(false)]
        public bool RuleMethod {
            get {
                return AllocParameter.Status == HrmPeriodAllocParameterStatus.LIST_OF_ORDER_ACCEPTED && TypeControl == FmCOrderTypeControl.TRUDEMK_FOT && Session.IsNewObject(this);
            }
        }
        //////////////////////Связи

        // связь с FmCOrder
        private fmCOrder _Order;
        [Indexed("AllocParameter", Unique = true)]
        [RuleRequiredField(DefaultContexts.Save)]
        public fmCOrder Order {
            get { return _Order; }
            set {
                SetPropertyValue<fmCOrder>("Order", ref _Order, value);
                if (!IsLoading && value != null) {
                    TypeControl = FmCOrderTypeControl.FOT;
                    NormKB = value.NormKB;
                    NormOZM = value.NormOZM;
                }
            }
        }

        private HrmPeriodAllocParameter _AllocParameter;
        [Association("AllocParameter-OrderControls")]// связь с HrmPeriodAllocParameter
        public HrmPeriodAllocParameter AllocParameter {
            get { return _AllocParameter; }
            set {
                SetPropertyValue<HrmPeriodAllocParameter>("AllocParameter", ref _AllocParameter, value);

            }
        }

        public HrmPeriodOrderControl(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            TypeControl = FmCOrderTypeControl.FOT;
        }

    }
}