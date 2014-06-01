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

[NonPersistent]
public class DepartmentItem<ORD> : XPCustomObject{
    public Department Department;
    public DepartmentGroupDep Group;
    public IList<ORD> OrderItems = new List<ORD>();
    public DepartmentItem() { }
    public DepartmentItem(Session session) : base(session) { }
}

[NonPersistent]
public class OrderItem<DEP> : XPCustomObject
{
    public fmCOrder Order;
    public FmCOrderTypeControl TypeControl;
    public IList<DEP> DepartmentItems = new List<DEP>();
    public OrderItem() { }
    public OrderItem(Session session) : base(session) { }
}

namespace NpoMash.Erm.Hrm.Salary {

    [MapInheritance(MapInheritanceType.ParentTable)]
    public abstract class HrmSalaryTaskReductionBase<DEP, ORD> : HrmSalaryTask
    where DEP:DepartmentItem<ORD>, new()
    where ORD:OrderItem<DEP>, new() {


        private HrmMatrix _MinimizeNumberOfDeviationsMatrix;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public HrmMatrix MinimizeNumberOfDeviationsMatrix {
            get { return _MinimizeNumberOfDeviationsMatrix; }
            set { SetPropertyValue<HrmMatrix>("MinimizeNumberOfDeviationsMatrix", ref _MinimizeNumberOfDeviationsMatrix, value); }
        }

        private HrmMatrix _MatrixPlan;
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public HrmMatrix MatrixPlan {
            get { return _MatrixPlan; }
            set { SetPropertyValue<HrmMatrix>("MatrixPlan", ref _MatrixPlan, value); }

        }

        private IList<DEP> _Department;
        [NonPersistent]
        public IList<DEP> Department {
            get {
                if (_Department == null) {
                    _Department = new List<DEP>();
                    departmentCreate();
                }
                return _Department;
            }
        }

        private IList<ORD> _Order;
        [NonPersistent]
        public IList<ORD> Order {
            get {
                if (_Order == null) {
                    _Order = new List<ORD>();
                    orderCreate();
                }
                return _Order;
            }
        }

        protected abstract void orderCreate();
        protected abstract void departmentCreate();
        protected abstract void LoadMatrixDepartmentLogic(HrmMatrix matrix, HrmMatrixColumn col, HrmMatrixRow row, DEP item);
        protected abstract void LoadMatrixOrderLogic(HrmMatrix matrix, HrmMatrixColumn col, HrmMatrixRow row, ORD item);
        protected abstract DEP DepartmentItemCreate();
        protected abstract ORD OrderItemCreate();

        protected void LoadMatrixOrder(HrmMatrix matrix, HrmMatrixColumn col, IList<ORD> items) {
            foreach (HrmMatrixRow row in matrix.Rows) {
                if (col != null && row.Cells.FirstOrDefault(x => x.Column == col) == null)
                    continue;
                ORD item = items.FirstOrDefault(x => x.Order == row.Order);
                if (item == null) {
                    item = OrderItemCreate(); //{// не передаем сюда сессию, а это норм??
                    item.Order = row.Order;
                    item.DepartmentItems = new List<DEP>();
                    item.TypeControl = row.Order.TypeControl;
                    //};
                    items.Add(item);
                }
                // здесь вызов какой-то логики
                LoadMatrixOrderLogic(matrix,col,row, item);

                if (col == null)
                    LoadMatrixDepartment(matrix, row, item.DepartmentItems);
            }

        }

        protected void LoadMatrixDepartment(HrmMatrix matrix, HrmMatrixRow row, IList<DEP> items) {
            foreach (HrmMatrixColumn col in matrix.Columns) {
                if (row != null && col.Cells.FirstOrDefault(x => x.Row == row) == null)
                    continue;
                DEP item = items.FirstOrDefault(x => x.Department == col.Department);
                if (item == null) {
                    item = DepartmentItemCreate();// {// не передаем сюда сессию, а это норм??
                    item.Department = col.Department; // ѕодразделение
                    item.OrderItems = new List<ORD>();
                    item.Group = col.Department.GroupDep;
                    //};
                    items.Add(item);
                }
                // здесь вызов какой-то логики
                LoadMatrixDepartmentLogic(matrix,col,row, item);

                if (row == null)
                    LoadMatrixOrder(matrix, col, item.OrderItems);
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
