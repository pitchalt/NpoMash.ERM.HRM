using System;
using FileHelpers;

namespace NpoMash.Erm.Hrm.Tests.ReferentialData {
    
    
    [FixedLengthRecord()]
    public class OrdrerImport {

        [FieldFixedLength(8)]
        public String Code;
    }
}
