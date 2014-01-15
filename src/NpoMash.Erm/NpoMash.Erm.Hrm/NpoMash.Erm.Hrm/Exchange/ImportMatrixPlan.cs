using System;
using FileHelpers;

namespace NpoMash.Erm.Hrm.Exchange {


    [FixedLengthRecord()]
    public class ImportMatrixPlan {

        [FieldFixedLength(4)]
        public Int16 Year;

        [FieldFixedLength(2)]
        public Int16 Month;

        [FieldFixedLength(5)]
        public String Department;

        [FieldFixedLength(8)]
        public String OrderCode;

        [FieldFixedLength(9)]
        public Int16 Norm;
    }
}