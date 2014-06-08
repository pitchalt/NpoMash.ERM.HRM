using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using IntecoAG.ERM.HRM;
using IntecoAG.ERM.FM.Order;
using IntecoAG.ERM.HRM.Organization;

namespace NpoMash.Erm.Hrm.Salary.BringingStructure {
    public class Matrix {
        public Dictionary<String, Ord> orders;
        public Dictionary<String, Dep> deps;
        public Dictionary<Tuple<Dep, Ord>, Cell> cellsInDictionary;
        private Journal _journal;
        public Journal journal { get { return _journal; } set { _journal = value; } }

        public Matrix() {
            orders = new Dictionary<string, Ord>();
            deps = new Dictionary<string, Dep>();
            cellsInDictionary = new Dictionary<Tuple<Dep, Ord>, Cell>();
            journal = new Journal();
        }
    }

    public class Ord {
        //private String _code;
        //public String code { get { return _code; } set { _code = value; } }
        public List<Cell> cells;
        private Matrix _matrix;
        public Matrix matrix { get { return _matrix; } set { _matrix = value; } }
        private bool _isControlled;
        public bool isControlled { get { return _isControlled; } set { _isControlled = value; } }
        private fmCOrder _realOrder;
        public fmCOrder realOrder { get { return _realOrder; } set { _realOrder = value; } }

        public Ord() {
            cells = new List<Cell>();
            isControlled = false;
            matrix = null;
            realOrder = null;
        }
    }

    public class Dep {
        //private String _code;
        //public String code { get { return _code; } set { _code = value; } }
        public List<Cell> cells;
        private Matrix _matrix;
        public Matrix matrix { get { return _matrix; } set { _matrix = value; } }
        private Decimal _fact;
        public Decimal fact {
            get { return _fact; }
            set {
                freeSpace += value - fact;
                _fact = value;
            }
        }
        private Decimal _plan;
        public Decimal plan { get { return _plan; } set { _plan = value; } }
        private Decimal _planControlled;
        public Decimal planControlled { get { return _planControlled; } set { _planControlled = value; } }
        private Decimal _freeSpace;
        public Decimal freeSpace { get { return _freeSpace; } set { _freeSpace = value; } }
        private Department _realDepartment;
        public Department realDepartment { get { return _realDepartment; } set { _realDepartment = value; } }
        private Int32 _nonZeroControlled;
        public Int32 nonZeroControlled { get { return _nonZeroControlled; } set { _nonZeroControlled = value; } }
        private Int32 _nonZeroUncontrolled;
        public Int32 nonZeroUncontrolled { get { return _nonZeroUncontrolled; } set { _nonZeroUncontrolled = value; } }
        public Int32 numberOfNonZeroOrders { get { return nonZeroControlled + nonZeroUncontrolled; } }
        public Dep() {
            cells = new List<Cell>();
            _fact = 0;
            plan = 0;
            planControlled = 0;
            freeSpace = 0;
            matrix = null;
            realDepartment = null;
            nonZeroControlled = 0;
            nonZeroUncontrolled = 0;
        }
    }

    public class Cell {
        private Ord _order;
        public Ord order { get { return _order; } set { _order = value; } }
        private Dep _dep;
        public Dep dep { get { return _dep; } set { _dep = value; } }
        private bool _isNotZero;
        public bool isNotZero { get { return _isNotZero; } }
        private bool _isNeedsToRestore;
        public bool isNeedsToRestore { get { return _isNeedsToRestore; } }
        private Decimal _startTime;
        public Decimal startTime {
            get { return _startTime; }
            set {
                _startTime = value;
                if (value > 0) {
                    if (dep.fact > 0) {
                        dep.fact -= 1;
                        value -= 1;
                        _isNeedsToRestore = true;
                    }
                    _isNotZero = true;
                    if (order.isControlled)
                        dep.nonZeroControlled += 1;
                    else dep.nonZeroUncontrolled += 1;
                }
                time = value;

            }
        }
        private Decimal _time;
        public Decimal time {
            get { return _time; }
            set {
                if (value != time) {
                    Decimal x = value - time;
                    dep.plan += x;
                    if (order.isControlled) {
                        dep.planControlled += x;
                        dep.freeSpace -= x;
                    }
                }
                _time = value;
            }
        }

        public List<Operation> minusOperations;
        public List<Operation> plusOperations;

        public Cell() {
            _startTime = 0;
            _time = 0;
            _isNotZero = false;
            _isNeedsToRestore = false;
            //nonZeroUncontrolled = 0;
            minusOperations = new List<Operation>();
            plusOperations = new List<Operation>();
            order = null;
            dep = null;
        }

        public Cell BestCellToPutIn(out Decimal size) {
            Cell result = null;
            size = 0;
            try {
                result = order.cells
                    .Where<Cell>(x => x != this && x.isNotZero && x.dep.freeSpace > 0)
                    .OrderByDescending<Cell, Decimal>(x => x.dep.freeSpace)
                    .First<Cell>();
                //.ElementAt(0);
                size = result.dep.freeSpace;
            }
            catch (InvalidOperationException) { }
            return result;
        }

