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
using DevExpress.ExpressApp.Editors;
using System.Collections;


namespace NpoMash.Erm.Hrm.Salary {
    public partial class HrmMatrixVC : ViewController {

        public enum DATA_SOURCE {
            generateTestData=0,
            dataFromFile=1,
            dataFromServer=2
        }


        private ChoiceActionItem setDataSourceItem;
        private ChoiceActionItem setFileTypeItem;
        
        public HrmMatrixVC() { 
            InitializeComponent(); 
            RegisterActions(components); 
           
            GetSourceDataAction.Items.Clear(); 
            setDataSourceItem=new ChoiceActionItem(CaptionHelper.GetMemberCaption(typeof(HrmPeriod), "Source"), null);
            GetSourceDataAction.Items.Add(setDataSourceItem);
            FillItemWithEnumValues(setDataSourceItem, typeof(DATA_SOURCE));

            setFileTypeItem = new ChoiceActionItem(CaptionHelper.GetMemberCaption(typeof(HrmPeriod), "FileType"), null);
            GetSourceDataAction.Items.Add(setFileTypeItem);
            FillItemWithEnumValues(setDataSourceItem, typeof(DATA_SOURCE));
        
        }

        private void FillItemWithEnumValues(ChoiceActionItem setDataSourceItem, Type enumType)  {
      
            foreach(object current in Enum.GetValues(enumType)) {
         EnumDescriptor ed = new EnumDescriptor(enumType);
         ChoiceActionItem item = new ChoiceActionItem(ed.GetCaption(current), current);
        
      }
   }

        
        protected override void OnActivated() { base.OnActivated(); }
        protected override void OnViewControlsCreated() { base.OnViewControlsCreated(); }
        protected override void OnDeactivated() { base.OnDeactivated(); }

        private void ImportSourceData_Execute(object sender, ParametrizedActionExecuteEventArgs e) {

        }

        private void GetSourceDataAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e) {


        }
    }
}
