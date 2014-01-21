using System;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.Layout;

using NpoMash.Erm.Hrm;
using IntecoAG.ERM.HRM;
using IntecoAG.ERM.FM.Order;
using NpoMash.Erm.Hrm.Salary;
using NpoMash.Erm.Hrm.Tests.Controllers;


namespace NpoMash.Erm.Hrm.Tests.StructuralTests {


    [TestFixture]
    public class HrmPeriodAllocParameterLogicTests {
        private TestApplication application;
        private fmCOrder order3;
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
            period.setStatus(HrmPeriodStatus.CLOSED);
            HrmPeriodAllocParameter param = os.CreateObject<HrmPeriodAllocParameter>();
            period.AllocParameters.Add(param);
            period.CurrentAllocParameter = param;
            HrmPeriodPayType period_pay_type = os.CreateObject<HrmPeriodPayType>();
            HrmSalaryPayType salary_pay_type = os.CreateObject<HrmSalaryPayType>();
            param.PeriodPayTypes.Add( period_pay_type);
            period_pay_type.AllocParameter = param;
            period_pay_type.PayType = salary_pay_type;
//            period.PeriodPrevious = period; ;
            //            object_space = application.CreateObjectSpace();
            /*
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
             * */
            TestWCLogic.referenceClassesGenerate( os );
            os.CommitChanges();
        }

        [Test]
        public void TestChangingStatus() {
            IObjectSpace os = application.CreateObjectSpace();
            TestWCLogic.referenceClassesGenerate( os );
            HrmPeriodAllocParameter param = HrmPeriodAllocParameterLogic.createParameters( os );
            HrmPeriodAllocParameterLogic.initParametersFromPreviousPeriod( os, param );
            foreach ( var each in param.OrderControls ) {
                if ( each.TypeControl == fmCOrderTypeCOntrol.FOT ) { each.TypeControl = fmCOrderTypeCOntrol.No_Ordered; }
            }
            HrmPeriodAllocParameterLogic.acceptParameters( os, param );
            os.CommitChanges();
        }

        [Test] //(Параметры расчета 2) Создание параметров и проверка статуса
        public void TestHrmPeriodAllocParameter_Create() {
            IObjectSpace os = application.CreateObjectSpace();
            HrmPeriodAllocParameter param = HrmPeriodAllocParameterLogic.createParameters( os );
            param.Period.PeriodPrevious = param.Period;
            HrmPeriodAllocParameterLogic.initParametersFromPreviousPeriod( os, param );
            ValidateAllocParameterWithOrders( os, param );
            Assert.AreEqual( param.Status, HrmPeriodAllocParameterStatus.OPEN_TO_EDIT );
        }

        [Test] //(Параметры расчета 4) Повторное создание параметров
        public void TestHrmPeriodAllocParameter_CreateDoubleParameters() {
            IObjectSpace os = application.CreateObjectSpace();
            HrmPeriodAllocParameter param = HrmPeriodAllocParameterLogic.createParameters( os );
            HrmPeriodAllocParameterLogic.initParametersFromPreviousPeriod( os, param );
            ValidateAllocParameterWithOrders( os, param );
            try { HrmPeriodAllocParameter new_param = HrmPeriodAllocParameterLogic.createParameters( os ); }
            catch ( OpenPeriodExistsException ) { }
        }

        [Test] //Проверка статуса Утвержден список контролируемых заказов
        public void TestHrmPeriodAllocParameter_CheckOfOrderAcceptedStatus() {
            IObjectSpace os = application.CreateObjectSpace();
            HrmPeriodAllocParameter param = HrmPeriodAllocParameterLogic.createParameters( os );
            HrmPeriodAllocParameterLogic.initParametersFromPreviousPeriod( os, param );
            ValidateAllocParameterWithOrders( os, param );
            HrmPeriodAllocParameterLogic.acceptParameters( os, param );
            Assert.AreEqual( param.Status, HrmPeriodAllocParameterStatus.LIST_OF_ORDER_ACCEPTED );
        }

