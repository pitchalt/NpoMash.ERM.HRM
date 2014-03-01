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

        private Decimal _PlanMoney;
        public Decimal PlanMoney {
            get { return _PlanMoney; }
            set { SetPropertyValue<Decimal>("PlanMoney", ref _PlanMoney, value); }
        }

        private Decimal _MoneyReserve;
        public Decimal MoneyReserve {
            get { return _MoneyReserve; }
            set { }
        }

        private Decimal _MoneyNoReserve;
        public Decimal MoneyNoReserve {
            get { return _MoneyNoReserve; }
            set { }
        }

        private Decimal _MoneyTravel;
        public Decimal MoneyTravel {
            get { return _MoneyTravel; }
            set { }
        }

        private Decimal _MoneyAllSumm;
        [ModelDefault("DisplayFormat", "{0:N}")]
        [RuleValueComparison(null, DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0)]
        public Decimal MoneyAllSumm {
            get { return _MoneyAllSumm; }
            set { SetPropertyValue<Decimal>("MoneyAllSumm", ref _MoneyAllSumm, value); }
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