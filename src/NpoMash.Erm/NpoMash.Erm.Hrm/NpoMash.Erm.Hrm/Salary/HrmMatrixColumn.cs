using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
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
using IntecoAG.Erm.HRM;
using IntecoAG.Erm.HRM.Organization;
using IntecoAG.Erm.FM.Order;

namespace NpoMash.Erm.Hrm.Salary {

    [Persistent("HrmMatrixColumn")]    
    public class HrmMatrixColumn : BaseObject {

        private Decimal _Sum;
        [ModelDefault("DisplayFormat", "{0:N}")]
        [RuleValueComparison(null, DefaultContexts.Save, ValueComparisonType.GreaterThan, 0)]
        public Decimal Sum {
            get { return _Sum; }
            set { SetPropertyValue<Decimal>("Sum", ref _Sum, value); }
        }

        private Department _Department; //Ссылка на Department 
        [Index(0), VisibleInListView(true), VisibleInDetailView(true)] 
        public Department Department {
            get { return _Department; }
            set { SetPropertyValue<Department>("Department", ref _Department, value); }
        }

        [Association("Column-Cells"),Aggregated] //Коллекция HrmMatrixCell
        public XPCollection<HrmMatrixCell> Cells {
            get { return GetCollection<HrmMatrixCell>("Cells"); }
        }

        private HrmMatrix _Matrix; //Ссылка на HrmMatrix 
        [Association("Matrix-Columns")]
        public HrmMatrix Matrix {
            get { return _Matrix; }
            set { SetPropertyValue<HrmMatrix>("Matrix", ref _Matrix, value); }
        }


        public HrmMatrixColumn(Session session): base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }
}
