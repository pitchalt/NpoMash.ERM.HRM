﻿using System;
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

namespace NpoMash.Erm.Hrm.Salary.BringingStructure {
    public class Matrix {
        public Dictionary<String,Ord> orders;
        public Dictionary<String,Dep> deps;
        public Dictionary<Tuple<Dep, Ord>, Cell> cellsInDictionary;
    }

    public class Ord {
        private String _code;
        public String code { get { return _code; } set { _code = value; } }
        public List<Cell> cells;
        private Matrix _matrix;
        public Matrix matrix { get { return _matrix; } set { _matrix = value; } }
        private bool _isControlled;
        public bool isControlled { get { return _isControlled; } set { _isControlled = value; } }

    }

    public class Dep {
        private String _code;
        public String code { get { return _code; } set { _code = value; } }
        public List<Cell> cells;
        private Matrix _matrix;
        public Matrix matrix { get { return _matrix; } set { _matrix = value; } }
        private Int16 _fact;
        public Int16 fact { get { return _fact; } set { _fact = value; } }
        private Int16 _plan;
        public Int16 plan { get { return _plan; } set { _plan = value; } }
        private Int16 _planControlled;
        public Int16 planControlled { get { return _planControlled; } set { _planControlled = value; } }
        private Int16 _freeSpace;
        public Int16 freeSpace { get { return _freeSpace; } set { _freeSpace = value; } }
    }

    public class Cell{
        private Ord _order;
        public Ord order { get { return _order; } set { _order = value; } }
        private Dep _dep;
        public Dep dep { get { return _dep; } set { _dep = value; } }
    }

    public class Operation {
        public List<Operation> minusOperations;
        public List<Operation> plusOperations;
        private Int16 _sum;
        public Int16 sum { get { return _sum; } set { _sum = value; } }
        private Int16 _operationNumber;
        public Int16 operationNumber { get { return _operationNumber; } set { _operationNumber = value; } }
        private Cell _takeFrom;
        public Cell takeFrom { get { return _takeFrom; } set { _takeFrom = value; } }
        private Cell _putInto;
        public Cell putInto { get { return _putInto; } set { _putInto = value; } }
        private OperationNode _operationNode;
        public OperationNode operationNode { get { return _operationNode; } set { _operationNode = value; } }
        
        public bool CompareOperations(Operation op) {
            return false;
        }

        public void revertChanges() { }

        public void print() { }

    }

    public class OperationNode {
        private Operation _currentOperation;
        public Operation currentOperation { get { return _currentOperation; } set { _currentOperation = value; } }
        public List<Operation> abortedOperations;
        private Journal _journal;
        public Journal journal { get { return _journal; } set { _journal = value; } }

        public void AbortOperation(){}
    }

    public class Journal {
        public List<OperationNode> operationsTree;
        private OperationNode _currentLastNode;
        public OperationNode currentLastNode { get { return _currentLastNode; } set { _currentLastNode = value; } }

        public void RevertToStep(Int16 n) { }
        public void PrintLog() { }
        
    }

    

}
