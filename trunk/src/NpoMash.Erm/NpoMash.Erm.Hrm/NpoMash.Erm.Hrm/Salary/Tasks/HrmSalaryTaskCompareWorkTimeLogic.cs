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
    public class HrmSalaryTaskCompareWorkTimeLogic : BaseObject {

        public static void InitObjects(IObjectSpace object_space, HrmSalaryTaskCompareWorkTime task) {


        }



        public static void CompareKBMatrix(IObjectSpace object_space, HrmSalaryTaskCompareWorkTime task) {
            task.AllocResultKB = task.Period.CurrentMatrixAllocResultKB;
            task.CurrentTimeSheetKB = task.Period.CurrentTimeSheetKB;
            //Вытаскиваем плановую матрицу
            foreach (HrmMatrix matrix in task.Period.Matrixs) {
                if (matrix.GroupDep == DepartmentGroupDep.DEPARTMENT_KB && matrix.Type == HrmMatrixType.TYPE_MATIX &&
                    matrix.Status == HrmMatrixStatus.MATRIX_ACCEPTED && matrix.TypeMatrix == HrmMatrixTypeMatrix.MATRIX_PLANNED) {
                    task.MatrixPlan = matrix;
                }
            }

            foreach (HrmMatrix matrix in task.Period.Matrixs) {
                if (matrix.GroupDep == DepartmentGroupDep.DEPARTMENT_KB && matrix.Type == HrmMatrixType.TYPE_MATIX &&
                    matrix.Status == HrmMatrixStatus.MATRIX_EXPORTED && matrix.TypeMatrix == HrmMatrixTypeMatrix.MATRIX_COERCED) {
                    task.MinimizeNumberOfDeviationsMatrix = matrix;
                }
            }

            task.AllocResultKB.GroupDep = DepartmentGroupDep.DEPARTMENT_KB;
            task.AllocResultKB.Status = HrmMatrixStatus.MATRIX_ACCEPTED;
            task.AllocResultKB.Type = HrmMatrixType.TYPE_ALLOC_RESULT;
            task.GroupDep = DepartmentGroupDep.DEPARTMENT_KB;
        }

        public static void CompareOZMMatrix(IObjectSpace object_space, HrmSalaryTaskCompareWorkTime task) {
            task.AllocResultOZM = task.Period.CurrentMatrixAllocResultOZM;
            task.CurrentTimeSheetOZM = task.Period.CurrentTimeSheetOZM;
            //Вытаскиваем плановую матрицу
            foreach (HrmMatrix matrix in task.Period.Matrixs) {
                if (matrix.GroupDep == DepartmentGroupDep.DEPARTMENT_OZM && matrix.Type == HrmMatrixType.TYPE_MATIX &&
                    matrix.Status == HrmMatrixStatus.MATRIX_ACCEPTED && matrix.TypeMatrix == HrmMatrixTypeMatrix.MATRIX_PLANNED) {
                    task.MatrixPlan = matrix;
                }
            }

            foreach (HrmMatrix matrix in task.Period.Matrixs) {
                if (matrix.GroupDep == DepartmentGroupDep.DEPARTMENT_OZM && matrix.Type == HrmMatrixType.TYPE_MATIX &&
                    matrix.Status == HrmMatrixStatus.MATRIX_EXPORTED && matrix.TypeMatrix == HrmMatrixTypeMatrix.MATRIX_COERCED) {
                    task.MinimizeNumberOfDeviationsMatrix = matrix;
                }
            }
            task.GroupDep = DepartmentGroupDep.DEPARTMENT_OZM;
            task.AllocResultOZM.GroupDep = DepartmentGroupDep.DEPARTMENT_OZM;
            task.AllocResultOZM.Status = HrmMatrixStatus.MATRIX_ACCEPTED;
            task.AllocResultOZM.Type = HrmMatrixType.TYPE_ALLOC_RESULT;
            task.GroupDep = DepartmentGroupDep.DEPARTMENT_OZM;
        }




        public HrmSalaryTaskCompareWorkTimeLogic(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }
}
