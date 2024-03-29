﻿using System;
using System.Globalization;

using FileHelpers;
using IntecoAG.ERM.FM.Order;

namespace NpoMash.Erm.Hrm.Tests.ImportReferentialData {


    [FixedLengthRecord()]
    public class ImportControlledOrder {

        [FieldFixedLength(5)]
        [FieldConverter(typeof(NewDateConverter))]
        public Int16 Year;

        [FieldFixedLength(2)]
        [FieldConverter(typeof(NewDateConverter))]
        public Int16 Month;

        [FieldFixedLength(9)]
        [FieldConverter(typeof(TrimConverter))]
        public String Code;

        [FieldFixedLength(2)]
        [FieldConverter(typeof(TrimConverter))]
        public String TypeControl;

        [FieldFixedLength(9)]
        [FieldConverter(typeof(NormConverter))]
        public Decimal NormKB;

        [FieldFixedLength(9)]
        [FieldConverter(typeof(NormConverter))]
        public Decimal NormOZM;

        internal class NewDateConverter : ConverterBase {
            public override object StringToField(string from) {
                return Convert.ToInt16(from.Trim());
            }
        }

        internal class TrimConverter : ConverterBase {
            public override object StringToField(string from) {
                return from.Trim();
            }
        }

        internal class NormConverter : ConverterBase {
            public override object StringToField(string from) {
                return Convert.ToDecimal(from.Trim(), CultureInfo.InvariantCulture.NumberFormat);
            }
        }
    }
}