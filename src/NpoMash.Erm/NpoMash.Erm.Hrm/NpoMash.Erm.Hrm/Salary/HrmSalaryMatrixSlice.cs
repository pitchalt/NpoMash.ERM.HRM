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
using NpoMash.Erm.Hrm.Salary.MatrixStructure;

namespace NpoMash.Erm.Hrm.Salary {

    [Persistent]
    public class HrmSalaryMatrixSlice : HrmSalaryPeriodObjectSlice, IHrmSalaryMatrix, IMatrixSlice {

        private HrmMatrix _Matrix;
        [Association("HrmMatrix-HrmSalaryMatrixSlices")] //Ссылка на HrmMatrix
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
    
IList<IHrmSalaryMatrixRow> IHrmSalaryMatrix.Rows
{
	get { return new ListConverter<IHrmSalaryMatrixRow, HrmSalaryRowSlice>(HrmSalaryRowSlices); }
}

IList<IHrmSalaryMatrixColumn> IHrmSalaryMatrix.Columns
{
    get { return new ListConverter<IHrmSalaryMatrixColumn, HrmSalaryColumnSlice>(HrmSalaryColumnSlices); }
}

// ////////////////////////////////////////
IntecoAG.XafExt.IndexedList.IIndex<ICellValue, IntecoAG.ERM.HRM.Organization.DepartmentGroupDep> IMatrix.Slices {
    get { throw new NotImplementedException(); }
}

IRowCollection IMatrixBase.Rows {
    get { throw new NotImplementedException(); }
}

IColumnCollection IMatrixBase.Columns {
    get { throw new NotImplementedException(); }
}

ICellValue IntecoAG.XafExt.IndexedList.IIndexable<ICellValue>.this[int index] {
    get { throw new NotImplementedException(); }
}

IEnumerator<ICellValue> IEnumerable<ICellValue>.GetEnumerator() {
    throw new NotImplementedException();
}

System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
    throw new NotImplementedException();
}

IntecoAG.ERM.FM.Order.fmCOrder ICellValue.Order {
    get { throw new NotImplementedException(); }
}

IntecoAG.ERM.HRM.Organization.Department ICellValue.Department {
    get { throw new NotImplementedException(); }
}

IntecoAG.ERM.HRM.Organization.DepartmentGroupDep ICellValue.GroupDep {
    get { throw new NotImplementedException(); }
}

IntecoAG.ERM.FM.Order.FmCOrderTypeControl ICellValue.TypeControl {
    get { throw new NotImplementedException(); }
}
    }
}
