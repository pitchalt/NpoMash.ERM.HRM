using System;
using FileHelpers;

namespace NpoMash.Erm.Hrm.Tests.ReferentialData {


    [FixedLengthRecord()]
    public class DepartmentImport {

        [FieldFixedLength(5)]
        public String Code;
        
        [FieldFixedLength(2)]
        public String Group;
    }
}
