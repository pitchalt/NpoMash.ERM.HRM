using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NpoMash.Erm.Hrm.Salary.Matrix {
    interface TestA {
    }

    interface TestB: TestA {
    }

    interface TestC {
        TestA A { get; }
        void M1(TestA arg1);
        TestA M2(TestA arg1);
    }

    interface TestD: TestC {
        TestB A { get; }
        void M1(TestB arg1);
        TestB M2(TestA arg1);
    }

    class TestE : TestD {

        public TestB A {
            get { throw new NotImplementedException(); }
        }

        TestA TestC.A {
            get { throw new NotImplementedException(); }
        }


        public void M1(TestB arg1) {
            throw new NotImplementedException();
        }


        public void M1(TestA arg1) {
            throw new NotImplementedException();
        }


        public TestB M2(TestA arg1) {
            throw new NotImplementedException();
        }


        TestA TestC.M2(TestA arg1) {
            throw new NotImplementedException();
        }
    }
}
