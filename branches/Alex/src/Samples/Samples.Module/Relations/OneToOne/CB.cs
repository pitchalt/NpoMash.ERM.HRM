using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace Samples.Module.Relations.OneToOne {
    //[DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    [NavigationItem("Relations/OneToOne")]
    [Persistent("RelationsOneToOneCB")]
    // Specify more UI options using a declarative approach (http://documentation.devexpress.com/#Xaf/CustomDocument2701).
    public class CB : BaseObject { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (http://documentation.devexpress.com/#Xaf/CustomDocument3146).
        public CB(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place your initialization code here (http://documentation.devexpress.com/#Xaf/CustomDocument2834).
        }

        private CA _PCA;
        public CA PCA {
            get { return _PCA; }
            set {
                if (_PCA == value)
                    return;
                CA old = _PCA;
                _PCA = value;
                if (!IsLoading) {
                    if (old != null && old.PCB == this)
                        old.PCB = null;
                    if (value != null)
                        value.PCB = this;
                    OnChanged("PCA", old, value);
                }
            }
        }
    }
}
