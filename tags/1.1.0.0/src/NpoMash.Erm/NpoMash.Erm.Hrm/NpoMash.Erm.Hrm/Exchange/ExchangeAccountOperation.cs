using System;
using System.Globalization;

using FileHelpers;

namespace NpoMash.Erm.Hrm.Exchange {


    [FixedLengthRecord()]
    public class ExchangeAccountOperation {

        [FieldFixedLength(2)]
        [FieldTrim(TrimMode.Both)]
        public String Sign;

        [FieldFixedLength(5)]
        [FieldTrim(TrimMode.Both)]
        public String Credit;

        [FieldFixedLength(5)]
        [FieldTrim(TrimMode.Both)]
        public String Debit;

        [FieldFixedLength(9)]
        [FieldTrim(TrimMode.Both)]
        public String OrderCode;

        [FieldFixedLength(6)]
        [FieldConverter(typeof(DepConverter))]
        public String DepartmentCode;

        [FieldFixedLength(4)]
        [FieldTrim(TrimMode.Both)]
        public String PayTypeCode;

        [FieldFixedLength(14)]
        [FieldConverter(typeof(TimeConverter))]
        public Decimal Time;

        [FieldFixedLength(17)]
        [FieldConverter(typeof(MoneyConverter))]
        public Decimal Money;

        internal class DepConverter : ConverterBase {
            public override object StringToField(string from) {
                return Convert.ToString(Convert.ToInt32(from.Trim()));
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