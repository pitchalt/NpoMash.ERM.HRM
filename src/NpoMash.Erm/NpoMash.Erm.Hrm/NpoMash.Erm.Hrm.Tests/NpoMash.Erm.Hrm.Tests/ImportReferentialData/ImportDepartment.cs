using System;
using FileHelpers;

namespace NpoMash.Erm.Hrm.Tests.ImportReferentialData {


    [FixedLengthRecord()]
    public class ImportDepartment {

        [FieldFixedLength(5)]
        [FieldConverter(typeof(DepConverter))]
        public String Code;

        [FieldFixedLength(2)]
        [FieldConverter(typeof(TrimConverter))]
        public String Group;

        internal class TrimConverter : ConverterBase {
            public override object StringToField(string from) {
                return from.Trim();
            }
        }

        internal class DepConverter : ConverterBase {
            public override object StringToField(string from) {
                return Convert.ToString(Convert.ToInt32(from.Trim()));
            }
        }
    }
}
