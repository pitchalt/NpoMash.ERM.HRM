using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using FileHelpers;

using IntecoAG.ERM.HRM;
using IntecoAG.ERM.FM.Order;
using NpoMash.Erm.Hrm.Salary;
using NpoMash.Erm.Hrm.Exchange;
using IntecoAG.ERM.HRM.Organization;
using NpoMash.Erm.Hrm.Tests.ImportReferentialData;

using DevExpress.ExpressApp;


namespace NpoMash.Erm.Hrm.Tests.Controllers {


    public static class TestWCLogic {

        private static int _Reference_Count = 50;
        public static int ReferenceCount {
            get { return _Reference_Count; }
            set { _Reference_Count = value; }
        }

        private static int _Department_Count = 50;
        public static int DepartmentCount {
            get { return _Department_Count; }
            set { _Department_Count = value; }
        }

        private static int _Allocparameter_Count = 1;
        public static int AllocparameterCount {
            get { return _Allocparameter_Count; }
            set { _Allocparameter_Count = value; }
        }

        private static int _Salarypaytype_Count = 100;

        public static void ImportDepartments(IObjectSpace local_object_space) {
            FileHelperEngine<ImportDepartments> ref_data = new FixedFileEngine<ImportDepartments>();
            ImportDepartments[] departments_imported = ref_data.ReadFile("../../../../../../../var/referential/ulddp.ncd");
            foreach (var current_department in departments_imported) {
                Department department_to_db = local_object_space.CreateObject<Department>();
                department_to_db.Code = current_department.DepartmentCode;
                department_to_db.BuhCode = current_department.BuhCode;
                if (String.IsNullOrEmpty(current_department.IsClosed)) { department_to_db.IsClosed = false; }
                else { department_to_db.IsClosed = true; }
                if (current_department.DepartmentGroup == null) { department_to_db.GroupDep = DepartmentGroupDep.DEPARTMENT_OZM; }
                else { department_to_db.GroupDep = DepartmentGroupDep.DEPARTMENT_KB; }
            }
        }

        public static void ImportOrders(IObjectSpace local_object_space) {
            FileHelperEngine<ImportOrder> order_data = new FixedFileEngine<ImportOrder>();
            ImportOrder[] orders_imported = order_data.ReadFile("../../../../../../../var/referential/result.dat");
            IDictionary<String, Decimal> kb_norms_of_orders = new Dictionary<String, Decimal>();
            IDictionary<String, Decimal> ozm_norms_of_orders = new Dictionary<String, Decimal>();
            IDictionary<String, FmCOrderTypeControl> full_orders_package = new Dictionary<String, FmCOrderTypeControl>();
            foreach (var order in orders_imported) {
                if (!full_orders_package.ContainsKey(order.Order_Code)) {
                    if (order.TypeControl == "Ф") {
                        full_orders_package.Add(order.Order_Code, FmCOrderTypeControl.FOT);
                        kb_norms_of_orders.Add(order.Order_Code, order.NormKB);
                        ozm_norms_of_orders.Add(order.Order_Code, order.NormOZM);
                    }
                    if (order.TypeControl == "ТФ") {
                        full_orders_package.Add(order.Order_Code, FmCOrderTypeControl.TRUDEMK_FOT);
                        kb_norms_of_orders.Add(order.Order_Code, order.NormKB);
                        ozm_norms_of_orders.Add(order.Order_Code, order.NormOZM);
                    }
                    if (order.TypeControl == null) {
                        full_orders_package.Add(order.Order_Code, FmCOrderTypeControl.NO_ORDERED);
                        kb_norms_of_orders.Add(order.Order_Code, order.NormKB);
                        ozm_norms_of_orders.Add(order.Order_Code, order.NormOZM);
                    }
                }
            }
            foreach (var new_order in full_orders_package) {
                fmCOrder order_to_db = local_object_space.CreateObject<fmCOrder>();
                order_to_db.Code = new_order.Key;
                order_to_db.TypeControl = new_order.Value;
                if (kb_norms_of_orders.ContainsKey(new_order.Key)) { order_to_db.NormKB = kb_norms_of_orders[new_order.Key] / 100; }
                if (ozm_norms_of_orders.ContainsKey(new_order.Key)) { order_to_db.NormOZM = ozm_norms_of_orders[new_order.Key] / 100; }
                order_to_db.TypeConstancy = FmCOrderTypeConstancy.CONST_ORDER_TYPE;
            }
        }

        public static void ImportPayTypes(IObjectSpace local_object_space) {
            FileHelperEngine<ImportPayTypes> paytypes_data = new FixedFileEngine<ImportPayTypes>();
            ImportPayTypes[] paytypes_code_imported = paytypes_data.ReadFile("../../../../../../../var/referential/pay_types.dat");
            IList<String> paytypes_list = new List<String>();
            foreach (var paytype in paytypes_code_imported) {
                if (!paytypes_list.Contains(paytype.PayTypeCode)) {
                    paytypes_list.Add(paytype.PayTypeCode);
                }
            }
            foreach (var new_paytype_code in paytypes_list) {
                HrmSalaryPayType paytype_to_db = local_object_space.CreateObject<HrmSalaryPayType>();
                paytype_to_db.Code = new_paytype_code;
                paytype_to_db.Name = "Здесь будет содержаться наименование кода оплаты";
            }
        }

