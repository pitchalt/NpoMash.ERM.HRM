﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NpoMash.Erm.Hrm.Salary;

namespace NpoMash.Erm.Hrm.Salary.ProvisionMatrixBringingStructure {

    public class ProvMat {
        public Dictionary<String,ProvDep> deps;
        public Dictionary<String,ProvOrd> ords;
        public List<ProvCell> cells;

        public ProvMat() {
            deps = new Dictionary<String, ProvDep>();
            ords = new Dictionary<String, ProvOrd>();
            cells = new List<ProvCell>();
        }
    }

    public class ProvCell {
        private ProvDep _dep;
        public ProvDep dep { get { return _dep; } set { _dep = value; } }
        private ProvOrd _ord;
        public ProvOrd ord { get { return _ord; } set { _ord = value; } }
        private Decimal _plan;
        public Decimal plan{get {return _plan;} set {_plan = value;}}
        private Decimal _constFact;
        public Decimal constFact { get { return _constFact; } set { _constFact = value; } }
        private Decimal _resereve;
        public Decimal reserve { get { return _resereve; } set { _resereve = value; } }
        private HrmMatrixCell _refToRealCell;
        public HrmMatrixCell refToRealCell { get { return _refToRealCell; } set { _refToRealCell = value; } }
        public Decimal planFactDifference { get { return plan - constFact - reserve; } }
        public ProvCell() {
            dep = null;
            ord = null;
            plan = 0;
            constFact = 0;
            reserve = 0;
            refToRealCell = null;
        }

    }

    public class ProvDep {
        private String _code;
        public String code { get { return _code; } set { _code = value; } }
        public List<ProvCell> cells;
        private Decimal _undistributedReserve;
        public Decimal undistributedReserve { get { return _undistributedReserve; } set { _undistributedReserve = value; } }
        private int _numberOfUncontrolledOrders;
        public int numberOfUncontrolledOrders { get { return _numberOfUncontrolledOrders; } set { _numberOfUncontrolledOrders = value; } }
        private int _numberOfControlledOrders;
        public int numberOfControlledOrders { get { return _numberOfControlledOrders; } set { _numberOfControlledOrders = value; } }
        private bool _isAlreadyBringed;
        public bool isAlreadyBringed { get { return _isAlreadyBringed; } set { _isAlreadyBringed = value; } }
        public ProvDep() {
            cells = new List<ProvCell>();
            undistributedReserve = 0;
            numberOfUncontrolledOrders = 0;
            numberOfControlledOrders = 0;
            isAlreadyBringed = false;
        }
    }

    public class ProvOrd {
        private String _code;
        public String code { get { return _code; } set { _code = value; } }
        public List<ProvCell> cells;
        private bool _isControlled;
        public bool isControlled { get { return _isControlled; } set { _isControlled = value; } }
        private Decimal _ordPlan;
        public Decimal ordPlan { get { return _ordPlan; } set { _ordPlan = value; } }
        public ProvOrd() {
            cells = new List<ProvCell>();
            isControlled = false;
            ordPlan = 0;
        }

    }

}