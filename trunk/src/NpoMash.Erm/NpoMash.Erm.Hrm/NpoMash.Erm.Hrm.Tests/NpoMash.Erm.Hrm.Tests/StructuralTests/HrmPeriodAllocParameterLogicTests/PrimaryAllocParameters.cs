﻿using System;
using System.Linq;
using System.Collections.Generic;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;

using IntecoAG.ERM.HRM;
using IntecoAG.ERM.FM.Order;
using NpoMash.Erm.Hrm.Salary;
using NpoMash.Erm.Hrm.Tests.Controllers;

using NUnit.Framework;

namespace NpoMash.Erm.Hrm.Tests.StructuralTests.HrmPeriodAllocParameterLogicTests {


    [TestFixture]
    public class PrimaryAllocParameters {
        
        protected TestApplication application;

        [SetUp]
        public void SetUp() {
            IObjectSpaceProvider object_space_provider = new XPObjectSpaceProvider(new MemoryDataStoreProvider());
            application = new TestApplication();
            ModuleBase test_module = new ModuleBase();
            test_module.AdditionalExportedTypes.Add(typeof(HrmPeriod));
            application.Modules.Add(test_module);
            application.Setup("TestApplication", object_space_provider);
            IObjectSpace object_space = application.CreateObjectSpace();
            TestWCLogic.referenceClassesGenerate(object_space);
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