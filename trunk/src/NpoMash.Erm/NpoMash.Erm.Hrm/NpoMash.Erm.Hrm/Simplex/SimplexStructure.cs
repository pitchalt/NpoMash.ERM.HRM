using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NpoMash.Erm.Hrm.Simplex {
    class SimpTab {
        public List<SimpRow> Rows;
        public List<SimpColumn> Columns;
        public List<CoefVector> Basis;
        SimpTab() {
            Rows = new List<SimpRow>();
            Columns = new List<SimpColumn>();
            Basis = new List<CoefVector>();
        }
    }

    class SimpRow {
        private Int64 _id;
        public Int64 id { get { return _id; } set { _id = value; } }
        private Double _P;
        public Double P { get { return _P; } set { _P = value; } }
        private SimpTab _Tab;
        public SimpTab Tab { get { return _Tab; } set { _Tab = value; } }
        public List<Coef> Coefs;
        public SimpRow(Int64 i) {
            id = i;
            P = 0;
            Tab = null;
            Coefs = new List<Coef>();
        }
    }

    class SimpColumn {
        private Int64 _id;
        public Int64 id { get { return _id; } set { _id = value; } }
        private SimpTab _Tab;
        public SimpTab Tab { get { return _Tab; } set { _Tab = value; } }
        private XVar _X;
        public XVar X { get { return _X; } set { _X = value; } }
        SimpColumn(Int64 i) {
            id = i;
            Tab = null;
            X = null;
        }
    }

    class CoefVector {
        private SimpColumn _Column;
        public SimpColumn Column { get { return _Column; } set { _Column = value; } }
        public List<Coef> Coefs;
        public CoefVector() {
            Column = null;
            Coefs = new List<Coef>();
        }
    }

    class Coef {
        private SimpRow _Row;
        public SimpRow Row { get { return _Row; } set { _Row = value; } }
        private CoefVector _Vect;
        public CoefVector Vect { get { return _Vect; } set { _Vect = value; } }
        Coef() {
            Row = null;
            Vect = null;
        }
    }

    class Func {
        public Dictionary<Int64, FuncElement> FuncElements;
        Func() {
            FuncElements = new Dictionary<Int64, FuncElement>();
        }
    }

    class FuncElement {
        private double _Coef;
        public double Coef { get { return _Coef; } set { _Coef = value; } }
        private double _Constant;
        public double Constant { get { return _Constant; } set { _Constant = value; } }
        private Int64 _Pow;
        public Int64 Pow { get { return _Pow; } set { _Pow = value; } }
        private XVar _x;
        public XVar x { get { return _x; } set { _x = value; } }
        private Func _Func;
        public Func Func { get { return _Func; } set { _Func = value; } }
        public FuncElement() {
            Coef = 1;
            Pow = 1;
            Constant = 0;
            x = null;
        }
    }

    class XVar {
        private Int64 _id;
        public Int64 id { get { return _id; } set { _id = value; } }
        private bool _IsAuxiliary;
        public bool IsAuxiliary { get { return _IsAuxiliary; } set { _IsAuxiliary = value; } }
        private Object _RefToRealObject;
        public Object RefToRealObject { get { return _RefToRealObject; } set { _RefToRealObject = value; } }
        public XVar(Int64 i) {
            IsAuxiliary = false;
            RefToRealObject = null;
            id = i;
        }
    }

    class CountProcess {
        private double _Eps;
        public double Eps { get { return _Eps; } set { _Eps = value; } }
        private ResultVector _CurrentResult;
        public ResultVector CurrentResult { get { return _CurrentResult; } set { _CurrentResult = value; } }
        private ResultVector _PreviousResult;
        public ResultVector PreviousResult { get { return _PreviousResult; } set { _PreviousResult = value; } }
        private ResultVector _BearingPlan;
        public ResultVector BearingPlan { get { return _BearingPlan; } set { _BearingPlan = value; } }

        public CountProcess() {
            Eps = 1;
            CurrentResult = null;
            PreviousResult = null;
            BearingPlan = null;
        }
    }

    class ResultVector {
        public Dictionary<Int64, Double> _VarValues;
        ResultVector() {
            _VarValues = new Dictionary<Int64, Double>();
        }
    }
    
}
