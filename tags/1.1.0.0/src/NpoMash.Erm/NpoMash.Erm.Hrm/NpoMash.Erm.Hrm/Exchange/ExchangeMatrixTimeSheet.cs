using System;
using System.Globalization;

using FileHelpers;

namespace NpoMash.Erm.Hrm.Exchange {

    [FixedLengthRecord()]
    public class ExchangeMatrixTimeSheet {

        [FieldFixedLength(5)]
        [FieldConverter(typeof(NewDateConverter))]
        public Int16 Year;

        [FieldFixedLength(2)]
        [FieldConverter(typeof(NewDateConverter))]
        public Int16 Month;

        [FieldFixedLength(6)]
        [FieldConverter(typeof(DepConverter))]
        public String Department_Code;

        [FieldFixedLength(13)]
        [FieldConverter(typeof(TimeConverter))]
        public Decimal BaseWorkTime;

        [FieldFixedLength(13)]
        [FieldConverter(typeof(TimeConverter))]
        public Decimal TravelWorkTime;

        [FieldFixedLength(13)]
        [FieldConverter(typeof(TimeConverter))]
        public Decimal ConstantWorkTime;

        internal class NewDateConverter : ConverterBase {
            public override object StringToField(string from) {
                return Convert.ToInt16(from.Trim());
            }
        }

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
    }
}