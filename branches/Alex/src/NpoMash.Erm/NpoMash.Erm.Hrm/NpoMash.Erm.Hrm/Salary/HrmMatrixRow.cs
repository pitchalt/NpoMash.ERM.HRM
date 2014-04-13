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
using IntecoAG.ERM.FM.Order;

namespace NpoMash.Erm.Hrm.Salary {

    [Persistent("HrmMatrixRow")]
    [DefaultProperty("Order")]
    public class HrmMatrixRow : BaseObject, IHrmSalaryMatrixRow,IRow {

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
        
        [Association("Row-Cells"), Aggregated]  //Коллекция HrmMatrixCell
        public XPCollection<HrmMatrixCell> Cells {
            get { return GetCollection<HrmMatrixCell>("Cells"); }
        }

        private fmCOrder _Order; //Ссылка на fmCOrder
        [Index(0), VisibleInListView(true), VisibleInDetailView(true)] 
        public fmCOrder Order {
            get { return _Order; }
            set { SetPropertyValue<fmCOrder>("Order", ref _Order, value); }
        }

        private HrmMatrix _Matrix; //Ссылка на HrmMatrix
        [Association("HrmMatrix-Rows")]
        public HrmMatrix Matrix {
            get { return _Matrix; }
            set { SetPropertyValue<HrmMatrix>("TYPE_MATIX", ref _Matrix, value); }
        }

        private HrmSalaryPeriodObjectBase _SalaryObject;
        [Association("SalaryObject-Row")]
        public HrmSalaryPeriodObjectBase SalaryObject {
            get { return _SalaryObject; }
            set { SetPropertyValue<HrmSalaryPeriodObjectBase>("SalaryObject", ref _SalaryObject, value); }
        }



        public HrmMatrixRow(Session session): base(session) {}
        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        IHrmSalaryMatrix IHrmSalaryMatrixRow.Matrix {
            get { throw new NotImplementedException(); }
        }

        IList<IHrmSalaryMatrixCell> IHrmSalaryMatrixRow.Cells {
            get { throw new NotImplementedException(); }
        }

        fmCOrder IHrmSalaryMatrixRow.Order {
            get { throw new NotImplementedException(); }
        }


        IntecoAG.XafExt.IndexedList.IIndex<ICellValue, fmCOrder> IntecoAG.XafExt.IndexedList.IIndexValue<ICellValue, fmCOrder>.Index {
            get { throw new NotImplementedException(); }
        }

        fmCOrder IntecoAG.XafExt.IndexedList.IIndexValue<ICellValue, fmCOrder>.Key {
            get { return Order; }
        }

        ICellValue IntecoAG.XafExt.IndexedList.IIndexValue<ICellValue, fmCOrder>.Value {
            get { throw new NotImplementedException(); }
        }

        ICellValue IntecoAG.XafExt.IndexedList.IIndexValue<ICellValue, fmCOrder>.this[int index] {
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