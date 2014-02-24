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


namespace NpoMash.Erm.Hrm.Salary {
    [Persistent("HrmSalaryTaskImportSourceData")]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "AcceptImport", Criteria = "isSourceDataImported", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance("", AppearanceItemType = "Action", TargetItems = "Delete, New", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, TargetItems = "*", Context = "Any", Enabled = false)]
    public class HrmSalaryTaskImportSourceData : HrmSalaryTask { 

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

        private HrmMatrixAllocPlan _MatrixPlanKB;
        public HrmMatrixAllocPlan MatrixPlanKB {
            get { return _MatrixPlanKB; }
            set { SetPropertyValue<HrmMatrixAllocPlan>("MatrixPlanKB", ref _MatrixPlanKB, value); }
        }

        private HrmMatrixAllocPlan _MatrixPlanOZM;
        public HrmMatrixAllocPlan MatrixPlanOZM {
            get { return _MatrixPlanOZM; }
            set { SetPropertyValue<HrmMatrixAllocPlan>("MatrixPlanOZM", ref _MatrixPlanOZM, value); }
        }
        
        public HrmSalaryTaskImportSourceData(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }
}