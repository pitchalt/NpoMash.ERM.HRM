using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NpoMash.Erm.Hrm.Simplex {
    //class SimpTab {
    //    public List<SimpRow> Rows;
    //    public List<SimpColumn> Columns;
    //    public List<CoefVector> Basis;
    //    SimpTab() {
    //        Rows = new List<SimpRow>();
    //        Columns = new List<SimpColumn>();
    //        Basis = new List<CoefVector>();
    //    }
    //}

    //class SimpRow {
    //    private Int64 _id;
    //    public Int64 id { get { return _id; } set { _id = value; } }
    //    private Double _P;
    //    public Double P { get { return _P; } set { _P = value; } }
    //    private SimpTab _Tab;
    //    public SimpTab Tab { get { return _Tab; } set { _Tab = value; } }
    //    public List<Coef> Coefs;
    //    public SimpRow(Int64 i) {
    //        id = i;
    //        P = 0;
    //        Tab = null;
    //        Coefs = new List<Coef>();
    //    }
    //}

    //class SimpColumn {
    //    private Int64 _id;
    //    public Int64 id { get { return _id; } set { _id = value; } }
    //    private SimpTab _Tab;
    //    public SimpTab Tab { get { return _Tab; } set { _Tab = value; } }
    //    private XVar _X;
    //    public XVar X { get { return _X; } set { _X = value; } }
    //    SimpColumn(Int64 i) {
    //        id = i;
    //        Tab = null;
    //        X = null;
    //    }
    //}

    //class CoefVector {
    //    private SimpColumn _Column;
    //    public SimpColumn Column { get { return _Column; } set { _Column = value; } }
    //    public List<Coef> Coefs;
    //    public CoefVector() {
    //        Column = null;
    //        Coefs = new List<Coef>();
    //    }
    //}

    //class Coef {
    //    private SimpRow _Row;
    //    public SimpRow Row { get { return _Row; } set { _Row = value; } }
    //    private CoefVector _Vect;
    //    public CoefVector Vect { get { return _Vect; } set { _Vect = value; } }
    //    Coef() {
    //        Row = null;
    //        Vect = null;
    //    }
    //}

    //class Func {
    //    public Dictionary<Int64, FuncElement> FuncElements;
    //    Func() {
    //        FuncElements = new Dictionary<Int64, FuncElement>();
    //    }
    //}

    //class FuncElement {
    //    private double _Coef;
    //    public double Coef { get { return _Coef; } set { _Coef = value; } }
    //    private double _Constant;
    //    public double Constant { get { return _Constant; } set { _Constant = value; } }
    //    private Int64 _Pow;
    //    public Int64 Pow { get { return _Pow; } set { _Pow = value; } }
    //    private XVar _x;
    //    public XVar x { get { return _x; } set { _x = value; } }
    //    private Func _Func;
    //    public Func Func { get { return _Func; } set { _Func = value; } }
    //    public FuncElement() {
    //        Coef = 1;
    //        Pow = 1;
    //        Constant = 0;
    //        x = null;
    //    }
    //}

    //class XVar {
    //    private Int64 _id;
    //    public Int64 id { get { return _id; } set { _id = value; } }
    //    private bool _IsAuxiliary;
    //    public bool IsAuxiliary { get { return _IsAuxiliary; } set { _IsAuxiliary = value; } }
    //    private Object _RefToRealObject;
    //    public Object RefToRealObject { get { return _RefToRealObject; } set { _RefToRealObject = value; } }
    //    public XVar(Int64 i) {
    //        IsAuxiliary = false;
    //        RefToRealObject = null;
    //        id = i;
    //    }
    //}

    //class CountProcess {
    //    private double _Eps;
    //    public double Eps { get { return _Eps; } set { _Eps = value; } }
    //    private ResultVector _CurrentResult;
    //    public ResultVector CurrentResult { get { return _CurrentResult; } set { _CurrentResult = value; } }
    //    private ResultVector _PreviousResult;
    //    public ResultVector PreviousResult { get { return _PreviousResult; } set { _PreviousResult = value; } }
    //    private ResultVector _BearingPlan;
    //    public ResultVector BearingPlan { get { return _BearingPlan; } set { _BearingPlan = value; } }

    //    public CountProcess() {
    //        Eps = 1;
    //        CurrentResult = null;
    //        PreviousResult = null;
    //        BearingPlan = null;
    //    }
    //}

    //class ResultVector {
    //    public Dictionary<Int64, Double> _VarValues;
    //    ResultVector() {
    //        _VarValues = new Dictionary<Int64, Double>();
    //    }
    //}

    struct SimplexLimitation {
        // свободный член (чему равно уравнение)
        public double freeMember;
        // коэффициенты уравнение, ключ - индекс переменной (минимально возможный - 0)
        public Dictionary<int, double> coefficients;
    }

    class SimplexTab {
        // список коэффициентов целевой функции
        public double[] target;
        // сама таблица
        public double[,] tab;
        // опорный план
        public double[] bearingPlan;
        // номера базисных векторов
        public double[] delta;
        public int[] basis;
        public int numberOfColumns;
        public int numberOfRows;
        public int maxIndexOfUnfictiveVar;



        public SimplexTab(List<SimplexLimitation> list_of_limitations, Dictionary<int, double> target_coefficients) {
            // максимальный индекс переменной, имеющейся в системе ограничений
            int max_variable_number = list_of_limitations.Max(x => x.coefficients.Keys.Max());
            // фиктивных переменных пока нет
            maxIndexOfUnfictiveVar = max_variable_number;
            int max_variable_number_with_fictive = max_variable_number;
            // определяем, сколько имеется ограничений
            int number_of_limitations = list_of_limitations.Count;
            numberOfRows = number_of_limitations;
            bearingPlan = new double[number_of_limitations];
            basis = new int[number_of_limitations];
            int current_limitation_index = 0;
            foreach (SimplexLimitation sl in list_of_limitations) {
                // по ходу заполняем столбец свободных членов
                bearingPlan[current_limitation_index] = sl.freeMember;
                // пробуем найти столбец, который возьмем за базисный
                int basis_index = 0;
                try {
                    basis_index = sl.coefficients.Keys.Where(x => sl.coefficients[x] == 1 && list_of_limitations
                        .Where(y => y.coefficients.ContainsKey(x) && y.coefficients[x] != 0).Count() == 1)
                        .First();
                }
                // если не нашли - вводим фиктивную переменную
                catch (ArgumentNullException) {
                    basis_index = ++max_variable_number_with_fictive;
                    sl.coefficients.Add(max_variable_number_with_fictive, 1);
                }
                basis[current_limitation_index] = basis_index;
                current_limitation_index++;
            }
            numberOfColumns = max_variable_number_with_fictive + 1;
            target = new double[numberOfColumns];
            delta = new double[numberOfColumns];

            // создаем и заполняем саму таблицу
            tab = new double[number_of_limitations, numberOfColumns];
            SimplexLimitation[] limitations_array = list_of_limitations.ToArray();
            for (int j = 0; j < numberOfColumns; j++) {
                if (target_coefficients.ContainsKey(j))
                    target[j] = target_coefficients[j];
                else target[j] = 0;
                // заполняем строку таблицы коэффициентами
                for (int i = 0; i < numberOfRows; i++) {
                    if (limitations_array[i].coefficients.ContainsKey(j))
                        tab[i, j] = limitations_array[i].coefficients[j];
                    else tab[i, j] = 0;
                }
            }
            CountDelta();
        }

        // эта функция обновляет ряд дельт в последней строке симплекс-таблицы,
        // перемножая соответсвующие вектора
        public void CountDelta() {
            for (int j = 0; j < numberOfColumns; j++) {
                double sum = 0;
                for (int i = 0; i < numberOfRows; i++) {
                    sum += tab[i, j] * target[basis[i]];
                }
                sum -= target[j];
                delta[j] = sum;
            }
        }
        // используется как опорный столбец при максимизации
        public int ColumnWithMinDelta() {
            int index = numberOfColumns - 1;
            double min_delt = delta[index];
            for (int i = 0; i < numberOfColumns - 1; i++) {
                if (delta[i] < min_delt) {
                    min_delt = delta[i];
                    index = i;
                }
            }
            return index;
        }
        // используется как опорный столбец при минимизации
        public int ColumnWithMaxDelta() {
            int index = numberOfColumns - 1;
            double max_delt = delta[index];
            for (int i = 0; i < numberOfColumns - 1; i++) {
                if (delta[i] > max_delt) {
                    max_delt = delta[i];
                    index = i;
                }
            }
            return index;
        }

        public int FindGuidingRow(int guiding_column) {
            bool founded = false;
            double min = 0;
            int guiding_row = 0;
            for (int i = 0; i < numberOfRows; i++) {
                if (tab[i, guiding_column] <= 0) continue;
                double x = bearingPlan[i] / tab[i, guiding_column];
                if (x < min || !founded) {
                    founded = true;
                    guiding_row = i;
                    min = x;
                }
            }
            if (!founded) throw new InvalidOperationException("There is no solution!");
            return guiding_row;
        }

        public void ConvertToNextTab(int guiding_row, int guiding_column) {
            double guiding_elem = tab[guiding_row, guiding_column];
            // делим опорную строку на разрешающий элемент
            for (int i = 0; i < numberOfColumns; i++)
                tab[guiding_row, i] /= guiding_elem;
            // во избежание погрешностей вручную устанавливаем разрешающий элемент = 1 в новой таблице
            tab[guiding_row, guiding_column] = 1;
            bearingPlan[guiding_row] /= guiding_elem;
            // теперь опорная строка входит в базис
            basis[guiding_row] = guiding_column;
            // делаем из опорного столбца единичный
            for (int i = 0; i < numberOfRows; i++) {
                // опорную строку уже преобразовывали, поэтму пропускаем
                if (i == guiding_row) continue;
                double row_multiplier = tab[i, guiding_column];
                // если множитель этой строки нулевой, то не будем толочь воду в ступе и сэкономим время
                if (row_multiplier == 0) continue;
                for (int j = 0; j < numberOfColumns; j++) {
                    // если это опрный столбец - то коэффициент напрямую обнуляем во избежание погрешностей
                    if (j == guiding_column) tab[i, j] = 0;
                    else tab[i, j] -= tab[guiding_row, j] * row_multiplier;
                }
                bearingPlan[i] -= bearingPlan[guiding_row] * row_multiplier;
            }
            // обновляем последнюю строку симплекс-таблицы по этой же формуле 
            // треугольника, так как это быстрее чем перемножать вектора
            double row_mul = delta[guiding_column];
            for (int i = 0; i < numberOfColumns; i++) {
                if (i == guiding_column) delta[i] = 0;
                else delta[i] -= tab[guiding_row, i] * row_mul;
            }

        }

    }
}
