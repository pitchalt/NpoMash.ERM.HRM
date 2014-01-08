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
using DevExpress.ExpressApp.Editors;
//
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.FM.Order;

namespace NpoMash.Erm.Hrm.Salary {

    [Persistent("HrmSalaryTaskMatrixReduction")]
    public class HrmSalaryTaskMatrixReduction : BaseObject {
        public HrmSalaryTaskMatrixReduction(Session session) : base(session) { }

        private HrmMatrix _MatrixPlan;
        public HrmMatrix MatrixPlan {
            get { return _MatrixPlan; }
            set { SetPropertyValue<HrmMatrix>("MatrixPlan", ref _MatrixPlan, value); }

        }

        private HrmMatrix _MatrixAlloc;
        public HrmMatrix MatrixAlloc {
            get { return _MatrixAlloc; }
            set { SetPropertyValue<HrmMatrix>("MatrixAlloc", ref _MatrixAlloc, value); }


        }

        private HrmTimeSheetGroup _TimeSheetGroup;
        public HrmTimeSheetGroup TimeSheetGroup {
            get { return _TimeSheetGroup; }
            set { SetPropertyValue<HrmTimeSheetGroup>("TimeSheetGroup", ref _TimeSheetGroup, value); }
        }

        private HrmPeriodAllocParameter _AllocParameters;
        public HrmPeriodAllocParameter AllocParameters {
            get { return _AllocParameters; }
            set { SetPropertyValue<HrmPeriodAllocParameter>("AllocParameters", ref _AllocParameters, value); }
        }

        private HrmPeriod _Period; // связь с HrmPeriod
        [Association("MatrixReduction-Period")]
        public HrmPeriod Period {
            get { return _Period; }
            set { SetPropertyValue<HrmPeriod>("Period", ref _Period, value); }
        }

        public static HrmSalaryTaskMatrixReduction initTaskMatrixReduction(HrmPeriod Period, IObjectSpace os) {
            var MatrixReduction = os.CreateObject<HrmSalaryTaskMatrixReduction>();
            MatrixReduction.Period = Period;
            MatrixReduction.AllocParameters = Period.CurrentAllocParameter;
            MatrixReduction.TimeSheetGroup = Period.CurrentTimeSheet.KB;

            foreach (var matrix in Period.Matrixs) {
                if (matrix.TypeMatrix == HRM_MATRIX_TYPE_MATRIX.Planned) {
                    MatrixReduction.MatrixPlan = matrix;
                }
            }
            MatrixReduction.MatrixAlloc = HrmMatrixLogic.makeAllocMatrix(MatrixReduction, os);

            return MatrixReduction;
        }



        [NonPersistent]
        public class DepartmentItem : XPCustomObject {
            public Department Department;
            public Int32 DepartmentPlan;
            public Int32 DepartmentAlloc;
            public Int32 DepartmentFact;

            public DepartmentItem(Session session) : base(session) { }
        }

        [NonPersistent]
        public class OrderItem : XPCustomObject {
            public fmCOrder Order;
            public fmCOrderTypeCOntrol TypeControl;
            public Int32 OrderPlan;

            public OrderItem(Session session) : base(session) { }
        }


        private IList<DepartmentItem> _Department;
        //[VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public IList<DepartmentItem> Department {
            get {
                if (_Department == null)
                    _Department = departmentCreate();
                return _Department;
            }
        }

        private IList<OrderItem> _Order;
     //[VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public IList<OrderItem> Order {
            get {
                if (_Order == null)
                    _Order = orderCreate();
                return _Order;
            }
        }

        IObjectSpace os;
        protected IList<OrderItem> orderCreate() {
            IList<OrderItem> orderList = new List<OrderItem>();
            foreach (HrmMatrixRow row in MatrixPlan.Rows) {
                OrderItem item = orderList.FirstOrDefault(x => x.Order == row.Order);
                if (item == null) {
                    item = new OrderItem(this.Session) {
                        Order = row.Order //Заказ
                    };
                }
                item.OrderPlan = Convert.ToInt32(row.Sum);//План по заказу
                item.TypeControl = row.Order.TypeControl; // Тип контроля
                orderList.Add(item);
            }
            return orderList;
        }

        protected IList<DepartmentItem> departmentCreate() {
            IList<DepartmentItem> departmentList = new List<DepartmentItem>();
            foreach (HrmMatrixColumn col in MatrixPlan.Columns) {
                DepartmentItem item = departmentList.FirstOrDefault(x => x.Department == col.Department);
                if (item == null) {
                    item = new DepartmentItem(this.Session) {
                        Department = col.Department // Подразделение
                    };
                }
                item.DepartmentPlan = Convert.ToInt32(col.Sum);// План по подразделению
                departmentList.Add(item);
            }
            foreach (HrmMatrixColumn col in MatrixAlloc.Columns) {
                DepartmentItem item = departmentList.FirstOrDefault(x => x.Department == col.Department);
                if (item == null) {
                    item = new DepartmentItem(this.Session) {
                        Department = col.Department // Подразделение
                    };
                }
                item.DepartmentAlloc = Convert.ToInt32(col.Sum);// План по подразделению
                departmentList.Add(item);
            }
            //заполняем факт по подразделению
            foreach (var t in TimeSheetGroup.TimeSheetDeps) {
                for (int i = 0; i < departmentList.Count; i++) {
                    if (t.Department.Code == departmentList[i].Department.Code) {
                        departmentList[i].DepartmentFact = t.MatrixWorkTime;
                    }
                }
            }
            return departmentList;
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }
}

