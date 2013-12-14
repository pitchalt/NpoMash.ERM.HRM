using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using NpoMash.Erm.Hrm;
using IntecoAG.Erm.FM.Order;
using IntecoAG.Erm.HRM;

namespace NpoMash.Erm.Hrm.Salary
{
    public static class AllocParametersLogic {
        private const Int16 INIT_NORM_NO_CONTROL_KB = 1000;
        private const Int16 INIT_NORM_NO_CONTROL_OZM = 2000;

        public static HrmPeriodAllocParameter createParameters(IObjectSpace os) {
            HrmPeriod new_period = HrmPeriodLogic.createPeriod(os); // здесь если уже есть открытый период сгенерируется исключение
            HrmPeriodAllocParameter alloc_parameter = initParameters(os,new_period);
            return alloc_parameter;
        }

        private static HrmPeriodAllocParameter initParameters(IObjectSpace os, HrmPeriod current_period) {
            HrmPeriodAllocParameter par = os.CreateObject<HrmPeriodAllocParameter>();
            par.Period = current_period;
            current_period.CurrentAllocParameter = par;
            current_period.AllocParameters.Add(par);
            par.setStatus(HrmPeriodAllocParameterStatus.OpenToEdit);
            initParametersFromPreviousPeriod(os, par);
            initOrderControls(os, par);
            return par;
        }


        private static void initParametersFromPreviousPeriod(IObjectSpace os, HrmPeriodAllocParameter par) {
            if (par.Period.PeriodPrevious == par.Period) {
                par.NormNoControlKB = INIT_NORM_NO_CONTROL_KB;
                par.NormNoControlOZM = INIT_NORM_NO_CONTROL_OZM;
                addAllPayTypes(os, par);
            }
            else {
                par.NormNoControlKB = par.Period.PeriodPrevious.CurrentAllocParameter.NormNoControlKB;
                par.NormNoControlOZM = par.Period.PeriodPrevious.CurrentAllocParameter.NormNoControlOZM;
                foreach (HrmPeriodPayType pay in par.Period.PeriodPrevious.CurrentAllocParameter.PeriodPayTypes) {
                    bool alreadyThere = false;
                    foreach (HrmPeriodPayType existingPay in par.PeriodPayTypes)// перебираем уже назначенные
                        //проверяя, нет ли в параметрах периода PayTypes-ов со ссылкой туда же
                        if (pay.PayType == existingPay.PayType) alreadyThere = true;
                    if (!alreadyThere)//если такой еще не добавляли...
                    {
                        HrmPeriodPayType pt = os.CreateObject<HrmPeriodPayType>();//то создаем
                        pt.PayType = pay.PayType;//задаем ссылку на нужный PayType
                        pt.AllocParameter = par;
                        par.PeriodPayTypes.Add(pt);//и добавляем в параметры периода
                    }
                }
            }
        }

        private static void addAllPayTypes(IObjectSpace os, HrmPeriodAllocParameter par) {
            foreach (HrmSalaryPayType salary in os.GetObjects<HrmSalaryPayType>(null,true)) {
                HrmPeriodPayType pay_type = os.CreateObject<HrmPeriodPayType>();
                pay_type.PayType = salary;
                pay_type.AllocParameter = par;
                par.PeriodPayTypes.Add(pay_type);
            }
        }

        private static void initOrderControls(IObjectSpace os, HrmPeriodAllocParameter par) {
            //теперь создаем HrmPeriodOrderControl-ы, для этого перебираем все fmCOrder
            foreach (fmCOrder order in os.GetObjects<fmCOrder>(null,true)) {
                if (order.TypeControl != fmCOrderTypeCOntrol.No_Ordered)//если контролируемый
                {
                    bool alreadyThere = false;//то проверяем не добавляли ли уже HrmPeriodOrderControl для него
                    foreach (var existingControl in par.OrderControls)
                        if (existingControl.Order == order) alreadyThere = true;
                    if (!alreadyThere)//если такого еще не было
                    {//то создаем новый HrmPeriodOrderControl и копируем в него параметры из fmCOrder-а
                        HrmPeriodOrderControl oc = os.CreateObject<HrmPeriodOrderControl>();
                        oc.Order = order;
                        oc.NormKB = order.NormKB;
                        oc.NormOZM = order.NormOZM;
                        //oc.TypeControl = order.TypeControl; вот так почему-то нельзя, приходится делать как написано ниже:
                        if (order.TypeControl == fmCOrderTypeCOntrol.FOT)
                            oc.TypeControl = fmCOrderTypeCOntrol.FOT;
                        else oc.TypeControl = fmCOrderTypeCOntrol.TrudEmk_FOT;
                        par.OrderControls.Add(oc);//и добавляем в коллекцию
                    }
                }
            }
        }

        public static void acceptParameters(IObjectSpace os, HrmPeriodAllocParameter alloc_parameter) {
            if (alloc_parameter.Status != HrmPeriodAllocParameterStatus.AllocParametersAccepted) {
                if (alloc_parameter.Status == HrmPeriodAllocParameterStatus.OpenToEdit)
                    alloc_parameter.setStatus(HrmPeriodAllocParameterStatus.ListOfOrderAccepted);
                else if (alloc_parameter.Status == HrmPeriodAllocParameterStatus.ListOfOrderAccepted)
                    alloc_parameter.setStatus(HrmPeriodAllocParameterStatus.AllocParametersAccepted);

                os.GetObjects<fmCOrder>();
                //обновление заказов в справочнике
                foreach (HrmPeriodOrderControl order_control in alloc_parameter.OrderControls) {
                    order_control.Order.TypeControl = order_control.TypeControl;
                    order_control.Order.NormKB = order_control.NormKB;
                    order_control.Order.NormOZM = order_control.NormOZM;
                }
            }
        }

        

    }//end of AllocParametersLogic class
}//end of namespace
