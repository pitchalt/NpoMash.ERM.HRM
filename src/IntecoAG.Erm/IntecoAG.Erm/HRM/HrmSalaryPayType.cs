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
    [DefaultClassOptions]

    public class HrmSalaryPayType : BaseObject
    {
        public HrmSalaryPayType(Session session) : base(session) { }

        private string _Code;
        private string _Name;

        public string Code
        {
            get { return _Code; }
            set { SetPropertyValue<string>("Code", ref _Code, value); }
        }

        public string Name
        {
            get { return _Name; }
            set { SetPropertyValue<string>("Name", ref _Name, value); }
        }


        public override void AfterConstruction()
        {
            base.AfterConstruction();

        }



    }
}
