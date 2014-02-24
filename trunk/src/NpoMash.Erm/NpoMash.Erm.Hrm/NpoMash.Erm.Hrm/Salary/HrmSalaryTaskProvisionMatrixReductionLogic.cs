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
using IntecoAG.ERM.HRM.Organization;

namespace NpoMash.Erm.Hrm.Salary {
    public class HrmSalaryTaskProvisionMatrixReductionLogic : BaseObject {

        public static HrmSalaryTaskProvisionMatrixReduction initProvisonMatrixTask(IObjectSpace os, HrmPeriod period, DepartmentGroupDep group_dep) {
            HrmSalaryTaskProvisionMatrixReduction task_provision_matrix_reduction = os.CreateObject<HrmSalaryTaskProvisionMatrixReduction>();
            period.PeriodTasks.Add(task_provision_matrix_reduction);
            task_provision_matrix_reduction.AllocParameters = period.CurrentAllocParameter;
           
            foreach (HrmMatrix matrix in period.Matrixs) {
                if (matrix.TypeMatrix == HrmMatrixTypeMatrix.MATRIX_PLANNED &&
                    matrix.Status == HrmMatrixStatus.MATRIX_ACCEPTED &&
                    matrix.GroupDep == group_dep) {
                        task_provision_matrix_reduction.MatrixPlan = matrix;
                }
            }

               return task_provision_matrix_reduction;
        }

        public static HrmMatrix createPlanMatrix() { return null; }

        
        public static HrmMatrix calculateProvisionMatrix() { return null; }



        public HrmSalaryTaskProvisionMatrixReductionLogic(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
        }
    }
}