using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
//
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
//

namespace NpoMash.Erm.Hrm.Salary {

    public class HrmSalaryObjectSlice : HrmSalaryPeriodObject {

        private HrmSalaryObject _SalaryObject; //—сылка на HrmSalaryObject
        [Association("SalaryObject-ObjectSlice")]
        public HrmSalaryObject SalaryObject {
            get { return _SalaryObject; }
            set { SetPropertyValue<HrmSalaryObject>("SalaryObject", ref _SalaryObject, value); }
        
        }



        public HrmSalaryObjectSlice(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }
}
