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
//
using IntecoAG.ERM.HRM.Organization;

namespace NpoMash.Erm.Hrm.Salary {
    [Persistent("HrmSalaryTaskMatrixReduction")]
    [NavigationItem("A1 Integration")]

    public class HrmSalaryTaskMatrixReduction : BaseObject {
        public HrmSalaryTaskMatrixReduction(Session session) : base(session) { }

//        public IList<Matr> Matrix;
        public DateTime Period;

        [NonPersistent] // Отображаем аттрибуты которые нужны 
        // допустим это список подразделений
        public class Matr : XPCustomObject {
            public Department Department;
            //            public string Order;
            //            public string TypeControl;
            public Int32 PlanTrudEmk;
            public Int32 NewTrudEmk;
            public Int32 DepartmentTrudEmk;
        }
        IList<Matr> collection {
            get {
                IList<Matr> result = new List<Matr>();
                result.Add(new Matr() {
                    // Ининтим объект значениями которые вычисляем из матриц
                                Department = null,
                                PlanTrudEmk = 0,
                                NewTrudEmk = 0,
                                DepartmentTrudEmk = 0
                            });
                return result;
            }
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }

}
