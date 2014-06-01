using System;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;
//
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
//
using IntecoAG.ERM.HRM.Organization;
using NpoMash.Erm.Hrm.Salary.Matrix;

namespace NpoMash.Erm.Hrm.Salary {

    public enum HrmMatrixStatus {
        MATRIX_SAVED = 0,
        MATRIX_ACCEPTED=1,
        MATRIX_CLOSED=2,
        MATRIX_EXPORTED=3,
        MATRIX_PRIMARY_ACCEPTED=4,
        MATRIX_ARCHIVE=5,
        MATRIX_DOWNLOADED =6,
        NOTDOWNLOADED = 7
    }

    public enum HrmMatrixType {
        TYPE_MATIX = 0,
        TYPE_ALLOC_RESULT = 1
    }

    public enum HrmMatrixTypeMatrix {
        MATRIX_PLANNED = 0,
        MATRIX_COERCED = 1,
        MATRIX_RESERVE = 2
    }

    public enum HrmMatrixVariant {
        MINIMIZE_NUMBER_OF_DEVIATIONS_VARIANT=0,
        MINIMIZE_MAXIMUM_DEVIATIONS_VARIANT=1,
        PROPORTIONS_METHOD_VARIANT=2,
    }


    [Persistent("HrmMatrix")]
    [Appearance("", AppearanceItemType = "Action", TargetItems = "Delete, New", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance(null, TargetItems = "*", Criteria = "isPlanned", Context = "Any", Enabled = false)]

    [DefaultProperty("Name")]
    public class HrmMatrix : BaseObject, IMatrix {

        /// <summary>
        /// Ссылка на HrmPeriodMatrixBaseObject
        /// </summary>
        [Aggregated]
        [Persistent]
        private HrmMatrixPeriodObject _MatrixPeriodObject;
        //public HrmMatrixPeriodObject _PeriodObject {
        //    get { return _MatrixPeriodObject; }
        //    set { SetPropertyValue<HrmMatrixPeriodObject>("MatrixPeriodObject", ref _MatrixPeriodObject, value); }
        //}
        //
        private HrmMatrixType _Type;
        [Appearance("", Enabled=false)]
        public HrmMatrixType Type {
            get { return _Type; }
            set { SetPropertyValue<HrmMatrixType>("Type", ref _Type, value); }
        }

        private HrmMatrixTypeMatrix _TypeMatrix;
        [Appearance("", Enabled=false)]
        public HrmMatrixTypeMatrix TypeMatrix {
            get { return _TypeMatrix; }
            set { SetPropertyValue<HrmMatrixTypeMatrix>("TypeMatrix", ref _TypeMatrix, value); }
        }

        private DepartmentGroupDep _GroupDep;
        [Appearance("", Enabled=false)]
        public DepartmentGroupDep GroupDep {
            get { return _GroupDep; }
            set { SetPropertyValue<DepartmentGroupDep>("GroupDep", ref _GroupDep, value); }
        }

        private HrmMatrixStatus _Status;
        // [Appearance("", Criteria = "isPlanned", Context = "DetailView,ListView", Visibility = ViewItemVisibility.Hide)]
        [Appearance("", Enabled=false)]
        public HrmMatrixStatus Status {
            get { return _Status; }
            set { SetPropertyValue<HrmMatrixStatus>("Status", ref _Status, value); }
        }

        private HrmMatrixVariant _Variant;
        [Appearance("", Criteria = "isPlanned", Context = "DetailView,ListView", Visibility = ViewItemVisibility.Hide)]
        [Appearance("", Enabled=false)]
        public HrmMatrixVariant Variant {
            get { return _Variant; }
            set { SetPropertyValue<HrmMatrixVariant>("Variant", ref _Variant, value); }
        }

        private Int16 _IterationNumber;
        [Appearance("", Criteria = "isPlanned", Context = "DetailView,ListView", Visibility = ViewItemVisibility.Hide)]
        [Appearance("", Enabled=false)]
        public Int16 IterationNumber {
            get { return _IterationNumber; }
            set { SetPropertyValue<Int16>("IterationNumber", ref _IterationNumber, value); }
        }

        [Association("HrmMatrix-Rows"), Aggregated] //Коллекция HrmMatrixRow
        public XPCollection<HrmMatrixRow> Rows {
            get { return GetCollection<HrmMatrixRow>("Rows"); }
        }

        [Association("HrmMatrix-Columns"), Aggregated] //Коллекция HrmMatrixColumn
        public XPCollection<HrmMatrixColumn> Columns {
            get { return GetCollection<HrmMatrixColumn>("Columns"); }
        }

        //[Association("HrmMatrix-Slices"),Aggregated] //Коллекция HrmMatrixSlice
        //public HrmSalaryMatrixSliceCollection Slices {
        //    get { return new HrmSalaryMatrixSliceCollection(this.Session, this, this.ClassInfo.GetMember("Slices")); }
        //}

        private HrmPeriod _Period; // Ссылка на HrmPeriod
        [Appearance("", Enabled=false)]
        [Association("Period-Matrixs")]
        public HrmPeriod Period {
            get { return _Period; }
            set {
                SetPropertyValue<HrmPeriod>("Period", ref _Period, value);
                if (!IsLoading) {
                    _MatrixPeriodObject.Period = value;
                    //PeriodBase = value;
                }
            }
        }

        [Browsable(false)]
        public bool isPlanned { get { return TypeMatrix == HrmMatrixTypeMatrix.MATRIX_PLANNED; } }

        public HrmMatrix(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            _MatrixPeriodObject = new HrmMatrixPeriodObject(this);
        }


        //public HrmSalaryPeriodObjectStatus PeriodObjectStatus {
        //    get {
        //        return _MatrixPeriodObject.Status;
        //    }
        //}
        public String PeriodObjectStatus {
            get {
                EnumDescriptor ed = new EnumDescriptor(typeof(HrmMatrixStatus));
                return ed.GetCaption(Status);
            }
        }

        public Type PeriodObjectType {
            get {
                return this.GetType();
            }
        }

        public Type TaskObjectType {
            get { return PeriodObjectType; }
        }

        public String TaskObjectName {
            get { return Name; }
        }

        public String Name {
            get {
                EnumDescriptor ed = new EnumDescriptor(typeof(HrmMatrixStatus));
                return ed.GetCaption(Status); //+ " " + (Period.Year * 100 + Period.Month).ToString() + " " + PeriodObjectType.Name;
            }
        }
    }

    //public class HrmSalaryMatrixSliceCollection : XPCollection<HrmSalaryMatrixSlice> {
    //    public HrmSalaryMatrixSliceCollection(Session session, HrmMatrix matrix, XPMemberInfo property) : base(session, matrix, property) { }
    //}

}