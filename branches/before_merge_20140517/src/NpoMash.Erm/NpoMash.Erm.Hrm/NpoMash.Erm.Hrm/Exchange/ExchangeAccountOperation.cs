using System;
using System.Globalization;

using FileHelpers;

namespace NpoMash.Erm.Hrm.Exchange {
 

    [FixedLengthRecord()]
    public class ExchangeAccountOperation {

        [FieldFixedLength(2)]
        [FieldConverter(typeof(SignConverter))]
        public String Sign;

        [FieldFixedLength(5)]
        [FieldConverter(typeof(CodeConverter))]
        public String Credit;

        [FieldFixedLength(5)]
        [FieldConverter(typeof(CodeConverter))]
        public String Debit;

        [FieldFixedLength(9)]
        [FieldConverter(typeof(CodeConverter))]
        public String OrderCode;

        [FieldFixedLength(6)]
        [FieldConverter(typeof(DepConverter))]
        public String DepartmentCode;

        [FieldFixedLength(4)]
        [FieldConverter(typeof(CodeConverter))]
        public String PayTypeCode;

        [FieldFixedLength(14)]
        [FieldConverter(typeof(TimeConverter))]
        public Decimal Time;

        [FieldFixedLength(17)]
        [FieldConverter(typeof(MoneyConverter))]
        public Decimal Money;

        internal class SignConverter : ConverterBase {
            public override object StringToField(string from) {
                return from.Trim();
            }
        }

        internal class DepConverter : ConverterBase {
            public override object StringToField(string from) {
                return Convert.ToString(Convert.ToInt32(from.Trim()));
            }
        }

        internal class CodeConverter : ConverterBase {
            public override object StringToField(string from) {
                return from.Trim();
            }
        }

        internal class TimeConverter : ConverterBase {
            public override object StringToField(string from) {
                return Convert.ToDecimal(from.Trim(), CultureInfo.InvariantCulture.NumberFormat);
            }
        }

        internal class MoneyConverter : ConverterBase {
            public override object StringToField(string from) {
                return Convert.ToDecimal(from.Trim(), CultureInfo.InvariantCulture.NumberFormat);
            }
        }
    } 
}