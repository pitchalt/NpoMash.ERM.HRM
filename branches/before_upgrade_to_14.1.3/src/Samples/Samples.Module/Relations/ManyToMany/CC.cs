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
    /// Класс СС входит в ассоциацию M-N с классом CD
    /// Ассоциация реализуется посредством класса CCLinkCD
    /// Для этого устанавливается обычная связь  1-N CCLinkCDs, и создается виртуальная
    /// коллекция класса CD, которая редактируется пользователем посредством пользовательского интерфейса,
    /// в процессе редактирования виртуальной коллекции обновляется реальная коллекция CCLinkCDs 
    /// В данном примере рассматривается две реализации одна посредством неофициального атрибута  ManyToManyAlias
    /// Вторая посредством виртуальной самодельной коллекции
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
        /// Применение неофициального атрибута ManyToManyAlias и неофициального метода GetList
        /// У атрибута два поля имя реальной коллекции "CCLinkCDs" и имя свойства в объекте реальной коллекции
        /// содержащего ссылку на объект из ассоциации "PCD"
        /// </summary>
        [ManyToManyAlias("CCLinkCDs", "PCD")]
        public IList<CD> SimpleWorkButNotLegal {
            get { return GetList<CD>("SimpleWorkButNotLegal"); }
        }
        /// <summary>
        /// Виртуальная самодельная коллекция
        /// </summary>
        public class MyCDCollection : XPCollection<CD> {
            protected CC _CC;
            /// <summary>
            /// Конструктор с параметром с дополнительным параметром obj 
            ///  base(session, false) второй аргумент препятствует загрузке 
            ///  полной коллекции из базы данных
            /// </summary>
            /// <param name="session"></param>
            /// <param name="obj">Ссылка на объект для доступа к реальной коллекции</param>
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
            /// Поскольку коллекции загружаются лениво (по первому обращению)
            /// и нигде в коде не проверяется загружена она или нет
            /// Load может вызываться много раз при BaseAdd поэтому используем блокировку IsLoaded
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
