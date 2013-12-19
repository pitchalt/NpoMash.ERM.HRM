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

namespace IntecoAG.Erm.FM.Order
{


    public enum fmCOrderTypeCOntrol
    {
        TrudEmk_FOT = 1,
        FOT = 2,
        No_Ordered = 3
    }
    public enum fmCOrdertypeConstancy { Null = 1, One = 2 }

    [Persistent("fmCOrder")]
    [DefaultProperty("Code")]
    [NavigationItem("ERM")]
    public class fmCOrder : BaseObject
    {
        
        private String _Code;
        public String Code {
               get { return _Code; }
               set { SetPropertyValue<string>("Code", ref _Code, value); } }

        private fmCOrderTypeCOntrol _TypeControl;
        public fmCOrderTypeCOntrol TypeControl{
               get { return _TypeControl; }
               set {
                   SetPropertyValue<fmCOrderTypeCOntrol>("TypeControl", ref _TypeControl, value);
                   if (IsSaving) { }
               } 
        
        
        }

        private fmCOrdertypeConstancy _TypeConstancy;
        public fmCOrdertypeConstancy TypeConstancy {
               get { return _TypeConstancy; }
            set { SetPropertyValue<fmCOrdertypeConstancy>("TypeConstancy", ref _TypeConstancy, value); } }

        private Decimal _NormKB;
        [RuleValueComparison(null, DefaultContexts.Save, ValueComparisonType.NotEquals, 0, TargetCriteria = "TypeControl != 'No_Ordered'")]
        [ModelDefault("DisplayFormat", "{0:N}")]
        public Decimal NormKB {
               get { return _NormKB; }
               set { SetPropertyValue<Decimal>("NormKB", ref _NormKB, value); } }

        private Decimal _NormOZM;
        [RuleValueComparison(null, DefaultContexts.Save, ValueComparisonType.NotEquals, 0, TargetCriteria = "TypeControl != 'No_Ordered'")]
        [ModelDefault("DisplayFormat", "{0:N}")]
        public Decimal NormOZM {
               get { return _NormOZM; }
               set { SetPropertyValue<Decimal>("NormOZM", ref _NormOZM, value); } }


        private void check() {  }



        public fmCOrder(Session session) : base(session) { }
        public override void AfterConstruction()
        {  base.AfterConstruction();
        TypeControl = fmCOrderTypeCOntrol.FOT;
        TypeConstancy = fmCOrdertypeConstancy.One;
        }

        
    }
}
