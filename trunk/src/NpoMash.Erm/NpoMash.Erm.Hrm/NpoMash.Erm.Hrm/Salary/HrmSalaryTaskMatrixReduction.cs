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
    [NavigationItem("A1 Integration")]

    public class HrmSalaryTaskMatrixReduction : BaseObject {
        public HrmSalaryTaskMatrixReduction(Session session) : base(session) { }

        private HrmMatrix _MatrixPlan;
        public HrmMatrix MatrixPlan {
            get { return _MatrixPlan; }
        }

        private HrmMatrix _MatrixAlloc;
        public HrmMatrix MatrixAlloc {
            get { return _MatrixAlloc; }
        }

        private HrmTimeSheetGroup _Group;
        public HrmTimeSheetGroup Group {
            get { return _Group; }
            set { SetPropertyValue<HrmTimeSheetGroup>("Group", ref _Group, value); }
        }

        private HrmPeriodAllocParameter _AllocParameters;
        public HrmPeriodAllocParameter AllocParameters {
            get { return _AllocParameters; }
            set { SetPropertyValue<HrmPeriodAllocParameter>("AllocParameters", ref _AllocParameters, value); }
        }

        [Association("MatrixReduction-Period"),Aggregated]
        public XPCollection<HrmPeriod> Period {
            get { return GetCollection<HrmPeriod>("Period"); }
        }


        IList<DepartmentItem> departmentItems=new List<DepartmentItem>();
        [NonPersistent]
        public class DepartmentItem : XPCustomObject {
            public Department Department;
            public Int32 PlanTrudEmk;
            public Int32 NewTrudEmk;
            public Int32 DepartmentTrudEmk;
            //еще должна быть ссылка на конкретный заказ
        }

        public void initDepartmentList(HrmMatrix MatrixPlan, HrmTimeSheet TimeSheet) {
            SetPropertyValue<HrmMatrix>("MatrixPlan", ref _MatrixPlan, MatrixPlan);
            // Идем по колонкам
            foreach (var column in MatrixPlan.Columns) {
                DepartmentItem item = new DepartmentItem();
                    item.Department = column.Department;
                    item.PlanTrudEmk = Convert.ToInt32(column.Sum);
                    item.NewTrudEmk = AllocProperty();  //Вычисляемое поле           
                    item.DepartmentTrudEmk = 000000000000000000000; //Сюда значение из TimeSheet
                departmentItems.Add(item);
            }

            departmentItems.Distinct();// Выделяем уникальные подразделения
        }

        IList<OrderItem> orderItems = new List<OrderItem>();
        [NonPersistent]
        public class OrderItem : XPCustomObject {
            public fmCOrder Order;
            public Int32 PlanTrudEmk;
            public Int32 NewTrudEmk;
            //Сюда ссыль на подразделение
        }

        public void initOrderList(HrmMatrix MatrixPlan) { 
        //идем по строкам
            foreach (var row in MatrixPlan.Rows) {
                OrderItem item = new OrderItem();
                    item.Order = row.Order;
                    item.PlanTrudEmk = Convert.ToInt32(row.Sum);
                    item.NewTrudEmk = AllocProperty(); //Вычисляемое поле
                    orderItems.Add(item);
            }
            orderItems.Distinct();
        }


        public Int32 AllocProperty() {

            //
            //
            //
            //
            //Сюда алгоритм
            HrmMatrix Alloc=null;//Новая матрица
            SetPropertyValue<HrmMatrix>("MatrixAlloc", ref _MatrixAlloc, Alloc);
            return 1;
        }


        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }

}
