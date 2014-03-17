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
    [Persistent("HrmSalaryTaskImportAccountOperation")]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "HrmSalaryTaskImportSourceDataVC_AcceptImport", Criteria = "isSourceDataImported", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance("", AppearanceItemType = "Action", TargetItems = "Delete, New", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, TargetItems = "*", Context = "Any", Enabled = false)]
    public class HrmSalaryTaskImportAccountOperation : HrmSalaryTask {

        public HrmSalaryTaskImportAccountOperation(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        private HrmMatrixAllocResult _MatrixAllocResultKB;
        public HrmMatrixAllocResult MatrixAllocResultKB {
            get { return _MatrixAllocResultKB; }
            set { SetPropertyValue<HrmMatrixAllocResult>("MatrixAllocResultKB", ref _MatrixAllocResultKB, value); }
        }

        private HrmMatrixAllocResult _MatrixAllocResultOZM;
        public HrmMatrixAllocResult MatrixAllocResultOZM {
            get { return _MatrixAllocResultOZM; }
            set { SetPropertyValue<HrmMatrixAllocResult>("MatrixAllocResult", ref _MatrixAllocResultOZM, value); }
        }
        [Browsable(false)]
       private bool isSourceDataImported {
            get { return State == HrmSalaryTaskState.HRM_SALARY_TASK_COMPLETED; }
        }




    }
}