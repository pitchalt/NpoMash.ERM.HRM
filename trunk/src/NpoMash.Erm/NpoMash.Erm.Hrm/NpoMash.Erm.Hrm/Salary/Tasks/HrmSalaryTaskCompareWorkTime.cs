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
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.FM.Order;

namespace NpoMash.Erm.Hrm.Salary {


    [Persistent("TaskKompareWorkTime")]

    [Appearance(null, AppearanceItemType = "ViewItem", TargetItems = "AllocResultOZM.Status,AllocResultOZM.TypeMatrix,AllocResultOZM.Type,AllocResultOZM.GroupDep", Criteria = "GroupDep=='DEPARTMENT_KB'", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, AppearanceItemType = "ViewItem", TargetItems = "AllocResultKB.Status,AllocResultKB.TypeMatrix,AllocResultKB.Type,AllocResultKB.GroupDep", Criteria = "GroupDep=='DEPARTMENT_OZM'", Context = "Any", Visibility = ViewItemVisibility.Hide)]

    [Appearance(null, AppearanceItemType = "Action", TargetItems = "AcceptCompareKB", Criteria = "GroupDep=='DEPARTMENT_OZM'", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "AcceptCompareOZM", Criteria = "GroupDep=='DEPARTMENT_KB'", Context = "Any", Visibility = ViewItemVisibility.Hide)]


    public class TaskKompareWorkTime : HrmSalaryBaseReductionElements {



        private HrmMatrix _AllocResultKB; //Первичная проводка КБ
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public HrmMatrix AllocResultKB {
            get { return _AllocResultKB; }
            set { SetPropertyValue<HrmMatrix>("AllocResultKB", ref _AllocResultKB, value); }
        }

        private HrmMatrix _AllocResultOZM; //Первичная проводка ОЗМ
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public HrmMatrix AllocResultOZM {
            get { return _AllocResultOZM; }
            set { SetPropertyValue<HrmMatrix>("AllocResultOZM", ref _AllocResultOZM, value); }
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
           
                LoadMatrixOrder(MatrixPlan, null, Order);
        }

        protected void departmentCreate() {

            LoadMatrixDepartment(MatrixPlan, null, Department);
        }

        protected void CleanMatrixOrder(HrmMatrix matrix, HrmMatrixColumn col, IList<OrderItem> items) {
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
