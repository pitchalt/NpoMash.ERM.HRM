﻿using System;
using FileHelpers;
using IntecoAG.ERM.FM.Order;

namespace NpoMash.Erm.Hrm.Tests.ImportReferentialData {
    
    
    [FixedLengthRecord()]
    public class ImportOrder {

        [FieldFixedLength(4)]
        [FieldConverter(typeof(TrimConverter))]
        public Int16 Year;

        [FieldFixedLength(2)]
        [FieldConverter(typeof(TrimConverter))]
        public Int16 Month;

        [FieldFixedLength(9)]
        [FieldConverter(typeof(TrimConverter))]
        public String Order_Code;

        [FieldFixedLength(2)]
        [FieldConverter(typeof(TrimConverter))]
        public String TypeControl;

        [FieldFixedLength(8)]
        [FieldConverter(typeof(NormConverter))]
        public Decimal NormKB;

        [FieldFixedLength(8)]
        [FieldConverter(typeof(NormConverter))]
        public Decimal NormOZM;

        internal class TrimConverter : ConverterBase {
            public override object StringToField(string from) {
                return from.Trim();
            }
        }

        internal class NormConverter : ConverterBase {
            public override object StringToField(string from) {
                return Convert.ToDecimal(from.Remove(from.Length - 3, 1).Trim());
            }
        }
    }
}