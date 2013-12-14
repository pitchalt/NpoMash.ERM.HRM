using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using System.ComponentModel;
using System.Collections.Generic;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace IntecoAG.Erm.HRM
{

    [Persistent("HrmSalaryPayType")]
    [DefaultProperty("Code")]
    public class HrmSalaryPayType : BaseObject
    {
       
        private String _Code;
        public String Code {
               get { return _Code; }
               set { SetPropertyValue<String>("Code", ref _Code, value); } }

        private String _Name;
        public String Name {
               get { return _Name; }
               set { SetPropertyValue<String>("Name", ref _Name, value); } }

        public HrmSalaryPayType(Session session) : base(session) { }
        public override void AfterConstruction()
        { base.AfterConstruction(); }



    }
}