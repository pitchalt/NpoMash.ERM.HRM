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
using IntecoAG.Erm.HRM;
using IntecoAG.Erm.HRM.Organization;
using IntecoAG.Erm.FM.Order;

namespace NpoMash.Erm.Hrm.Salary {

    [Persistent("HrmAccountOperation")]
    [DefaultProperty("Department")] 
    public class HrmAccountOperation : BaseObject {

        private HrmAccount _Debit;
        public HrmAccount Debit {
            get { return _Debit; }
            set { SetPropertyValue<HrmAccount>("Debit", ref _Debit, value); }
        }

        private HrmAccount _Credit;
        public HrmAccount Credit {
            get { return _Credit; }
            set { SetPropertyValue<HrmAccount>("Credit", ref _Credit, value); }
        }

        private HrmSalaryPayType _PayType; //—сылка на HrmSalaryPayType 
        public HrmSalaryPayType PayType {
            get { return _PayType; }
            set { SetPropertyValue<HrmSalaryPayType>("PayType", ref _PayType, value); }
        }

        private Department _Department; //—сылка на Department 
        public Department Department {
            get { return _Department; }
            set { SetPropertyValue<Department>("Department", ref _Department, value); }
        }

        private HrmMatrixCell _Cell; //—сылка на HrmMatrixCell
        public HrmMatrixCell Cell {
            get { return _Cell; }
            set { SetPropertyValue<HrmMatrixCell>("Cell", ref _Cell, value); }
        }

        private fmCOrder _Order; //—сылка на fmCOrder
        public fmCOrder Order {
            get { return _Order; }
            set { SetPropertyValue<fmCOrder>("Order", ref _Order, value); }
        }

        private HrmMatrixAllocResult _AllocResult; //—сылка на HrmMatrixAllocResult
        [Association("AccountOperations-AllocResult")]
        public HrmMatrixAllocResult AllocResult {
            get { return _AllocResult; }
            set { SetPropertyValue<HrmMatrixAllocResult>("AllocResult", ref _AllocResult, value); }
        }


        public HrmAccountOperation(Session session): base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }
}
