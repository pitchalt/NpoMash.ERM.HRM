using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
//
using DevExpress.Xpo;
using DevExpress.ExpressApp;
//using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
//
using NpoMash.Erm.Hrm.Salary.Matrix;

namespace NpoMash.Erm.Hrm.Salary {

    [Persistent]
    public class HrmSalaryMatrixSlice : HrmSalaryPeriodObjectSlice {

        private HrmMatrix _Matrix;
        [Association("HrmMatrix-Slices")] //Ссылка на HrmMatrix
        public HrmMatrix Matrix {
            get { return _Matrix; }
            set { SetPropertyValue<HrmMatrix>("Matrix", ref _Matrix, value); }
        }


        [Association("HrmSalaryMatrixSlice-HrmSalaryRowSlice"), Aggregated] //Коллекция HrmMatrixRow
        public XPCollection<HrmSalaryRowSlice> HrmSalaryRowSlices {
            get { return GetCollection<HrmSalaryRowSlice>("HrmSalaryRowSlices"); }
        }

        [Association("HrmSalaryMatrixSlice-HrmSalaryColumnSlices"), Aggregated] //Коллекция HrmMatrixColumn
        public XPCollection<HrmSalaryColumnSlice> HrmSalaryColumnSlices {
            get { return GetCollection<HrmSalaryColumnSlice>("HrmSalaryColumnSlices"); }
        }


        public HrmSalaryMatrixSlice(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    
    }
}
