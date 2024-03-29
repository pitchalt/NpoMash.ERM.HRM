using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Configuration;
using System.ComponentModel;
using System.Collections.Generic;

using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

using NpoMash.Erm.Hrm;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.HRM;
using IntecoAG.ERM.HRM.Organization;

namespace NpoMash.Erm.Hrm.Salary {
    public static class HrmAllocParameterLogic {
        public const Int16 INIT_NORM_NO_CONTROL_KB = 1000;
        public const Int16 INIT_NORM_NO_CONTROL_OZM = 2000;

        public static HrmAllocParameter createParameters(IObjectSpace os) {
            HrmPeriod new_period = HrmPeriodLogic.createPeriod(os); // ����� ���� ��� ���� �������� ������ ������������� ����������
            HrmAllocParameter alloc_parameter = initParameters(os, new_period);
            return alloc_parameter;
        }

        public static HrmAllocParameter initParameters(IObjectSpace os, HrmPeriod current_period) {
            HrmAllocParameter par = os.CreateObject<HrmAllocParameter>();
            par.Period = current_period;
            current_period.CurrentAllocParameter = par;
            current_period.AllocParameters.Add(par);
            par.StatusSet(HrmPeriodAllocParameterStatus.OPEN_TO_EDIT);
            //initParametersFromPreviousPeriod(os, par);
            par.NormNoControlKB = par.Period.PeriodPrevious.CurrentAllocParameter.NormNoControlKB;
            par.NormNoControlOZM = par.Period.PeriodPrevious.CurrentAllocParameter.NormNoControlOZM;
            InitPeriodPaytypes(os, par);
            initDepartmentControlls(os, par);
            initOrderControls(os, par);
            return par;
        }

        public static void InitPeriodPaytypes(IObjectSpace local_object_space, HrmAllocParameter alloc_parameter) {
            foreach (HrmSalaryPayType paytype in local_object_space.GetObjects<HrmSalaryPayType>()) {
                HrmAllocParameterPayType period_paytype = local_object_space.CreateObject<HrmAllocParameterPayType>();
                period_paytype.PayType = paytype;
                if (paytype.Type == IntecoAG.ERM.HRM.HrmPayTypes.PROVISION_CODE) { period_paytype.Type = HrmPayTypes.PROVISION_CODE; }
                if (paytype.Type == IntecoAG.ERM.HRM.HrmPayTypes.TRAVEL_CODE) { period_paytype.Type = HrmPayTypes.TRAVEL_CODE; }
                if (paytype.Type == IntecoAG.ERM.HRM.HrmPayTypes.BASE_CODE) { period_paytype.Type = HrmPayTypes.BASE_CODE; }
                period_paytype.AllocParameter = alloc_parameter;
                alloc_parameter.PeriodPayTypes.Add(period_paytype);
            }
        }

        /*
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
                    foreach (HrmPeriodPayType existingPay in par.PeriodPayTypes)// ���������� ��� �����������
                        //��������, ��� �� � ���������� ������� PayTypes-�� �� ������� ���� ��
                        if (pay.PayType == existingPay.PayType) alreadyThere = true;
                    if (!alreadyThere)//���� ����� ��� �� ���������...
                    {/*
                        HrmPeriodPayType pt = os.CreateObject<HrmPeriodPayType>();//�� �������
                        pt.PayType = pay.PayType;//������ ������ �� ������ PayType
                        pt.AllocParameter = par;
                        par.PeriodPayTypes.Add(pt);//� ��������� � ��������� �������
                    //}
               }
           }
        }

        public static void addAllPayTypes(IObjectSpace os, HrmPeriodAllocParameter par) {
            foreach (HrmSalaryPayType salary in os.GetObjects<HrmSalaryPayType>(null,true)) {
                HrmPeriodPayType pay_type = os.CreateObject<HrmPeriodPayType>();
                pay_type.PayType = salary;
                pay_type.AllocParameter = par;
                par.PeriodPayTypes.Add(pay_type);
            }
        }
*/
        public static void initDepartmentControlls(IObjectSpace local_object_space, HrmAllocParameter alloc_parameter) {
            foreach (Department dep in local_object_space.GetObjects<Department>()) {
                HrmAllocParameterDepartmentControl dep_control = local_object_space.CreateObject<HrmAllocParameterDepartmentControl>();
                dep_control.AllocParameter = alloc_parameter;
                dep_control.Department = dep;
                dep_control.BuhCode = dep.BuhCode;
                dep_control.Group = dep.GroupDep;
                alloc_parameter.DepartmentControl.Add(dep_control);
            }
        }

