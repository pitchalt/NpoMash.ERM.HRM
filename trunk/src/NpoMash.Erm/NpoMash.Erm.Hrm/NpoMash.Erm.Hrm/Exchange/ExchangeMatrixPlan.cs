using System;
using System.Globalization;

using FileHelpers;

namespace NpoMash.Erm.Hrm.Exchange {


    [FixedLengthRecord()]
    public class ExchangeMatrixPlan {

        [FieldFixedLength(5)]
        [FieldConverter(typeof(NewDateConverter))]
        [FieldAlign(AlignMode.Right)]
        public Int16 Year;

        [FieldFixedLength(2)]
        [FieldTrim(TrimMode.Both)]
        [FieldAlign(AlignMode.Right)]
        public String Month;

        [FieldFixedLength(6)]
        [FieldConverter(typeof(DepConverter))]
        [FieldAlign(AlignMode.Right)]
        public String DepartmentCode;

        [FieldFixedLength(9)]
        [FieldTrim(TrimMode.Both)]
        [FieldAlign(AlignMode.Left)]
        public String OrderCode;

        [FieldFixedLength(13)]
        [FieldConverter(typeof(TimeConverter))]
        [FieldAlign(AlignMode.Right)]
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

        internal class TimeConverter : ConverterBase {
            public override object StringToField(string from) {
                return Convert.ToDecimal(from.Trim(), CultureInfo.InvariantCulture.NumberFormat);
            }
        }
    }
}