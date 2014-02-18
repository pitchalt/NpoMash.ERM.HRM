using System;
using FileHelpers;

namespace NpoMash.Erm.Hrm.Exchange {
 

    [FixedLengthRecord()]
    public class ImportAccountOperation {

        [FieldFixedLength(2)]
        [FieldConverter(typeof(SignConverter))]
        public String Sign;

        [FieldFixedLength(5)]
        [FieldConverter(typeof(CodeConverter))]
        public String CreditCode;

        [FieldFixedLength(5)]
        [FieldConverter(typeof(CodeConverter))]
        public String DebitCode;

        [FieldFixedLength(9)]
        [FieldConverter(typeof(CodeConverter))]
        public String OrderCode;

        [FieldFixedLength(6)]
        [FieldConverter(typeof(CodeConverter))]
        public String DepartmentCode;

        [FieldFixedLength(4)]
        [FieldConverter(typeof(CodeConverter))]
        public String PayTypeCode;

        [FieldFixedLength(14)]
        [FieldConverter(typeof(TimeConverter))]
        public Int64 Time;

        [FieldFixedLength(17)]
        [FieldConverter(typeof(MoneyConverter))]
        public Int64 Money;

        internal class SignConverter : ConverterBase {
            public override object StringToField(string from) {
                return from.Trim();
            }
        }

        internal class CodeConverter : ConverterBase {
            public override object StringToField(string from) {
                return from.Trim();
            }
        }

        internal class TimeConverter : ConverterBase {
            public override object StringToField(string from) {
                return Convert.ToInt64(from.Remove(from.Length - 4, 1).Trim());
            }
        }

        internal class MoneyConverter : ConverterBase {
            public override object StringToField(string from) {
                return Convert.ToInt64(from.Remove(from.Length - 3, 1).Trim());
            }
        }
    } 
}