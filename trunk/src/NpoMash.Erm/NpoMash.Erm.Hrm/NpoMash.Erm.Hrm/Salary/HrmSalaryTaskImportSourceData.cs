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
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

// With XPO, the data model is declared by classes (so-called Persistent Objects) that will define the database structure, and consequently, the user interface (http://documentation.devexpress.com/#Xaf/CustomDocument2600).
namespace NpoMash.Erm.Hrm.Salary {
    // Specify various UI options for your persistent class and its properties using a declarative approach via built-in attributes (http://documentation.devexpress.com/#Xaf/CustomDocument3146).
    //[ImageName("BO_Contact")]
    //[DefaultProperty("PersistentProperty")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewAndDetailView, true, NewItemRowPosition.Top)]
    [Persistent("HrmSalaryTaskImportSourceData")]
    public class HrmSalaryTaskImportSourceData : HrmSalaryTask { // You can use a different base persistent class based on your requirements (http://documentation.devexpress.com/#Xaf/CustomDocument3146).

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
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here or place it only when the IsLoading property is false.
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place here your initialization code (check out http://documentation.devexpress.com/#Xaf/CustomDocument2834 for more details).
        }

    }
}
