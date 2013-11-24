using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace NpoMash.Erm.Hrm.Salary
{
    using IntecoAG.Erm.HRM;

    public enum HrmPeriodAllocParameterStatus
    { }

    [Persistent("HrmPeriodAllocParameter")]

    public class HrmPeriodAllocParameter : BaseObject
    { 
       
        private HrmPeriodAllocParameterStatus _Status;
        public HrmPeriodAllocParameterStatus Status {
               get { return _Status; }
               set { SetPropertyValue<HrmPeriodAllocParameterStatus>("Status", ref _Status, value); } }


        [Association("PeriodAllocParameters-OrderControls"), Aggregated]  // связь с HrmPeriodOrderControl
        public XPCollection<HrmPeriodOrderControl> OrderControls {
               get{ return GetCollection<HrmPeriodOrderControl>("OrderControls");} }


        private HrmPeriod _HrmPeriod;  //Связь с HrmPeriod
        public HrmPeriod HrmPeriod { 
               get { return _HrmPeriod; }
               set { SetPropertyValue<HrmPeriod>("HrmPeriod", ref _HrmPeriod, value); }}

        // связь с HrmSalaryPayType
        public XPCollection<HrmSalaryPayType> PayTypes {        
               get { return GetCollection<HrmSalaryPayType>("PayTypes");}}


        public HrmPeriodAllocParameter(Session session) : base(session) { }
        public override void AfterConstruction(){
        base.AfterConstruction();}
    
    }
}
