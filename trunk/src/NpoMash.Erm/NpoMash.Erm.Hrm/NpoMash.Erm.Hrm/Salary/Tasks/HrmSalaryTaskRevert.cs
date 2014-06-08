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
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;


namespace NpoMash.Erm.Hrm.Salary {

    [MapInheritance(MapInheritanceType.ParentTable)]
    [Appearance("", AppearanceItemType = "Action", TargetItems = "Delete, New", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, TargetItems = "*", Context = "Any", Enabled = false)]
    public class HrmSalaryTaskRevert : HrmSalaryTask {

        private HrmTimeSheet _TimeSheetKB;
        public HrmTimeSheet TimeSheetKB {
            get { return _TimeSheetKB; }
            set { SetPropertyValue<HrmTimeSheet>("TimeSheetKB", ref _TimeSheetKB, value); }
        }

        private HrmTimeSheet _TimeSheetOZM;
        public HrmTimeSheet TimeSheetOZM {
            get { return _TimeSheetOZM; }
            set { SetPropertyValue<HrmTimeSheet>("TimeSheetOZM", ref _TimeSheetOZM, value); }
        }

        private HrmMatrixPlan _MatrixPlanKB;
        public HrmMatrixPlan MatrixPlanKB {
            get { return _MatrixPlanKB; }
            set { SetPropertyValue<HrmMatrixAllocPlan>("MatrixPlanKB", ref _MatrixPlanKB, value); }
        }

        private HrmMatrixPlan _MatrixPlanOZM;
        public HrmMatrixPlan MatrixPlanOZM {
            get { return _MatrixPlanOZM; }
            set { SetPropertyValue<HrmMatrixAllocPlan>("HrmMatrixAllocPlan", ref _MatrixPlanOZM, value); }
        }

        private HrmAllocParameter _AllocParameter;
        public HrmAllocParameter AllocParameter {
            get { return _AllocParameter; }
            set { SetPropertyValue<HrmAllocParameter>("HrmPeriodAllocParameter", ref _AllocParameter, value); }
        }

        private HrmMatrix _MatrixAllocResultKB;
        public HrmMatrix MatrixAllocResultKB {
            get { return _MatrixAllocResultKB; }
            set { SetPropertyValue<HrmMatrix>("MatrixAllocResultKB", ref _MatrixAllocResultKB, value); }
        }

        private HrmMatrix _MatrixAllocResultOZM;
        public HrmMatrix MatrixAllocResultOZM {
            get { return _MatrixAllocResultOZM; }
            set { SetPropertyValue<HrmMatrix>("MatrixAllocResultOZM", ref _MatrixAllocResultOZM, value); }
        }

        private HrmMatrix _MatrixReductionKB;
        public HrmMatrix MatrixReductionKB {
            get { return _MatrixReductionKB; }
            set { SetPropertyValue<HrmMatrix>("MatrixReductionKB", ref _MatrixReductionKB, value); }
        }

        private HrmMatrix _MatrixReductionOZM;
        public HrmMatrix MatrixReductionOZM {
            get { return _MatrixReductionOZM; }
            set { SetPropertyValue<HrmMatrix>("MatrixReductionOZM", ref _MatrixReductionOZM, value); }
        }

        private HrmMatrix _MatrixProvision;
        public HrmMatrix MatrixProvision {
            get { return _MatrixProvision; }
            set { SetPropertyValue<HrmMatrix>("MatrixProvision", ref _MatrixProvision, value); }
        }

        public HrmSalaryTaskRevert(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

        protected override void InObjectsLoad() {

        }

    }
}