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
    public class HrmMatrixCell : BaseObject, IHrmSalaryMatrixCell {

        private Int64 _Time;
        public Int64 Time {
            get { return _Time; }
            set { SetPropertyValue<Int64>("Time", ref _Time, value); }
        }

        private Decimal _PlanMoney;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:N}")]
        public Decimal PlanMoney {
            get { return _PlanMoney; }
            set { SetPropertyValue<Decimal>("PlanMoney", ref _PlanMoney, value); }
        }

        private Decimal _MoneyReserve;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:N}")]
        public Decimal MoneyReserve {
            get { return _MoneyReserve; }
            set { SetPropertyValue<Decimal>("MoneyReserve", ref _MoneyReserve, value); }
        }

        private Decimal _MoneyNoReserve;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:N}")]
        public Decimal MoneyNoReserve {
            get { return _MoneyNoReserve; }
            set { SetPropertyValue<Decimal>("MoneyNoReserve", ref _MoneyNoReserve, value); }
        }

        private Decimal _MoneyTravel;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:N}")]
        public Decimal MoneyTravel {
            get { return _MoneyTravel; }
            set { SetPropertyValue<Decimal>("MoneyTravel", ref _MoneyTravel, value); }
        }

        private Decimal _MoneyAllSumm;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
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