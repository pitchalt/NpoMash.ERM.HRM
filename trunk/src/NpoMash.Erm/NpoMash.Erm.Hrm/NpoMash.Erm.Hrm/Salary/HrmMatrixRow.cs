using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
//
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
//
using IntecoAG.Erm.HRM;
using IntecoAG.Erm.FM.Order;

namespace NpoMash.Erm.Hrm.Salary {

    [Persistent("HrmMatrixRow")]
    [DefaultProperty("Order")]     
    public class HrmMatrixRow : BaseObject {

        private Decimal _Sum;
        public Decimal Sum {
            get { return _Sum; }
            set { SetPropertyValue<Decimal>("Sum", ref _Sum, value); }
        }
        
        [Association("Row-Cells"), Aggregated]  //Коллекция HrmMatrixCell
        public XPCollection<HrmMatrixCell> Cells {
            get { return GetCollection<HrmMatrixCell>("Cells"); }
        }

        private fmCOrder _Order; //Ссылка на fmCOrder
        public fmCOrder Order {
            get { return _Order; }
            set { SetPropertyValue<fmCOrder>("Order", ref _Order, value); }
        }

        private HrmMatrix _Matrix; //Ссылка на HrmMatrix
        [Association("Matrix-Rows")]
        public HrmMatrix Matrix {
            get { return _Matrix; }
            set { SetPropertyValue<HrmMatrix>("Matrix", ref _Matrix, value); }
        }

        public HrmMatrixRow(Session session): base(session) {}
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }
}
