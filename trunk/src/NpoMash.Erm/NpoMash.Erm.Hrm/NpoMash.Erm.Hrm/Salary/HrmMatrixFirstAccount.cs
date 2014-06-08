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
    /// <summary>
    /// Матрица первичной проводки
    /// </summary>
    [MapInheritance(MapInheritanceType.ParentTable)]
    public class HrmMatrixFirstAccount : HrmMatrixAllocResult {

        public HrmMatrixFirstAccount(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }
}