        [Test] //(Параметры расчета 5) Выполняться не должно
        public void TestHrmPeriodAllocParameter_EditingControlType() {
            IObjectSpace os = application.CreateObjectSpace();
            HrmPeriodAllocParameter param = HrmPeriodAllocParameterLogic.createParameters( os );
            HrmPeriodAllocParameterLogic.initParametersFromPreviousPeriod( os, param );
            ValidateAllocParameterWithOrders( os, param );
            HrmPeriodAllocParameterLogic.acceptParameters(os, param);
//            var orderCollection = os.GetObjects<fmCOrder>();
            foreach ( var each in param.OrderControls ) {
                if ( each.TypeControl == fmCOrderTypeCOntrol.TrudEmk_FOT ) { each.TypeControl = fmCOrderTypeCOntrol.FOT; }
            }
            os.CommitChanges();
            ValidateAllocParameterWithOrders( os, param );
        }

        [Test] //(Параметры расчета 6) Выполняться не должно
        public void TestHrmPeriodAllocParameter_AddNew_TF_Order() {
            IObjectSpace os = application.CreateObjectSpace();
            HrmPeriodAllocParameter param = HrmPeriodAllocParameterLogic.createParameters( os );
            HrmPeriodAllocParameterLogic.initParametersFromPreviousPeriod( os, param );
            os.CommitChanges();
            ValidateAllocParameterWithOrders( os, param );
            HrmPeriodAllocParameterLogic.acceptParameters( os, param );
            var new_order = os.CreateObject<fmCOrder>();
            new_order.Code = "newCode";
            new_order.NormKB = 40;
            new_order.NormOZM = 60;
 //           new_order.TypeConstancy = fmCOrdertypeConstancy.One;
            new_order.TypeControl = fmCOrderTypeCOntrol.TrudEmk_FOT;
            os.CommitChanges();
            ValidateAllocParameterWithOrders( os, param );
        }

        [Test] //(Параметры расчета 7) Выполняться не должно
        public void TestHrmPeriodAllocParameter_Delete_TF_Order() {
            IObjectSpace os = application.CreateObjectSpace();
            HrmPeriodAllocParameter param = HrmPeriodAllocParameterLogic.createParameters( os );
            HrmPeriodAllocParameterLogic.initParametersFromPreviousPeriod( os, param );
            ValidateAllocParameterWithOrders( os, param );
            HrmPeriodAllocParameterLogic.acceptParameters( os, param );
            IList<HrmPeriodOrderControl> delete_list = null;
            foreach ( var each in param.OrderControls ) {
                if ( each.TypeControl == fmCOrderTypeCOntrol.TrudEmk_FOT ) { delete_list.Add( each ); }
            }
            os.Delete( delete_list );
            os.CommitChanges();
            ValidateAllocParameterWithOrders( os, param );
        }

        [Test] //(Параметры расчета 8) Выполняться не должно
        public void TestHrmPeriodAllocParameter_TF_EditingControlType() {
            IObjectSpace os = application.CreateObjectSpace();
            HrmPeriodAllocParameter param = HrmPeriodAllocParameterLogic.createParameters( os );
            HrmPeriodAllocParameterLogic.initParametersFromPreviousPeriod( os, param );
            ValidateAllocParameterWithOrders( os, param );
            HrmPeriodAllocParameterLogic.acceptParameters( os, param );
            var orderCollection = os.GetObjects<fmCOrder>();
            foreach ( var each in param.OrderControls ) {
                if ( each.TypeControl != fmCOrderTypeCOntrol.TrudEmk_FOT ) { each.TypeControl = fmCOrderTypeCOntrol.TrudEmk_FOT; }
            }
            os.CommitChanges();
            ValidateAllocParameterWithOrders( os, param );
        }

        [Test]//Проверка статуса утверждены параметры расчета
        public void TestHrmPeriodAllocParameter_CheckStatus_AllocParametersAccepted() {
            IObjectSpace os = application.CreateObjectSpace();
            HrmPeriodAllocParameter param = HrmPeriodAllocParameterLogic.createParameters( os );
            HrmPeriodAllocParameterLogic.initParametersFromPreviousPeriod( os, param );
            ValidateAllocParameterWithOrders( os, param );
            HrmPeriodAllocParameterLogic.acceptParameters( os, param );
            HrmPeriodAllocParameterLogic.acceptParameters( os, param );
            ValidateAllocParameterWithOrders( os, param );
            Assert.AreEqual( param.Status, HrmPeriodAllocParameterStatus.ALLOC_PARAMETERS_ACCEPTED );
        }

