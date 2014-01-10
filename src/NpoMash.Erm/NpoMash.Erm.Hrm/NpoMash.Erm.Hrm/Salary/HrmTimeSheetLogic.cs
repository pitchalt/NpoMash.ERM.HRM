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
using IntecoAG.ERM.HRM.Organization;

namespace NpoMash.Erm.Hrm.Salary {

    public static class HrmTimeSheetLogic{

        public static void loadTimeSheetIntoPeriod(IObjectSpace os, HrmPeriod period) {
            Random rand = new Random();
            HrmTimeSheet time_sheet = os.CreateObject<HrmTimeSheet>();
            HrmTimeSheetGroup kb_time_sheet = os.CreateObject<HrmTimeSheetGroup>();
            HrmTimeSheetGroup ozm_time_sheet = os.CreateObject<HrmTimeSheetGroup>();
            time_sheet.Period = period;
            period.TimeSheets.Add(time_sheet);
            period.CurrentTimeSheet = time_sheet;
            time_sheet.KB = kb_time_sheet;
            kb_time_sheet.GroupDep = DEPARTMENT_GROUP_DEP.KB;
            time_sheet.OZM = ozm_time_sheet;
            ozm_time_sheet.GroupDep = DEPARTMENT_GROUP_DEP.OZM;
            kb_time_sheet.TimeSheet = time_sheet;
            ozm_time_sheet.TimeSheet = time_sheet;
            foreach (Department current_department in os.GetObjects<Department>()){
                HrmTimeSheetDep sheet_dep = os.CreateObject<HrmTimeSheetDep>();
                sheet_dep.Department = current_department;
                sheet_dep.TimeSheet = time_sheet;
                time_sheet.TimeSheetDeps.Add(sheet_dep);
                sheet_dep.MatrixWorkTime = rand.Next(100, 999);
                sheet_dep.AdditionWorkTime = rand.Next(100, 999);
                if (current_department.GroupDep == DEPARTMENT_GROUP_DEP.KB) {
                    sheet_dep.TimeSheetGroup = kb_time_sheet;
                    kb_time_sheet.TimeSheetDeps.Add(sheet_dep);
                }
                if (current_department.GroupDep == DEPARTMENT_GROUP_DEP.OZM) {
                    sheet_dep.TimeSheetGroup = ozm_time_sheet;
                    ozm_time_sheet.TimeSheetDeps.Add(sheet_dep);
                }

            }


        }

    }
}
