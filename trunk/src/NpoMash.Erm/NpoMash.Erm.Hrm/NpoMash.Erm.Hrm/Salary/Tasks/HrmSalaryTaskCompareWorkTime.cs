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
    [Appearance(null, AppearanceItemType = "ViewItem", TargetItems = "AllocResultOZM.Status,AllocResultOZM.TypeMatrix,AllocResultOZM.Type,AllocResultOZM.GroupDep,AllocResultOZM.Columns,AllocResultOZM.Rows", Criteria = "GroupDep=='DEPARTMENT_KB'", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, AppearanceItemType = "ViewItem", TargetItems = "AllocResultKB.Status,AllocResultKB.TypeMatrix,AllocResultKB.Type,AllocResultKB.GroupDep,AllocResultKB.Columns,AllocResultKB.Rows", Criteria = "GroupDep=='DEPARTMENT_OZM'", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "AcceptCompareKB", Criteria = "GroupDep=='DEPARTMENT_OZM'", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "AcceptCompareOZM", Criteria = "GroupDep=='DEPARTMENT_KB'", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    public class HrmSalaryTaskCompareWorkTime : HrmSalaryTaskReductionBase<HrmSalaryTaskCompareWorkTime.DepartmentItem2, HrmSalaryTaskCompareWorkTime.OrderItem2> {
        
        [NonPersistent]
        public class DepartmentItem2 : HrmSalaryTaskReductionBase<HrmSalaryTaskCompareWorkTime.DepartmentItem2, HrmSalaryTaskCompareWorkTime.OrderItem2>.DepartmentItem {
            public DepartmentItem2(Session session) : base(session) { }
            public DepartmentItem2() { }
            //Поля для контроля трудоемкости
            public Decimal DepartmentPlan;
            public Decimal DepartmentTravelPlan;
            public Decimal ConstantDepTime;
            private Decimal fact_Constant;
            public Decimal Fact_Constant {
                get { return fact_Constant = DepartmentFact - ConstantDepTime; }
            }
            public Decimal DepartmentFact;
            public Decimal TravelFact;
            private Decimal plan_Fact;
            public Decimal Plan_Fact {
                get { return plan_Fact=DepartmentPlan - DepartmentFact; }
            }
            public Decimal CoercedValue;
        }

        [NonPersistent]
        public class OrderItem2 : HrmSalaryTaskReductionBase<HrmSalaryTaskCompareWorkTime.DepartmentItem2, HrmSalaryTaskCompareWorkTime.OrderItem2>.OrderItem {
            public OrderItem2(Session session) : base(session) { }
            public OrderItem2() { }
            //Поля для контроля трудоемкости
            public Decimal OrderPlan;
            public Decimal OrderFact;
            public Decimal TravelPlan;
            public Decimal ConstantOrderTime;
            private Decimal orderFact_ConstantOrderTime;
            public Decimal OrderFact_ConstantOrderTime {
                get { return orderFact_ConstantOrderTime = OrderFact - ConstantOrderTime; }
            }
            public Decimal TravelFact;
            private Decimal plan_Fact;
            public Decimal Plan_Fact {
                get { return plan_Fact = OrderPlan - OrderFact; }
            }
            public Decimal CoercedValue;
        }

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

        private HrmTimeSheet _CurrentTimeSheetKB; // Ссылка на HrmTimeSheet
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public HrmTimeSheet CurrentTimeSheetKB {
            get { return _CurrentTimeSheetKB; }
            set { SetPropertyValue<HrmTimeSheet>("CurrentTimeSheetKB", ref _CurrentTimeSheetKB, value); }
        }

        private HrmTimeSheet _CurrentTimeSheetOZM; // Ссылка на HrmTimeSheet
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public HrmTimeSheet CurrentTimeSheetOZM {
            get { return _CurrentTimeSheetOZM; }
            set { SetPropertyValue<HrmTimeSheet>("CurrentTimeSheetOZM", ref _CurrentTimeSheetOZM, value); }
        }

        protected override void orderCreate() {
            if (MatrixPlan != null)
                LoadMatrixOrder(MatrixPlan, null, Order);
            if (MinimizeNumberOfDeviationsMatrix != null)
                LoadMatrixOrder(MinimizeNumberOfDeviationsMatrix, null, Order);
            if (GroupDep == DepartmentGroupDep.DEPARTMENT_KB && AllocResultKB != null)
                LoadMatrixOrder(AllocResultKB, null, Order);
            if (GroupDep == DepartmentGroupDep.DEPARTMENT_OZM && AllocResultOZM != null)
                LoadMatrixOrder(AllocResultOZM, null, Order);
        }

        protected override void departmentCreate() {
            if (MatrixPlan != null)
                LoadMatrixDepartment(MatrixPlan, null, Department);
            if (MinimizeNumberOfDeviationsMatrix != null)
                LoadMatrixDepartment(MinimizeNumberOfDeviationsMatrix, null, Department);
            if (GroupDep == DepartmentGroupDep.DEPARTMENT_KB && AllocResultKB != null)
                LoadMatrixDepartment(AllocResultKB, null, Department);
            if (GroupDep == DepartmentGroupDep.DEPARTMENT_OZM && AllocResultOZM != null)
                LoadMatrixDepartment(AllocResultOZM, null, Department);
        }


        

        protected override void LoadMatrixOrderLogic(HrmMatrix matrix, HrmMatrixColumn col, HrmMatrixRow row, OrderItem2 item) {
            foreach (HrmMatrixCell cell in row.Cells) {
                if (col != null && cell.Column != col)
                    continue;
                switch (matrix.TypeMatrix) {
                    case HrmMatrixTypeMatrix.MATRIX_PLANNED:
                        item.OrderPlan += cell.Time;
                        item.TravelPlan += cell.TravelTime;
                        item.ConstantOrderTime += cell.ConstOrderTime;
                        break;
                    case HrmMatrixTypeMatrix.MATRIX_COERCED:
                        switch (matrix.Variant) {
                            case HrmMatrixVariant.MINIMIZE_NUMBER_OF_DEVIATIONS_VARIANT:
                                item.CoercedValue += cell.Time;
                                break;
                        }
                        break;
                       
                    default:
                        break;
                }
                if (matrix.Type == HrmMatrixType.TYPE_ALLOC_RESULT) {
                    item.OrderFact = cell.Time;
                    item.TravelFact = cell.TravelTime;
                }
            }
        }


        protected override void LoadMatrixDepartmentLogic(HrmMatrix matrix, HrmMatrixColumn col, HrmMatrixRow row, DepartmentItem2 item) {
            foreach (HrmMatrixCell cell in col.Cells) {
                if (row != null && cell.Row != row)
                    continue;
                switch (matrix.TypeMatrix) {
                    case HrmMatrixTypeMatrix.MATRIX_PLANNED:
                        item.DepartmentPlan += cell.Time;
                        item.DepartmentTravelPlan += cell.TravelTime;
                        item.ConstantDepTime += cell.ConstOrderTime;
                        break;
                    case HrmMatrixTypeMatrix.MATRIX_COERCED:
                        switch (matrix.Variant) {
                            case HrmMatrixVariant.MINIMIZE_NUMBER_OF_DEVIATIONS_VARIANT:
                                item.CoercedValue += cell.Time;
                                break;
                        }
                        break;
                    default:
                        break;
                }
                if (matrix.Type == HrmMatrixType.TYPE_ALLOC_RESULT) {
                    item.DepartmentFact = cell.Time;
                    item.TravelFact = cell.TravelTime;
                }
            }
        }

        protected override DepartmentItem2 DepartmentItemCreate() {
            return new DepartmentItem2(this.Session);
        }

        protected override OrderItem2 OrderItemCreate() {
            return new OrderItem2(this.Session);
        }

        public HrmSalaryTaskCompareWorkTime(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

        protected override void InObjectsLoad() {

        }

    }
}
