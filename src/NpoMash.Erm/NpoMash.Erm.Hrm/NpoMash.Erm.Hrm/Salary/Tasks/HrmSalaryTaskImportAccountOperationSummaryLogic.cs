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
using IntecoAG.ERM.FM.Order;
using NpoMash.Erm.Hrm.Exchange;
using IntecoAG.ERM.HRM;

namespace NpoMash.Erm.Hrm.Salary {
    public static class HrmSalaryTaskImportAccountOperationSummaryLogic {


        public static void CreateTestAccountOperationSummary(IObjectSpace local_object_space, HrmSalaryTaskImportAccountOperationSummary local_task)
        {
            HrmMatrixLastAccount matrix_alloc_result_summary = local_object_space.CreateObject<HrmMatrixLastAccount>();
            
            for (int dep = 0; dep < 100;dep++ ) {
                HrmMatrixColumn new_column = local_object_space.CreateObject<HrmMatrixColumn>();
                
                Random rand= new Random();
              
                Int32 dep_code=rand.Next(10000,50000);
                Int32 dep_group=rand.Next(0, 2);
                Int32 row_count = rand.Next(1, 51);

                Department new_dep = local_object_space.CreateObject<Department>();
                new_dep.BuhCode = Convert.ToString(dep_code);
                if (dep_group == 0) new_dep.GroupDep = DepartmentGroupDep.DEPARTMENT_KB;
                if (dep_group == 1) new_dep.GroupDep = DepartmentGroupDep.DEPARTMENT_OZM;
                new_column.Department = new_dep;
               
                matrix_alloc_result_summary.Columns.Add(new_column);
                new_column.Matrix = matrix_alloc_result_summary;

                // В этой колонке создаем новую строку и соответственно ячейку
                for (int row = 0; row <= Convert.ToInt32(row_count); row++) {
                HrmMatrixRow new_row = local_object_space.CreateObject<HrmMatrixRow>();
                HrmMatrixCell new_cell = local_object_space.CreateObject<HrmMatrixCell>();
                fmCOrder new_order = local_object_space.CreateObject<fmCOrder>();

                Int32 order_code = rand.Next(50000, 90001);
                Int32 order_control = rand.Next(0, 3);
            
                //Инициализируем строку
                new_order.Code = Convert.ToString(order_code);
                if (order_control == 0) new_order.TypeControl = FmCOrderTypeControl.FOT;
                if (order_control == 1) new_order.TypeControl = FmCOrderTypeControl.TRUDEMK_FOT;
                if (order_control == 2) new_order.TypeControl = FmCOrderTypeControl.NO_ORDERED;
                new_row.Order = new_order;
                //Инициализируем ячейку
                new_cell.Time = 100;
                new_cell.TravelTime = 200;
                new_cell.PlanMoney = 500;
                new_cell.MoneyNoReserve = 600;
                new_cell.TravelMoney = 200;
                new_cell.ConstOrderTime = 346;
                //Связываем ячейку с этой колонкой 
                new_cell.Column = new_column;
                //В коллекцию ячеек колонки добавляем новую ячейку
                new_column.Cells.Add(new_cell);
                //Связываем ячейку с этой строкой
                new_cell.Row = new_row;
                //В коллекцию ячеек строки добавляем новую ячейку
                new_row.Cells.Add(new_cell);
                //Добавляем в матрицу созданную строку
                matrix_alloc_result_summary.Rows.Add(new_row);
                new_row.Matrix = matrix_alloc_result_summary;
                }

            }

            local_task.GroupDep = IntecoAG.ERM.HRM.Organization.DepartmentGroupDep.DEPARTMENT_KB_OZM;
            matrix_alloc_result_summary.Type = HrmMatrixType.TYPE_ALLOC_RESULT;
            matrix_alloc_result_summary.Status = HrmMatrixStatus.MATRIX_DOWNLOADED;
            local_task.MatrixAllocResultSummary = matrix_alloc_result_summary;
            local_task.MatrixAllocResultSummary.GroupDep = IntecoAG.ERM.HRM.Organization.DepartmentGroupDep.DEPARTMENT_KB_OZM;
            CreateTestAllocResultMatrix(local_object_space, matrix_alloc_result_summary, CreateReservePayType(local_object_space), CreateNoReservePayType(local_object_space));
            local_task.Period.CurrentMatrixAllocResultSummary = matrix_alloc_result_summary;
            local_task.Period.Matrixs.Add(matrix_alloc_result_summary);         
        }

