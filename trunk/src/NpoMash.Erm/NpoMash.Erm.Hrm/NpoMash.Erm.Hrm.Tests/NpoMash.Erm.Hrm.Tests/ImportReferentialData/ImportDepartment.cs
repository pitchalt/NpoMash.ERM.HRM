using System;
using FileHelpers;

namespace NpoMash.Erm.Hrm.Tests.ImportReferentialData {


    [FixedLengthRecord()]
    public class ImportDepartment {

        [FieldFixedLength(5)]
        [FieldConverter(typeof(TrimConverter))]
        public String Code;
        
        [FieldFixedLength(2)]
        [FieldConverter(typeof(TrimConverter))]
        public String Group;

        internal class TrimConverter : ConverterBase {
            public override object StringToField(string from) {
                String src = from.Trim();
                return src;
            }
        }
    }
}
