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

namespace Samples.Module.Relations.ManyToMany {
    /// <summary>
    /// ����� �� ������ � ���������� M-N � ������� CD
    /// ���������� ����������� ����������� ������ CCLinkCD
    /// ��� ����� ��������������� ������� �����  1-N CCLinkCDs, � ��������� �����������
    /// ��������� ������ CD, ������� ������������� ������������� ����������� ����������������� ����������,
    /// � �������� �������������� ����������� ��������� ����������� �������� ��������� CCLinkCDs 
    /// � ������ ������� ��������������� ��� ���������� ���� ����������� �������������� ��������  ManyToManyAlias
    /// ������ ����������� ����������� ����������� ���������
    /// </summary>
    [NavigationItem("Relations/ManyToMany")]
    [Persistent("RelationsManyToManyCC")]
    // Specify more UI options using a declarative approach (http://documentation.devexpress.com/#Xaf/CustomDocument2701).
    public class CC : BaseObject { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (http://documentation.devexpress.com/#Xaf/CustomDocument3146).
        public CC(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place your initialization code here (http://documentation.devexpress.com/#Xaf/CustomDocument2834).
        }

        [Association("CC-CCLinkCD"), DevExpress.Xpo.Aggregated]
        public XPCollection<CCLinkCD> CCLinkCDs {
            get { return GetCollection<CCLinkCD>("CCLinkCDs"); }
        }

        /// <summary>
        /// ���������� �������������� �������� ManyToManyAlias � �������������� ������ GetList
        /// � �������� ��� ���� ��� �������� ��������� "CCLinkCDs" � ��� �������� � ������� �������� ���������
        /// ����������� ������ �� ������ �� ���������� "PCD"
        /// </summary>
        [ManyToManyAlias("CCLinkCDs", "PCD")]
        public IList<CD> SimpleWorkButNotLegal {
            get { return GetList<CD>("SimpleWorkButNotLegal"); }
        }
        /// <summary>
        /// ����������� ����������� ���������
        /// </summary>
        public class MyCDCollection : XPCollection<CD> {
            protected CC _CC;
            /// <summary>
            /// ����������� � ���������� � �������������� ���������� obj 
            ///  base(session, false) ������ �������� ������������ �������� 
            ///  ������ ��������� �� ���� ������
            /// </summary>
            /// <param name="session"></param>
            /// <param name="obj">������ �� ������ ��� ������� � �������� ���������</param>
            public MyCDCollection(Session session, CC obj): base(session, false)  {
                _CC = obj;
            }

            public override int BaseAdd(object newObject) {
                CD add_object = (CD)newObject;
                CCLinkCD link = _CC.CCLinkCDs.FirstOrDefault(x => x.PCD == add_object);
                if (link == null) {
                    link = new CCLinkCD(this.Session);
                    _CC.CCLinkCDs.Add(link);
                    link.PCD = add_object;
                }
                return base.BaseAdd(newObject);
            }

            public override bool BaseRemove(object theObject) {
                CD remove_object = (CD)theObject;
                CCLinkCD link = _CC.CCLinkCDs.FirstOrDefault(x => x.PCD == remove_object);
                if (link != null)
                    this.Session.Delete(link);
                return base.BaseRemove(theObject);
            }

            /// <summary>
            /// ��������� ��������� ����������� ������ (�� ������� ���������)
            /// � ����� � ���� �� ����������� ��������� ��� ��� ���
            /// Load ����� ���������� ����� ��� ��� BaseAdd ������� ���������� ���������� IsLoaded
            /// </summary>
            public override void Load() {
                if (!IsLoaded) {
                    base.Load();
                    foreach (CCLinkCD link in _CC.CCLinkCDs) {
                        base.BaseAdd(link.PCD);
                    }
                }
            }

        }

        private XPCollection<CD> _HardButOfficial;
        public XPCollection<CD> HardButOfficial {
            get {
                if (_HardButOfficial == null) {
                    _HardButOfficial = new MyCDCollection(this.Session, this);
                }
                return _HardButOfficial;
            }
        }

        protected override void OnLoading() {
            base.OnLoading();
            _HardButOfficial = null;
        }
    }
}
