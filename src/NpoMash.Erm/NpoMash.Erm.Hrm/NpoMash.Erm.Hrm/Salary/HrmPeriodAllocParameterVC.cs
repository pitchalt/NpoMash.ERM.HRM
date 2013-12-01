using System;
using System.Linq;
using System.Text;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Templates;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Model.NodeGenerators;

using IntecoAG.Erm.FM.Order;
namespace NpoMash.Erm.Hrm.Salary
{
    // For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppViewControllertopic.
    public partial class HrmPeriodAllocParameterVC : ViewController
    {
        public HrmPeriodAllocParameterVC()
        {
            InitializeComponent();
            RegisterActions(components);
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void CreateAllocParameters_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            HrmPeriodAllocParameter par = e.CurrentObject as HrmPeriodAllocParameter;
            if (par == null) return;
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace())
            {//проверяем есть ли предыдущий период и назначены ли ему параметры
                if (par.HrmPeriod.PeriodPrevious != null &&
                    par.HrmPeriod.PeriodPrevious.HrmPeriodAllocParameter != null)
                {//теперь создаем PeriodPayTypes-ы, беря их из предыдущего периода
                    foreach (var pay in par.HrmPeriod.PeriodPrevious.HrmPeriodAllocParameter.PeriodPayTypes)
                    {
                        bool alreadyThere = false;
                        foreach (var existingPay in par.PeriodPayTypes)// перебираем уже назначенные
                            //проверяя, нет ли в параметрах периода PayTypes-ов со ссылкой туда же
                            if (pay.PayType == existingPay.PayType) alreadyThere = true;
                        if (!alreadyThere)//если такой еще не добавляли...
                        {
                            HrmPeriodPayType pt = os.CreateObject<HrmPeriodPayType>();//то создаем
                            pt.PayType = pay.PayType;//задаем ссылку на нужный PayType
                            par.PeriodPayTypes.Add(pt);//и добавляем в параметры периода
                        }
                    }
                }
                //теперь создаем HrmPeriodOrderControl-ы, для этого перебираем все fmCOrder
                foreach (var order in os.GetObjects<fmCOrder>())
                {
                    if (order.TypeControl != fmCOrder.fmCOrderTypeCOntrol.No_Ordered)//если контролируемый
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
                            oc.NormNoControl = order.NormNoControl;
                            //oc.TypeControl = order.TypeControl;  вот так почему-то нельзя, приходится делать как написано ниже:
                            if (order.TypeControl == fmCOrder.fmCOrderTypeCOntrol.FOT)
                                oc.TypeControl=HrmPeriodOrderControl.HrmPeriodOrderTypeControl.FOT;
                            else oc.TypeControl = HrmPeriodOrderControl.HrmPeriodOrderTypeControl.TrudEmk_FOT;
                            par.OrderControls.Add(oc);//и добавляем в коллекцию
                        }
                    }
                }
                os.CommitChanges(); //сохраняем изменения в корневой ObjectSpace
            }
        }
    }
}
