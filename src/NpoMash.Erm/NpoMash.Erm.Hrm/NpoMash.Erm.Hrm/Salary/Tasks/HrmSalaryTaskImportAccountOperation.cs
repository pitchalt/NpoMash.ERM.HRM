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
    [MapInheritance(MapInheritanceType.ParentTable)]
   [Appearance(null, AppearanceItemType = "Action", TargetItems = "HrmSalaryTaskImportSourceDataVC_AcceptImport", Criteria = "isSourceDataImported", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance("", AppearanceItemType = "Action", TargetItems = "Delete, New", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, TargetItems = "*", Context = "Any", Enabled = false)]
    [Appearance(null, AppearanceItemType = "ViewItem", TargetItems = "MatrixAllocResultKB.ConstantTime", Context = "Any", Visibility = ViewItemVisibility.Hide)]

    public class HrmSalaryTaskImportAccountOperation : HrmSalaryTask {

        public HrmSalaryTaskImportAccountOperation(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        private HrmMatrixAllocResult _MatrixAllocResultKB;
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public HrmMatrixAllocResult MatrixAllocResultKB {
            get { return _MatrixAllocResultKB; }
            set { SetPropertyValue<HrmMatrixAllocResult>("MatrixAllocResultKB", ref _MatrixAllocResultKB, value); }
        }

        private HrmMatrixAllocResult _MatrixAllocResultOZM;
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public HrmMatrixAllocResult MatrixAllocResultOZM {
            get { return _MatrixAllocResultOZM; }
            set { SetPropertyValue<HrmMatrixAllocResult>("MatrixAllocResult", ref _MatrixAllocResultOZM, value); }
        }

        [Browsable(false)]
        private bool isSourceDataImported {
            get { return !(State == HrmSalaryTaskState.HRM_SALARY_TASK_CREATED); }
        }

        protected override void InObjectsLoad() {
            if (MatrixAllocResultKB != null)
                InObjects.Add(MatrixAllocResultKB);
            if (MatrixAllocResultOZM != null)
                InObjects.Add(MatrixAllocResultOZM);
        }

        public String Name {
            get {
                return "������ ��������� ��������" + " " + (Period.Month + "-" + Period.Year).ToString();
            }
        }


    }
}