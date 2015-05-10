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

    public static class HrmSalaryTaskCompareWorkTimeLogic  {

        public static void InitObjects(IObjectSpace object_space, HrmSalaryTaskCompareWorkTime task) {


        }



        public static void CompareKBMatrix(IObjectSpace object_space, HrmSalaryTaskCompareWorkTime task) {
            task.AllocResultKB = task.Period.CurrentMatrixAllocResultKB;
            task.CurrentTimeSheetKB = task.Period.CurrentTimeSheetKB;
            task.MinimizeNumberOfDeviationsMatrix = task.Period.CurrentKBmatrixReduction.MinimizeNumberOfDeviationsMatrix;
   
            //Вытаскиваем плановую матрицу
            foreach (HrmMatrix matrix in task.Period.Matrixs) {
                if (matrix.GroupDep == DepartmentGroupDep.DEPARTMENT_KB && matrix.Type == HrmMatrixType.TYPE_MATIX &&
                    matrix.Status == HrmMatrixStatus.MATRIX_ACCEPTED && matrix.TypeMatrix == HrmMatrixTypeMatrix.MATRIX_PLANNED) {
                    task.MatrixPlan = matrix;
                }
                }


           // task.AllocResultKB.GroupDep = DepartmentGroupDep.DEPARTMENT_KB;
          //  task.AllocResultKB.Status = HrmMatrixStatus.MATRIX_ACCEPTED;
          //  task.AllocResultKB.Type = HrmMatrixType.TYPE_ALLOC_RESULT;
         //   task.GroupDep = DepartmentGroupDep.DEPARTMENT_KB;
            }


        public static void CompareOZMMatrix(IObjectSpace object_space, HrmSalaryTaskCompareWorkTime task) {
            task.AllocResultOZM = task.Period.CurrentMatrixAllocResultOZM;
            task.CurrentTimeSheetOZM = task.Period.CurrentTimeSheetOZM;
            task.MinimizeNumberOfDeviationsMatrix = task.Period.CurrentOZMmatrixReduction.MinimizeNumberOfDeviationsMatrix;

            //Вытаскиваем плановую матрицу
            foreach (HrmMatrix matrix in task.Period.Matrixs) {
                if (matrix.GroupDep == DepartmentGroupDep.DEPARTMENT_OZM && matrix.Type == HrmMatrixType.TYPE_MATIX &&
                    matrix.Status == HrmMatrixStatus.MATRIX_ACCEPTED && matrix.TypeMatrix == HrmMatrixTypeMatrix.MATRIX_PLANNED) {
                    task.MatrixPlan = matrix;
                }
            }
            

            task.GroupDep = DepartmentGroupDep.DEPARTMENT_OZM;
            task.AllocResultOZM.GroupDep = DepartmentGroupDep.DEPARTMENT_OZM;
            task.AllocResultOZM.Status = HrmMatrixStatus.MATRIX_ACCEPTED;
            task.AllocResultOZM.Type = HrmMatrixType.TYPE_ALLOC_RESULT;
            task.GroupDep = DepartmentGroupDep.DEPARTMENT_OZM;
        }

    }
}
