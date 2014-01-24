using System;
using FileHelpers;
using IntecoAG.ERM.FM.Order;

namespace NpoMash.Erm.Hrm.Tests.ImportReferentialData {
    
    
    [FixedLengthRecord()]
    public class ImportOrder {

        [FieldFixedLength(4)]
        public Int16 Year;

        [FieldFixedLength(2)]
        public Int16 Month;

        [FieldFixedLength(6)]
        public String Code;

        [FieldFixedLength(2)]
        public String TypeControl;

        [FieldFixedLength(7)]
        [FieldConverter(typeof(NormConverter))]
        public Decimal NormKB;

        [FieldFixedLength(7)]
        [FieldConverter(typeof(NormConverter))]
        public Decimal NormOZM;

        internal class NormConverter : ConverterBase {
            public override object StringToField(string from) {
                decimal src = Convert.ToDecimal(from);
                return src/100;
            }
        }
    }
}