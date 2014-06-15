using System;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;
//
using DevExpress.Xpo;
using DevExpress.Xpo.Helpers;
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
//
using NpoMash.Erm.Hrm.Salary;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.HRM.Organization;
//
namespace NpoMash.Erm.Hrm {

    public enum HrmPeriodStatus {
        OPENED = 0,
        SOURCE_DATA_LOADED = 1,
        LIST_OF_CONTROLLED_ORDERS_ACCEPTED = 2,
        READY_TO_CALCULATE_COERCED_MATRIXS = 3,
        CLOSED = 4,
        READY_TO_EXPORT_CORCED_MATRIXS = 5,
        COERCED_MATRIXES_EXPORTED = 6,
        ACCOUNT_OPERATION_FIRST_IMPORTED = 7,
        READY_TO_RESERVE_MATRIX_CREATE = 8,
        READY_TO_RESERVE_MATRIX_UPLOAD = 9,
        RESERVE_MATRIX_UPLOADED = 10,
        ACCOUNT_OPERATION_LAST_IMPORTED = 11
    }

    [NavigationItem("A1 Integration")]
    [Persistent("HrmPeriod")]
    [RuleCombinationOfPropertiesIsUnique("", DefaultContexts.Save, "Year, Month")]
    [Appearance("Enabled", TargetItems = "*", Criteria = "Status = 'closed'", Context = "Any", Enabled = false)]
    [Appearance("Visibility", AppearanceItemType = "Action", TargetItems = "Delete, New", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "HrmPeriodVC_ExportBringingMatrix", Criteria = "isReadyToExportMatrixes", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "BringingKBMatrixAction", Criteria = "isReadyToBringMatrixes", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "GetSourceDataAction", Criteria = "isSourceDataImported", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "HrmPeriodVC_ImportAccountOperationLast", Criteria = "Status!='RESERVE_MATRIX_UPLOADED'", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "HrmPeriodVC_RevertState", Criteria = "isReadyToRevertChanges", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "HrmPeriodVC_ClosePeriod", Criteria = "isReadyToClosePeriod", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "HrmPeriodVC_CreateReportSummary", Criteria = "isReadyToCreateLastAccountReports", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "HrmPeriodVC_CreateReportKB", Criteria = "isReadyToCreateFirstAccountReports", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "HrmPeriodVC_CreateReportOZM", Criteria = "isReadyToCreateFirstAccountReports", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "BringingKBMatrixAction", Criteria = "kbReductionExists", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "BringingOZMMatrixAction", Criteria = "ozmReductionExists", Context = "Any", Visibility = ViewItemVisibility.Hide)]   
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "BringProvisionMatrix", Criteria = "isReadyToBringProvision", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "ExportReserveMatrix", Criteria = "Status!='READY_TO_RESERVE_MATRIX_UPLOAD'", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "HrmPeriodVC_ImportAccountOperation", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "AccountOperationImport", Criteria = "isReadyToImportAccountOperation", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "HrmPeriodVC_CreateReportKB", Criteria = "!isKBCoercedMatrixExported", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "HrmPeriodVC_CreateReportOZM", Criteria = "!isOZMCoercedMatrixExported", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "BringingOZMMatrixAction", Criteria = "isReadyToBringMatrixes", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, AppearanceItemType = "Action", TargetItems = "HrmPeriodVC_RevertState", Criteria = "isReverted", Context = "Any", Visibility = ViewItemVisibility.Hide)]


    public class HrmPeriod : BaseObject, IPeriod, IPersistentInterface<IPeriod>, IPersistentInterfaceData<IPeriod> {

        [Persistent("Year")]
        private Int16 _Year;
        [Indexed("Month", Unique = true)]
        [PersistentAlias("_Year")]
        public Int16 Year {
            get { return _Year; }
        }

        [Persistent("Month")]
        private Int16 _Month;
        [PersistentAlias("_Month")]
        public Int16 Month {
            get { return _Month; }

        }

        private HrmTimeSheet _CurrentTimeSheetKB; // Ссылка на HrmTimeSheet
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public HrmTimeSheet CurrentTimeSheetKB {
            get { return _CurrentTimeSheetKB; }
            set { SetPropertyValue<HrmTimeSheet>("CurrentTimeSheetKB", ref _CurrentTimeSheetKB, value); }
        }

        private HrmTimeSheet _CurrentTimeSheetOZM; // Ссылка на HrmTimeSheet
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public HrmTimeSheet CurrentTimeSheetOZM {
            get { return _CurrentTimeSheetOZM; }
            set { SetPropertyValue<HrmTimeSheet>("CurrentTimeSheetOZM", ref _CurrentTimeSheetOZM, value); }
        }

        private HrmSalaryTaskMatrixReduction _CurrentOZMmatrixReduction;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public HrmSalaryTaskMatrixReduction CurrentOZMmatrixReduction {
            get { return _CurrentOZMmatrixReduction; }
            set { SetPropertyValue<HrmSalaryTaskMatrixReduction>("CurrentOZMmatrixReduction", ref _CurrentOZMmatrixReduction, value); }
        }


        private HrmSalaryTaskMatrixReduction _CurrentKBmatrixReduction;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public HrmSalaryTaskMatrixReduction CurrentKBmatrixReduction {
            get { return _CurrentKBmatrixReduction; }
            set { SetPropertyValue<HrmSalaryTaskMatrixReduction>("CurrentKBmatrixReduction", ref _CurrentKBmatrixReduction, value); }
        }

        private HrmSalaryTaskProvisionMatrixReduction _CurrentProvisionMatrix; //Матрица резерва
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public HrmSalaryTaskProvisionMatrixReduction CurrentProvisionMatrix {
            get { return _CurrentProvisionMatrix; }
            set { SetPropertyValue<HrmSalaryTaskProvisionMatrixReduction>("CurrentProvisionMatrix", ref _CurrentProvisionMatrix, value); }
        }

        private HrmSalaryTaskImportAccountOperation _CurrentAccountOperation;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public HrmSalaryTaskImportAccountOperation CurrentAccountOperation {
            get { return _CurrentAccountOperation; }
            set { SetPropertyValue<HrmSalaryTaskImportAccountOperation>("CurrentAccountOperation", ref _CurrentAccountOperation, value); }

        }

        [Association("Period-TimeSheets")] // Коллекция HrmTimeSheet
        public XPCollection<HrmTimeSheet> TimeSheets {
            get { return GetCollection<HrmTimeSheet>("TimeSheets"); }
        }


        [Persistent("Status")]
        private HrmPeriodStatus _Status;
        [RuleRequiredField(DefaultContexts.Save)]
        [PersistentAlias("_Status")]
        public HrmPeriodStatus Status {
            get { return _Status; }
            // set { SetPropertyValue<HrmPeriodStatus>("Status", ref _Status, value); }
        }

        [Association("HrmPeriod-HrmSalaryTask"), Aggregated]
        public XPCollection<HrmSalaryTask> PeriodTasks {
            get { return GetCollection<HrmSalaryTask>("PeriodTasks"); }
        }

        private HrmMatrixPlan _CurrentMatrixAllocPlanSummary;
        [VisibleInLookupListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public HrmMatrixPlan CurrentMatrixAllocPlanSummary {
            get { return _CurrentMatrixAllocPlanSummary; }
            set { SetPropertyValue<HrmMatrixPlan>("CurrentMatrixAllocPlanSummary", ref _CurrentMatrixAllocPlanSummary, value); }
        }

        private HrmMatrixLastAccount _CurrentMatrixAllocResultSummary;
        [VisibleInLookupListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public HrmMatrixLastAccount CurrentMatrixAllocResultSummary {
            get { return _CurrentMatrixAllocResultSummary; }
            set { SetPropertyValue<HrmMatrixLastAccount>("CurrentMatrixAllocResultSummary", ref _CurrentMatrixAllocResultSummary, value); }
        }

        private HrmMatrixPlan _CurrentMatrixAllocPlanKB;
        [VisibleInLookupListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public HrmMatrixPlan CurrentMatrixAllocPlanKB {
            get { return _CurrentMatrixAllocPlanKB; }
            set { SetPropertyValue<HrmMatrixPlan>("CurrentMatrixAllocPlanKB", ref _CurrentMatrixAllocPlanKB, value); }
        }

        private HrmMatrixPlan _CurrentMatrixAllocPlanOZM;
        [VisibleInLookupListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public HrmMatrixPlan CurrentMatrixAllocPlanOZM {
            get { return _CurrentMatrixAllocPlanOZM; }
            set { SetPropertyValue<HrmMatrixPlan>("CurrentMatrixAllocPlanOZM", ref _CurrentMatrixAllocPlanOZM, value); }
        }

        private HrmMatrixAllocResult _CurrentMatrixAllocResultKB;
        [VisibleInLookupListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public HrmMatrixAllocResult CurrentMatrixAllocResultKB {
            get { return _CurrentMatrixAllocResultKB; }
            set { SetPropertyValue<HrmMatrixAllocResult>("CurrentMatrixAllocResultKB", ref _CurrentMatrixAllocResultKB, value); }
        }

        private HrmMatrixAllocResult _CurrentMatrixAllocResultOZM;
        [VisibleInLookupListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public HrmMatrixAllocResult CurrentMatrixAllocResultOZM {
            get { return _CurrentMatrixAllocResultOZM; }
            set { SetPropertyValue<HrmMatrixAllocResult>("CurrentMatrixAllocResultOZM", ref _CurrentMatrixAllocResultOZM, value); }
        }

        private HrmAllocParameter _CurrentAllocParameter; // Ссылка на HrmPeriodAllocParameter
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public HrmAllocParameter CurrentAllocParameter {
            get { return _CurrentAllocParameter; }
            set { SetPropertyValue<HrmAllocParameter>("CurrentAllocParameter", ref _CurrentAllocParameter, value); }
        }

        [Association("Period-AllocParameters")]   // коллекция HrmPeriodAllocParameter
        public XPCollection<HrmAllocParameter> AllocParameters {
            get { return GetCollection<HrmAllocParameter>("AllocParameters"); }
        }

        private HrmPeriod _PeriodPrevious; // Сслыка на самого себя
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public HrmPeriod PeriodPrevious {
            get { return _PeriodPrevious; }
            set { SetPropertyValue<HrmPeriod>("PeriodPrevious", ref _PeriodPrevious, value); }
        }


        [Association("Period-Matrixs"), Aggregated] //Коллекция Matrixs
        [Index(0), VisibleInListView(true), VisibleInDetailView(true)]
        public XPCollection<HrmMatrix> Matrixs {
            get { return GetCollection<HrmMatrix>("Matrixs"); }
        }


        public void setStatus(HrmPeriodStatus stat) {
            if (stat == HrmPeriodStatus.READY_TO_CALCULATE_COERCED_MATRIXS) {
                try {
                    stat = HrmPeriodLogic.SetReadyToCalculateCoercedMatrixesStatus(this);
                }
                catch (InvalidOperationException) {
                    stat = Status;
                }
            }
            SetPropertyValue<HrmPeriodStatus>("Status", ref _Status, stat);
        }

        [Association("HrmPeriod-HrmSalaryLogRecord")]
        [Browsable(false)]
        public XPCollection<HrmLogRecord> LogRecordCol {
            get {
                return GetCollection<HrmLogRecord>("LogRecordCol");
            }
        }



        public void Init(Int16 y, Int16 m) {
            SetPropertyValue<Int16>("Year", ref _Year, y);
            SetPropertyValue<Int16>("Month", ref _Month, m);
        }

        public HrmPeriod(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            setStatus(HrmPeriodStatus.OPENED);


        }

        [Aggregated]
        public IList<ILogRecord> LogRecords {
            get {
                return new ListConverter<ILogRecord, HrmLogRecord>(LogRecordCol);
            }
        }

        public void LogRecord(LogRecordType type, Department department, fmCOrder order, String text) {
            HrmLogRecord record = new HrmLogRecord(this.Session);
            record.Init(type, text, this, null, department, order);
        }

        [Association("HrmPeriod-HrmPeriodObject")]
        [Aggregated]
        protected XPCollection<HrmPeriodObject> PeriodObjectCol {
            get { return GetCollection<HrmPeriodObject>("PeriodObjectCol"); }
        }

        PersistentInterfaceMorpher<IPeriodObject> _PeriodObjects;
        [PersistentAlias("[PeriodObjectCol]")]
        [Aggregated]
        public IList<IPeriodObject> PeriodObjects {
            get {
                if (this._PeriodObjects == null)
                    this._PeriodObjects = new PersistentInterfaceMorpher<IPeriodObject>(new ListConverter<IPersistentInterfaceData<IPeriodObject>, HrmPeriodObject>(this.PeriodObjectCol));
                return (IList<IPeriodObject>)this._PeriodObjects;
            }
        }

        public IPersistentInterfaceData<IPeriod> PersistentInterfaceData {
            get { return this; }
        }

        public IPeriod Instance {
            get { return this; }
        }

        [Browsable(false)]
        private bool isReadyToCreateReserveMatrix {
            get {
                return !(Status == HrmPeriodStatus.ACCOUNT_OPERATION_FIRST_IMPORTED && 
                (
                    //(CurrentKBmatrixReduction.MinimizeMaximumDeviationsMatrix.Status == HrmMatrixStatus.MATRIX_ACCEPTED)||
                    (CurrentKBmatrixReduction.MinimizeNumberOfDeviationsMatrix.Status == HrmMatrixStatus.MATRIX_ACCEPTED)
                    //||(CurrentKBmatrixReduction.ProportionsMethodMatrix.Status == HrmMatrixStatus.MATRIX_ACCEPTED)
                    ) && 
                (
                    //(CurrentOZMmatrixReduction.MinimizeMaximumDeviationsMatrix.Status == HrmMatrixStatus.MATRIX_ACCEPTED)||
                    (CurrentOZMmatrixReduction.MinimizeNumberOfDeviationsMatrix.Status == HrmMatrixStatus.MATRIX_ACCEPTED)
                    //|| (CurrentOZMmatrixReduction.ProportionsMethodMatrix.Status == HrmMatrixStatus.MATRIX_ACCEPTED)
                    ) && 
                CurrentAllocParameter.Status == HrmPeriodAllocParameterStatus.LIST_OF_ORDER_ACCEPTED);
            }
        }

        public Type PeriodObjectType {
            get { return typeof(HrmPeriod); }
        }

        public String Name {
            get {
                EnumDescriptor ed = new EnumDescriptor(typeof(HrmPeriodAllocParameterStatus));

                return "Период" + " " + (Month + "-" + Year).ToString();
            }
        }







        [Browsable(false)]
        private bool isReadyToRevertChanges { get { return (Status == HrmPeriodStatus.OPENED); } }

        [Browsable(false)]
        private bool isReadyToClosePeriod { get { return !(Status == HrmPeriodStatus.ACCOUNT_OPERATION_LAST_IMPORTED && CurrentMatrixAllocResultSummary.Status == HrmMatrixStatus.MATRIX_ACCEPTED); } }

        [Browsable(false)]
        private bool isReadyToCreateLastAccountReports { get { return !(Status == HrmPeriodStatus.ACCOUNT_OPERATION_LAST_IMPORTED && CurrentMatrixAllocResultSummary.Status == HrmMatrixStatus.MATRIX_DOWNLOADED); } }

        [Browsable(false)]
        private bool isReadyToImportAccountOperationLast { get { return !(Status == HrmPeriodStatus.RESERVE_MATRIX_UPLOADED); } }

        [Browsable(false)]
        private bool isReadyToCreateFirstAccountReports { get { return !(Status == HrmPeriodStatus.ACCOUNT_OPERATION_FIRST_IMPORTED); } }

        [Browsable(false)]
        private bool isReadyToImportAccountOperation { get { return !(Status == HrmPeriodStatus.COERCED_MATRIXES_EXPORTED); } }

        [Browsable(false)]
        private bool isReadyToExportMatrixes { get { return !(Status == HrmPeriodStatus.READY_TO_EXPORT_CORCED_MATRIXS); } }

        [Browsable(false)]
        private bool isReadyToBringMatrixes { get { return !(Status == HrmPeriodStatus.READY_TO_CALCULATE_COERCED_MATRIXS); } }

        [Browsable(false)]
        private bool isReadyToBringProvision { get { 
            return !(Status == HrmPeriodStatus.READY_TO_RESERVE_MATRIX_CREATE 
                && CurrentAllocParameter.Status == HrmPeriodAllocParameterStatus.ALLOC_PARAMETERS_ACCEPTED
                && (this.CurrentProvisionMatrix == null || 
                this.CurrentProvisionMatrix.ReserveMatrixEvristic == null
                || this.CurrentProvisionMatrix.ReserveMatrixSimplex == null) );
        } }

        [Browsable(false)]
        private bool isSourceDataImported { get { return HrmPeriodLogic.SourceDataIsLoaded(this) || (Status == HrmPeriodStatus.CLOSED); } }
        //private bool isSourceDataImported { get { return !(HrmPeriodLogic.SourceDataIsLoaded(this) && CurrentAllocParameter.Status == HrmPeriodAllocParameterStatus.LIST_OF_ORDER_ACCEPTED); } }

        [Browsable(false)]
        private bool isAccountOperationCompared {
            get { return HrmPeriodLogic.AccountOperationCompared(this) && CurrentAllocParameter.Status==HrmPeriodAllocParameterStatus.ALLOC_PARAMETERS_ACCEPTED; }
        }

        [Browsable(false)]
        private bool isKBCoercedMatrixExported { get { return HrmPeriodLogic.KBAccountOperationCompared(this); } }

        [Browsable(false)]
        private bool isOZMCoercedMatrixExported { get { return HrmPeriodLogic.OZMAccountOperationCompared(this); } }

        [Browsable(false)]
        private bool isReverted { get { return (Status == HrmPeriodStatus.CLOSED); } }

        [Browsable(false)]
        private bool kbReductionExists {
            get {
                if (CurrentKBmatrixReduction != null) {
                    return true;
                }
                else return false;
            }
        }

        [Browsable(false)]
        private bool ozmReductionExists {
            get {
                if (CurrentOZMmatrixReduction != null) {
                    return true;
                }
                else return false;
            }
        }
    }
}