        [Test]//(Параметры расчета 9) Выполняться не должно
        public void TestHrmPeriodAllocParameter_AllocFieldEditing() {
            IObjectSpace os = application.CreateObjectSpace();
            HrmPeriodAllocParameter param = HrmPeriodAllocParameterLogic.createParameters( os );
            HrmPeriodAllocParameterLogic.initParametersFromPreviousPeriod( os, param );
            ValidateAllocParameterWithOrders( os, param );
            HrmPeriodAllocParameterLogic.acceptParameters( os, param );
            HrmPeriodAllocParameterLogic.acceptParameters( os, param );
            param.NormNoControlKB = 50;
            param.NormNoControlOZM = 11111;
            os.CommitChanges();
            ValidateAllocParameterWithOrders( os, param );
        }

        [Test]//(Параметры расчета 10) Выполняться не должно
        public void TestHrmPeriodAllocParameter_DeleteCreatedAndSavedAllocs() {
            IObjectSpace os = application.CreateObjectSpace();
            HrmPeriodAllocParameter param = HrmPeriodAllocParameterLogic.createParameters( os );
            HrmPeriodAllocParameterLogic.initParametersFromPreviousPeriod( os, param );
            ValidateAllocParameterWithOrders( os, param );
            HrmPeriodAllocParameterLogic.acceptParameters( os, param );
            HrmPeriodAllocParameterLogic.acceptParameters( os, param );
            os.Delete( os.GetObjects<HrmPeriodOrderControl>() );
            os.CommitChanges();
            ValidateAllocParameterWithOrders( os, param );
        }

        [Test] //Радактирование параметров до их утверждения
        public void TestHrmPeriodAllocParameter_ConfirmTrud_EditAdd() {
            IObjectSpace os = application.CreateObjectSpace();
            IList<fmCOrder> orders = os.GetObjects<fmCOrder>();
            HrmPeriodAllocParameter param = HrmPeriodAllocParameterLogic.createParameters(os);
            HrmPeriodAllocParameterLogic.initParametersFromPreviousPeriod( os, param );
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

        [Test] // удаление параметров до их утверждения
        public void TestHrmPeriodAllocParameter_ConfirmTrud_AddRemoveOrderControl() {
            IObjectSpace os = application.CreateObjectSpace();
            IList<fmCOrder> orders = os.GetObjects<fmCOrder>();
            HrmPeriodAllocParameter param = HrmPeriodAllocParameterLogic.createParameters(os);
            HrmPeriodAllocParameterLogic.initParametersFromPreviousPeriod( os, param );
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

        [Test] // Выявление наличия мусора в базе после первой стадии подтвеждения заказов
        public void Test_SearchDataTrashFirst() {
            IObjectSpace os = application.CreateObjectSpace();
            HrmPeriodAllocParameter alloc_parameter = HrmPeriodAllocParameterLogic.createParameters( os );
            HrmPeriodAllocParameterLogic.initParametersFromPreviousPeriod( os, alloc_parameter );
            HrmPeriodAllocParameterLogic.acceptParameters( os, alloc_parameter );
            os.CommitChanges();
            foreach ( var each in os.GetObjects<HrmPeriodOrderControl>() ) {
                Assert.NotNull( each.AllocParameter );
                Assert.NotNull( each.Order );
                Assert.NotNull( each.This );
            }
        }

        [Test] // Выявление наличия мусора в базу после второй стадии подтверждения
        public void Test_SearchDataTrashSecond() {
            IObjectSpace os = application.CreateObjectSpace();
            HrmPeriodAllocParameter alloc_parameter = HrmPeriodAllocParameterLogic.createParameters( os );
            HrmPeriodAllocParameterLogic.acceptParameters( os, alloc_parameter );
            HrmPeriodAllocParameterLogic.acceptParameters( os, alloc_parameter );
            os.CommitChanges();
            foreach ( var each in os.GetObjects<HrmPeriodOrderControl>() ) {
                Assert.NotNull( each.AllocParameter );
                Assert.NotNull( each.Order );
                Assert.NotNull( each.This );
            }
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