        public static void initOrderControls(IObjectSpace os, HrmAllocParameter par) {
            //������ ������� HrmPeriodOrderControl-�, ��� ����� ���������� ��� fmCOrder
            foreach (fmCOrder order in os.GetObjects<fmCOrder>(null, true)) {
                if (order.TypeControl != FmCOrderTypeControl.NO_ORDERED)//���� ��������������
                {
                    /*bool alreadyThere = false;//�� ��������� �� ��������� �� ��� HrmPeriodOrderControl ��� ����
                    foreach (var existingControl in par.OrderControls)
                        if (existingControl.Order == order) alreadyThere = true;
                    if (!alreadyThere)//���� ������ ��� �� ����
                    {//�� ������� ����� HrmPeriodOrderControl � �������� � ���� ��������� �� fmCOrder-� */
                    HrmAllocParameterOrderControl oc = os.CreateObject<HrmAllocParameterOrderControl>();
                    oc.Order = order;
                    oc.NormKB = order.NormKB;
                    oc.NormOZM = order.NormOZM;
                    //oc.TypeControl = order.TypeControl; ��� ��� ������-�� ������, ���������� ������ ��� �������� ����:
                    if (order.TypeControl == FmCOrderTypeControl.FOT)
                        oc.TypeControl = FmCOrderTypeControl.FOT;
                    else oc.TypeControl = FmCOrderTypeControl.TRUDEMK_FOT;
                    par.OrderControls.Add(oc);//� ��������� � ���������
                }
            }
        }


        public static void acceptParameters(IObjectSpace os, HrmAllocParameter alloc_parameter) {
            if (alloc_parameter.Status != HrmPeriodAllocParameterStatus.ALLOC_PARAMETERS_ACCEPTED) {
                if (alloc_parameter.Status == HrmPeriodAllocParameterStatus.OPEN_TO_EDIT) {
                    alloc_parameter.StatusSet(HrmPeriodAllocParameterStatus.LIST_OF_ORDER_ACCEPTED);
                    Directory.CreateDirectory(ConfigurationManager.AppSettings["FileExchangePath.ROOT"] + Convert.ToString(alloc_parameter.Period.CurrentAllocParameter.Year * 100 + alloc_parameter.Period.CurrentAllocParameter.Month));
                    //alloc_parameter.Period.setStatus(HrmPeriodStatus.LIST_OF_CONTROLLED_ORDERS_ACCEPTED);
                }
                else if (alloc_parameter.Status == HrmPeriodAllocParameterStatus.LIST_OF_ORDER_ACCEPTED) {
                    UpdatePayTypes(os, alloc_parameter);
                    alloc_parameter.StatusSet(HrmPeriodAllocParameterStatus.ALLOC_PARAMETERS_ACCEPTED);
                }
                if (alloc_parameter.Period.Status == HrmPeriodStatus.OPENED)
                    alloc_parameter.Period.setStatus(HrmPeriodStatus.READY_TO_CALCULATE_COERCED_MATRIXS);
                else if (alloc_parameter.Period.Status == HrmPeriodStatus.ACCOUNT_OPERATION_FIRST_IMPORTED && HrmPeriodLogic.AccountOperationCompared(alloc_parameter.Period)) {
                    alloc_parameter.Period.setStatus(HrmPeriodStatus.READY_TO_RESERVE_MATRIX_CREATE);
                }

                //���������� ������� � �����������
                updateFmCOrders(os, alloc_parameter);
            }
        }

        public static void UpdatePayTypes(IObjectSpace local_object_space, HrmAllocParameter alloc_parameter) {
            foreach (var period_paytype in alloc_parameter.PeriodPayTypes) {
                if (period_paytype.Type == HrmPayTypes.PROVISION_CODE) { period_paytype.PayType.Type = IntecoAG.ERM.HRM.HrmPayTypes.PROVISION_CODE; }
                else { period_paytype.PayType.Type = IntecoAG.ERM.HRM.HrmPayTypes.TRAVEL_CODE; }
            }
        }

        public static void updateFmCOrders(IObjectSpace os, HrmAllocParameter alloc_parameter) {
            List<HrmAllocParameterOrderControl> order_controls_to_delete = new List<HrmAllocParameterOrderControl>();
            foreach (var order in os.GetObjects<fmCOrder>()) {
                bool in_order_controls = false;

                foreach (var order_control in alloc_parameter.OrderControls) {
                    if (order_control.Order == order) {
                        if (order_control.TypeControl == FmCOrderTypeControl.NO_ORDERED) {
                            //alloc_parameter.OrderControls.Remove(order_control);
                            //order_control.Delete();
                            order_controls_to_delete.Add(order_control);
                        }
                        else {
                            in_order_controls = true;
                            order.TypeControl = order_control.TypeControl;
                            order.NormKB = order_control.NormKB;
                            order.NormOZM = order_control.NormOZM;
                        }
                    }
                }
                if (!in_order_controls) order.TypeControl = FmCOrderTypeControl.NO_ORDERED;
            }
            alloc_parameter.OrderControls.DeleteObjectOnRemove = true;
            foreach (HrmAllocParameterOrderControl order_control in order_controls_to_delete)
                alloc_parameter.OrderControls.Remove(order_control);
        }

    }//end of AllocParametersLogic class
}//end of namespace