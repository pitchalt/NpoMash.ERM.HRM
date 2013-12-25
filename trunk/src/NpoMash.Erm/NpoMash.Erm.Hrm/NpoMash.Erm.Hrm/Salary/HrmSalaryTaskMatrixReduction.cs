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

        [Persistent("MatrixPlan")]
        private HrmMatrix _MatrixPlan;
         [PersistentAlias("_MatrixPlan")]
        public HrmMatrix MatrixPlan {
            get { return _MatrixPlan; }
        }

        [Persistent("MatrixAlloc")]
        private HrmMatrix _MatrixAlloc;
         [PersistentAlias("_MatrixAlloc")]
        public HrmMatrix MatrixAlloc {
            get { return _MatrixAlloc; }
        }

         [Persistent("TimeSheetGroup")]
         private HrmTimeSheetGroup _TimeSheetGroup;
         [PersistentAlias("_TimeSheetGroup")]
         public HrmTimeSheetGroup TimeSheetGroup {
             get { return _TimeSheetGroup; }
        }

         [Persistent("AllocParameters")]
        private HrmPeriodAllocParameter _AllocParameters;
         [PersistentAlias("_AllocParameters")]
        public HrmPeriodAllocParameter AllocParameters {
            get { return _AllocParameters; }
        }

        [Association("MatrixReduction-Period")]
        public XPCollection<HrmPeriod> Period {
            get { return GetCollection<HrmPeriod>("Period"); }
        }

        public void initTaskMatrixReduction(HrmMatrix MatrixPlan, HrmTimeSheetGroup TimeSheet, HrmPeriodAllocParameter AllocParameters) {
            SetPropertyValue<HrmMatrix>("MatrixPlan", ref _MatrixPlan, MatrixPlan);
            SetPropertyValue<HrmTimeSheetGroup>("TimeSheetGroup", ref _TimeSheetGroup,TimeSheet);
            SetPropertyValue<HrmPeriodAllocParameter>("AllocParameters", ref _AllocParameters, AllocParameters); 
        }

        public void setMatrixAlloc(HrmMatrix MatrixAlloc) {
            SetPropertyValue<HrmMatrix>("MatrixAlloc", ref _MatrixAlloc, MatrixAlloc);        
        }

        [NonPersistent]
        public class DepartmentItem : XPCustomObject {
            public Department Department;
            public Int32 DepartmentPlan;
            public Int32 NewTrudEmk;
            public Int32 DepartmentFact;
        }

        [NonPersistent]
        public class OrderItem : XPCustomObject {
            public fmCOrder Order;
            public fmCOrderTypeCOntrol TypeControl;
            public Int32 OrderPlan;
        }


        private IList<DepartmentItem> _Department;
        public IList<DepartmentItem> Department {
            get { if (_Department == null)
                    _Department = departmentCreate();
                return _Department; 
            }
        }

        private IList<OrderItem> _Order;
        public IList<OrderItem> Order {
            get {
                if (_Order == null)
                    _Order = orderCreate();
                return _Order;
            }
        }

        protected IList<OrderItem> orderCreate() {
            IList<OrderItem> orderList = new List<OrderItem>();
            foreach (HrmMatrixRow row in MatrixPlan.Rows) {
                OrderItem item = orderList.FirstOrDefault(x => x.Order ==row.Order);
                if (item == null) {
                    item = new OrderItem() {
                        Order = row.Order
                    };
                }
                item.OrderPlan += row.Cells.Sum(x => x.Time);
                orderList.Add(item);

            }
            return orderList;
        }

        protected IList<DepartmentItem> departmentCreate() {
            IList<DepartmentItem> departmentList = new List<DepartmentItem>();
            foreach (HrmMatrixColumn col in MatrixPlan.Columns) {
                DepartmentItem item = departmentList.FirstOrDefault(x => x.Department == col.Department);
                if (item == null) {
                    item = new DepartmentItem() {
                        Department = col.Department
                    };
                }
                item.DepartmentPlan += col.Cells.Sum(x => x.Time);
                departmentList.Add(item);

            }
            return departmentList;
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }}

