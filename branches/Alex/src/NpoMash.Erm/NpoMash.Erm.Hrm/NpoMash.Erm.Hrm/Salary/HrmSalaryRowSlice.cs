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
    public class HrmSalaryRowSlice : BaseObject {

        private HrmSalaryObjectSlice _SalaryObjectSlice; // —сылка на HrmSalaryObjectSlice
        public HrmSalaryObjectSlice SalaryObjectSlice {
            get { return _SalaryObjectSlice; }
            set { SetPropertyValue<HrmSalaryObjectSlice>("SalaryObjectSlice", ref _SalaryObjectSlice, value); }
        }


        private HrmMatrixRow _Row; //—сылка на HrmMatrixRow
        public HrmMatrixRow Row {
            get { return _Row; }
            set { SetPropertyValue<HrmMatrixRow>("Row", ref _Row, value); }
        }


        public HrmSalaryRowSlice(Session session) : base(session) { }
        public override void AfterConstruction() {  base.AfterConstruction(); }
    }
}
