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


    [Persistent("TaskKompareWorkTimeLogic")]
    public class TaskKompareWorkTimeLogic : BaseObject {



        public static HrmSalaryTaskMatrixReduction CompareKBMatrix(IObjectSpace object_space, HrmSalaryTask task) { return null; }

        public static HrmSalaryTaskMatrixReduction CompareOZMMatrix(IObjectSpace object_space, HrmSalaryTask task) { return null; }




        public TaskKompareWorkTimeLogic(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }
}
