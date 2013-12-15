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

        private Int16 _Time;
        public Int16 Time {
            get { return _Time; }
            set { SetPropertyValue<Int16>("Time", ref _Time,value); }
        }

        private Decimal _Sum;
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

        public HrmMatrixCell(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }
}
