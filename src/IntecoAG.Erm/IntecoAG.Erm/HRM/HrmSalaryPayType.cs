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

namespace IntecoAG.ERM.HRM {

    public enum HrmPayTypes {
        PROVISION_CODE = 0,
        TRAVEL_CODE = 1,
        BASE_CODE = 2
    }

    [Persistent("HrmSalaryPayType")]
    [DefaultProperty("Code")]
    [NavigationItem("ERM")]
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

        private HrmPayTypes _Type;
        public HrmPayTypes Type {
            get { return _Type; }
            set { SetPropertyValue<HrmPayTypes>("Type", ref _Type, value); }
        }

        public HrmSalaryPayType(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }
}