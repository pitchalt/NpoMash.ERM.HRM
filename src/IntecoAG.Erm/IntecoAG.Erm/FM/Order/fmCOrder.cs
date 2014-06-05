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
using DevExpress.ExpressApp.ConditionalAppearance;

namespace IntecoAG.ERM.FM.Order {


    public enum FmCOrderTypeControl {
        TRUDEMK_FOT = 1,
        FOT = 2,
        NO_ORDERED = 3
    }
    public enum FmCOrderTypeConstancy {
        UN_CONST_ORDER_TYPE = 0,
        CONST_ORDER_TYPE = 1
    }

    public enum FmCOrderAllowToTurf { }
    public enum FmCOrderStatus { }

    [Persistent("fmCOrder")]
    [DefaultProperty("Code")]
    [NavigationItem("ERM")]
    [RuleCriteria("", DefaultContexts.Save, "NormKB>= 0 and NormOZM>= 0", CustomMessageTemplate = "Значения НормаКБ или НормаОЗМ не должны быть меньше нуля.")]
    public class fmCOrder : BaseObject {

        private FmCOrderStatus _Status;
        public FmCOrderStatus Status {
            get { return _Status; }
            set { SetPropertyValue<FmCOrderStatus>("Status", ref _Status, value); }
        }

        private FmCOrderAllowToTurf _AllowTo;
        public FmCOrderAllowToTurf AllowTo {
            get { return _AllowTo; }
            set { SetPropertyValue<FmCOrderAllowToTurf>("AllowTo", ref _AllowTo, value); }
        }


        private String _Code;
        public String Code {
            get { return _Code; }
            set { SetPropertyValue<string>("Code", ref _Code, value); }
        }

        private FmCOrderTypeControl _TypeControl;
        public FmCOrderTypeControl TypeControl {
            get { return _TypeControl; }
            set {
                SetPropertyValue<FmCOrderTypeControl>("TypeControl", ref _TypeControl, value);
                if (IsSaving) { }
            }


        }

        private FmCOrderTypeConstancy _TypeConstancy;
        public FmCOrderTypeConstancy TypeConstancy {
            get { return _TypeConstancy; }
            set { SetPropertyValue<FmCOrderTypeConstancy>("TypeConstancy", ref _TypeConstancy, value); }
        }

        private Decimal _NormKB;
        [RuleValueComparison(null, DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0, TargetCriteria = "TypeControl != 'No_Ordered'")]
        [ModelDefault("DisplayFormat", "{0:N}")]
        public Decimal NormKB {
            get { return _NormKB; }
            set { SetPropertyValue<Decimal>("NormKB", ref _NormKB, value); }
        }

        private Decimal _NormOZM;
        [RuleValueComparison(null, DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0, TargetCriteria = "TypeControl != 'No_Ordered'")]
        [ModelDefault("DisplayFormat", "{0:N}")]
        public Decimal NormOZM {
            get { return _NormOZM; }
            set { SetPropertyValue<Decimal>("NormOZM", ref _NormOZM, value); }
        }


        public fmCOrder(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            TypeControl = FmCOrderTypeControl.FOT;
            TypeConstancy = FmCOrderTypeConstancy.CONST_ORDER_TYPE;
        }
    }
}
