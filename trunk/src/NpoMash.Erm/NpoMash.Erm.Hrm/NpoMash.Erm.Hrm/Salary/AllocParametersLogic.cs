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
    public static class AllocParametersLogic {

        public static void createParameters(IObjectSpace os,HrmPeriodAllocParameter obj ) {
            HrmPeriod last_period = HrmPeriodLogic.findLastPeriod(os);
            if (last_period != null && last_period.Status == HrmPeriodStatus.Opened)
            {
                if (last_period.CurrentAllocParameter != null){
                    if (last_period.CurrentAllocParameter.Status == HrmPeriodAllocParameterStatus.OpenToEdit ||
                        last_period.CurrentAllocParameter.Status == HrmPeriodAllocParameterStatus.ListOfOrderAccepted)
                        throw new Exception("”же есть параметры, открытые дл€ редактировани€");
                    if (last_period.CurrentAllocParameter.Status == HrmPeriodAllocParameterStatus.AllocParametersAccepted)
                        throw new Exception("ѕараметры дл€ текущего периода уже утверждены");
                }
                else
                    throw new Exception("≈сть открытый период без параметров");
                    //throw new Exception("ѕоследний период не закрыт");
            HrmPeriod current_period = HrmPeriodLogic.createPeriod(os);
            }
            
        }


        public static void acceptParameters(IObjectSpace os, HrmPeriodAllocParameter alloc_parameter) {
            alloc_parameter.Status = HrmPeriodAllocParameterStatus.AllocParametersAccepted;
        }


    }//end of AllocParametersLogic class
}//end of namespace
