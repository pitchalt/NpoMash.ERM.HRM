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
    public class fmCOrder : BaseObject
    {
        
        private String _Code;
        public String Code {
               get { return _Code; }
               set { SetPropertyValue<string>("Code", ref _Code, value); } }

        private fmCOrderTypeCOntrol _TypeControl;
        public fmCOrderTypeCOntrol TypeControl{
               get { return _TypeControl; }
            set { SetPropertyValue<fmCOrderTypeCOntrol>("TypeControl", ref _TypeControl, value); } }

        private fmCOrdertypeConstancy _TypeConstancy;
        public fmCOrdertypeConstancy TypeConstancy {
               get { return _TypeConstancy; }
            set { SetPropertyValue<fmCOrdertypeConstancy>("TypeConstancy", ref _TypeConstancy, value); } }

        private Decimal _NormKB;
        public Decimal NormKB {
               get { return _NormKB; }
               set { SetPropertyValue<Decimal>("NormKB", ref _NormKB, value); } }

        private Decimal _NormOZM;
        public Decimal NormOZM {
               get { return _NormOZM; }
               set { SetPropertyValue<Decimal>("NormOZM", ref _NormOZM, value); } }

        private Decimal _NormNoControl;
        public Decimal NormNoControl {
            get { return _NormNoControl; }
            set { SetPropertyValue<Decimal>("NormNoControl", ref _NormNoControl, value); } }



        public fmCOrder(Session session) : base(session) { }
        public override void AfterConstruction()
        {  base.AfterConstruction();
        TypeControl = fmCOrderTypeCOntrol.FOT;
        TypeConstancy = fmCOrdertypeConstancy.One; }

    }
}
