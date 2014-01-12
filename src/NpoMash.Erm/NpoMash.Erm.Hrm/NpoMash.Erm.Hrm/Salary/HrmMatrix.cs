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
//
using IntecoAG.ERM.HRM.Organization;
namespace NpoMash.Erm.Hrm.Salary {

    public enum HRM_MATRIX_STATUS {
        Opened = 0,
        Saved = 1,
        Accepted=2,
        Closed=3,
        Exported=4
    }

    public enum HRM_MATRIX_TYPE {
        Matrix = 0,
        AllocResult = 1
    }

    public enum HRM_MATRIX_TYPE_MATRIX {
        Planned = 0,
        Coerced = 1,
        ReserveMatrix = 2
    }

    //public enum HRM_MATRIX_GROUP_DEP {
       // KB = 0,
       // OZM = 1
   // }

    public enum HRM_MATRIX_VARIANT { 
        MinimizeNumberOfDeviations=0,
        MinimizeMaximumDeviations=1,
        ProportionsMethod=2
    }


    [Persistent("HrmMatrix")]
   // [Appearance("Enabled", TargetItems = "*", Context = "Any", Criteria = "TypeMatrix='Planned'", Enabled = false)]
    public class HrmMatrix : BaseObject {

        private HRM_MATRIX_TYPE _Type;
        public HRM_MATRIX_TYPE Type {
            get { return _Type; }
            set { SetPropertyValue<HRM_MATRIX_TYPE>("Type", ref _Type, value); }
        }

        private HRM_MATRIX_TYPE_MATRIX _TypeMatrix;
        //[Appearance("", Criteria = "'Planned'", Visibility = ViewItemVisibility.Hide)]
        public HRM_MATRIX_TYPE_MATRIX TypeMatrix {
            get { return _TypeMatrix; }
            set { SetPropertyValue<HRM_MATRIX_TYPE_MATRIX>("TypeMatrix", ref _TypeMatrix, value); }
        }

        private DEPARTMENT_GROUP_DEP _GroupDep;
        public DEPARTMENT_GROUP_DEP GroupDep {
            get { return _GroupDep; }
            set { SetPropertyValue<DEPARTMENT_GROUP_DEP>("GroupDep", ref _GroupDep, value); }
        }

        private HRM_MATRIX_STATUS _Status;
        [Appearance("", Criteria = "isPlanned", Context = "DetailView,ListView", Visibility = ViewItemVisibility.Hide)]
        public HRM_MATRIX_STATUS Status {
            get { return _Status; }
            set { SetPropertyValue<HRM_MATRIX_STATUS>("Status", ref _Status, value); }
        }

        private HRM_MATRIX_VARIANT _Variant;
        [Appearance("", Criteria = "isPlanned", Context = "DetailView,ListView", Visibility = ViewItemVisibility.Hide)]
        public HRM_MATRIX_VARIANT Variant {
            get { return _Variant; }
            set { SetPropertyValue<HRM_MATRIX_VARIANT>("Variant", ref _Variant, value); }
        }

        private Int16 _IterationNumber;
        [Appearance("", Criteria = "isPlanned", Context = "DetailView,ListView", Visibility = ViewItemVisibility.Hide)]
        public Int16 IterationNumber {
            get { return _IterationNumber; }
            set { SetPropertyValue<Int16>("IterationNumber", ref _IterationNumber, value); }
        }

        [Association("Matrix-Rows"), Aggregated] //Коллекция HrmMatrixRow
        public XPCollection<HrmMatrixRow> Rows {
            get { return GetCollection<HrmMatrixRow>("Rows"); }
        }

        [Association("Matrix-Columns"), Aggregated] //Коллекция HrmMatrixColumn
        public XPCollection<HrmMatrixColumn> Columns {
            get { return GetCollection<HrmMatrixColumn>("Columns"); }
        }

        private HrmPeriod _Period; // Ссылка на HrmPeriod
        [Association("Period-Matrixs")]
        public HrmPeriod Period {
            get { return _Period; }
            set { SetPropertyValue<HrmPeriod>("Period", ref _Period, value); }
        }

        [Browsable(false)]
        public bool isPlanned { get { return TypeMatrix == HRM_MATRIX_TYPE_MATRIX.Planned; } }

        public HrmMatrix(Session session) : base(session) { }
        public override void AfterConstruction() {base.AfterConstruction(); }
    }
}
