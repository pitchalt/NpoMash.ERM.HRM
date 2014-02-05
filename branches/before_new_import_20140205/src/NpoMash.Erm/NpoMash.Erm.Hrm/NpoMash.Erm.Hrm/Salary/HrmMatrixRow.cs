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
using IntecoAG.ERM.HRM;
using IntecoAG.ERM.FM.Order;

namespace NpoMash.Erm.Hrm.Salary {

    [Persistent("HrmMatrixRow")]
    [DefaultProperty("Order")]
    public class HrmMatrixRow : BaseObject {

         [ModelDefault("DisplayFormat", "{0:N}")]
         [RuleValueComparison(null, DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0)]
        [NonPersistent()]
        public Decimal Sum {
            get {
                Decimal result = 0;
                foreach (HrmMatrixCell current_cell in Cells) result += current_cell.Time;
                return result;
                //return _Sum; 
            }
            //set { SetPropertyValue<Decimal>("Sum", ref _Sum, value); }
        }
        
        [Association("Row-Cells"), Aggregated]  //Коллекция HrmMatrixCell
        public XPCollection<HrmMatrixCell> Cells {
            get { return GetCollection<HrmMatrixCell>("Cells"); }
        }

        private fmCOrder _Order; //Ссылка на fmCOrder
        [Index(0), VisibleInListView(true), VisibleInDetailView(true)] 
        public fmCOrder Order {
            get { return _Order; }
            set { SetPropertyValue<fmCOrder>("Order", ref _Order, value); }
        }

        private HrmMatrix _Matrix; //Ссылка на HrmMatrix
        [Association("TYPE_MATIX-Rows")]
        public HrmMatrix Matrix {
            get { return _Matrix; }
            set { SetPropertyValue<HrmMatrix>("TYPE_MATIX", ref _Matrix, value); }
        }

        public HrmMatrixRow(Session session): base(session) {}
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }
}
