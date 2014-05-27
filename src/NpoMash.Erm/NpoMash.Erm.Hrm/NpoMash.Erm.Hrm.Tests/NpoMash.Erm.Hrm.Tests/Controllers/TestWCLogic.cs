using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Configuration;
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

        public const Int16 INIT_NORM_NO_CONTROL_KB = 1000;
        public const Int16 INIT_NORM_NO_CONTROL_OZM = 2000;

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

        public static void UpdateDepartments(IObjectSpace local_object_space) {
            FileHelperEngine<ImportDepartments> ref_dep_data = new FixedFileEngine<ImportDepartments>();
            ImportDepartments[] departments_imported = ref_dep_data.ReadFile(ConfigurationManager.AppSettings["FileExchangePath.ROOT"] + "referential/ulddp.ncd");
            IDictionary<String, Department> departments_in_db = new Dictionary<String, Department>();
            foreach (var current_department in departments_imported) {
                if (!departments_in_db.ContainsKey(current_department.BuhCode)) {
                    Department department_to_db = local_object_space.CreateObject<Department>();
                    department_to_db.Code = current_department.DepartmentCode;
                    department_to_db.BuhCode = current_department.BuhCode;
                    if (String.IsNullOrEmpty(current_department.IsClosed)) { department_to_db.IsClosed = false; }
                    else { department_to_db.IsClosed = true; }
                    if (String.IsNullOrEmpty(current_department.DepartmentGroup)) { department_to_db.GroupDep = DepartmentGroupDep.DEPARTMENT_OZM; }
                    else { department_to_db.GroupDep = DepartmentGroupDep.DEPARTMENT_KB; }
                }
            }
        }

        public static void initOrderControls(IObjectSpace os, HrmPeriodAllocParameter par) {
            FixedFileEngine<ImportControlledOrder> order_data = new FixedFileEngine<ImportControlledOrder>();
            ImportControlledOrder[] orders_imported = order_data.ReadFile(ConfigurationManager.AppSettings["FileExchangePath.ROOT"] + "referential/ControlledOrders.ncd");
            IDictionary<String, fmCOrder> order_in_db = os.GetObjects<fmCOrder>(null, true).ToDictionary<fmCOrder, String>(x => x.Code);
            foreach (var order in orders_imported) {
                HrmPeriodOrderControl oc = os.CreateObject<HrmPeriodOrderControl>();
                if (!order_in_db.ContainsKey(order.Code)) {
                    fmCOrder order_to_db = os.CreateObject<fmCOrder>();
                    oc.Order = order_to_db;
                    order_to_db.Code = order.Code;
                    oc.NormKB = order.NormKB / 100;
                    oc.NormOZM = order.NormOZM / 100;
                    order_to_db.NormKB = oc.NormKB;
                    order_to_db.NormOZM = oc.NormOZM;
                    if (order.TypeControl == "Ф") { 
                        oc.TypeControl = FmCOrderTypeControl.FOT;
                        order_to_db.TypeControl = FmCOrderTypeControl.FOT;
                    }
                    else { 
                        oc.TypeControl = FmCOrderTypeControl.TRUDEMK_FOT;
                        order_to_db.TypeControl = FmCOrderTypeControl.TRUDEMK_FOT;
                    }
                    order_in_db.Add(order.Code, order_to_db); 
                    par.OrderControls.Add(oc);
                }
                else {
                    oc.Order = order_in_db[order.Code];
                    oc.NormKB = order.NormKB / 100;
                    oc.NormOZM = order.NormOZM / 100;
                    if (order.TypeControl == "Ф") { oc.TypeControl = FmCOrderTypeControl.FOT; }
                    else { oc.TypeControl = FmCOrderTypeControl.TRUDEMK_FOT; }
                    par.OrderControls.Add(oc);
                }
            }
        }

        public static void UpdateOrders(IObjectSpace local_object_space) {
            FileHelperEngine<ImportOrder> order_data = new FixedFileEngine<ImportOrder>();
            ImportOrder[] orders_imported = order_data.ReadFile(ConfigurationManager.AppSettings["FileExchangePath.ROOT"] + "referential/Orders.ncd");
            IDictionary<String, Decimal> kb_norms_of_orders = new Dictionary<String, Decimal>();
            IDictionary<String, Decimal> ozm_norms_of_orders = new Dictionary<String, Decimal>();
            IDictionary<String, FmCOrderTypeControl> full_orders_package = new Dictionary<String, FmCOrderTypeControl>();
            IDictionary<String, fmCOrder> orders_in_db = new Dictionary<String, fmCOrder>();
            foreach (var current_order in orders_imported) {
                if (!full_orders_package.ContainsKey(current_order.Order_Code)) {
                    if (current_order.TypeControl == "Ф") {
                        full_orders_package.Add(current_order.Order_Code, FmCOrderTypeControl.FOT);
                        kb_norms_of_orders.Add(current_order.Order_Code, current_order.NormKB);
                        ozm_norms_of_orders.Add(current_order.Order_Code, current_order.NormOZM);
                    }
                    if (current_order.TypeControl == "ТФ") {
                        full_orders_package.Add(current_order.Order_Code, FmCOrderTypeControl.TRUDEMK_FOT);
                        kb_norms_of_orders.Add(current_order.Order_Code, current_order.NormKB);
                        ozm_norms_of_orders.Add(current_order.Order_Code, current_order.NormOZM);
                    }
                    if (String.IsNullOrEmpty(current_order.TypeControl)) {
                        full_orders_package.Add(current_order.Order_Code, FmCOrderTypeControl.NO_ORDERED);
                        kb_norms_of_orders.Add(current_order.Order_Code, current_order.NormKB);
                        ozm_norms_of_orders.Add(current_order.Order_Code, current_order.NormOZM);
                    }
                }
            }
            foreach (var new_order in full_orders_package) {
                if (!orders_in_db.ContainsKey(new_order.Key)) {
                    fmCOrder order_to_db = local_object_space.CreateObject<fmCOrder>();
                    order_to_db.Code = new_order.Key;
                    order_to_db.TypeControl = new_order.Value;
                    if (kb_norms_of_orders.ContainsKey(new_order.Key)) { order_to_db.NormKB = kb_norms_of_orders[new_order.Key]; }
                    if (ozm_norms_of_orders.ContainsKey(new_order.Key)) { order_to_db.NormOZM = ozm_norms_of_orders[new_order.Key]; }
                    order_to_db.TypeConstancy = FmCOrderTypeConstancy.CONST_ORDER_TYPE;
                    orders_in_db.Add(new_order.Key, order_to_db);
                }
            }
        }

        public static void UpdatePayTypes(IObjectSpace local_object_space) {
            FileHelperEngine<ImportPayTypes> paytypes_data = new FixedFileEngine<ImportPayTypes>();
            ImportPayTypes[] paytypes_imported = paytypes_data.ReadFile(ConfigurationManager.AppSettings["FileExchangePath.ROOT"] + "referential/PAY_TYPE.NCD");
            IDictionary<String, HrmSalaryPayType> paytypes_in_db = new Dictionary<String, HrmSalaryPayType>();
            foreach (var current_paytype in paytypes_imported) {
                if (!paytypes_in_db.ContainsKey(current_paytype.Code)) {
                    HrmSalaryPayType paytype_to_db = local_object_space.CreateObject<HrmSalaryPayType>();
                    paytype_to_db.Code = current_paytype.Code;
                    if (paytype_to_db.Code == "104" || paytype_to_db.Code == "147") { paytype_to_db.Type = IntecoAG.ERM.HRM.HrmPayTypes.TRAVEL_CODE; }
                    if (paytype_to_db.Code == "220" || paytype_to_db.Code == "300" || paytype_to_db.Code == "301" ||
                        paytype_to_db.Code == "302" || paytype_to_db.Code == "303" || paytype_to_db.Code == "304" ||
                        paytype_to_db.Code == "305" || paytype_to_db.Code == "306" || paytype_to_db.Code == "307" ||
                        paytype_to_db.Code == "308" || paytype_to_db.Code == "406" || paytype_to_db.Code == "310" ||
                        paytype_to_db.Code == "311" || paytype_to_db.Code == "312" || paytype_to_db.Code == "315" ||
                        paytype_to_db.Code == "316" || paytype_to_db.Code == "322" || paytype_to_db.Code == "324" ||
                        paytype_to_db.Code == "328" || paytype_to_db.Code == "331" || paytype_to_db.Code == "341" ||
                        paytype_to_db.Code == "380" || paytype_to_db.Code == "381" || paytype_to_db.Code == "382" ||
                        paytype_to_db.Code == "401" || paytype_to_db.Code == "404" || paytype_to_db.Code == "405") {
                        paytype_to_db.Type = IntecoAG.ERM.HRM.HrmPayTypes.PROVISION_CODE;
                    }
                    else { paytype_to_db.Type = IntecoAG.ERM.HRM.HrmPayTypes.BASE_CODE; }
                    paytype_to_db.Name = current_paytype.Name;
                    paytypes_in_db.Add(paytype_to_db.Code, paytype_to_db);
                }
            }
        }

        public static void SalaryPayTypeGenerate(IObjectSpace local_object_space) {
            var random = new Random();
            IList<String> list_paytype_code = new List<String>();
            list_paytype_code.Add("101");    
            for (int i = 0 ; i < _Salarypaytype_Count; i++) {
                String paytype_code = Convert.ToString(random.Next(102, 1000));
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

        public static HrmPeriodAllocParameter createParameters(IObjectSpace os) {
            HrmPeriod new_period = HrmPeriodLogic.createPeriod(os);
            HrmPeriodAllocParameter alloc_parameter = initParameters(os, new_period);
            return alloc_parameter;
        }

        public static HrmPeriodAllocParameter initParameters(IObjectSpace os, HrmPeriod current_period) {
            HrmPeriodAllocParameter par = os.CreateObject<HrmPeriodAllocParameter>();
            par.Period = current_period;
            current_period.CurrentAllocParameter = par;
            current_period.AllocParameters.Add(par);
            par.StatusSet(HrmPeriodAllocParameterStatus.OPEN_TO_EDIT);
            initParametersFromPreviousPeriod(os, par);
            initDepartmentControlls(os, par);
            initOrderControls(os, par);
            return par;
        }

        public static void initParametersFromPreviousPeriod(IObjectSpace os, HrmPeriodAllocParameter par) {
            if (par.Period.PeriodPrevious == par.Period) {
                par.NormNoControlKB = INIT_NORM_NO_CONTROL_KB;
                par.NormNoControlOZM = INIT_NORM_NO_CONTROL_OZM;
                addAllPayTypes(os, par);
            }
            else {
                par.NormNoControlKB = par.Period.PeriodPrevious.CurrentAllocParameter.NormNoControlKB;
                par.NormNoControlOZM = par.Period.PeriodPrevious.CurrentAllocParameter.NormNoControlOZM;
                foreach (HrmPeriodPayType pay in par.Period.PeriodPrevious.CurrentAllocParameter.PeriodPayTypes) {
                    /*bool alreadyThere = false;
                    foreach (HrmPeriodPayType existingPay in par.PeriodPayTypes)// перебираем уже назначенные
                        //проверяя, нет ли в параметрах периода PayTypes-ов со ссылкой туда же
                        if (pay.PayType == existingPay.PayType) alreadyThere = true;
                    if (!alreadyThere)//если такой еще не добавляли...
                    {*/
                    HrmPeriodPayType pt = os.CreateObject<HrmPeriodPayType>();//то создаем
                    pt.PayType = pay.PayType;//задаем ссылку на нужный PayType
                    pt.AllocParameter = par;
                    par.PeriodPayTypes.Add(pt);//и добавляем в параметры периода
                    //}
                }
            }
        }

        public static void initDepartmentControlls(IObjectSpace local_object_space, HrmPeriodAllocParameter alloc_parameter) {
            foreach (Department dep in local_object_space.GetObjects<Department>(null, true)) {
                HrmPeriodDepartmentControl dep_control = local_object_space.CreateObject<HrmPeriodDepartmentControl>();
                dep_control.AllocParameter = alloc_parameter;
                dep_control.Department = dep;
                dep_control.BuhCode = dep.BuhCode;
                dep_control.Group = dep.GroupDep;
                alloc_parameter.DepartmentControl.Add(dep_control);
            }
        }

        public static void addAllPayTypes(IObjectSpace os, HrmPeriodAllocParameter par) {
            foreach (HrmSalaryPayType salary in os.GetObjects<HrmSalaryPayType>(null, true)) {
                HrmPeriodPayType pay_type = os.CreateObject<HrmPeriodPayType>();
                pay_type.PayType = salary;
                pay_type.AllocParameter = par;
                par.PeriodPayTypes.Add(pay_type);
            }
        }

        public static void addTestData(IObjectSpace a_object_space) {
            for (int i = 0 ; i < _Allocparameter_Count ; i++) {
                var alloc_parameter = createParameters(a_object_space);
                alloc_parameter.StatusSet(HrmPeriodAllocParameterStatus.ALLOC_PARAMETERS_ACCEPTED);
                foreach (var each in a_object_space.GetObjects<HrmPeriod>(null, true)) {
                    each.setStatus(HrmPeriodStatus.CLOSED);
                }
            }
        }
    }
}