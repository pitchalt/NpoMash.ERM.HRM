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

        public void initialize(HrmMatrix MatrixPlan, HrmTimeSheetGroup TimeSheet, HrmPeriodAllocParameter AllocParameters) {
            SetPropertyValue<HrmMatrix>("MatrixPlan", ref _MatrixPlan, MatrixPlan);
            SetPropertyValue<HrmTimeSheetGroup>("TimeSheetGroup", ref _TimeSheetGroup,TimeSheet);
            SetPropertyValue<HrmPeriodAllocParameter>("AllocParameters", ref _AllocParameters, AllocParameters); }

        [NonPersistent]
        public class DepartmentItem : XPCustomObject {
            public Department Department;
            public Int32 PlanTrudEmk;
            public Int32 NewTrudEmk;
            public Int32 DepartmentTrudEmk;
        }
        
        IList<DepartmentItem> A {
         get {  return new List<DepartmentItem>(); }
        }

        public void CreateDep() {
            DepartmentItem B = new DepartmentItem();
            A.Add(B);
        }


        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }}

