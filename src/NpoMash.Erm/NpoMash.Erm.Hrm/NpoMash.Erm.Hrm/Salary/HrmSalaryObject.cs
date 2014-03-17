using System;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;
//
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
//

namespace NpoMash.Erm.Hrm.Salary {

    public class HrmSalaryObject : HrmSalaryPeriodObject {

        [Association("SalaryObject-ObjectSlice"), Aggregated] //Коллекция HrmSalaryObjectSlice
        public XPCollection<HrmSalaryObjectSlice> ObjectSlice {
            get { return GetCollection<HrmSalaryObjectSlice>("ObjectSlice"); }
        }

        [Association("SalaryObject-Column"), Aggregated] //Коллекция HrmMatrixColumn
        public XPCollection<HrmMatrixColumn> Column {
            get { return GetCollection<HrmMatrixColumn>("Column"); }
        }

        [Association("SalaryObject-Row"), Aggregated] //Коллекция HrmMatrixRow
        public XPCollection<HrmMatrixRow> Row {
            get { return GetCollection<HrmMatrixRow>("Row"); }
        }


        private HrmPeriod _Period; // Ссылка на HrmPeriod
        [Association("Period-SalaryObject")]
        public HrmPeriod Period {
            get { return _Period; }
            set { SetPropertyValue<HrmPeriod>("Period", ref _Period, value); }
        }



        public HrmSalaryObject(Session session) : base(session) { }
        public override void AfterConstruction() {  base.AfterConstruction(); }
    }
}
