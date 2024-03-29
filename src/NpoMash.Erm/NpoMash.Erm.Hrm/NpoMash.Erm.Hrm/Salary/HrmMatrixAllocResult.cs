using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
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
namespace NpoMash.Erm.Hrm.Salary {

    [MapInheritance(MapInheritanceType.ParentTable)]
    public abstract class HrmMatrixAllocResult : HrmMatrix {

        [Association("AccountOperations-TYPE_ALLOC_RESULT"), Aggregated]
        public XPCollection<HrmAccountOperation> AccountOperations {
            get { return GetCollection<HrmAccountOperation>("AccountOperations"); }
        }

        public HrmMatrixAllocResult(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

        public String Name1 {
            get {

                return "��������� �������������";// +" " + (Period.Month + "-" + Period.Year).ToString();
            }
        }
    }
}