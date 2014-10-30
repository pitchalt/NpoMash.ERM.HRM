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
using DevExpress.ExpressApp.ConditionalAppearance;
//

namespace NpoMash.Erm.Hrm.Salary {
    /// <summary>
    /// Плановая матрица
    /// </summary>
    [MapInheritance(MapInheritanceType.ParentTable)]
    public class HrmMatrixPlan : HrmMatrix {

        public HrmMatrixPlan(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }



        public String Name1 {
            get {

                return "Базовая матрица";// +" " + (Period.Month + "-" + Period.Year).ToString();
            }
        }
    }
}