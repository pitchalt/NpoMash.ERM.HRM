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
    [MapInheritance(MapInheritanceType.ParentTable)]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "HrmSalaryTaskCompareAccountOperationSummaryVC_AcceptCompare", Criteria = "isCompareAccepted", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance("", AppearanceItemType = "Action", TargetItems = "Delete, New", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, TargetItems = "*", Context = "Any", Enabled = false)]

    public class HrmSalaryTaskCompareAccountOperationSummary : HrmSalaryTask {

        public HrmSalaryTaskCompareAccountOperationSummary(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

        private HrmMatrixPlan _MatrixAllocPlanSummary;
        public HrmMatrixPlan MatrixAllocPlanSummary {
            get { return _MatrixAllocPlanSummary; }
            set { SetPropertyValue<HrmMatrixPlan>("MatrixAllocPlanSummary", ref _MatrixAllocPlanSummary, value); }
        }

        private HrmMatrixLastAccount _MatrixAllocResultSummary;
        public HrmMatrixLastAccount MatrixAllocResultSummary {
            get { return _MatrixAllocResultSummary; }
            set { SetPropertyValue<HrmMatrixLastAccount>("MatrixAllocResultSummary", ref _MatrixAllocResultSummary, value); }
        }

        [Browsable(false)]
        private bool isCompareAccepted { get { return !(MatrixAllocResultSummary.Status == HrmMatrixStatus.MATRIX_DOWNLOADED); } }
    
        protected override void InObjectsLoad() {
        }

        public String Name {
            get {
                return "Сравнение проводки";
            }
        }
    }
}