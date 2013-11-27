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
using NpoMash.Erm.Hrm.Salary;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
namespace NpoMash.Erm.Hrm
{

    public partial class HrmPeriodVC : ViewController
    {
        public HrmPeriodVC()
        {
            InitializeComponent();
            RegisterActions(components);

        }
        protected override void OnActivated()
        {
            base.OnActivated();

        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();

        }
        protected override void OnDeactivated()
        {

            base.OnDeactivated();
        }



        private void OpenPeriodAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace rootObjectspace = Application.CreateObjectSpace();
            HrmPeriod obj = rootObjectspace.CreateObject<HrmPeriod>();

            var HrmPeriodCollection = rootObjectspace.GetObjects<HrmPeriod>();

            var count = HrmPeriodCollection.Count();
            var maxMonth = HrmPeriodCollection.Max(myProd => myProd.Month);
            var maxYear = HrmPeriodCollection.Max(myProd => myProd.Year);

            HrmPeriodAllocParameter obj1 = rootObjectspace.CreateObject<HrmPeriodAllocParameter>();
            obj.HrmPeriodAllocParameter = obj1;
            e.ShowViewParameters.CreatedView = Application.CreateDetailView(rootObjectspace, obj1);
        }


    }
}
