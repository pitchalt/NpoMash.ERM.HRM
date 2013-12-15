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

namespace NpoMash.Erm.Hrm.Salary {

    public enum HrmMatrixStatus {
        Opened = 0,
        Saved = 1,
        Accepted,
        Closed
    }

    public enum HrmMatrixType {
        Matrix = 0,
        AllocResult = 1
    }

    public enum HrmMatrixTypeMatrix {
        Planned = 0,
        Coerced = 1,
        ReserveMatrix = 2
    }

    public enum HrmMatrixGroupDep {
        KB = 0,
        OZM = 1
    }

    public enum HrmMatrixVariant { }

    [Persistent("HrmMatrix")]
    public class HrmMatrix : BaseObject {

        private HrmMatrixType _Type;
        public HrmMatrixType Type {
            get { return _Type; }
            set { SetPropertyValue<HrmMatrixType>("Type", ref _Type, value); }
        }

        private HrmMatrixTypeMatrix _TypeMatrix;
        public HrmMatrixTypeMatrix TypeMatrix {
            get { return _TypeMatrix; }
            set { SetPropertyValue<HrmMatrixTypeMatrix>("TypeMatrix", ref _TypeMatrix, value); }
        }

        private HrmMatrixGroupDep _GroupDep;
        public HrmMatrixGroupDep GroupDep {
            get { return _GroupDep; }
            set { SetPropertyValue<HrmMatrixGroupDep>("GroupDep", ref _GroupDep, value); }
        }

        private HrmMatrixStatus _Status;
        public HrmMatrixStatus Status {
            get { return _Status; }
            set { SetPropertyValue<HrmMatrixStatus>("Status", ref _Status, value); }
        }

        private HrmMatrixVariant _Variant;
        public HrmMatrixVariant Variant {
            get { return _Variant; }
            set { SetPropertyValue<HrmMatrixVariant>("Variant", ref _Variant, value); }
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

        public HrmMatrix(Session session) : base(session) { }
        public override void AfterConstruction() {base.AfterConstruction(); }
    }
}
