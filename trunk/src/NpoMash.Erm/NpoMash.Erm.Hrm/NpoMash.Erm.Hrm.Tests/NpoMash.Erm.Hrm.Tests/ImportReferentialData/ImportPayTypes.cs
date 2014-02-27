using System;
using FileHelpers;

namespace NpoMash.Erm.Hrm.Tests.ImportReferentialData {
    
    
    [FixedLengthRecord(FixedMode.AllowLessChars)]
    public class ImportPayTypes {
 
        [FieldFixedLength(3)]
        [FieldConverter(typeof(PayTypeConverter))]
        public String PayTypeCode;

        internal class PayTypeConverter : ConverterBase {
            public override object StringToField(string from) {
                return from.Trim();
            }
        }
    }
}