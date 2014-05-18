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


    [Persistent("TaskKompareWorkTime")]
    public class TaskKompareWorkTime : HrmSalaryTaskMatrixReduction {

        [NonPersistent]
        public class DepartmentItem : XPCustomObject {
            public Department Department;
            public DepartmentGroupDep Group;
            public Decimal DepartmentPlan;
            public Decimal DepartmentTravelPlan;
            public Decimal ConstantOrderType;
            public Decimal DepartmentFact;
            public Decimal DepartmentTravelFact;
            public Decimal Plan_Fact;
            public IList<OrderItem> OrderItems = new List<OrderItem>();
            public DepartmentItem(Session session) : base(session) { }
        }

        [NonPersistent]
        public class OrderItem : XPCustomObject {
            public fmCOrder Order;
            public FmCOrderTypeControl TypeControl;
            public Decimal OrderPlan;
            public Decimal TravelPlan;
            public Decimal ConstantOrderType;
            public Decimal OrderFact_ConstantOrderType;
            public Decimal TravelFact;
            public Decimal Plan_Fact;
            public IList<DepartmentItem> DepartmentItems = new List<DepartmentItem>();
            public OrderItem(Session session) : base(session) { }
        }

        private IList<DepartmentItem> _Department;
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public IList<DepartmentItem> Department {
            get {
                if (_Department == null) {
                    _Department = new List<DepartmentItem>();
                    departmentCreate();
                }
                return _Department;
            }
        }

        private IList<OrderItem> _Order;
        [NonPersistent]
        public IList<OrderItem> Order {
            get {
                if (_Order == null) {
                    _Order = new List<OrderItem>();
                    orderCreate();
                }
                return _Order;
            }
        }

        protected void orderCreate() {
            LoadMatrixOrder(MinimizeNumberOfDeviationsMatrix, null, Order);
        }

        protected void departmentCreate() {
            LoadMatrixDepartment(MinimizeNumberOfDeviationsMatrix, null, Department);
        }


        protected void LoadMatrixOrder(HrmMatrix matrix, HrmMatrixColumn col, IList<OrderItem> items) {
            foreach (HrmMatrixRow row in matrix.Rows) {
                if (col != null && row.Cells.FirstOrDefault(x => x.Column == col) == null)
                    continue;
                OrderItem item = items.FirstOrDefault(x => x.Order == row.Order);
                if (item == null) {
                    item = new OrderItem(this.Session) {
                        Order = row.Order,
                        DepartmentItems = new List<DepartmentItem>(),
                        TypeControl = row.Order.TypeControl
                    };
                    items.Add(item);
                }

                foreach (HrmMatrixCell cell in row.Cells) {
                    if (col != null && cell.Column != col)
                        continue;
                    switch (matrix.TypeMatrix) {
                        case HrmMatrixTypeMatrix.MATRIX_PLANNED:
                            item.OrderPlan += cell.Time;
                            break;



                        default:
                            break;
                    }
                }

                if (col == null)
                    LoadMatrixDepartment(matrix, row, item.DepartmentItems);
            }

        }


        protected void LoadMatrixDepartment(HrmMatrix matrix, HrmMatrixRow row, IList<DepartmentItem> items) {
            foreach (HrmMatrixColumn col in matrix.Columns) {
                if (row != null && col.Cells.FirstOrDefault(x => x.Row == row) == null)
                    continue;
                DepartmentItem item = items.FirstOrDefault(x => x.Department == col.Department);
                if (item == null) {
                    item = new DepartmentItem(this.Session) {
                        Department = col.Department, // Подразделение
                        OrderItems = new List<OrderItem>(),
                        Group = col.Department.GroupDep
                    };
                    items.Add(item);
                }


                foreach (HrmMatrixCell cell in col.Cells) {
                    if (row != null && cell.Row != row)
                        continue;
                    switch (matrix.TypeMatrix) {
                        case HrmMatrixTypeMatrix.MATRIX_PLANNED:
                            item.DepartmentPlan += cell.Time;
                            break;



                        default:
                            break;
                    }
                }

                if (row == null)
                    LoadMatrixOrder(matrix, col, item.OrderItems);
            }
        }



        public TaskKompareWorkTime(Session session): base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }
}
