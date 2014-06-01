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

    [NonPersistent]
    public class DepartmentItemComp : DepartmentItem<OrderItemComp> {
        public DepartmentItemComp(Session session) : base(session) { }
        public DepartmentItemComp() { }
        //���� ��� �������� ������������
        public Decimal DepartmentPlan;
        public Decimal DepartmentTravelPlan;
        public Decimal ConstantOrderType;
        public Decimal DepartmentFact;
        public Decimal DepartmentTravelFact;
        public Decimal Plan_Fact;
    }

    [NonPersistent]
    public class OrderItemComp : OrderItem<DepartmentItemComp> {
        public OrderItemComp(Session session) : base(session) { }
        public OrderItemComp() { }
        //���� ��� �������� ������������
        public Decimal OrderPlan;
        public Decimal TravelPlan;
        public Decimal ConstantOrderType;
        public Decimal OrderFact_ConstantOrderType;
        public Decimal TravelFact;
        public Decimal Plan_Fact;
    }


    [Persistent("TaskKompareWorkTime")]
    [Appearance(null, AppearanceItemType = "ViewItem", TargetItems = "AllocResultOZM.Status,AllocResultOZM.TypeMatrix,AllocResultOZM.Type,AllocResultOZM.GroupDep", Criteria = "GroupDep=='DEPARTMENT_KB'", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, AppearanceItemType = "ViewItem", TargetItems = "AllocResultKB.Status,AllocResultKB.TypeMatrix,AllocResultKB.Type,AllocResultKB.GroupDep", Criteria = "GroupDep=='DEPARTMENT_OZM'", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "AcceptCompareKB", Criteria = "GroupDep=='DEPARTMENT_OZM'", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "AcceptCompareOZM", Criteria = "GroupDep=='DEPARTMENT_KB'", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    public class TaskKompareWorkTime : HrmSalaryTaskReductionBase<DepartmentItemComp, OrderItemComp> {

        private HrmMatrix _AllocResultKB; //��������� �������� ��
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public HrmMatrix AllocResultKB {
            get { return _AllocResultKB; }
            set { SetPropertyValue<HrmMatrix>("AllocResultKB", ref _AllocResultKB, value); }
        }

        private HrmMatrix _AllocResultOZM; //��������� �������� ���
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public HrmMatrix AllocResultOZM {
            get { return _AllocResultOZM; }
            set { SetPropertyValue<HrmMatrix>("AllocResultOZM", ref _AllocResultOZM, value); }
        }
   
        protected override void orderCreate() {
            LoadMatrixOrder(MatrixPlan, null, Order);
        }

        protected override void departmentCreate() {
            LoadMatrixDepartment(MatrixPlan, null, Department);
        }


        /*protected void LoadMatrixOrder(HrmMatrix matrix, HrmMatrixColumn col, IList<OrderItemComp> items) {
            foreach (HrmMatrixRow row in matrix.Rows) {
                if (col != null && row.Cells.FirstOrDefault(x => x.Column == col) == null)
                    continue;
                OrderItemComp item = items.FirstOrDefault(x => x.Order == row.Order);
                if (item == null) {
                    item = new OrderItemComp(this.Session) {
                        Order = row.Order,
                        DepartmentItems = new List<DepartmentItemComp>(),
                        TypeControl = row.Order.TypeControl
                    };
                    items.Add(item);
                }

                

                if (col == null)
                    LoadMatrixDepartment(matrix, row, item.DepartmentItems);
            }

        }

        protected void LoadMatrixDepartment(HrmMatrix matrix, HrmMatrixRow row, IList<DepartmentItemComp> items) {
            foreach (HrmMatrixColumn col in matrix.Columns) {
                if (row != null && col.Cells.FirstOrDefault(x => x.Row == row) == null)
                    continue;
                DepartmentItemComp item = items.FirstOrDefault(x => x.Department == col.Department);
                if (item == null) {
                    item = new DepartmentItemComp(this.Session) {
                        Department = col.Department, // �������������
                        OrderItems = new List<OrderItemComp>(),
                        Group = col.Department.GroupDep
                    };
                    items.Add(item);
                }
                if (row == null)
                    LoadMatrixOrder(matrix, col, item.OrderItems);
            }
        }*/

        protected override void LoadMatrixOrderLogic(HrmMatrix matrix, HrmMatrixColumn col, HrmMatrixRow row, OrderItemComp item) {
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
        }


        protected override void LoadMatrixDepartmentLogic(HrmMatrix matrix, HrmMatrixColumn col, HrmMatrixRow row, DepartmentItemComp item) {
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
        }

        protected override DepartmentItemComp DepartmentItemCreate() {
            return new DepartmentItemComp(this.Session);
        }

        protected override OrderItemComp OrderItemCreate() {
            return new OrderItemComp(this.Session);
        }

        public TaskKompareWorkTime(Session session): base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

        protected override void InObjectsLoad() {

        }

    }
}