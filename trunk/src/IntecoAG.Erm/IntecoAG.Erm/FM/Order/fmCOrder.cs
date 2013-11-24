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

namespace IntecoAG.Erm.FM.Order
{
 
    public enum TypeControl
    { }

    public enum TypeConstancy
    { }

    [Persistent("fmCOrder")]

    public class fmCOrder : BaseObject
    {
        public fmCOrder(Session session) : base(session) { }

        private String _Code;
        private TypeControl _TypeControl;
        private TypeConstancy _TypeConstancy;
        private Decimal _NormKB;
        private Decimal _NormOZM;

        public String Code
        {
            get { return _Code; }
            set { SetPropertyValue<string>("Code", ref _Code, value); }
        }

        public TypeControl TypeControl
        {
            get { return _TypeControl; }
            set { SetPropertyValue<TypeControl>("TypeControl", ref _TypeControl, value); }
        }

        public TypeConstancy TypeConstancy
        {
            get { return _TypeConstancy; }
            set { SetPropertyValue<TypeConstancy>("TypeConstancy", ref _TypeConstancy, value); }
        }

        public Decimal NormKB
        {
            get { return _NormKB; }
            set { SetPropertyValue<Decimal>("NormKB", ref _NormKB, value); }
        }

        public Decimal NormOZM
        {
            get { return _NormOZM; }
            set { SetPropertyValue<Decimal>("NormOZM", ref _NormOZM, value); }
        }



        public override void AfterConstruction()
        {
            base.AfterConstruction();

        }
    }
}
