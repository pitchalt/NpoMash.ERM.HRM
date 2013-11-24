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
    [DefaultClassOptions]
    
    public class HrmPeriodOrderControl : BaseObject
    { 
        public HrmPeriodOrderControl(Session session) : base(session) { }

        private string _TypeControl;
        private int _NormKB;
        private int _NormOZM;
        private fmCOrder _Order;
        private HrmPeriodAllocParameter _PeriodAllocParameter;

        public string TypeControl
        {
            get { return _TypeControl; }
            set { SetPropertyValue("Type_Control", ref _TypeControl, value); }
        }

        public int NormKB
        {
            get { return _NormKB; }
            set { SetPropertyValue("Norm_KB", ref _NormKB, value); }
        }

        public int NormOZM
        {
            get { return _NormOZM; }
            set { SetPropertyValue("Norm_OZM", ref _NormOZM, value); }
        }


        //////////////////////Связи


        public fmCOrder Order // связь с FmCOrder
        {
            get { return _Order; }
            set { SetPropertyValue<fmCOrder>("Order", ref _Order, value); }
        }



        [Association("PeriodAllocParameters-OrderControls")]// связь с HrmPeriodAllocParameter
        public HrmPeriodAllocParameter PeriodAllocParameter
        {
            get { return _PeriodAllocParameter; }
            set { SetPropertyValue<HrmPeriodAllocParameter>("PeriodAllocParameters", ref _PeriodAllocParameter, value); }
        }


        public override void AfterConstruction()
        {
            base.AfterConstruction();
           
        }
      

    }
}
