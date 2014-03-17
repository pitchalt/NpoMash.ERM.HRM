using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;

namespace NpoMash.Erm.Hrm.Salary {
    [Persistent("HrmSlaryTaskCompareKBAccountOperation")]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "AcceptCompareKB", Criteria = "State=='HRM_SALARY_TASK_COMPLETED'", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance("", AppearanceItemType = "Action", TargetItems = "Delete, New", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, TargetItems = "*", Context = "Any", Enabled = false)]
    public class HrmSalaryTaskCompareKBAccountOperation : HrmSalaryTask {
        
        public HrmSalaryTaskCompareKBAccountOperation(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

        private HrmMatrixAllocPlan _MatrixPlanKB;
        public HrmMatrixAllocPlan MatrixPlanKB {
            get { return _MatrixPlanKB; }
            set { SetPropertyValue<HrmMatrixAllocPlan>("MatrixPlanKB", ref _MatrixPlanKB, value); }
        }

        private HrmMatrix _ReducMatrixKB;
        public HrmMatrix ReducMatrixKB {
            get { return _ReducMatrixKB; }
            set { SetPropertyValue<HrmMatrix>("ReducMatrixKB", ref _ReducMatrixKB, value); }
        }

        private HrmMatrixAllocResult _MatrixAllocResultKB;
        public HrmMatrixAllocResult MatrixAllocResultKB {
            get { return _MatrixAllocResultKB; }
            set { SetPropertyValue<HrmMatrixAllocResult>("MatrixAllocResultKB", ref _MatrixAllocResultKB, value); }
        }

        [Browsable(false)]
        private bool isCompareAcceptedKB { get { return State == HrmSalaryTaskState.HRM_SALARY_TASK_COMPLETED; } }
    }
}