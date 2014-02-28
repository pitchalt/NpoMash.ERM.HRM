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

namespace NpoMash.Erm.Hrm.Salary {

    [Persistent("HrmMatrixCell")]   
    public class HrmMatrixCell : BaseObject {

        private Int64 _Time;
        public Int64 Time {
            get { return _Time; }
            set { SetPropertyValue<Int64>("Time", ref _Time, value); }
        }

        private Int64 _TravelTime;
        public Int64 TravelTime {
            get { return _TravelTime; }
            set { SetPropertyValue<Int64>("TravelTime", ref _TravelTime, value); }
        }

        private Decimal _Money;
        public Decimal Money {
            get { return _Money; }
            set { SetPropertyValue<Decimal>("Money", ref _Money, value); }
        }

        private Decimal _Sum;
        [ModelDefault("DisplayFormat", "{0:N}")]
        [RuleValueComparison(null, DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0)]
        public Decimal Sum {
            get { return _Sum; }
            set { SetPropertyValue<Decimal>("Sum", ref _Sum, value); }
        }

        private HrmMatrixColumn _Column; // —сылка на HrmMatrixColumn
        [Association("Column-Cells")]
        public HrmMatrixColumn Column {
            get { return _Column; }
            set { SetPropertyValue<HrmMatrixColumn>("Column", ref _Column, value); }
        }

        private HrmMatrixRow _Row; // —сылка на HrmMatrixRow
        [Association("Row-Cells")]
        public HrmMatrixRow Row {
            get { return _Row; }
            set { SetPropertyValue<HrmMatrixRow>("Row", ref _Row, value); }
        }

        [Association("Cell-AccountOperations")]
        public XPCollection<HrmAccountOperation> AccountOperations {
            get { return GetCollection<HrmAccountOperation>("AccountOperations"); }
        }

        public HrmMatrixCell(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }
}