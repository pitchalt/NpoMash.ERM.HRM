using System;
using System.Text;
using FileHelpers;

using IntecoAG.ERM.HRM.Organization;

namespace NpoMash.Erm.Hrm.Tests.ImportReferentialData {


    [FixedLengthRecord()]
    public class ImportDepartments {

        [FieldFixedLength(10)]
        [FieldConverter(typeof(DepartmentConverter))]
        public String DepartmentCode;

        [FieldFixedLength(1)]
        [FieldConverter(typeof(GroupDepConverter))]
        public String DepartmentGroup;

        [FieldFixedLength(1)]
        [FieldConverter(typeof(BoolConverter))]
        public String IsClosed;

        [FieldFixedLength(6)]
        [FieldConverter(typeof(PayTypeCodeConverter))]
        public String BuhCode;

        [FieldFixedLength(80)]
        public String Name;

        [FieldFixedLength(250)]
        public String FullName;

        [FieldFixedLength(8)]
        public String OpenDate;

        [FieldFixedLength(8)]
        public String CloseDate;

        internal class DepartmentConverter : ConverterBase {
            public override object StringToField(string from) {
                return from.Trim();
            }
        }

        internal class GroupDepConverter : ConverterBase {
            public override object StringToField(string from) {
                return from.Trim();
            }
        }

        internal class BoolConverter : ConverterBase {
            public override object StringToField(string from) {
                return from.Trim();
            }
        }

        internal class PayTypeCodeConverter : ConverterBase {
            public override object StringToField(string from) {
                return from.Trim();
            }
        }
    }
}