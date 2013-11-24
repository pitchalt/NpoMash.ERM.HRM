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
    [DefaultClassOptions]

    public enum TypeControl
    { }

    public enum TypeConstancy
    { }


    public class fmCOrder : BaseObject
    {
        public fmCOrder(Session session) : base(session) { }

        private string _Code;
        private TypeControl _TypeControl;
        private TypeConstancy _TypeConstancy;
        private decimal _NormKB;
        private decimal _NormOZM;

        public string Code
        {
            get { return _Code; }
            set { SetPropertyValue<string>("Code", ref _Code, value); }
        }

        public TypeControl TypeControl
        {
            get { return _TypeControl; }
            set { SetPropertyValue<TypeControl>("Type_Control", ref _TypeControl, value); }
        }

        public TypeConstancy TypeConstancy
        {
            get { return _TypeConstancy; }
            set { SetPropertyValue<TypeConstancy>("Type_Constancy", ref _TypeConstancy, value); }
        }

        public decimal NormKB
        {
            get { return _NormKB; }
            set { SetPropertyValue<decimal>("Norm_KB", ref _NormKB, value); }
        }

        public decimal NormOZM
        {
            get { return _NormOZM; }
            set { SetPropertyValue<decimal>("Norm_OZM", ref _NormOZM, value); }
        }



        public override void AfterConstruction()
        {
            base.AfterConstruction();

        }
    }
}
