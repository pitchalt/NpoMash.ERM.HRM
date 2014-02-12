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

        private static int _Reference_Count = 20;
        public static int ReferenceCount {
            get { return _Reference_Count; }
            set { _Reference_Count = value; }
        }

        private static int _Department_Count = 5;
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
            FileHelperEngine<ImportDepartment> department_data = new FixedFileEngine<ImportDepartment>();
            ImportDepartment[] departments_imported = department_data.ReadFile("../../../../../../../var/referential/Departments.dat");
            IDictionary<String, DepartmentGroupDep> full_departments_package = new Dictionary<String, DepartmentGroupDep>();
            foreach (var current_department in departments_imported) {
                if (!full_departments_package.ContainsKey(current_department.Code)) {
                    if (current_department.Group == "01") { full_departments_package.Add(current_department.Code, DepartmentGroupDep.DEPARTMENT_KB); }
                    else { full_departments_package.Add(current_department.Code, DepartmentGroupDep.DEPARTMENT_OZM); }
                }
            }
            foreach (var new_department_code in full_departments_package.Keys) {
                Department department_to_db = local_object_space.CreateObject<Department>();
                department_to_db.Code = new_department_code;
                department_to_db.GroupDep = full_departments_package[new_department_code];
            }
        }

        public static void ImportOrders(IObjectSpace local_object_space) {
            FileHelperEngine<ImportOrder> order_data = new FixedFileEngine<ImportOrder>();
            FileHelperEngine<ImportMatrixPlan> plan_data = new FixedFileEngine<ImportMatrixPlan>();
            ImportOrder[] order_list = order_data.ReadFile("../../../../../../../var/referential/ControlledOrders.dat");
            ImportMatrixPlan[] plan_list = plan_data.ReadFile("../../../../../../../var/Matrix_Plan.dat");
            IDictionary<String, Decimal> kb_norms_of_orders = new Dictionary<String, Decimal>();
            IDictionary<String, Decimal> ozm_norms_of_orders = new Dictionary<String, Decimal>(); 
            IDictionary<String, FmCOrderTypeControl> full_orders_package = new Dictionary<String, FmCOrderTypeControl>();
            foreach (var order in order_list) {
                if (order.TypeControl == "Ф") { 
                    full_orders_package.Add(order.Order_Code, FmCOrderTypeControl.FOT);
                    kb_norms_of_orders.Add(order.Order_Code, order.NormKB);
                    ozm_norms_of_orders.Add(order.Order_Code, order.NormOZM);
                }
                else { 
                    full_orders_package.Add(order.Order_Code, FmCOrderTypeControl.TRUDEMK_FOT);
                    kb_norms_of_orders.Add(order.Order_Code, order.NormKB);
                    ozm_norms_of_orders.Add(order.Order_Code, order.NormOZM);
                }
            }
            foreach (var plan in plan_list) { if (!full_orders_package.ContainsKey(plan.OrderCode)) { full_orders_package.Add(plan.OrderCode, FmCOrderTypeControl.NO_ORDERED); } }
            foreach (var new_order in full_orders_package) {
                fmCOrder order_to_db = local_object_space.CreateObject<fmCOrder>();
                order_to_db.Code = new_order.Key;
                order_to_db.TypeControl = new_order.Value;
                if (kb_norms_of_orders.ContainsKey(new_order.Key)) { order_to_db.NormKB = kb_norms_of_orders[new_order.Key] / 100; }
                if (ozm_norms_of_orders.ContainsKey(new_order.Key)) { order_to_db.NormOZM = ozm_norms_of_orders[new_order.Key] / 100; }
                if (!kb_norms_of_orders.ContainsKey(new_order.Key) && !ozm_norms_of_orders.ContainsKey(new_order.Key)) {
                    order_to_db.NormKB = 0;
                    order_to_db.NormOZM = 0;
                }
                order_to_db.TypeConstancy = FmCOrderTypeConstancy.CONST_ORDER_TYPE;
            }
        }

        public static void SalaryPayTypeGenerate(IObjectSpace local_object_space) {
            var random = new Random();
            IList<String> list_paytype_code = new List<String>();
            for (int i = 0 ; i < _Salarypaytype_Count; i++) {
                String paytype_code = Convert.ToString(random.Next(1, 1000));
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

        public static void referenceClassesGenerate(IObjectSpace local_object_space) {
            var random = new Random();
            for (int i = 0 ; i < _Department_Count ; i++) {
                var department = local_object_space.CreateObject<Department>();
                department.Code = Convert.ToString(random.Next(1, 4001));
                if (Convert.ToDecimal(i) < System.Math.Round(Convert.ToDecimal(_Department_Count / 2))) {
                    department.GroupDep = DepartmentGroupDep.DEPARTMENT_KB;
                }
                else {
                    department.GroupDep = DepartmentGroupDep.DEPARTMENT_OZM;
                }
            }
            for (int i = 0 ; i < _Reference_Count; i++) {
                var fmCorder = local_object_space.CreateObject<fmCOrder>();
                var hrmSalaryPayType = local_object_space.CreateObject<HrmSalaryPayType>();
                int type_control = random.Next(1, 4);
                int type_constancy = random.Next(1, 3);
                fmCorder.Code = Convert.ToString(random.Next(1000, 100000));
                if (type_control == 1) { fmCorder.TypeControl = FmCOrderTypeControl.FOT; }
                if (type_control == 2) { fmCorder.TypeControl = FmCOrderTypeControl.NO_ORDERED; }
                if (type_control == 3) { fmCorder.TypeControl = FmCOrderTypeControl.TRUDEMK_FOT; }
                if (type_constancy == 1) { fmCorder.TypeConstancy = FmCOrderTypeConstancy.UN_CONST_ORDER_TYPE; }
                if (type_constancy == 2) { fmCorder.TypeConstancy = FmCOrderTypeConstancy.CONST_ORDER_TYPE; }
                fmCorder.NormKB = Convert.ToDecimal(random.Next(1000, 100000));
                fmCorder.NormOZM = Convert.ToDecimal(random.Next(1000, 100000));
                hrmSalaryPayType.Code = Convert.ToString(random.Next(1000, 100000));
                hrmSalaryPayType.Name = Convert.ToString(random.Next(1000, 100000));
            }
            var fmCorderUnConTroll = local_object_space.CreateObject<fmCOrder>();
            fmCorderUnConTroll.Code = Convert.ToString(random.Next(1000, 100000));
            fmCorderUnConTroll.TypeControl = FmCOrderTypeControl.NO_ORDERED;
            fmCorderUnConTroll.TypeConstancy = FmCOrderTypeConstancy.UN_CONST_ORDER_TYPE;
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