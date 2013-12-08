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

namespace NpoMash.Erm.Hrm.Salary
{
    [Persistent("AllocParametersLogic")]
    public class AllocParametersLogic : BaseObject {

        public static void createParameters(IObjectSpace os,HrmPeriodAllocParameter obj ) { }
        public static void acceptParameters(IObjectSpace os, HrmPeriodAllocParameter obj) { }






        public AllocParametersLogic(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
       
    }
}
