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
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Layout;

namespace NpoMash.Erm.Hrm.Salary {

    [MapInheritance(MapInheritanceType.ParentTable)]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "AcceptImport", Criteria = "isSourceDataImported", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance("", AppearanceItemType = "Action", TargetItems = "Delete, New", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, TargetItems = "*", Context = "Any", Enabled = false)]

    public class HrmSalaryTaskImportSourceData : HrmSalaryTask { 

        private HrmTimeSheet _TimeSheetKB;
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public HrmTimeSheet TimeSheetKB {
            get { return _TimeSheetKB; }
            set { SetPropertyValue<HrmTimeSheet>("TimeSheetKB", ref _TimeSheetKB, value); }
        }

        private HrmTimeSheet _TimeSheetOZM;
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public HrmTimeSheet TimeSheetOZM {
            get { return _TimeSheetOZM; }
            set { SetPropertyValue<HrmTimeSheet>("TimeSheetOZM", ref _TimeSheetOZM, value); }
        }

        private HrmMatrixPlan _MatrixPlanKB;
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public HrmMatrixPlan MatrixPlanKB {
            get { return _MatrixPlanKB; }
            set { SetPropertyValue<HrmMatrixPlan>("MatrixPlanKB", ref _MatrixPlanKB, value); }
        }

        private HrmMatrixPlan _MatrixPlanOZM;
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public HrmMatrixPlan MatrixPlanOZM {
            get { return _MatrixPlanOZM; }
            set { SetPropertyValue<HrmMatrixPlan>("MatrixPlanOZM", ref _MatrixPlanOZM, value); }
        }

        public Type PeriodObjectType {
            get { return typeof(HrmSalaryTaskImportSourceData); }
        }

        public String Name {
            get {
                return "Импорт исходных данных" + " " + (Period.Month + "-" + Period.Year).ToString();

            }
        }



        public Type TaskObjectType {
            get { return PeriodObjectType; }
        }

        public String TaskObjectName {
            get { return Name; }
        }

        public String TaskObjectStatus {
            get { return Period.CurrentAllocParameter.Status.ToString(); }
        }

        [Browsable(false)]
        public bool isSourceDataImported {
            get { return !(State == HrmSalaryTaskState.HRM_SALARY_TASK_CREATED); } 
        }

        public HrmSalaryTaskImportSourceData(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        protected override void InObjectsLoad() {
            if (Period.AllocParameters != null)
                InObjects.Add(Period.CurrentAllocParameter);
        }
    }
}