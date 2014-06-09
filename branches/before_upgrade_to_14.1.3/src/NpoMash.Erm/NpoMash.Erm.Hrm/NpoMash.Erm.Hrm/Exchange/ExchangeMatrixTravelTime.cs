using System;
using System.Globalization;

using FileHelpers;

namespace NpoMash.Erm.Hrm.Exchange {


    [FixedLengthRecord()]
    public class ExchangeMatrixTravelTime {

        [FieldFixedLength(5)]
        [FieldConverter(typeof(DateConverter))]
        public Int16 Year;

        [FieldFixedLength(2)]
        [FieldConverter(typeof(DateConverter))]
        public Int16 Month;

        [FieldFixedLength(6)]
        [FieldConverter(typeof(DepConverter))]
        public String DepartmentCode;

        [FieldFixedLength(9)]
        [FieldTrim(TrimMode.Both)]
        public String OrderCode;

        [FieldFixedLength(13)]
        [FieldConverter(typeof(TimeConverter))]
        public Decimal TravelTime;

        internal class DateConverter : ConverterBase {
            public override object StringToField(string from) {
                return Convert.ToInt16(from.Trim());
            }
        }

        internal class DepConverter : ConverterBase {
            public override object StringToField(string from) {
                return Convert.ToString(Convert.ToInt64(from.Trim()));
            }
        }

        internal class TimeConverter : ConverterBase {
            public override object StringToField(string from) {
                return Convert.ToDecimal(from.Trim(), CultureInfo.InvariantCulture.NumberFormat);
            }
        }
    }
}