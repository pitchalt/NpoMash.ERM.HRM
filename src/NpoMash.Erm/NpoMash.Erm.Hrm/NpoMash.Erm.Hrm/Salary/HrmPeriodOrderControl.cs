using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
//
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
//
using IntecoAG.Erm.FM.Order;
//
namespace NpoMash.Erm.Hrm.Salary {

    [Persistent("HrmPeriodOrderControl")]

    [Appearance("Enable", TargetItems = "TypeControl", Criteria = "AllocParameter.Status=='ListOfOrderAccepted' and TypeControl=='TrudEmk_FOT'", Context = "Any", BackColor = "Green", FontColor = "White", Enabled = false)] //5
    [Appearance("En", TargetItems = "Order.TypeControl", Criteria = "AllocParameter.Status=='ListOfOrderAccepted'", Context = "Any", BackColor = "Green", FontColor = "White", Enabled = false)] //5

    public class HrmPeriodOrderControl : BaseObject {

        private fmCOrderTypeCOntrol _TypeControl;
        public fmCOrderTypeCOntrol TypeControl {
            get { return _TypeControl; }
            set {
                SetPropertyValue<fmCOrderTypeCOntrol>("TypeControl", ref _TypeControl, value);
            }
        }


        private Decimal _NormKB;
        //        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "TypeControl != 'No_Ordered'")]
        [RuleValueComparison(null, DefaultContexts.Save, ValueComparisonType.NotEquals, 0, TargetCriteria = "TypeControl != 'No_Ordered'")]
        public Decimal NormKB {
            get { return _NormKB; }
            set { SetPropertyValue<Decimal>("NormKB", ref _NormKB, value); }
        }

        private Decimal _NormOZM;
        //        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "TypeControl != 'No_Ordered'")]
        [RuleValueComparison(null, DefaultContexts.Save, ValueComparisonType.NotEquals, 0, TargetCriteria = "TypeControl != 'No_Ordered'")]
        public Decimal NormOZM {
            get { return _NormOZM; }
            set { SetPropertyValue<Decimal>("NormOZM", ref _NormOZM, value); }
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
                    TypeControl = value.TypeControl;
                    NormKB = value.NormKB;
                    NormOZM = value.NormOZM;
                }
            }
        }

        private HrmPeriodAllocParameter _AllocParameter;
        [Association("AllocParameter-OrderControls")]// связь с HrmPeriodAllocParameter
        public HrmPeriodAllocParameter AllocParameter {
            get { return _AllocParameter; }
            set { SetPropertyValue<HrmPeriodAllocParameter>("AllocParameter", ref _AllocParameter, value); }
        }


        public HrmPeriodOrderControl(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            TypeControl = fmCOrderTypeCOntrol.FOT;
        }

    }
}
