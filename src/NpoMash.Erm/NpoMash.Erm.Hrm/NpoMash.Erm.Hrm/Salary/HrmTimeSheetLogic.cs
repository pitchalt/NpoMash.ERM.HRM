using System;
using System.IO;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;

using FileHelpers;

using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using NpoMash.Erm.Hrm.Exchange;
using IntecoAG.ERM.HRM.Organization;

namespace NpoMash.Erm.Hrm.Salary {

    public static class HrmTimeSheetLogic {

        public static void TaskSheetInit(IObjectSpace os, HrmSalaryTaskImportSourceData task) {
            task.TimeSheetKB = os.CreateObject<HrmTimeSheet>();
            task.TimeSheetKB.GroupDep = DepartmentGroupDep.DEPARTMENT_KB;
            task.Period.TimeSheets.Add(task.TimeSheetKB);
            task.Period.CurrentTimeSheetKB = task.TimeSheetKB;
            //
            task.TimeSheetOZM = os.CreateObject<HrmTimeSheet>();
            task.TimeSheetOZM.GroupDep = DepartmentGroupDep.DEPARTMENT_OZM;
            task.Period.TimeSheets.Add(task.TimeSheetOZM);
            task.Period.CurrentTimeSheetOZM = task.TimeSheetOZM;
        }   
     
        public static void loadTimeSheetIntoPeriod(IObjectSpace os,  HrmSalaryTaskImportSourceData task) {
            Random rand = new Random();
            //
            TaskSheetInit(os, task);
            //            HrmTimeSheetGroup kb_time_sheet = time_sheet.KB;
            //            HrmTimeSheetGroup ozm_time_sheet = time_sheet.OZM;
            //            time_sheet.Period = period;
            //kb_time_sheet.GroupDep = DEPARTMENT_GROUP_DEP.KB;
            //ozm_time_sheet.GroupDep = DEPARTMENT_GROUP_DEP.OZM;
            //kb_time_sheet.TimeSheet = time_sheet;
            //ozm_time_sheet.TimeSheet = time_sheet;
            foreach (Department current_department in os.GetObjects<Department>()) {
                HrmTimeSheetDep sheet_dep = os.CreateObject<HrmTimeSheetDep>();
                sheet_dep.Department = current_department;
                //sheet_dep.TimeSheet = time_sheet;
                //time_sheet.TimeSheetDeps.Add(sheet_dep);
                sheet_dep.BaseWorkTime = rand.Next(100, 999);
                sheet_dep.AdditionWorkTime = rand.Next(100, 999);
                if (current_department.GroupDep == DepartmentGroupDep.DEPARTMENT_KB) {
                    //sheet_dep.TimeSheetGroup = kb_time_sheet;
                    task.TimeSheetKB.TimeSheetDeps.Add(sheet_dep);
                }
                if (current_department.GroupDep == DepartmentGroupDep.DEPARTMENT_OZM) {
                    //sheet_dep.TimeSheetGroup = ozm_time_sheet;
                    task.TimeSheetOZM.TimeSheetDeps.Add(sheet_dep);
                }
            }
        }

        public static void ImportData(IObjectSpace os, HrmSalaryTaskImportSourceData task) {
            TaskSheetInit(os, task);
            var engine = new FileHelperEngine<ImportMatrixTimeSheet>();
            ImportMatrixTimeSheet[] stream = engine.ReadFile("../../../../../../../var/Matrix_TimeSheet.dat");
            IList<Department> deps = os.GetObjects<Department>();
            foreach (var each in stream) {
                Department dep = deps.FirstOrDefault(x => x.Code == each.Code.Trim());
                        HrmTimeSheetDep sheet_dep = os.CreateObject<HrmTimeSheetDep>();
                        sheet_dep.Department = dep;
                        //sheet_dep.TimeSheet = time_sheet;
                        //time_sheet.TimeSheetDeps.Add(sheet_dep);
                        sheet_dep.BaseWorkTime = each.MatrixWorkTime;
                        sheet_dep.AdditionWorkTime = 0;
                        if (dep.GroupDep == DepartmentGroupDep.DEPARTMENT_KB) {
                            task.TimeSheetKB.TimeSheetDeps.Add(sheet_dep);
                        }
                        if (dep.GroupDep == DepartmentGroupDep.DEPARTMENT_OZM) {
                            task.TimeSheetOZM.TimeSheetDeps.Add(sheet_dep);
                        }
                //foreach (Department current_department in ) {
                //    if (String.Compare(current_department.Code, each.Code.Trim()) == 0) {
                //    }
                //}
            }
        }
    }
}