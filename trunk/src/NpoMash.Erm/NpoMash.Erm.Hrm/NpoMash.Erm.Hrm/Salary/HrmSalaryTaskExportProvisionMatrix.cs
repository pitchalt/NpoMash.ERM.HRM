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

    [Persistent("HrmSalaryTaskExportProvisionMatrix")]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "HrmSalaryTaskExportProvisionMatrixVC_ExportProvisionMatrix", Criteria = "isMatrixExported", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance("", AppearanceItemType = "Action", TargetItems = "Delete, New", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, TargetItems = "*", Context = "Any", Enabled = false)]
    public class HrmSalaryTaskExportProvisionMatrix : HrmSalaryTask { 
        public HrmSalaryTaskExportProvisionMatrix(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

        private HrmMatrix _ProvisionMatrix;
        public HrmMatrix ProvisionMatrix {
            get { return _ProvisionMatrix; }
            set { SetPropertyValue<HrmMatrix>("ProvisionMatrix", ref _ProvisionMatrix, value); }
        }

        [Browsable(false)]
        private bool isMatrixExported { get { return State == HrmSalaryTaskState.HRM_SALARY_TASK_COMPLETED; } }
    }
}