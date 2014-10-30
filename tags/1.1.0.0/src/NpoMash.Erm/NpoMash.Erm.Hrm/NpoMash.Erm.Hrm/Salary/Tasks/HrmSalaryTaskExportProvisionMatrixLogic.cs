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

    public static class HrmSalaryTaskExportProvisionMatrixLogic {

        public static void InitObjects(HrmSalaryTaskExportProvisionMatrix local_task) {
            //local_task.ProvisionMatrix = local_task.Period.CurrentProvisionMatrix.ReserveMatrixSimplex;
            if (local_task.Period.CurrentProvisionMatrix.ReserveMatrixEvristic.Status == HrmMatrixStatus.MATRIX_PRIMARY_ACCEPTED) {
                local_task.ProvisionMatrix = local_task.Period.CurrentProvisionMatrix.ReserveMatrixEvristic;
            }
            else {
                local_task.ProvisionMatrix = local_task.Period.CurrentProvisionMatrix.ReserveMatrixSimplex;
            }
        }

        public static void ExportProvisonMatrix(HrmSalaryTaskExportProvisionMatrix local_task) {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            var engine = new FileHelperEngine<ExchangeMatrixPlan>();
            String current_month = null;
            if (local_task.Period.Month < 10) { current_month = "0" + Convert.ToString(local_task.Period.Month); }
            else { current_month = Convert.ToString(local_task.Period.Month); }
            IList<ExchangeMatrixPlan> records = new List<ExchangeMatrixPlan>();
            foreach (var column in local_task.ProvisionMatrix.Columns) {
                foreach (var cell in column.Cells) {
                    var record = new ExchangeMatrixPlan() {
                        Year = local_task.Period.Year,
                        Month = current_month,
                        DepartmentCode = cell.Column.Department.BuhCode,
                        OrderCode = cell.Row.Order.Code,
                        Time = Math.Round(cell.NewProvision, 2)
                    };
                    records.Add(record);
                }
            }
            engine.WriteFile(ConfigurationManager.AppSettings["FileExchangePath.ROOT"] + Convert.ToString(local_task.Period.CurrentAllocParameter.Year * 100 + local_task.Period.CurrentAllocParameter.Month) +"/Matrix_Reserve.ncd", records);
            local_task.ProvisionMatrix.Status = HrmMatrixStatus.MATRIX_EXPORTED;
        }
    }
}