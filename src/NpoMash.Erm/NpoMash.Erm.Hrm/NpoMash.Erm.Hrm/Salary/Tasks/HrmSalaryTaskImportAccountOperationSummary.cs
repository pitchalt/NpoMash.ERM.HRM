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
    [Persistent("HrmSalaryTaskImportAccountOperationSummary")]
   // [Appearance(null, AppearanceItemType = "Action", TargetItems = "HrmSalaryTaskImportAccountOperationSummaryVC_AcceptImport", Criteria = "isSourceDataImported", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance("", AppearanceItemType = "Action", TargetItems = "Delete, New", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, TargetItems = "*", Context = "Any", Enabled = false)]
    public class HrmSalaryTaskImportAccountOperationSummary : HrmSalaryTask {
        public HrmSalaryTaskImportAccountOperationSummary(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

        private HrmMatrixAllocResult _MatrixAllocResultSummary;
        public HrmMatrixAllocResult MatrixAllocResultSummary {
            get { return _MatrixAllocResultSummary; }
            set { SetPropertyValue<HrmMatrixAllocResult>("MatrixAllocResultSummary", ref _MatrixAllocResultSummary, value); }
        }

        protected override void InObjectsLoad() {

        }

    }
}