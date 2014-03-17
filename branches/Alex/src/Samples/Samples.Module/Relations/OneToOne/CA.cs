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
    /// <summary>
    /// CA и CB классы входят в ассициацию 1-1
    /// </summary>
    [NavigationItem("Relations/OneToOne")]
    [Persistent("RelationsOneToOneCA")]
    // Specify more UI options using a declarative approach (http://documentation.devexpress.com/#Xaf/CustomDocument2701).
    public class CA : BaseObject { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (http://documentation.devexpress.com/#Xaf/CustomDocument3146).
        public CA(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place your initialization code here (http://documentation.devexpress.com/#Xaf/CustomDocument2834).
        }

        private CB _PCB;
        public CB PCB {
            get { return _PCB; }
            set {
                if (_PCB == value) 
                    return;
                CB old = _PCB;
                _PCB = value;
                if (!IsLoading) {
                    if (old != null && old.PCA == this)
                        old.PCA = null;
                    if (value != null)
                        value.PCA = this;
                    OnChanged("PCB", old, value);
                }
            }
        }
    }
}
