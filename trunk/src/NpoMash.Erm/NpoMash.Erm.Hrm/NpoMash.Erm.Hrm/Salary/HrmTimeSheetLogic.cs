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

        public static void loadTimeSheetIntoPeriod(IObjectSpace os, HrmPeriod period) {
            Random rand = new Random();
            HrmTimeSheet time_sheet = os.CreateObject<HrmTimeSheet>();
            HrmTimeSheetGroup kb_time_sheet = time_sheet.KB;
            HrmTimeSheetGroup ozm_time_sheet = time_sheet.OZM;
            time_sheet.Period = period;
            period.TimeSheets.Add(time_sheet);
            period.CurrentTimeSheet = time_sheet;
            time_sheet.KB = kb_time_sheet;
            kb_time_sheet.GroupDep = DEPARTMENT_GROUP_DEP.KB;
            time_sheet.OZM = ozm_time_sheet;
            ozm_time_sheet.GroupDep = DEPARTMENT_GROUP_DEP.OZM;
            kb_time_sheet.TimeSheet = time_sheet;
            ozm_time_sheet.TimeSheet = time_sheet;
            foreach (Department current_department in os.GetObjects<Department>()) {
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

        public static void ImportData(IObjectSpace object_space, HrmPeriod period) {
            var engine = new FileHelperEngine<ImportMatrixTimeSheet>();
            HrmTimeSheet time_sheet = object_space.CreateObject<HrmTimeSheet>();
            HrmTimeSheetGroup kb_time_sheet = time_sheet.KB;
            HrmTimeSheetGroup ozm_time_sheet = time_sheet.OZM;
            time_sheet.Period = period;
            period.TimeSheets.Add(time_sheet);
            period.CurrentTimeSheet = time_sheet;
            time_sheet.KB = kb_time_sheet;
            kb_time_sheet.GroupDep = DEPARTMENT_GROUP_DEP.KB;
            time_sheet.OZM = ozm_time_sheet;
            ozm_time_sheet.GroupDep = DEPARTMENT_GROUP_DEP.OZM;
            kb_time_sheet.TimeSheet = time_sheet;
            ozm_time_sheet.TimeSheet = time_sheet;
            ImportMatrixTimeSheet[] stream = engine.ReadFile("../../../../../../../var/Matrix_TimeSheet.dat");
            foreach (var each in stream) {
                foreach (Department current_department in object_space.GetObjects<Department>()) {
                    if (String.Compare(current_department.Code, each.Code) == 0) {
                        HrmTimeSheetDep sheet_dep = object_space.CreateObject<HrmTimeSheetDep>();
                        sheet_dep.Department = current_department;
                        sheet_dep.TimeSheet = time_sheet;
                        time_sheet.TimeSheetDeps.Add(sheet_dep);
                        sheet_dep.MatrixWorkTime = each.MatrixWorkTime;
                        sheet_dep.AdditionWorkTime = 0;
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
    }
}