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
using IntecoAG.ERM.HRM;
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.FM.Order;
using NpoMash.Erm.Hrm.Salary.Matrix;

namespace NpoMash.Erm.Hrm.Salary {

    [Persistent("HrmSalaryMatrixColumn")]
    public class HrmMatrixColumn : XPObject {


        [ModelDefault("DisplayFormat", "{0:N}")]
        [RuleValueComparison(null, DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0)]
        [NonPersistent()]
        public Decimal Sum {
            get {
                Decimal result = 0;
                foreach (HrmMatrixCell current_cell in Cells) result += current_cell.Time;
                return result;
            }
        }
        [ModelDefault("DisplayFormat", "{0:N}")]
        [RuleValueComparison(null, DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0)]
        [NonPersistent()]
        public Decimal TravelSum {
            get {
                Decimal result = 0;
                foreach (HrmMatrixCell current_cell in Cells) result += current_cell.TravelTime;
                return result;

            }
        }
        [ModelDefault("DisplayFormat", "{0:N}")]
        [RuleValueComparison(null, DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0)]
        [NonPersistent()]
        public Decimal ConstantTime {
            get {
                Decimal result = 0;
                foreach (HrmMatrixCell current_cell in Cells) result += current_cell.ConstOrderTime;
                return result;

            }
        }

        private Department _Department; //������ �� Department 
        [Index(0), VisibleInListView(true), VisibleInDetailView(true)]
        public Department Department {
            get { return _Department; }
            set { SetPropertyValue<Department>("Department", ref _Department, value); }
        }

        [Association("Column-Cells"), Aggregated] //��������� HrmMatrixCell
        public XPCollection<HrmMatrixCell> Cells {
            get { return GetCollection<HrmMatrixCell>("Cells"); }
        }

        private HrmMatrix _Matrix; //������ �� HrmMatrix 
        [Association("HrmMatrix-Columns")]
        public HrmMatrix Matrix {
            get { return _Matrix; }
            set { SetPropertyValue<HrmMatrix>("TYPE_MATIX", ref _Matrix, value); }
        }

        //
        //private HrmSalaryPeriodObjectBase _SalaryObject; // ������ �� HrmSalaryObject
        //[Association("SalaryObject-Column")]
        //public HrmSalaryPeriodObjectBase SalaryObject {
        //    get { return _SalaryObject; }
        //    set { SetPropertyValue<HrmSalaryPeriodObjectBase>("SalaryObject", ref _SalaryObject, value); }
        //}

        public HrmMatrixColumn(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

    }
}