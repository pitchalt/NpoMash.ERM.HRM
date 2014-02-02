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
                sheet_dep.BaseWorkTime = rand.Next(10000,15000);
                sheet_dep.AdditionWorkTime = 0;
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
    }
}