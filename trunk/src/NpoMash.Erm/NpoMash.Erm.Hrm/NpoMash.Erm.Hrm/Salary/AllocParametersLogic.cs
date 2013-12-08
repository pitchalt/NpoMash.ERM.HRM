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
using NpoMash.Erm.Hrm;

namespace NpoMash.Erm.Hrm.Salary
{
    //[Persistent("AllocParametersLogic")]
    public class AllocParametersLogic : BaseObject {

        public static void createParameters(IObjectSpace os,HrmPeriodAllocParameter obj ) {
            HrmPeriod current_period = HrmPeriodLogic.findLastPeriod(os);
            if (current_period.Status == HrmPeriodStatus.closed) throw new Exception("Последний период закрыт");
            
            current_period.CurrentAllocParameter != null &&
                current_period.CurrentAllocParameter.Status == HrmPeriodAllocParameterStatus.OpenToEdit){

            }
            
        }
        public static void acceptParameters(IObjectSpace os, HrmPeriodAllocParameter obj) { }






        public AllocParametersLogic(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
       
    }
}
