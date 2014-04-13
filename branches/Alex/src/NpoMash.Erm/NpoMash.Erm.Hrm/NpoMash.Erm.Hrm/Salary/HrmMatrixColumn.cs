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

namespace NpoMash.Erm.Hrm.Salary {

    [Persistent("HrmMatrixColumn")]    
    public class HrmMatrixColumn : BaseObject, IHrmSalaryMatrixColumn,IColumn {

        //private Decimal _Sum;
        [ModelDefault("DisplayFormat", "{0:N}")]
        [RuleValueComparison(null, DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0)]
        [NonPersistent()]
        public Decimal Sum {
            get {
                Decimal result = 0;
                foreach (HrmMatrixCell current_cell in Cells) result += current_cell.Time;
                return result;
                //return _Sum; 
            }
            //set { SetPropertyValue<Decimal>("Sum", ref _Sum, value); }
        }

        private Department _Department; //������ �� Department 
        [Index(0), VisibleInListView(true), VisibleInDetailView(true)] 
        public Department Department {
            get { return _Department; }
            set { SetPropertyValue<Department>("Department", ref _Department, value); }
        }

        [Association("Column-Cells"),Aggregated] //��������� HrmMatrixCell
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
        private HrmSalaryPeriodObjectBase _SalaryObject; // ������ �� HrmSalaryObject
        [Association("SalaryObject-Column")]
        public HrmSalaryPeriodObjectBase SalaryObject {
            get { return _SalaryObject; }
            set { SetPropertyValue<HrmSalaryPeriodObjectBase>("SalaryObject", ref _SalaryObject, value); }
        }





        public HrmMatrixColumn(Session session): base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

        IHrmSalaryMatrix IHrmSalaryMatrixColumn.Matrix {
            get { throw new NotImplementedException(); }
        }

        IList<IHrmSalaryMatrixCell> IHrmSalaryMatrixColumn.Cells {
            get { throw new NotImplementedException(); }
        }

        Department IHrmSalaryMatrixColumn.Department {
            get { throw new NotImplementedException(); }
        }


// ///////////////////////////////////////////////////////////////////
        IntecoAG.XafExt.IndexedList.IIndex<ICellValue, Department> IntecoAG.XafExt.IndexedList.IIndexValue<ICellValue, Department>.Index {
            get { throw new NotImplementedException(); }
        }

        Department IntecoAG.XafExt.IndexedList.IIndexValue<ICellValue, Department>.Key {
            get { return Department; }
        }

        ICellValue IntecoAG.XafExt.IndexedList.IIndexValue<ICellValue, Department>.Value {
            get { throw new NotImplementedException(); }
        }

        ICellValue IntecoAG.XafExt.IndexedList.IIndexValue<ICellValue, Department>.this[int index] {
            get { throw new NotImplementedException(); }
        }

        IEnumerator<ICellValue> IEnumerable<ICellValue>.GetEnumerator() {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            throw new NotImplementedException();
        }

    }
}