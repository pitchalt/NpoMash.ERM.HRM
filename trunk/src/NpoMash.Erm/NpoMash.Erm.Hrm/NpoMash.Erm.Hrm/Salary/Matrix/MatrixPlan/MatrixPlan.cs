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
using IntecoAG.XafExt.DataStruct;
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.FM.Order;
//
namespace NpoMash.Erm.Hrm.Salary.Matrix.MatrixPlan {

    [DefaultClassOptions]
    public class MatrixPlan : BaseObject, ISliced {

        public MatrixPlan(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        public IIndexedList<DepartmentGroupDep, ISlice> Slices {
            get { throw new NotImplementedException(); }
        }

        public IIndexedList<Department, IColumn> Columns {
            get { throw new NotImplementedException(); }
        }

        public IIndexedList<fmCOrder, IRow> Rows {
            get { throw new NotImplementedException(); }
        }

        public IValue Value {
            get { throw new NotImplementedException(); }
        }

        public decimal TimeWork {
            get { throw new NotImplementedException(); }
        }

        public decimal TimeTravel {
            get { throw new NotImplementedException(); }
        }
    }

    [Persistent]
    public class MatrixPlanSlice : BaseObject, ISlice {

        public MatrixPlanSlice(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }


        public DepartmentGroupDep DepartmentGroup {
            get { throw new NotImplementedException(); }
        }

        public ISliced SlicedMatrix {
            get { throw new NotImplementedException(); }
        }

        public IIndexedList<Department, IColumn> Columns {
            get { throw new NotImplementedException(); }
        }

        public IIndexedList<fmCOrder, IRow> Rows {
            get { throw new NotImplementedException(); }
        }

        public IValue Value {
            get { throw new NotImplementedException(); }
        }

        public decimal TimeWork {
            get { throw new NotImplementedException(); }
        }

        public decimal TimeTravel {
            get { throw new NotImplementedException(); }
        }
    }

    [Persistent]
    public class MatrixPlanColumn : BaseObject, IColumn {

        public MatrixPlanColumn(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }


        public Department Department {
            get { throw new NotImplementedException(); }
        }

        public DepartmentGroupDep GroupDep {
            get { throw new NotImplementedException(); }
        }

        public IMatrix Matrix {
            get { throw new NotImplementedException(); }
        }

        public IIndexedList<fmCOrder, ICell> Cells {
            get { throw new NotImplementedException(); }
        }

        public IValue Value {
            get { throw new NotImplementedException(); }
        }

        public decimal TimeWork {
            get { throw new NotImplementedException(); }
        }

        public decimal TimeTravel {
            get { throw new NotImplementedException(); }
        }
    }

    [Persistent]
    public class MatrixPlanRow : BaseObject, IRow {

        public MatrixPlanRow(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        public FmCOrderTypeControl OrderControl {
            get { throw new NotImplementedException(); }
        }

        public fmCOrder Order {
            get { throw new NotImplementedException(); }
        }

        public IMatrix Matrix {
            get { throw new NotImplementedException(); }
        }

        public IIndexedList<Department, ICell> Cells {
            get { throw new NotImplementedException(); }
        }

        public IValue Value {
            get { throw new NotImplementedException(); }
        }

        public decimal TimeWork {
            get { throw new NotImplementedException(); }
        }

        public decimal TimeTravel {
            get { throw new NotImplementedException(); }
        }
    }

    [Persistent]
    public class MatrixPlanCell : BaseObject, ICell {

        public MatrixPlanCell(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }


        public IColumn Column {
            get { throw new NotImplementedException(); }
        }

        public IRow Row {
            get { throw new NotImplementedException(); }
        }

        public IValue Value {
            get { throw new NotImplementedException(); }
        }

        public decimal TimeWork {
            get { throw new NotImplementedException(); }
        }

        public decimal TimeTravel {
            get { throw new NotImplementedException(); }
        }
    }

}
