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

namespace IntecoAG.Erm.HRM
{

    [Persistent("HrmSalaryPayType")]

    public class HrmSalaryPayType : BaseObject
    {
        public HrmSalaryPayType(Session session) : base(session) { }

        private String _Code;
        private String _Name;

        public String Code
        {
            get { return _Code; }
            set { SetPropertyValue<String>("Code", ref _Code, value); }
        }

        public String Name
        {
            get { return _Name; }
            set { SetPropertyValue<String>("Name", ref _Name, value); }
        }


        public override void AfterConstruction()
        {
            base.AfterConstruction();

        }



    }
}
