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
using NpoMash.Erm.Hrm.Salary.Matrix;

namespace NpoMash.Erm.Hrm.Salary {

    [Persistent("HrmSalaryMatrixCell")]
    public class HrmMatrixCell : XPObject {

        private Decimal _Time;
        [ModelDefault("DisplayFormat", "{0:N}")]
        public Decimal Time {
            get { return _Time; }
            set { SetPropertyValue<Decimal>("Time", ref _Time, value); }
        }

        private Decimal _TravelTime;
        [ModelDefault("DisplayFormat", "{0:N}")]
        public Decimal TravelTime {
            get { return _TravelTime; }
            set { SetPropertyValue<Decimal>("TravelTime", ref _TravelTime, value); }
        }

        private Decimal _PlanMoney;
        [ModelDefault("DisplayFormat", "{0:N}")]
        public Decimal PlanMoney {
            get { return _PlanMoney; }
            set { SetPropertyValue<Decimal>("PlanMoney", ref _PlanMoney, value); }
        }

        private Decimal _SourceProvision;
        [ModelDefault("DisplayFormat", "{0:N}")]
        public Decimal SourceProvision {
            get { return _SourceProvision; }
            set { SetPropertyValue<Decimal>("SourceProvision", ref _SourceProvision, value); }
        }


        private Decimal _MoneyNoReserve;
        [ModelDefault("DisplayFormat", "{0:N}")]
        public Decimal MoneyNoReserve {
            get { return _MoneyNoReserve; }
            set { SetPropertyValue<Decimal>("MoneyNoReserve", ref _MoneyNoReserve, value); }
        }

        private Decimal _NewProvision;
        [ModelDefault("DisplayFormat", "{0:N}")]
        public Decimal NewProvision {
            get { return _NewProvision; }
            set { SetPropertyValue<Decimal>("NewProvision", ref _NewProvision, value); }
        }

        private Decimal _ProvisionDelta;
        [ModelDefault("DisplayFormat", "{0:N}")]
        public Decimal ProvisionDelta {
            get { return _ProvisionDelta; }
            set { SetPropertyValue<Decimal>("ProvisionDelta", ref _ProvisionDelta, value); }
        }

        private Decimal _MoneyTravel;
        [ModelDefault("DisplayFormat", "{0:N}")]
        public Decimal TravelMoney {
            get { return _MoneyTravel; }
            set { SetPropertyValue<Decimal>("MoneyTravel", ref _MoneyTravel, value); }
        }

        private Decimal _MoneyAllSumm;
        [ModelDefault("DisplayFormat", "{0:N}")]
        [RuleValueComparison(null, DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0)]
        public Decimal MoneyAllSumm {
            get { return _MoneyAllSumm; }
            set { SetPropertyValue<Decimal>("MoneyAllSumm", ref _MoneyAllSumm, value); }
        }

        private Decimal _ConstOrderTime;
        [ModelDefault("DisplayFormat", "{0:N}")]
        public Decimal ConstOrderTime {
            get { return _ConstOrderTime; }
            set { SetPropertyValue<Decimal>("ConstOrderTime", ref _ConstOrderTime, value); }
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