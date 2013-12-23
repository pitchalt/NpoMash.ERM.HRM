using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;



namespace NpoMash.Erm.Hrm.Salary {
    [Persistent("HrmSalaryTaskMatrixReduction")]
    [NavigationItem("A1 Integration")]

    public class HrmSalaryTaskMatrixReduction : BaseObject {
        public HrmSalaryTaskMatrixReduction(Session session) : base(session) { }

        public IList<Matr> Matrix;
        public DateTime Period;

        [NonPersistent] // Отображаем аттрибуты которые нужны 
        public class Matr : XPCustomObject {
            public string Department;
            public string Order;
            public string TypeControl;
            public string PlanTrudEmk;
            public string NewTrudEmk;
            public string DepartmentTrudEmk;
        }
        IList<Matr> collection { get { return new List<Matr>(); } }

            public override void AfterConstruction() { base.AfterConstruction(); }
    }

    }
