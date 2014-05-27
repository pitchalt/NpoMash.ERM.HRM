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
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
//

namespace NpoMash.Erm.Hrm.Salary {

    [MapInheritance(MapInheritanceType.ParentTable)]
    public class HrmMatrixPeriodObject : HrmPeriodObject {
        [ExplicitLoading(0)]
        [DevExpress.Xpo.Aggregated]
        [Persistent("Matrix")]
        private HrmMatrix _Matrix;

        public override Type ObjectType {
            get { return _Matrix.GetType(); }
        }

        public override IPeriodObject Instance {
            get { return _Matrix; }
        }

        public HrmMatrixPeriodObject(Session session)
            : base(session) {
        }
        public HrmMatrixPeriodObject(HrmMatrix instance)
            : base(instance.Session) {
                _Matrix = instance;
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }

    }
}
