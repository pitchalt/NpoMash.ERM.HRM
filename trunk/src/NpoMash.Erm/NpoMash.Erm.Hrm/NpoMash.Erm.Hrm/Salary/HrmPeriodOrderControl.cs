using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
//
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
//
using IntecoAG.Erm.FM.Order;
//
namespace NpoMash.Erm.Hrm.Salary
{
   
    [Persistent("HrmPeriodOrderControl")]
    public class HrmPeriodOrderControl : BaseObject
    { 
       

        private String _TypeControl;
        private Decimal _NormKB;
        private Decimal _NormOZM;
        private fmCOrder _Order;
        private HrmPeriodAllocParameter _PeriodAllocParameter;

        public String TypeControl
        {
            get { return _TypeControl; }
            set { SetPropertyValue<String>("TypeControl", ref _TypeControl, value); }
        }

        public Decimal NormKB
        {
            get { return _NormKB; }
            set { SetPropertyValue<Decimal>("NormKB", ref _NormKB, value); }
        }

        public Decimal NormOZM
        {
            get { return _NormOZM; }
            set { SetPropertyValue<Decimal>("NormOZM", ref _NormOZM, value); }
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
            set { SetPropertyValue<HrmPeriodAllocParameter>("PeriodAllocParameter", ref _PeriodAllocParameter, value); }
        }

        public HrmPeriodOrderControl(Session session) : base(session) { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
           
        }
      

    }
}
