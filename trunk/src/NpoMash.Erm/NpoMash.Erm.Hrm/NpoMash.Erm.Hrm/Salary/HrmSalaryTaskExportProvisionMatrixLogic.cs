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
            local_task.ProvisionMatrix = local_task.Period.CurrentProvisionMatrix.ProvisionMatrix;
            local_task.GroupDep = DepartmentGroupDep.DEPARTMENT_KB_OZM;
        }

        public static void ExportProvisonMatrix(HrmSalaryTaskExportProvisionMatrix local_task) {
            local_task.ProvisionMatrix = local_task.Period.CurrentProvisionMatrix.ProvisionMatrix;
            var engine = new FileHelperEngine<ExchangeMatrixPlan>();
            IList<ExchangeMatrixPlan> records = new List<ExchangeMatrixPlan>();
            foreach (var column in local_task.ProvisionMatrix.Columns) {
                foreach (var cell in column.Cells) {
                    var record = new ExchangeMatrixPlan() {
                        Year = local_task.Period.Year,
                        Month = local_task.Period.Month,
                        DepartmentCode = cell.Column.Department.BuhCode,
                        OrderCode = cell.Row.Order.Code,
                        Time = Convert.ToInt64(cell.SourceProvision)
                    };
                    records.Add(record);
                }
            }
            engine.WriteFile(ConfigurationManager.AppSettings["FileExchangePath.ROOT"] + "Matrix_Reserve.ncd", records);
        }
    }
}