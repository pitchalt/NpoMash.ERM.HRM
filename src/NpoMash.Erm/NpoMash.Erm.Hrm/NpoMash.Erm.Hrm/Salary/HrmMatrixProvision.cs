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

    /// <summary>
    /// ������� �������
    /// </summary>
    [MapInheritance(MapInheritanceType.ParentTable)]
    public class HrmMatrixProvision : HrmMatrix {

        public HrmMatrixProvision(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

        public String Name1 {
            get {

                return "������� �������";// +" " + (Period.Month + "-" + Period.Year).ToString();
            }
        }

    
    }
}
