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

namespace NpoMash.Erm.Hrm.Salary {

    public class HrmPeriodMatrixBaseObject : HrmSalaryPeriodObjectBase {

        private HrmMatrix _Matrix;
        public HrmMatrix Matrix {
            get { return _Matrix; }
            set { SetPropertyValue<HrmMatrix>("Matrix", ref _Matrix, value); }
        }










        public HrmPeriodMatrixBaseObject(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }
}