        public Cell BestCellToTakeFrom(out Decimal size) {
            Cell result = null;
            size = 0;
            try {
                result = order.cells
                    .Where<Cell>(x => x != this && x.time > 0 && (x.dep.nonZeroUncontrolled > 0 || x.dep.freeSpace < 0))
                    .OrderBy<Cell, Decimal>(x => x.dep.freeSpace).First<Cell>();
                //.ElementAt(0);
                Decimal result_free_space = result.dep.freeSpace;
                if (result_free_space > 0)
                    size = Math.Min(result_free_space, result.time);
                else
                    size = -result.time;
                //Math.Max(result.dep.freeSpace, );
            }
            catch (InvalidOperationException) { }
            return result;
        }

        public Decimal DistributionPotential() {
            Decimal result = 0;
            foreach (Cell cell in order.cells) {
                Decimal x = cell.dep.freeSpace;
                if (x > 0 && cell != this)
                    result += x;
            }
            return result;
        }

        public Decimal DistributionSize() {
            return Math.Min(DistributionPotential(), Math.Min(-dep.freeSpace, time - 1));
        }

        public Decimal DistributionDifficulty() {
            Decimal result = 0;
            Decimal ds = DistributionSize();
            List<Cell> list = new List<Cell>(order.cells.Where(x => x.dep.freeSpace > 0));
            list.OrderByDescending(x => x.dep.freeSpace);
            List<Cell>.Enumerator en = list.GetEnumerator();
            while (en.Current != null && ds > 0) {
                ds -= en.Current.dep.freeSpace;
                result++;
            }
            return result;
        }

        public Decimal DistributionQuality() {
            Decimal result = 0;
            Decimal dd = DistributionDifficulty();
            if (dd != 0)
                result = DistributionSize() / dd;
            return result;
        }

    }

    public class Operation {
        private Decimal _sum;
        public Decimal sum { get { return _sum; } set { _sum = value; } }
        private Int32 _operationNumber;
        public Int32 operationNumber { get { return _operationNumber; } set { _operationNumber = value; } }
        private Cell _takeFrom;
        public Cell takeFrom { get { return _takeFrom; } set { _takeFrom = value; } }
        private Cell _putInto;
        public Cell putInto { get { return _putInto; } set { _putInto = value; } }
        private OperationNode _operationNode;
        public OperationNode operationNode { get { return _operationNode; } set { _operationNode = value; } }

        public Operation() {
            sum = 0;
            operationNumber = 0;
            takeFrom = null;
            putInto = null;
            operationNode = null;
        }

        public Operation(Decimal time, Cell take_from, Cell put_into) {
            sum = Math.Abs(time);
            operationNumber = 0;
            takeFrom = null;
            putInto = null;
            operationNode = null;
            if (take_from != null) {
                takeFrom = take_from;
                take_from.minusOperations.Add(this);
                take_from.time -= sum;
            }
            if (put_into != null) {
                putInto = put_into;
                put_into.plusOperations.Add(this);
                put_into.time += sum;

            }
        }

        public bool CompareOperations(Operation op) {
            return false;
        }

        public void revertChanges() {
            takeFrom.time += sum;
            putInto.time -= sum;
            takeFrom.minusOperations.Remove(this);
            putInto.plusOperations.Remove(this);
        }

        public void print() { }

    }

    public class OperationNode {
        private Int32 _nodeNumber;
        public Int32 nodeNumber { get { return _nodeNumber; } set { _nodeNumber = value; } }
        private Operation _currentOperation;
        public Operation currentOperation { get { return _currentOperation; } set { _currentOperation = value; } }
        public List<Operation> abortedOperations;
        private Journal _journal;
        public Journal journal { get { return _journal; } set { _journal = value; } }

        public OperationNode() {
            currentOperation = null;
            abortedOperations = new List<Operation>();
            journal = null;
        }

        public void AbortOperation() {
            currentOperation.revertChanges();
            abortedOperations.Add(currentOperation);
            currentOperation = null;
        }

    }

    public class Journal {
        public Dictionary<Int32, OperationNode> operationsTree;
        private Int32 _stepNumber;
        public Int32 stepNumber { get { return _stepNumber; } set { _stepNumber = value; } }
        public Journal() {
            operationsTree = new Dictionary<Int32, OperationNode>();
            stepNumber = 0;
        }

        public void MakeOperation(Decimal sum, Cell take_from, Cell put_into) {
            if (sum == 0) return;
            stepNumber += 1;
            OperationNode on = new OperationNode();
            on.journal = this;
            on.nodeNumber = stepNumber;
            operationsTree[stepNumber] = on;
            Operation op = new Operation(sum, take_from, put_into);
            on.currentOperation = op;
            op.operationNumber = stepNumber;
            op.operationNode = on;
        }

        public void RevertToStep(Int32 n) {
            if (n < 0 || n > stepNumber) throw new InvalidOperationException("There is now such step!");
            for (Int32 i =stepNumber ; i > n ; i--) {
                operationsTree[i].AbortOperation();
                operationsTree.Remove(i);
                stepNumber--;
            }
            operationsTree[n].AbortOperation();
        }

        public void PrintLog() {
            for (Int32 i = 1 ; i <= stepNumber ; i++)
                operationsTree[i].currentOperation.print();
        }
    }
}