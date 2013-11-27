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
 
    public enum TypeControl
    { }

    public enum TypeConstancy
    { }

    [Persistent("fmCOrder")]

    public class fmCOrder : BaseObject
    {
        
        private String _Code;
        public String Code {
               get { return _Code; }
               set { SetPropertyValue<string>("Code", ref _Code, value); } }

        private TypeControl _TypeControl;
        public TypeControl TypeControl {
               get { return _TypeControl; }
               set { SetPropertyValue<TypeControl>("TypeControl", ref _TypeControl, value); } }

        private TypeConstancy _TypeConstancy;
        public TypeConstancy TypeConstancy {
               get { return _TypeConstancy; }
               set { SetPropertyValue<TypeConstancy>("TypeConstancy", ref _TypeConstancy, value); } }

        private Decimal _NormKB;
        public Decimal NormKB {
               get { return _NormKB; }
               set { SetPropertyValue<Decimal>("NormKB", ref _NormKB, value); } }

        private Decimal _NormOZM;
        public Decimal NormOZM {
               get { return _NormOZM; }
               set { SetPropertyValue<Decimal>("NormOZM", ref _NormOZM, value); } }


        public fmCOrder(Session session) : base(session) { }
        public override void AfterConstruction()
        {  base.AfterConstruction(); }
    }
}
