﻿using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using FileHelpers;

using IntecoAG.ERM.HRM;
using IntecoAG.ERM.FM.Order;
using NpoMash.Erm.Hrm.Salary;
using IntecoAG.ERM.HRM.Organization;
using NpoMash.Erm.Hrm.Tests.ReferentialData;

using DevExpress.ExpressApp;


namespace NpoMash.Erm.Hrm.Tests.Controllers {


    public static class TestWCLogic {

        private const int _REFERENCE_COUNT = 10;
        private const int _DEPARTMENT_COUNT = 20;
        private const int _ALLOCPARAMETER_COUNT = 3;
        private const int _SALARYPAYTYPE_COUNT = 100;

        public static void addTimeSheets(IObjectSpace local_object_space) {
            foreach (var each in local_object_space.GetObjects<HrmPeriod>()) {
                var time_sheet = local_object_space.CreateObject<HrmTimeSheet>();
                var time_sheet_deps = local_object_space.CreateObject<HrmTimeSheetDep>();
            }
        }

        public static void ImportDeps(IObjectSpace local_object_space) {
            var engine = new FixedFileEngine<DepartmentImport>();
            DepartmentImport[] stream = engine.ReadFile("../../../../../../../var/referential/Dep.dat");
            foreach (var each in stream) {
                var department = local_object_space.CreateObject<Department>();
                department.Code = each.Code.Trim();
                if (each.Group == "01") {
                    department.GroupDep = DepartmentGroupDep.DEPARTMENT_KB;
                }
                else {
                    department.GroupDep = DepartmentGroupDep.DEPARTMENT_OZM;
                }
            }
        }

        public static void ImportOrders(IObjectSpace local_object_space) {
            var order_data = new FixedFileEngine<OrderImport>();
            OrderImport[] order_list = order_data.ReadFile("../../../../../../../var/referential/Orders.dat");
            foreach (var order in order_list) {
                var fmCorder = local_object_space.CreateObject<fmCOrder>();
                fmCorder.Code = order.Code.Trim();
                fmCorder.NormKB = order.NormKB;
                fmCorder.NormOZM = order.NormOZM;
                fmCorder.TypeConstancy = FmCOrderTypeConstancy.CONST_ORDER_TYPE;
                if (order.TypeControl.Trim() == "Ф") {
                    fmCorder.TypeControl = FmCOrderTypeControl.FOT;
                }
                if (order.TypeControl.Trim() == "ТФ") {
                    fmCorder.TypeControl = FmCOrderTypeControl.TRUDEMK_FOT;
                }
                if (order.TypeControl.Trim() == "") {
                    fmCorder.TypeControl = FmCOrderTypeControl.NO_ORDERED;
                }
            }
        }

        public static void SalaryPayTypeGenerate(IObjectSpace local_object_space) {
            var random = new Random();
            for (int i = 0 ; i<_SALARYPAYTYPE_COUNT ; i++) {
                var hrmSalaryPayType = local_object_space.CreateObject<HrmSalaryPayType>();
                hrmSalaryPayType.Code = Convert.ToString(random.Next(1000, 100000));
                hrmSalaryPayType.Name = Convert.ToString(random.Next(1000, 100000));
            }
        }

        public static void referenceClassesGenerate(IObjectSpace local_object_space) {
            var random = new Random();
            for (int i = 0 ; i < _DEPARTMENT_COUNT ; i++) {
                var department = local_object_space.CreateObject<Department>();
                department.Code = Convert.ToString(random.Next(1000, 4001));
                if (Convert.ToDecimal(i) < System.Math.Round(Convert.ToDecimal(_DEPARTMENT_COUNT / 2))) {
                    department.GroupDep = DepartmentGroupDep.DEPARTMENT_KB;
                }
                else {
                    department.GroupDep = DepartmentGroupDep.DEPARTMENT_OZM;
                }
            }
            for (int i = 0 ; i < _REFERENCE_COUNT ; i++) {
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
        }

        public static void addTestData(IObjectSpace a_object_space) {
            for (int i = 0 ; i < _ALLOCPARAMETER_COUNT ; i++) {
                var alloc_parameter = HrmPeriodAllocParameterLogic.createParameters(a_object_space);
                alloc_parameter.StatusSet(HrmPeriodAllocParameterStatus.ALLOC_PARAMETERS_ACCEPTED);
                foreach (var each in a_object_space.GetObjects<HrmPeriod>(null, true)) {
                    each.setStatus(HrmPeriodStatus.CLOSED);
                }
            }
        }
    }
}