using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
//
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Templates;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
//
using NpoMash.Erm.Hrm.Salary;

namespace NpoMash.Erm.Hrm {

    public partial class HrmPeriodVC : ViewController {
        public HrmPeriodVC() {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnActivated() { 
            base.OnActivated(); 
        }

        protected override void OnViewControlsCreated() { 
            base.OnViewControlsCreated(); 
        }

        protected override void OnDeactivated() { 
            base.OnDeactivated(); 
        }

        private void OpenPeriodAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            IObjectSpace rootObjectspace = Application.CreateObjectSpace();
            HrmPeriod period = rootObjectspace.CreateObject<HrmPeriod>();

            var HrmPeriodCollection = rootObjectspace.GetObjects<HrmPeriod>();
            var maxYear = HrmPeriodCollection.Max(Period => Period.Year);
            List<HrmPeriod> HrmPeriodMaxYearsCollection = new List<HrmPeriod>(); //Список периодов с максимальным годом

            //Формируем этот лист
            foreach (var a in HrmPeriodCollection) { 
                if (a.Year == maxYear) { 
                    HrmPeriodMaxYearsCollection.Add(a); 
                } 
            }
            var count = HrmPeriodMaxYearsCollection.Count(); //Для проверки работоспособности
            var maxMonth = HrmPeriodMaxYearsCollection.Max(myProd => myProd.Month); //Максимальный месяц в этой коллекции

            foreach (var t in HrmPeriodCollection) {
                if (t.Year == maxYear && t.Month == maxMonth) { 
                    period.PeriodPrevious = t; 
                }
            }

            period.Year = maxYear;
            period.Month = maxMonth;
            period.addMonth();

            HrmPeriodAllocParameter period_parameters = rootObjectspace.CreateObject<HrmPeriodAllocParameter>();
            period.HrmPeriodAllocParameter = period_parameters;
            e.ShowViewParameters.CreatedView = Application.CreateDetailView(rootObjectspace, period_parameters);
        }


    }
}
