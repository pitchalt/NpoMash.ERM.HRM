using System;
using FileHelpers;

namespace NpoMash.Erm.Hrm.Exchange {


    [FixedLengthRecord()]
    public class ImportMatrixPlan {

        [FieldFixedLength(4)]
        [FieldConverter(typeof(TrimConverter))]
        public Int16 Year;

        [FieldFixedLength(2)]
        [FieldConverter(typeof(TrimConverter))]
        public Int16 Month;

        [FieldFixedLength(5)]
        [FieldConverter(typeof(TrimConverter))]
        public String Department_Code;

        [FieldFixedLength(9)]
        [FieldConverter(typeof(TrimConverter))]
        public String OrderCode;

        [FieldFixedLength(9)]
        [FieldConverter(typeof(TimeConverter))]
        public Int64 Time;

        internal class TrimConverter : ConverterBase {
            public override object StringToField(string from) {
                String src = from.Trim();
                return src;
            }
        }

        internal class TimeConverter : ConverterBase {
            public override object StringToField(string from) {
                Int64 src = Convert.ToInt64(from.Trim());
                return src;
            }
        }
    }
}