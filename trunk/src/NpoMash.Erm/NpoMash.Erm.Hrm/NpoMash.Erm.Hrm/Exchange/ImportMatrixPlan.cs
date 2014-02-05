using System;
using FileHelpers;

namespace NpoMash.Erm.Hrm.Exchange {


    [FixedLengthRecord()]
    public class ImportMatrixPlan {

        [FieldFixedLength(4)]
        [FieldConverter(typeof(NewDateConverter))]
        public Int16 Year;

        [FieldFixedLength(2)]
        [FieldConverter(typeof(NewDateConverter))]
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

        internal class NewDateConverter : ConverterBase {
            public override object StringToField(string from) {
                return Convert.ToInt16(from.Trim());
            }
        }

        internal class TrimConverter : ConverterBase {
            public override object StringToField(string from) {
                return from.Trim();
            }
        }

        internal class TimeConverter : ConverterBase {
            public override object StringToField(string from) {
                return Convert.ToInt64(from.Trim());
            }
        }
    }
}