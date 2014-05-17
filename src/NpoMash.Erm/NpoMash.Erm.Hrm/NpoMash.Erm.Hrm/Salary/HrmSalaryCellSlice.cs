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

    [NonPersistent]
    public class HrmSalaryCellSlice : BaseObject {

        private HrmSalaryRowSlice _RowSlice;
        public HrmSalaryRowSlice RowSlice {
            get { return _RowSlice; }
            set { SetPropertyValue<HrmSalaryRowSlice>("RowSlice", ref _RowSlice, value); }
        }

        private HrmMatrixCell _RealCell;
        public HrmMatrixCell RealCell {
            get { return _RealCell; }
            set { SetPropertyValue<HrmMatrixCell>("RealCell", ref _RealCell, value); }
        }

        private HrmSalaryColumnSlice _ColumnSlice;
        public HrmSalaryColumnSlice ColumnSlice {
            get { return _ColumnSlice; }
            set { SetPropertyValue<HrmSalaryColumnSlice>("ColumnSlice", ref _ColumnSlice, value); }
        }


        public HrmSalaryCellSlice(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }
}
