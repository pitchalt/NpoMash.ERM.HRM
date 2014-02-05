using System;
using FileHelpers;

namespace NpoMash.Erm.Hrm.Exchange {

    [FixedLengthRecord()]
    public class ImportMatrixTimeSheet {

        [FieldFixedLength(5)]
        public String Code;

        [FieldFixedLength(7)]
        public Int32 MatrixWorkTime;
    }
}