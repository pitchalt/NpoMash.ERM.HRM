using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NpoMash.Erm.Hrm.Salary;

namespace NpoMash.Erm.Hrm.Salary.ProvisionMatrixBringingStructure {

    public class ProvMat {
        public Dictionary<String,ProvDep> deps;
        public Dictionary<String,ProvOrd> ords;
        public List<ProvCell> cells;
        // значение целевой функции последней контрольной точки
        private decimal _checkPointTargetFunctionValue;
        public decimal checkPointTargetFunctionValue { get { return _checkPointTargetFunctionValue; } }

        public ProvMat() {
            deps = new Dictionary<String, ProvDep>();
            ords = new Dictionary<String, ProvOrd>();
            cells = new List<ProvCell>();
            
        }

        // сделать контрольную точку
        public void MakeCheckPoint() {
            foreach (ProvCell cell in cells)
                cell.MakeCheckPoint();
            _checkPointTargetFunctionValue = CountTargetFunctionValue();

        }

        public void RevertToCheckPoint() {
            foreach (ProvCell cell in cells)
                cell.RevertToCheckPoint();
        }

        public decimal CountTargetFunctionValue() {
            // коэффициенты значимости критериев
            int cells_coefficient = 1;
            int ords_coefficient = 10;
            // здесь расчет значения целевой функции
            decimal result = ords_coefficient * ords.Values.Where(x => x.isControlled).Sum(x =>
                    (x.ordPlan - x.cells.Sum(y => y.reserve + y.realBase)) *
                    (x.ordPlan - x.cells.Sum(y => y.reserve + y.realBase)) / (x.ordPlan + 1)) 
                +
                cells_coefficient * cells.Where(x => x.ord.isControlled).Sum(x =>
                    (x.plan - x.realBase - x.reserve) * (x.plan - x.realBase - x.reserve) / (x.plan + 1));
            return result;
        }

    }

    public class ProvCell {
        // ссылка на подразделение
        private ProvDep _dep;
        public ProvDep dep { get { return _dep; } set { _dep = value; } }
        // ссылка на заказ
        private ProvOrd _ord;
        public ProvOrd ord { get { return _ord; } set { _ord = value; } }
        // план по ячейке
        private Decimal _plan;
        public Decimal plan{get {return _plan;} set {_plan = value;}}
        // неизменная составляющая в ячейке, или "база"
        public Decimal constFact { get { return realBase + virtualBase; } }
        // база из реальных данных
        private Decimal _realBase;
        public Decimal realBase { get { return _realBase; } set { _realBase = value; } }
        // виртуальная база, используемая в эвристическом алгоритме приведения
        private Decimal _virtualBase;
        public Decimal virtualBase { get { return _virtualBase; } set {
            // сумма реальной и виртуальной базы не может быть отрицательной
            if (realBase + value < 0) throw new InvalidOperationException("Sum of real and virtual base must be greater or equal 0, but was" + (realBase + value).ToString());
            _virtualBase = value; } }
        // резерв в ячейке
        private Decimal _resereve;
        public Decimal reserve { get { return _resereve; } set { _resereve = value; } }
        // ссылка на ячейку для быстрой записи результатов расчета
        private HrmMatrixCell _refToRealCell;
        public HrmMatrixCell refToRealCell { get { return _refToRealCell; } set { _refToRealCell = value; } }
        // разница между планом и фактом, то есть величина недогруза
        public Decimal planFactDifference { get { return plan - constFact - reserve; } }
        // значения виртуальной базы и резерва ячейки в последней контрольной точке
        private Decimal _checkPointVirtualBase;
        public Decimal checkPointVirtualBase { get { return _checkPointVirtualBase; } }
        private Decimal _checkPointReserve;
        public Decimal checkPointReserve { get { return _checkPointReserve; } }

        public ProvCell() {
            dep = null;
            ord = null;
            plan = 0;
            realBase = 0;
            virtualBase = 0;
            reserve = 0;
            _checkPointReserve = 0;
            _checkPointVirtualBase = 0;
            virtualBase = 0;
            refToRealCell = null;
        }

        public void MakeCheckPoint() {
            _checkPointVirtualBase = virtualBase;
            _checkPointReserve = reserve;
        }

        public void RevertToCheckPoint() {
            virtualBase = checkPointVirtualBase;
            reserve = checkPointReserve;

        }

    }

    public class ProvDep {
        // код подразделения
        private String _code;
        public String code { get { return _code; } set { _code = value; } }
        // список ячеек в подразделении
        public List<ProvCell> cells;
        // нераспределенный резерв
        private Decimal _undistributedReserve;
        public Decimal undistributedReserve { get { return _undistributedReserve; } set { _undistributedReserve = value; } }
        // число неконтролируемых заказов
        private int _numberOfUncontrolledOrders;
        public int numberOfUncontrolledOrders { get { return _numberOfUncontrolledOrders; } set { _numberOfUncontrolledOrders = value; } }
        // число контролируемых заказов
        private int _numberOfControlledOrders;
        public int numberOfControlledOrders { get { return _numberOfControlledOrders; } set { _numberOfControlledOrders = value; } }
        // отметка что подразделение уже приведено (предварительно)
        private bool _isAlreadyBringed;
        public bool isAlreadyBringed { get { return _isAlreadyBringed; } set { _isAlreadyBringed = value; } }
        // сколько всего резерва в подразделении, пригодится для ускорения работы и контроля что ничего не потеряно
        private decimal _reserveOfDep;
        public decimal reserveOfDep { get { return _reserveOfDep; } set { _reserveOfDep = value; } }

        // для отладки
        private String _nonBuhDep;
        public String nonBuhDep { get { return _nonBuhDep; } set { _nonBuhDep = value; } }

        public ProvDep() {
            cells = new List<ProvCell>();
            undistributedReserve = 0;
            numberOfUncontrolledOrders = 0;
            numberOfControlledOrders = 0;
            isAlreadyBringed = false;
        }

        // метод для подготовки подразделения к повторному перераспределению
        public void unbring() {
            foreach (ProvCell cell in cells)
                cell.reserve = 0;
            undistributedReserve = reserveOfDep;
            isAlreadyBringed = false;
        }

    }

    public class ProvOrd {
        // код заказа
        private String _code;
        public String code { get { return _code; } set { _code = value; } }
        // список ячеек в заказе
        public List<ProvCell> cells;
        // признак контролируемости заказа
        private bool _isControlled;
        public bool isControlled { get { return _isControlled; } set { _isControlled = value; } }
        // план по заказу
        private Decimal _ordPlan;
        public Decimal ordPlan { get { return _ordPlan; } set { _ordPlan = value; } }
        // заказ приведен окончательно, больше не будем трогать виртуальную базу
        // (однако отклонение в данном заказе еще может поменяться при приведении других связанных с ним полностью контролируемых заказов)
        private bool _isFinallyBringed;
        public bool isFinallyBringed { get { return _isFinallyBringed; } set { _isFinallyBringed = value; } }
        // величина перегруза в заказе (ввел свойство, уже достало писать это каждый раз)
        public Decimal ordDeviation { get { return cells.Sum(x => x.reserve + x.realBase) - ordPlan; } }
        public ProvOrd() {
            cells = new List<ProvCell>();
            isControlled = false;
            ordPlan = 0;
            isFinallyBringed = false;
        }

    }

}
