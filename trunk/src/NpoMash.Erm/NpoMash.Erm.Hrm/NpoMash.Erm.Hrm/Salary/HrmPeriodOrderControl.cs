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

        public enum HrmPeriodOrderTypeControl {
            TrudEmk_FOT = 1,
            FOT = 2,
            No_Ordered = 3}

        private HrmPeriodOrderTypeControl _TypeControl;
        public HrmPeriodOrderTypeControl TypeControl{
            get { return _TypeControl; }
            set { SetPropertyValue<HrmPeriodOrderTypeControl>("TypeControl", ref _TypeControl, value); } }

        private Decimal _NormKB;
        public Decimal NormKB {
               get { return _NormKB; }
               set { SetPropertyValue<Decimal>("NormKB", ref _NormKB, value); } }

        private Decimal _NormOZM;
        public Decimal NormOZM {
               get { return _NormOZM; }
               set { SetPropertyValue<Decimal>("NormOZM", ref _NormOZM, value); } }

        //////////////////////Связи

        // связь с FmCOrder
        private fmCOrder _Order;
        public fmCOrder Order{
               get { return _Order; }
               set { SetPropertyValue<fmCOrder>("Order", ref _Order, value); }}

        private HrmPeriodAllocParameter _AllocParameter;
        [Association("AllocParameter-OrderControls")]// связь с HrmPeriodAllocParameter
        public HrmPeriodAllocParameter AllocParameter
        {
            get { return _AllocParameter; }
            set { SetPropertyValue<HrmPeriodAllocParameter>("AllocParameter", ref _AllocParameter, value); }
        }


        public HrmPeriodOrderControl(Session session) : base(session) { }
        public override void AfterConstruction()
        { base.AfterConstruction();
        TypeControl = HrmPeriodOrderTypeControl.FOT;
        }

    }
}
