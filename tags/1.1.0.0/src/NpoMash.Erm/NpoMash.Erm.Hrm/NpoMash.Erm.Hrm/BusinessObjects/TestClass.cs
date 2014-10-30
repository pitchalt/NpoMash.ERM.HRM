using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NpoMash.Erm.Hrm.BusinessObjects {
    public class TestClass <T>
        where T : TestClass<T>.Int1
    {
        public class Int1 {
            public Int32 A;
        }

        public IList<T> Values;
        public void Test() {
            foreach (T vt in Values) {
                vt.A = 100;
            }
        }
    }

    public class TestClass2 : TestClass<TestClass2.Int2> {
        public class Int2 : Int1 {
            public Int32 B;
        }

        public void Test2() {
            foreach (var vt in Values) {
                vt.A = 100;
                vt.B = 200;
            }
        }

    }

}
