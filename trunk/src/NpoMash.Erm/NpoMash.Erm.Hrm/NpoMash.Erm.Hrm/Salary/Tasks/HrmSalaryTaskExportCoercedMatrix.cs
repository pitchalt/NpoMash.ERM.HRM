using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
//
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
//
namespace NpoMash.Erm.Hrm.Salary {

    [Persistent("HrmSalaryExportCoercedMatrix")]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "HrmSalaryTaskExportCoercedMatrixVC_ExportCoercedMatrix", Criteria = "isMatrixExported", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance("", AppearanceItemType = "Action", TargetItems = "Delete, New", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, TargetItems = "*", Context = "Any", Enabled = false)]
    public class HrmSalaryTaskExportCoercedMatrix : HrmSalaryTask {
        public HrmSalaryTaskExportCoercedMatrix(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

        private HrmMatrix _KBCoercedMatrix;
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public HrmMatrix KBCoercedMatrix {
            get { return _KBCoercedMatrix; }
            set { SetPropertyValue<HrmMatrix>("KBCoercedMatrix", ref _KBCoercedMatrix, value); }
        }

        private HrmMatrix _OZMCoercedMatrix;
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public HrmMatrix OZMCoercedMatrix {
            get { return _OZMCoercedMatrix; }
            set { SetPropertyValue<HrmMatrix>("OZMCoercedMatrix", ref _OZMCoercedMatrix, value); }
        }

        [Browsable(false)]
        private bool isMatrixExported { get { return State == HrmSalaryTaskState.HRM_SALARY_TASK_COMPLETED; } }

        protected override void InObjectsLoad() {
            if (KBCoercedMatrix != null)
                InObjects.Add(KBCoercedMatrix);
            if (OZMCoercedMatrix != null)
                InObjects.Add(OZMCoercedMatrix);
        }
    }
}