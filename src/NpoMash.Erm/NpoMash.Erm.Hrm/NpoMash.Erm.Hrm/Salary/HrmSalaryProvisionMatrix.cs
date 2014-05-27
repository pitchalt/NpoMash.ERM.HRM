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
    public class HrmSalaryProvisionMatrix : HrmMatrix { 

        public HrmSalaryProvisionMatrix(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }
}