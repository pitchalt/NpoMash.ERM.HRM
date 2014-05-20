using System;
using System.Linq;
using System.Text;
using System.Configuration;
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
using FileHelpers;
using IntecoAG.ERM.HRM;
using IntecoAG.ERM.FM.Order;
using NpoMash.Erm.Hrm.Exchange;
using IntecoAG.ERM.HRM.Organization;

namespace NpoMash.Erm.Hrm.Salary {

    public static class HrmSalaryTaskExportCoercedMatrixLogic {

        public static void InitObjects(HrmSalaryTaskExportCoercedMatrix local_task) {
            local_task.KBCoercedMatrix = local_task.Period.CurrentKBmatrixReduction.MinimizeNumberOfDeviationsMatrix;
            local_task.OZMCoercedMatrix = local_task.Period.CurrentOZMmatrixReduction.MinimizeNumberOfDeviationsMatrix;
        }

        public static void ExportCoercedMatrix(HrmSalaryTaskExportCoercedMatrix local_task) {
            local_task.KBCoercedMatrix = local_task.Period.CurrentKBmatrixReduction.MinimizeNumberOfDeviationsMatrix;
            local_task.OZMCoercedMatrix = local_task.Period.CurrentOZMmatrixReduction.MinimizeNumberOfDeviationsMatrix;
            local_task.KBCoercedMatrix.Status = local_task.Period.CurrentKBmatrixReduction.MinimizeNumberOfDeviationsMatrix.Status;
            local_task.OZMCoercedMatrix.Status = local_task.Period.CurrentOZMmatrixReduction.MinimizeNumberOfDeviationsMatrix.Status;
            var engine = new FileHelperEngine<ExchangeMatrixPlan>();
            IList<ExchangeMatrixPlan> records = new List<ExchangeMatrixPlan>();
            foreach (var column in local_task.KBCoercedMatrix.Columns) {
                foreach (var cell in column.Cells){
                    var record = new ExchangeMatrixPlan() {
                        Year = local_task.Period.Year,
                        Month = local_task.Period.Month,
                        DepartmentCode = cell.Column.Department.BuhCode,
                        OrderCode = cell.Row.Order.Code,
                        Time = cell.Time
                    };
                    records.Add(record);
                }
            }
            foreach (var column in local_task.OZMCoercedMatrix.Columns) {
                foreach (var cell in column.Cells) {
                    var record = new ExchangeMatrixPlan() {
                        Year = local_task.Period.Year,
                        Month = local_task.Period.Month,
                        DepartmentCode = cell.Column.Department.BuhCode,
                        OrderCode = cell.Row.Order.Code,
                        Time = cell.Time
                    };
                    records.Add(record);
                }
            }
            engine.WriteFile(ConfigurationManager.AppSettings["FileExchangePath.ROOT"] + Convert.ToString(local_task.Period.CurrentAllocParameter.Year * 100 + local_task.Period.CurrentAllocParameter.Month) +"Matrix_Reduce.ncd", records);
        }
    }
}