using System;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;
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
using IntecoAG.ERM.HRM;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.FM.FinAccount;
using IntecoAG.ERM.HRM.Organization;

namespace NpoMash.Erm.Hrm.Salary {

    [Persistent("HrmAccountOperation")]
    [DefaultProperty("Department")]
    public class HrmAccountOperation : XPObject {

        private String _Sign;
        public String Sign {
            get { return _Sign; }
            set { SetPropertyValue<String>("Sign", ref _Sign, value); }
        }

        private String _Credit;
        public String Credit {
            get { return _Credit; }
            set { SetPropertyValue<String>("Credit", ref _Credit, value); }
        }

        private String _Debit;
        public String Debit {
            get { return _Debit; }
            set { SetPropertyValue<String>("Debit", ref _Debit, value); }
        }

        private fmCOrder _Order; //—сылка на fmCOrder
        public fmCOrder Order {
            get { return _Order; }
            set { SetPropertyValue<fmCOrder>("Order", ref _Order, value); }
        }

        private Department _Department; //—сылка на Department 
        public Department Department {
            get { return _Department; }
            set { SetPropertyValue<Department>("Department", ref _Department, value); }
        }

        private HrmSalaryPayType _PayType; //—сылка на HrmSalaryPayType 
        public HrmSalaryPayType PayType {
            get { return _PayType; }
            set { SetPropertyValue<HrmSalaryPayType>("PayType", ref _PayType, value); }
        }

        private Decimal _Time;
        [ModelDefault("DisplayFormat", "{0:N}")]
        public Decimal Time {
            get { return _Time; }
            set { SetPropertyValue<Decimal>("Time", ref _Time, value); }
        }

        private Decimal _Money;
        public Decimal Money {
            get { return _Money; }
            set { SetPropertyValue<Decimal>("Money", ref _Money, value); }
        }

        private HrmMatrixCell _Cell; //—сылка на HrmMatrixCell
        [Association("Cell-AccountOperations")]
        public HrmMatrixCell Cell {
            get { return _Cell; }
            set { SetPropertyValue<HrmMatrixCell>("Cell", ref _Cell, value); }
        }

        private HrmMatrixAllocResult _AllocResult; //—сылка на HrmMatrixAllocResult
        [Association("AccountOperations-TYPE_ALLOC_RESULT")]
        public HrmMatrixAllocResult AllocResult {
            get { return _AllocResult; }
            set { SetPropertyValue<HrmMatrixAllocResult>("TYPE_ALLOC_RESULT", ref _AllocResult, value); }
        }


        public HrmAccountOperation(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }
}