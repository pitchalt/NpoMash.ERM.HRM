using System;
using FileHelpers;

namespace NpoMash.Erm.Hrm.Tests.ImportReferentialData {


    [FixedLengthRecord()]
    public class ImportPayTypes {

        [FieldFixedLength(3)]
        [FieldConverter(typeof(PayTypeConverter))]
        public String Code;

        [FieldFixedLength(7)]
        [FieldConverter(typeof(PayTypeConverter))]
        public String From;

        [FieldFixedLength(7)]
        [FieldConverter(typeof(PayTypeConverter))]
        public String To;

        [FieldFixedLength(12)]
        [FieldConverter(typeof(PayTypeConverter))]
        public String ShortName;

        [FieldFixedLength(40)]
        [FieldConverter(typeof(PayTypeConverter))]
        public String Name;

        internal class PayTypeConverter : ConverterBase {
            public override object StringToField(string from) {
                return from.Trim();
            }
        }
    }
}