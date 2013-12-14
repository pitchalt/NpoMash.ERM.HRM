using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.Layout;

using IntecoAG.Erm.FM.Order;
using NpoMash.Erm.Hrm;
using NpoMash.Erm.Hrm.Salary;
using NpoMash.Erm.Hrm.Tests.Controllers;


namespace NpoMash.Erm.Hrm.Tests.StructuralTests {

    [TestFixture]
    public class HrmPeriodAllocParameterLogicTests {
        private TestApplication application;
        private fmCOrder order1;
        private fmCOrder order2;
        private fmCOrder order3;
        private fmCOrder order4;
        [SetUp]
        public void setUp() {
            IObjectSpaceProvider object_space_provider = new XPObjectSpaceProvider(new MemoryDataStoreProvider());
            application = new TestApplication();
            ModuleBase test_module = new ModuleBase();
            test_module.AdditionalExportedTypes.Add(typeof(HrmPeriod));
            application.Modules.Add(test_module);
            application.Setup("TestApplication", object_space_provider);
            IObjectSpace os = application.CreateObjectSpace();
            HrmPeriod period = os.CreateObject<HrmPeriod>();
            period.Init(2013, 10);
            period.Status = HrmPeriodStatus.closed;
            HrmPeriodAllocParameter param = os.CreateObject<HrmPeriodAllocParameter>();
            period.AllocParameters.Add(param);
            period.CurrentAllocParameter = param;
            //            object_space = application.CreateObjectSpace();
            order1 = os.CreateObject<fmCOrder>();
            order1.Code = "Code1";
            order1.NormKB = 11;
            order1.NormOZM = 21;
            order1.TypeControl = fmCOrderTypeCOntrol.TrudEmk_FOT;
            order2 = os.CreateObject<fmCOrder>();
            order2.Code = "Code2";
            order2.NormKB = 12;
            order2.NormOZM = 22;
            order2.TypeControl = fmCOrderTypeCOntrol.FOT;
            order3 = os.CreateObject<fmCOrder>();
            order3.Code = "Code3";
            order3.NormKB = 13;
            order3.NormOZM = 23;
            order3.TypeControl = fmCOrderTypeCOntrol.No_Ordered;
            order4 = os.CreateObject<fmCOrder>();
            order4.Code = "Code4";
            order4.NormKB = 0;
            order4.NormOZM = 0;
            order4.TypeControl = fmCOrderTypeCOntrol.No_Ordered;
            os.CommitChanges();
        }

        [Test]
        public void TestHrmPeriodAllocParameterCreate() {
            IObjectSpace os = application.CreateObjectSpace();
            IList<fmCOrder> orders = os.GetObjects<fmCOrder>(); 
            HrmPeriodAllocParameter param = HrmPeriodAllocParameterLogic.createParameters(os);
            ValidateAllocParameterWithOrders(os, param);
            Assert.AreEqual(param.Status, HrmPeriodAllocParameterStatus.OpenToEdit);
        }

        [Test]
        public void TestHrmPeriodAllocParameter_ConfirmTrud_EditAdd() {
            IObjectSpace os = application.CreateObjectSpace();
            IList<fmCOrder> orders = os.GetObjects<fmCOrder>();
            HrmPeriodAllocParameter param = HrmPeriodAllocParameterLogic.createParameters(os);
            os.CommitChanges();
            os = application.CreateObjectSpace();
            param = os.GetObject<HrmPeriodAllocParameter>(param);
            HrmPeriodOrderControl change_norm = param.OrderControls[0];
            change_norm.NormKB = 5;
            change_norm.NormOZM = 15;
            HrmPeriodOrderControl change_remove = param.OrderControls[1];
            change_remove.TypeControl = fmCOrderTypeCOntrol.No_Ordered;
            HrmPeriodOrderControl change_add = os.CreateObject<HrmPeriodOrderControl>();
            param.OrderControls.Add(change_add);
            change_add.Order = os.GetObject<fmCOrder>(order3);
            HrmPeriodAllocParameterLogic.acceptParameters(os, param);
            os.CommitChanges();
            os = application.CreateObjectSpace();
            param = os.GetObject<HrmPeriodAllocParameter>(param);
            ValidateAllocParameterWithOrders(os, param);
        }

        [Test]
        public void TestHrmPeriodAllocParameter_ConfirmTrud_AddRemoveOrderControl() {
            IObjectSpace os = application.CreateObjectSpace();
            IList<fmCOrder> orders = os.GetObjects<fmCOrder>();
            HrmPeriodAllocParameter param = HrmPeriodAllocParameterLogic.createParameters(os);
            os.CommitChanges();
            os = application.CreateObjectSpace();
            param = os.GetObject<HrmPeriodAllocParameter>(param);
            HrmPeriodOrderControl change_remove = param.OrderControls[1];
            os.Delete(change_remove);
            HrmPeriodOrderControl change_add = os.CreateObject<HrmPeriodOrderControl>();
            param.OrderControls.Add(change_add);
            change_add.Order = os.GetObject<fmCOrder>(order3);
            HrmPeriodAllocParameterLogic.acceptParameters(os, param);
            os.CommitChanges();
            os = application.CreateObjectSpace();
            param = os.GetObject<HrmPeriodAllocParameter>(param);
            ValidateAllocParameterWithOrders(os, param);
        }

        private void ValidateAllocParameterWithOrders(IObjectSpace os, HrmPeriodAllocParameter param) {
            IList<fmCOrder> orders = os.GetObjects<fmCOrder>();
            Int32 order_count = 0;
            Int32 order_control_count = param.OrderControls.Count;
            foreach (fmCOrder order in orders) {
                HrmPeriodOrderControl order_control = param.OrderControls.FirstOrDefault(x => x.Order == order);
                if (order.TypeControl == fmCOrderTypeCOntrol.TrudEmk_FOT ||
                    order.TypeControl == fmCOrderTypeCOntrol.FOT) {
                    order_count++;
                    Assert.IsNotNull(order_control);
                    Assert.AreEqual(order_control.TypeControl, order.TypeControl);
                    Assert.AreEqual(order_control.NormKB, order.NormKB);
                    Assert.AreEqual(order_control.NormOZM, order.NormOZM);
                }
                else {
                    if (order_control != null) {
                        Assert.AreEqual(order_control.TypeControl, fmCOrderTypeCOntrol.No_Ordered);
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
