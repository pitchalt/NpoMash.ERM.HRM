using System;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;
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
    public class HrmAllocParameterPeriodObject : HrmPeriodObject {

        [ExplicitLoading(0)]
        [DevExpress.Xpo.Aggregated]
        [Persistent("AllocParameter")]
        private HrmAllocParameter _AllocParameter;
        //        public HrmPeriodAllocParameter AllocParameters {
        //            get { return _AllocParameters; }
        //            set { SetPropertyValue<HrmPeriodAllocParameter>("AllocParameters", ref _AllocParameters, value); }
        //        }

        public override IPeriodObject Instance {
            get {
                return this._AllocParameter;
            }
        }

        public override Type PeriodObjectType {
            get { return typeof(HrmAllocParameter); }
        }

        public HrmAllocParameterPeriodObject(Session session)
            : base(session) {
        }

        public HrmAllocParameterPeriodObject(HrmAllocParameter instance)
            : base(instance.Session) {
            _AllocParameter = instance;
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

    }
}
