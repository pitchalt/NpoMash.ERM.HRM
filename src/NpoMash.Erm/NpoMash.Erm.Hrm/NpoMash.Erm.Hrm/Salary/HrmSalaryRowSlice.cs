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
    
    [Persistent]
    public class HrmSalaryRowSlice : BaseObject, IHrmSalaryMatrixRow {

      //  private HrmSalaryPeriodObjectSlice _ObjectSlice; // —сылка на HrmSalaryObjectSlice
        //public HrmSalaryPeriodObjectSlice ObjectSlice {
           // get { return _ObjectSlice; }
           // set { SetPropertyValue<HrmSalaryPeriodObjectSlice>("ObjectSlice", ref _ObjectSlice, value); }
      //  }


        private HrmMatrixRow _Row; //—сылка на HrmMatrixRow
        public HrmMatrixRow Row {
            get { return _Row; }
            set { SetPropertyValue<HrmMatrixRow>("Row", ref _Row, value); }
        }

        private HrmSalaryMatrixSlice _HrmSalaryMatrixSlice;
        [Association("HrmSalaryMatrixSlice-HrmSalaryRowSlice")] //—сылка HrmMatrixSlice
        public HrmSalaryMatrixSlice HrmSalaryMatrixSlice {
            get { return _HrmSalaryMatrixSlice; }
            set { SetPropertyValue<HrmSalaryMatrixSlice>("HrmSalaryMatrixSlice", ref _HrmSalaryMatrixSlice, value); }
        }


        public HrmSalaryRowSlice(Session session) : base(session) { }
        public override void AfterConstruction() {  base.AfterConstruction(); }

        IHrmSalaryMatrix IHrmSalaryMatrixRow.Matrix {
            get { return HrmSalaryMatrixSlice; }
        }

        IList<IHrmSalaryMatrixCell> IHrmSalaryMatrixRow.Cells {
            get { return new ListConverter<IHrmSalaryMatrixCell, HrmMatrixCell>(Row.Cells); }
        }

        IntecoAG.ERM.FM.Order.fmCOrder IHrmSalaryMatrixRow.Order {
            get { return Row.Order; }
        }
    }
}