        public static void SalaryPayTypeGenerate(IObjectSpace local_object_space) {
            var random = new Random();
            IList<String> list_paytype_code = new List<String>();
            for (int i = 0 ; i < _Salarypaytype_Count; i++) {
                String paytype_code = Convert.ToString(random.Next(100, 1000));
                if (!list_paytype_code.Contains(paytype_code)) {
                    list_paytype_code.Add(paytype_code);
                }
                else { _Salarypaytype_Count += 1; }
            }
            foreach (var current_code in list_paytype_code) {
                HrmSalaryPayType paytype_to_db = local_object_space.CreateObject<HrmSalaryPayType>();
                paytype_to_db.Code = current_code;
                paytype_to_db.Name = "Здесь будет содержаться наименование кода оплаты";
            }
        }

        public static void AddControlledOrders(IObjectSpace local_objecy_space, Int32 count) {
            var random = new Random();
            IList<String> list_order_code = new List<string>();
            for (int i = 0 ; i < count ; i++) {
                String order_code = Convert.ToString(random.Next(100000, 100000000));
                if (!list_order_code.Contains(order_code)) { list_order_code.Add(order_code); }
                else { count += 1; }
            }
            foreach (var code in list_order_code) {
                var controlled_order = local_objecy_space.CreateObject<fmCOrder>();
                controlled_order.Code = code;
                controlled_order.TypeControl = FmCOrderTypeControl.TRUDEMK_FOT;
                controlled_order.TypeConstancy = FmCOrderTypeConstancy.CONST_ORDER_TYPE;
            }
        }

        public static void DepartmentsGenerate(IObjectSpace local_object_space) {
            var random = new Random();
            IList<String> list_department_code = new List<String>();
            for (int i = 0 ; i < _Department_Count ; i++) {
                String department_code = Convert.ToString(random.Next(1,100000));
                if (!list_department_code.Contains(department_code)) { list_department_code.Add(department_code); }
                else { DepartmentCount++; }
            }
            foreach (var code in list_department_code) {
                Department department_to_db = local_object_space.CreateObject<Department>();
                department_to_db.BuhCode = code;
                department_to_db.Code = code;
                if (random.Next(1, 3) == 1) { department_to_db.GroupDep = DepartmentGroupDep.DEPARTMENT_KB; }
                else { department_to_db.GroupDep = DepartmentGroupDep.DEPARTMENT_OZM; }
            }
        }

        public static void OrdersGenerate(IObjectSpace local_object_space) {
            var random = new Random();
            IList<String> list_order_code = new List<String>();
            for (int i = 0 ; i < ReferenceCount ; i++) {
                String order_code = Convert.ToString(random.Next(100000, 100000000));
                if (!list_order_code.Contains(order_code)) { list_order_code.Add(order_code); }
                else { ReferenceCount++; }
            }
            foreach (var code in list_order_code) {
                int type_control = random.Next(1, 4);
                int type_constancy = random.Next(1, 3);
                fmCOrder order_to_db = local_object_space.CreateObject<fmCOrder>();
                order_to_db.Code = code;
                if (type_control == 1) { order_to_db.TypeControl = FmCOrderTypeControl.FOT; }
                if (type_control == 2) { order_to_db.TypeControl = FmCOrderTypeControl.NO_ORDERED; }
                if (type_control == 3) { order_to_db.TypeControl = FmCOrderTypeControl.TRUDEMK_FOT; }
                if (type_constancy == 1) { order_to_db.TypeConstancy = FmCOrderTypeConstancy.CONST_ORDER_TYPE; }
                if (type_constancy == 2) { order_to_db.TypeConstancy = FmCOrderTypeConstancy.UN_CONST_ORDER_TYPE; }
                if (order_to_db.TypeControl != FmCOrderTypeControl.NO_ORDERED) {
                    order_to_db.NormKB = random.Next(0, 500);
                    order_to_db.NormOZM = random.Next(0, 500);
                }
                else { 
                    order_to_db.NormKB = 0; 
                    order_to_db.NormOZM = 0;
                }

            }
        }

        public static void addTestData(IObjectSpace a_object_space) {
            for (int i = 0 ; i < _Allocparameter_Count ; i++) {
                var alloc_parameter = HrmPeriodAllocParameterLogic.createParameters(a_object_space);
                alloc_parameter.StatusSet(HrmPeriodAllocParameterStatus.ALLOC_PARAMETERS_ACCEPTED);
                foreach (var each in a_object_space.GetObjects<HrmPeriod>(null, true)) {
                    each.setStatus(HrmPeriodStatus.CLOSED);
                }
            }
        }
    }
}