using System;
using System.Globalization;

using FileHelpers;

namespace NpoMash.Erm.Hrm.Exchange {
    
    
    [FixedLengthRecord()]
    public class ExchangeConstOrderTime {

        [FieldFixedLength(5)]
        [FieldConverter(typeof(NewDateConverter))]
        public Int16 Year;

        [FieldFixedLength(2)]
        [FieldConverter(typeof(NewDateConverter))]
        public Int16 Month;

        [FieldFixedLength(6)]
        [FieldConverter(typeof(DepConverter))]
        public String DepartmentCode;

        [FieldFixedLength(9)]
        [FieldConverter(typeof(TrimConverter))]
        public String OrderCode;

        [FieldFixedLength(13)]
        [FieldConverter(typeof(TimeConverter))]
        public Decimal Time;

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

        internal class TrimConverter : ConverterBase {
            public override object StringToField(string from) {
                return from.Trim();
            }
        }

        internal class TimeConverter : ConverterBase {
            public override object StringToField(string from) {
                return Convert.ToDecimal(from.Trim(), CultureInfo.InvariantCulture.NumberFormat);
            }
        }
    }
}