using System;
using System.Text;
//
using FileHelpers;
//
using DevExpress;

namespace NpoMash.Erm.Hrm.Exchange {


    [FixedLengthRecord()]
    public class ImportMatrixPlan {

        [FieldFixedLength(4)]
        private Int16 _Year;
        public Int16 Year {
            get { return _Year; }
            set { _Year = value; }
        }

        [FieldFixedLength(2)]
        private Int16 _Month;
        public Int16 Month {
            get { return _Year; }
            set { _Month = value; }
        }

        [FieldFixedLength(5)]
        private String _Department;
        public String Department {
            get { return _Department; }
            set { _Department = value; }
        }

        [FieldFixedLength(8)]
        private String _Code;
        public String Code {
            get { return _Code; }
            set { _Code = value; }
        }

        [FieldFixedLength(9)]
        private String _Norm;
        public String Norm {
            get { return _Norm; }
            set { _Norm = value; }
        }

        public void ImportData() { }
    }
}