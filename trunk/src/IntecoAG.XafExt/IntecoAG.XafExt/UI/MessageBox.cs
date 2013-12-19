using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
//
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
//
namespace IntecoAG.XafExt.UI {

    [NonPersistent]
    [DefaultProperty("MessageCaption")]
    public class MessageBox : BaseObject { // You can use a different base persistent class based on your requirements (http://documentation.devexpress.com/#Xaf/CustomDocument3146).
        public MessageBox(Session session) : base(session) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place here your initialization code (check out http://documentation.devexpress.com/#Xaf/CustomDocument2834 for more details).
        }

        private String _MessageCaption;
        public String MessageCaption {
            get { return _MessageCaption; }
        }

        private String _MessageText;
        [Size(SizeAttribute.Unlimited)]
        public String MessageText {
            get { return _MessageText; }
        }

        public void Init(String caption, String text) {
            _MessageCaption = caption;
            _MessageText = text;
        }

        public static DialogController InitMessageBox(XafApplication app, ShowViewParameters view_parameters, String caption, String text) {
            IObjectSpace os = app.CreateObjectSpace();
            MessageBox msg_box = os.CreateObject<MessageBox>();
            msg_box.Init(caption, text);
            view_parameters.CreatedView = app.CreateDetailView(os, msg_box);
            view_parameters.TargetWindow = TargetWindow.NewModalWindow;
            var dialog_controller = app.CreateController<DialogController>();
//            dialog_controller.Accepting += new EventHandler<DialogControllerAcceptingEventArgs>(dialog_controller_Accepting);
//            dialog_controller.Cancelling += new EventHandler(dialog_controller_Cancelling);
            view_parameters.Controllers.Add(dialog_controller);
            return dialog_controller;
        }

    }
}
