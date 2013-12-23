using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Collections;
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



namespace NpoMash.Erm.Hrm.Salary {
    public partial class HrmMatrixVC : ViewController {

       /* public enum dataSource {
            GENERATED_DATA=1,
            DATA_FROM_SERVER=2,
            
        }
        public enum fileSource {
            DATA_FROM_XML_FILE = 1,
            DATA_FROM_STRUCTURED_FILE = 2
        }
        */

       // private ChoiceActionItem setDataSourceItem;
       // private ChoiceActionItem setFileTypeItem;
        
        public HrmMatrixVC() { 
            InitializeComponent(); 
            RegisterActions(components); 
           /*
            GetSourceDataAction.Items.Clear();
            setDataSourceItem=new ChoiceActionItem(CaptionHelper.GetMemberCaption(typeof(HrmPeriod), "Source"), null);
            FillItemWithEnumValues(setDataSourceItem, typeof(dataSource));
            GetSourceDataAction.Items.Add(setDataSourceItem);
            

            setFileTypeItem = new ChoiceActionItem(CaptionHelper.GetMemberCaption(typeof(HrmPeriod), "FileType"), null);
            FillItemWithEnumValues(setFileTypeItem, typeof(fileSource));
            GetSourceDataAction.Items.Add(setFileTypeItem);
            
        */
        }

       /* private void FillItemWithEnumValues(ChoiceActionItem parentItem, Type enumType)  {
      
            foreach(object current in Enum.GetValues(enumType)) {
         EnumDescriptor ed = new EnumDescriptor(enumType);
         ChoiceActionItem item = new ChoiceActionItem(ed.GetCaption(current), current);
         parentItem.Items.Add(item);
      }
   }*/

        protected override void OnActivated() { base.OnActivated(); }
        protected override void OnViewControlsCreated() { base.OnViewControlsCreated(); }
        protected override void OnDeactivated() { base.OnDeactivated(); }

        private void ImportSourceData_Execute(object sender, ParametrizedActionExecuteEventArgs e) {

        }

        private void GetSourceDataAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e) {
            HrmPeriod period = (HrmPeriod)e.CurrentObject;
            IObjectSpace os = ObjectSpace.CreateNestedObjectSpace();
            HrmPeriod current_period = os.GetObject<HrmPeriod>(period);
            if (current_period.Status == HrmPeriodStatus.Opened || 
                current_period.Status == HrmPeriodStatus.ListOfControlledOrdersAccepted) {
                    if (e.SelectedChoiceActionItem.Id == "GenerateTestData") {
                        HrmMatrix matrix = HrmMatrixLogic.setTestData(os, current_period);
                        current_period.Status = HrmPeriodStatus.SourceDataLoaded;
                        matrix.Status = HRM_MATRIX_STATUS.Accepted;
                        e.ShowViewParameters.CreatedView = Application.CreateDetailView(os, matrix);
                    }
                    if (e.SelectedChoiceActionItem.Id == "GetDataFromServer") {

                    }
                    if (e.SelectedChoiceActionItem.Id == "XmlFile") {

                    }
                    if (e.SelectedChoiceActionItem.Id == "StructuredFile") {

                    }

            }
        }

        private void BringingMatrix_Execute(object sender, SimpleActionExecuteEventArgs e) {

        }



    }
}
