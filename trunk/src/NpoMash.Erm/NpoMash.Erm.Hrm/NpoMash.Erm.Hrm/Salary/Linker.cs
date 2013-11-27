using System;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;
//
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
//
using IntecoAG.Erm.HRM;
namespace NpoMash.Erm.Hrm.Salary
{
    [DefaultClassOptions]
    
    public class Linker : BaseObject
    {



        private HrmSalaryPayType _PayTypes;  //Ñâÿçü ñ HrmSalaryPayType
        public HrmSalaryPayType PayTypes{
            get { return _PayTypes; }
            set { SetPropertyValue<HrmSalaryPayType>("PayTypes", ref _PayTypes, value); } }


        public Linker(Session session) : base(session) { }
        public override void AfterConstruction()
        { base.AfterConstruction(); }
      
    }
}
