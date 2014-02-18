﻿using System;
using FileHelpers;

namespace NpoMash.Erm.Hrm.Exchange {
 

    [FixedLengthRecord()]
    public class ImportMatrixTravelTime {

        [FieldFixedLength(4)]
        [FieldConverter(typeof(DateConverter))]
        public Int16 Year;

        [FieldFixedLength(2)]
        [FieldConverter(typeof(DateConverter))]
        public Int16 Month;

        [FieldFixedLength(5)]
        [FieldConverter(typeof(CodeConverter))]
        public String DepartmentCode;

        [FieldFixedLength(9)]
        [FieldConverter(typeof(CodeConverter))]
        public String OrderCode;

        [FieldFixedLength(12)]
        [FieldConverter(typeof(TimeConverter))]
        public Int64 TravelTime;

        internal class DateConverter : ConverterBase {
            public override object StringToField(string from) {
                return Convert.ToInt16(from.Trim());
            }
        }

        internal class CodeConverter : ConverterBase {
            public override object StringToField(string from) {
                return from.Trim();
            }
        }

        internal class TimeConverter : ConverterBase {
            public override object StringToField(string from) {
                return Convert.ToInt64(from.Remove(from.Length - 3, 1).Trim());
            }
        }
    } 
}