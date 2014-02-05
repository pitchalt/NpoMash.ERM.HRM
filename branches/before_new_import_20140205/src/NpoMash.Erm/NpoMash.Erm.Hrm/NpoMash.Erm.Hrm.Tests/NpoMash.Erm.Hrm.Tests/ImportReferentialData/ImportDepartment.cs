using System;
using FileHelpers;

namespace NpoMash.Erm.Hrm.Tests.ImportReferentialData {


    [FixedLengthRecord()]
    public class ImportDepartment {

        [FieldFixedLength(5)]
        public String Code;
        
        [FieldFixedLength(2)]
        public String Group;
    }
}
