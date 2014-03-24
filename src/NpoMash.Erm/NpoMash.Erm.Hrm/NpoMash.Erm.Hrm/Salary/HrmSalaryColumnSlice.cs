using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace NpoMash.Erm.Hrm.Salary {
    public class HrmSalaryColumnSlice : BaseObject {

        private HrmMatrixColumn _Column; // —сылка на HrmMatrixColumn
        public HrmMatrixColumn Column {
            get { return _Column; }
            set { SetPropertyValue<HrmMatrixColumn>("Column", ref _Column, value); }
        }

        private HrmSalaryPeriodObjectSlice _ObjectSlice;// —сылка на HrmSalaryObjectSlice
        public HrmSalaryPeriodObjectSlice ObjectSlice {
            get { return _ObjectSlice; }
            set { SetPropertyValue<HrmSalaryPeriodObjectSlice>("ObjectSlice", ref _ObjectSlice, value); }

        }


        public HrmSalaryColumnSlice(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }
}
