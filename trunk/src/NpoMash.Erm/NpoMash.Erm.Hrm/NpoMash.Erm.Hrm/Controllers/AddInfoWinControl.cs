using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
//
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;

using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Templates;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Model.NodeGenerators;
//
using IntecoAG.Erm.HRM;
using IntecoAG.Erm.FM.Order;
using NpoMash.Erm.Hrm.Salary;
using IntecoAG.Erm.HRM.Organization;

namespace NpoMash.Erm.Hrm.Controllers
{

    public partial class AddInfoWinControl : WindowController
    {
       

        private void AddDemoInfo_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

            IObjectSpace objectSpace = Application.CreateObjectSpace();
            Random random = new Random();

            var _PayTypesList = new List<HrmSalaryPayType>();
            for (int i = 0; i < 15; i++) // Коды оплат
            {
                var _PayTypes = objectSpace.CreateObject<HrmSalaryPayType>(); // Создаем объект КодыОплат
                _PayTypes.Name = "Основная заработная плата";
                _PayTypes.Code = Convert.ToString(random.Next(0, 1000));
                _PayTypesList.Add(_PayTypes);
            }

            var _OrderList = new List<fmCOrder>();
            for (int i = 0; i < 15; i++) // Заказы
            {
                var _Order = objectSpace.CreateObject<fmCOrder>();
                _Order.Code = Convert.ToString(random.Next(100000, 1000000));
                int variant_control = random.Next(1, 4);
                if (variant_control == 1) { _Order.TypeControl = fmCOrderTypeCOntrol.No_Ordered; }
                if (variant_control == 2) { _Order.TypeControl = fmCOrderTypeCOntrol.FOT; }
                if (variant_control == 3) { _Order.TypeControl = fmCOrderTypeCOntrol.TrudEmk_FOT; }
                int variant_constansy = random.Next(1, 3);
                if (variant_constansy == 1) { _Order.TypeConstancy = fmCOrdertypeConstancy.Null; }
                if (variant_constansy == 2) { _Order.TypeConstancy = fmCOrdertypeConstancy.One; }
                _Order.NormKB = Convert.ToDecimal(random.Next(100, 1000));
                _Order.NormOZM = Convert.ToDecimal(random.Next(100, 1000));
                _Order.NormNoControl = Convert.ToDecimal(random.Next(100, 1000));
                _OrderList.Add(_Order);
            }

            var _Period_One = objectSpace.CreateObject<HrmPeriod>(); // Создаем первый период
            _Period_One.Month = 10;
            _Period_One.Year = 2013;
            _Period_One.Status =HrmPeriodStatus.closed;

            var _AllocParameters = objectSpace.CreateObject<HrmPeriodAllocParameter>(); // Создаем объект параметров расчета
            _AllocParameters.Status = HrmPeriodAllocParameterStatus.ListOfOrderAccepted; // Устанавливаем статус
            _AllocParameters.HrmPeriod = _Period_One; //Устанавливаем созданный период
            _Period_One.HrmPeriodAllocParameter = _AllocParameters;


            foreach (var order in _OrderList) // Контроль заказа относительно того контроллируемый заказ или нет
            {
                if (order.TypeControl == fmCOrderTypeCOntrol.FOT || order.TypeControl == fmCOrderTypeCOntrol.TrudEmk_FOT)
                {
                    var _OrderControl = objectSpace.CreateObject<HrmPeriodOrderControl>();
                    int variant_control = random.Next(1, 4);
                    if (variant_control == 1) { _OrderControl.TypeControl = HrmPeriodOrderTypeControl.FOT; }
                    if (variant_control == 2) { _OrderControl.TypeControl = HrmPeriodOrderTypeControl.No_Ordered; }
                    if (variant_control == 3) { _OrderControl.TypeControl = HrmPeriodOrderTypeControl.TrudEmk_FOT; }
                    _OrderControl.NormKB = Convert.ToDecimal(random.Next(100, 1000));
                    _OrderControl.NormOZM = Convert.ToDecimal(random.Next(100, 1000));
                    _OrderControl.NormNoControl = Convert.ToDecimal(random.Next(100, 1000));
                    _OrderControl.Order = order;
                    _AllocParameters.OrderControls.Add(_OrderControl);
                }
            }


            foreach (var paytypes in _PayTypesList) // и последний шаг, формируем коллекцию типов оплат в параметрах расчета
            {
                var PeriodPayType = objectSpace.CreateObject<HrmPeriodPayType>();
                PeriodPayType.PayType = paytypes;
                _AllocParameters.PeriodPayTypes.Add(PeriodPayType);
            }

            objectSpace.CommitChanges(); //Отправляем данные в базу     
        }

        public AddInfoWinControl()
        { InitializeComponent(); RegisterActions(components); }
        protected override void OnActivated()
        { base.OnActivated(); }
        protected override void OnDeactivated()
        { base.OnDeactivated(); }
    }
}
