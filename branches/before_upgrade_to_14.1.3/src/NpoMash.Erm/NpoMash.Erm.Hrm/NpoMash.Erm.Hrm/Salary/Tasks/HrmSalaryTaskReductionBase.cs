using System;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;
//
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
//
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.FM.Order;

namespace NpoMash.Erm.Hrm.Salary {

    [MapInheritance(MapInheritanceType.ParentTable)]
    public abstract class HrmSalaryTaskReductionBase : HrmSalaryTask {

        [NonPersistent]
        public abstract class DepartmentItemBase : XPCustomObject {
            public Department Department;
            public DepartmentGroupDep Group;
            public abstract IList<OrderItemBase> OrderItemBases { get; }

            public DepartmentItemBase() { }
            public DepartmentItemBase(Session session) : base(session) { }
        }

        [NonPersistent]
        public abstract class OrderItemBase : XPCustomObject {
            public fmCOrder Order;
            public FmCOrderTypeControl TypeControl;
            public abstract IList<DepartmentItemBase> DepartmentItemBases { get; }
 
            public OrderItemBase() { }
            public OrderItemBase(Session session) : base(session) { }
        }


        [Browsable(false)]
        public abstract IList<DepartmentItemBase> DepartmentItemBases { get; }

        [Browsable(false)]
        public abstract IList<OrderItemBase> OrderItemBases { get; }


        protected abstract void orderCreate();
        protected abstract void departmentCreate();
        protected abstract void LoadMatrixDepartmentLogic(HrmMatrix matrix, HrmMatrixColumn col, HrmMatrixRow row, DepartmentItemBase item);
        protected abstract void LoadMatrixOrderLogic(HrmMatrix matrix, HrmMatrixColumn col, HrmMatrixRow row, OrderItemBase item);
        protected abstract DepartmentItemBase DepartmentItemCreate();
        protected abstract OrderItemBase OrderItemCreate();

        protected void LoadMatrixOrder(HrmMatrix matrix, HrmMatrixColumn col, IList<OrderItemBase> items) {
            foreach (HrmMatrixRow row in matrix.Rows) {
                if (col != null && row.Cells.FirstOrDefault(x => x.Column == col) == null)
                    continue;
                OrderItemBase item = items.FirstOrDefault(x => x.Order == row.Order);
                if (item == null) {
                    item = OrderItemCreate();
                    item.Order = row.Order;
                    item.TypeControl = row.Order.TypeControl;
                    items.Add(item);
                }
                // здесь вызов какой-то логики
                LoadMatrixOrderLogic(matrix,col,row, item);

                if (col == null)
                    LoadMatrixDepartment(matrix, row, item.DepartmentItemBases);
            }

        }

        protected void LoadMatrixDepartment(HrmMatrix matrix, HrmMatrixRow row, IList<DepartmentItemBase> items) {
            foreach (HrmMatrixColumn col in matrix.Columns) {
                if (row != null && col.Cells.FirstOrDefault(x => x.Row == row) == null)
                    continue;
                DepartmentItemBase item = items.FirstOrDefault(x => x.Department == col.Department);
                if (item == null) {
                    item = DepartmentItemCreate();
                    item.Department = col.Department; // Подразделение
                    item.Group = col.Department.GroupDep;
                    items.Add(item);
                }
                // здесь вызов какой-то логики
                LoadMatrixDepartmentLogic(matrix,col,row, item);

                if (row == null)
                    LoadMatrixOrder(matrix, col, item.OrderItemBases);
            }
        }

        public HrmSalaryTaskReductionBase(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }
}