        public static HrmSalaryPayType CreateReservePayType(IObjectSpace local_object_space)
        {
            HrmSalaryPayType paytype_in_db = null;
            foreach (var paytype in local_object_space.GetObjects<HrmSalaryPayType>())
            {
                paytype_in_db = paytype;
            }
            return paytype_in_db;
        }

        public static HrmSalaryPayType CreateNoReservePayType(IObjectSpace local_object_space)
        {
            HrmSalaryPayType paytype_to_db = local_object_space.CreateObject<HrmSalaryPayType>();
            paytype_to_db.Code = "100";
            paytype_to_db.Name = "Код оплаты, не входящий в резерв";
            local_object_space.CommitChanges();
            return paytype_to_db;
        }

        public static void CreateTestAllocResultMatrix(IObjectSpace local_object_space, HrmMatrixAllocResult matrix_alloc_result,
            HrmSalaryPayType reserve_paytype, HrmSalaryPayType no_reserve_paytype)
        {
            var random = new Random();
            foreach (var column in matrix_alloc_result.Columns)
            {
                    foreach (var cell in column.Cells)
                    {
                        int how_many_accounts = random.Next(1, 4);
                        switch (how_many_accounts)
                        {
                            case 1:
                                HrmAccountOperation reserve_account_first = local_object_space.CreateObject<HrmAccountOperation>();
                                reserve_account_first.Department = cell.Column.Department;
                                reserve_account_first.PayType = reserve_paytype;
                                reserve_account_first.Order = cell.Row.Order;
                                reserve_account_first.Money = random.Next(100, 1000);
                                cell.AccountOperations.Add(reserve_account_first);
                                matrix_alloc_result.AccountOperations.Add(reserve_account_first);
                                break;
                            case 2:
                                HrmAccountOperation reserve_account_second = local_object_space.CreateObject<HrmAccountOperation>();
                                HrmAccountOperation no_reserve_account_second = local_object_space.CreateObject<HrmAccountOperation>();
                                reserve_account_second.Department = cell.Column.Department;
                                reserve_account_second.PayType = reserve_paytype;
                                reserve_account_second.Order = cell.Row.Order;
                                reserve_account_second.Money = random.Next(1, 1000);
                                no_reserve_account_second.Department = cell.Column.Department;
                                no_reserve_account_second.PayType = no_reserve_paytype;
                                no_reserve_account_second.Order = cell.Row.Order;
                                no_reserve_account_second.Money = random.Next(1, 1000);
                                cell.AccountOperations.Add(reserve_account_second);
                                cell.AccountOperations.Add(no_reserve_account_second);
                                matrix_alloc_result.AccountOperations.Add(reserve_account_second);
                                matrix_alloc_result.AccountOperations.Add(no_reserve_account_second);
                                break;
                            case 3:
                                HrmAccountOperation reserve_account_third = local_object_space.CreateObject<HrmAccountOperation>();
                                HrmAccountOperation no_reserve_account_third = local_object_space.CreateObject<HrmAccountOperation>();
                                reserve_account_third.Department = cell.Column.Department;
                                reserve_account_third.PayType = reserve_paytype;
                                reserve_account_third.Order = cell.Row.Order;
                                reserve_account_third.Money = random.Next(1, 1000);
                                no_reserve_account_third.Department = cell.Column.Department;
                                no_reserve_account_third.PayType = no_reserve_paytype;
                                no_reserve_account_third.Order = cell.Row.Order;
                                no_reserve_account_third.Money = random.Next(1, 1000);
                                cell.AccountOperations.Add(reserve_account_third);
                                cell.AccountOperations.Add(no_reserve_account_third);
                                matrix_alloc_result.AccountOperations.Add(reserve_account_third);
                                matrix_alloc_result.AccountOperations.Add(no_reserve_account_third);
                                break;
                        }
                    }
                
            }
        }

    }
}