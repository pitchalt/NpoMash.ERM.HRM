using System;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Collections.Generic;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;

using Samples.Module.Relations;
using Samples.Module.Relations.OneToOne;
using Samples.Module.Relations.ManyToMany;

namespace Samples.Module.FunctionalTests {

    public class TestApplication : XafApplication {
        public TestApplication() : base() {
            this.DatabaseVersionMismatch += new System.EventHandler<DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs>(this.TestWinFormApp_DatabaseVersionMismatch);
        }
        protected override DevExpress.ExpressApp.Layout.LayoutManager CreateLayoutManagerCore( bool simple ) {
            return null;
        }
        private void TestWinFormApp_DatabaseVersionMismatch( object sender, DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs e ) {
            e.Updater.Update();
            e.Handled = true;
        }
    }

    [TestFixture]
    public class UnitTest {
        private TestApplication application;
        private CA ca;
        private CB cb;
        private CC cc;
        private CD cd;
        [SetUp]
        public void setUp() {
            IObjectSpaceProvider object_space_provider = new XPObjectSpaceProvider( new MemoryDataStoreProvider() );
            application = new TestApplication();
            ModuleBase test_module = new ModuleBase();
            test_module.AdditionalExportedTypes.Add( typeof( CCLinkCD ) );
            application.Modules.Add( test_module );
            application.Setup("TestApplication", object_space_provider);
            IObjectSpace object_space = application.CreateObjectSpace();
            CA ca = object_space.CreateObject<CA>();
            CB cb = object_space.CreateObject<CB>();
            ca.PCB = cb;
            object_space.CommitChanges();
        }

        [Test]
        public void oneToOneRelationCA() {
            CA self = null;
            IObjectSpace object_space = application.CreateObjectSpace();
            CB newCB = object_space.CreateObject<CB>();
            var oldCA = object_space.GetObjects<CA>( null, true );
            foreach ( var each in oldCA ) {
                each.PCB = newCB;
                self = each;
            }
            object_space.CommitChanges();
            Assert.AreEqual( self.PCB, newCB );
        }

        [Test]
        public void oneToOneRelationCB() {
            CB self = null;
            IObjectSpace object_space = application.CreateObjectSpace();
            CA newCA = object_space.CreateObject<CA>();
            var oldCB = object_space.GetObjects<CB>( null, true );
            foreach ( var each in oldCB ) {
                each.PCA = newCA;
                self = each;
            }
            Assert.AreEqual( self.PCA, newCA );
        }

        [Test]
        public void ManyToManyRelation() {
            IObjectSpace object_space = application.CreateObjectSpace();
            CCLinkCD linker = object_space.CreateObject<CCLinkCD>();
            for ( int i = 0 ; i < 10 ; i++ ) {
                CC cc = object_space.CreateObject<CC>();
                CD cd = object_space.CreateObject<CD>();
                linker.PCC = cc;
                linker.PCD = cd;
                cc.CCLinkCDs.Add( linker );
            }
        }
    }
}