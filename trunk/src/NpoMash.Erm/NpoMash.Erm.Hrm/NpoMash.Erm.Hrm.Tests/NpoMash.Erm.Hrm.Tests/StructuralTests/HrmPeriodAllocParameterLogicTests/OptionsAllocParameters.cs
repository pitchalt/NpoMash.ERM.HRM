using System;
using System.Linq;
using System.Collections.Generic;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.ConditionalAppearance;

using IntecoAG.ERM.HRM;
using IntecoAG.ERM.FM.Order;
using NpoMash.Erm.Hrm.Salary;
using NpoMash.Erm.Hrm.Tests.Controllers;

using NUnit.Framework;

namespace NpoMash.Erm.Hrm.Tests.StructuralTests.HrmPeriodAllocParameterLogicTests {


    [TestFixture]
    public class OptionsAllocParameters {

        protected TestApplication application;
        protected AppearanceTarget target;
        protected AppearanceController controller;
        protected DetailView detail_view;

        [SetUp]
        protected virtual void SetUp() {
            IObjectSpaceProvider object_space_provider = new XPObjectSpaceProvider(new MemoryDataStoreProvider());
            application = new TestApplication();
            ModuleBase test_module = new ModuleBase();
            test_module.AdditionalExportedTypes.Add(typeof(HrmPeriod));
            application.Modules.Add(new ConditionalAppearanceModule());
            application.Modules.Add(test_module);
            application.Setup("TestApplication", object_space_provider);
            IObjectSpace object_space = application.CreateObjectSpace();
            target = new AppearanceTarget();
            controller = new AppearanceController();
            TestWCLogic.SalaryPayTypeGenerate(object_space);
            TestWCLogic.DepartmentsGenerate(object_space);
            TestWCLogic.OrdersGenerate(object_space);
            TestWCLogic.addTestData(object_space);
            object_space.CommitChanges();
        }

        protected void ValidateAllocParameterWithOrders(IObjectSpace os, HrmPeriodAllocParameter param) {
            IList<fmCOrder> orders = os.GetObjects<fmCOrder>();
            Int32 order_count = 0;
            Int32 order_control_count = param.OrderControls.Count;
            foreach (fmCOrder order in orders) {
                HrmPeriodOrderControl order_control = param.OrderControls.FirstOrDefault(x => x.Order == order);
                if (order.TypeControl == FmCOrderTypeControl.TRUDEMK_FOT ||
                    order.TypeControl == FmCOrderTypeControl.FOT) {
                    order_count++;
                    Assert.IsNotNull(order_control);
                    Assert.AreEqual(order_control.TypeControl, order.TypeControl);
                    Assert.AreEqual(order_control.NormKB, order.NormKB);
                    Assert.AreEqual(order_control.NormOZM, order.NormOZM);
                }
                else {
                    if (order_control != null) {
                        Assert.AreEqual(order_control.TypeControl, FmCOrderTypeControl.NO_ORDERED);
                        order_control_count--;
                    }
                    else
                        Assert.IsNull(order_control);
                }
            }
            Assert.AreEqual(order_control_count, order_count);
        }
    }
}