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
    [Persistent("HrmSalaryTaskCompareOZMAccountOperation")]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "HrmSalaryTaskCompareOZMAccountOperationVC_AcceptCompare", Criteria = "isCompareAccepted", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance("", AppearanceItemType = "Action", TargetItems = "Delete, New", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, TargetItems = "*", Context = "Any", Enabled = false)]
    public class HrmSalaryTaskCompareOZMAccountOperation : HrmSalaryTask { 

        public HrmSalaryTaskCompareOZMAccountOperation(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

        private HrmMatrixAllocPlan _MatrixAllocPlanOZM;
        public HrmMatrixAllocPlan MatrixAllocPlanOZM {
            get { return _MatrixAllocPlanOZM; }
            set { SetPropertyValue<HrmMatrixAllocPlan>("MatrixAllocPlanOZM", ref _MatrixAllocPlanOZM, value); }
        }

        private HrmMatrixAllocResult _MatrixAllocResultOZM;
        public HrmMatrixAllocResult MatrixAllocResultOZM {
            get { return _MatrixAllocResultOZM; }
            set { SetPropertyValue<HrmMatrixAllocResult>("MatrixAllocResultOZM", ref _MatrixAllocResultOZM, value); }
        }

        [Browsable(false)]
        private bool isCompareAccepted { get { return !(MatrixAllocResultOZM.Status == HrmMatrixStatus.MATRIX_PRIMARY_ACCEPTED); } }
    }
}