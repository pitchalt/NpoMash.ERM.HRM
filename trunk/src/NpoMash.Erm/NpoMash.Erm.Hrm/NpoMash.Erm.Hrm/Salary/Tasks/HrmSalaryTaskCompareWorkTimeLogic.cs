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
using IntecoAG.ERM.HRM.Organization;

namespace NpoMash.Erm.Hrm.Salary {


    [Persistent("TaskKompareWorkTimeLogic")]
    public class TaskKompareWorkTimeLogic : BaseObject {

        public static void InitObjects(IObjectSpace object_space, TaskKompareWorkTime task) {


        }



        public static void CompareKBMatrix (IObjectSpace object_space, TaskKompareWorkTime task) {
            task.AllocResultKB = task.Period.CurrentMatrixAllocResultKB;
            task.AllocResultKB.GroupDep = DepartmentGroupDep.DEPARTMENT_KB;
            task.AllocResultKB.Status = HrmMatrixStatus.MATRIX_ACCEPTED;
            task.AllocResultKB.Type = HrmMatrixType.TYPE_ALLOC_RESULT;
            task.AllocResultKB.TypeMatrix = HrmMatrixTypeMatrix.MATRIX_PLANNED;
            task.GroupDep = DepartmentGroupDep.DEPARTMENT_KB;
          }

        public static void CompareOZMMatrix(IObjectSpace object_space, TaskKompareWorkTime task) {
            task.AllocResultOZM = task.Period.CurrentMatrixAllocResultOZM;
            task.GroupDep = DepartmentGroupDep.DEPARTMENT_OZM;

        }




        public TaskKompareWorkTimeLogic(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }
}
