﻿using System;
using FileHelpers;

namespace NpoMash.Erm.Hrm.Exchange {

    [FixedLengthRecord()]
    public class ImportMatrixTimeSheet {

        [FieldFixedLength(4)]
        [FieldConverter(typeof(TrimConverter))]
        public Int16 Year;
        
        [FieldFixedLength(2)]
        [FieldConverter(typeof(TrimConverter))]
        public Int16 Month;
        
        [FieldFixedLength(5)]
        [FieldConverter(typeof(TrimConverter))]
        public String Department_Code;
        
        [FieldFixedLength(7)]
        [FieldConverter(typeof(WorkTimeConverter))]
        public Int64 BaseWorkTime;
        
        [FieldFixedLength(5)]
        [FieldConverter(typeof(WorkTimeConverter))]
        public Int64 ConstantWorkTime;
        
        [FieldFixedLength(9)]
        [FieldConverter(typeof(WorkTimeConverter))]
        public Int64 TravelWorkTime;

        internal class TrimConverter : ConverterBase {
            public override object StringToField(string from) {
                String src = from.Trim();
                return src;
            }
        }

        internal class WorkTimeConverter : ConverterBase {
            public override object StringToField(string from) {
                Int64 src = Convert.ToInt64(from.Trim());
                return src * 100;
            }
        }
    }
}