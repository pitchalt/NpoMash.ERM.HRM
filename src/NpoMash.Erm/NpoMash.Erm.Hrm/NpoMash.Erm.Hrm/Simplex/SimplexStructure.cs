using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NpoMash.Erm.Hrm;
using NpoMash.Erm.Hrm.Salary;

namespace NpoMash.Erm.Hrm.Simplex {

    public struct SimplexLimitation {
        // свободный член (чему равно уравнение)
        public double freeMember;
        // коэффициенты уравнение, ключ - индекс переменной (минимально возможный - 0)
        public Dictionary<int, double> coefficients;
    }

    public class SimplexTab {
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

        class ReserveOptimizeCriteria {
            // сама симплекс-таблица, в которой будет происходить оптимизация линеаризированной целевой функции
            SimplexTab table;
            // коэффициент критерия при отклонении по ячейкам
            public int cellsCoefficient;
            // коэффициент критерия при отклонении по заказам
            public int ordersCoefficient;
            // связь переменных с контролируемыми ячейками
            public Dictionary<int, HrmMatrixCell> realControlledCells;
            // число переменных
            public int numberOfVariables;
            // связь переменных, содержащих резерв неконтролируемых заказов соответствующего подразделения
            public Dictionary<int, HrmMatrixColumn> realDepsWithUncontrolledOrders; 
            // плановое значение распределения минус постоянная часть в ячейке
            public Dictionary<int, double> cellsPlans;
            // текущая точка
            public Dictionary<int, double> current_values;
            // список переменных в заказе, доступ по коду
            public Dictionary<String, Dictionary<int, double>> variablesInOrder;
            // план по заказу минус постоянная часть в ячейках по заказу
            public Dictionary<String,double> ordersPlan;
            // весь резерв по подразделению (величина, которая не должна измениться)
            public Dictionary<String, double> departmentReserve;

            public ReserveOptimizeCriteria(HrmSalaryTaskProvisionMatrixReduction card,int cell_coef, int order_coef){
                cellsCoefficient = cell_coef;
                ordersCoefficient = order_coef;
                realControlledCells = new Dictionary<int, HrmMatrixCell>();
                realDepsWithUncontrolledOrders = new Dictionary<int, HrmMatrixColumn>();
                cellsPlans = new Dictionary<int, double>();
                variablesInOrder = new Dictionary<string, Dictionary<int, double>>();
                departmentReserve = new Dictionary<string, double>();
                ordersPlan = new Dictionary<string, double>();
                current_values = new Dictionary<int, double>();
                // теперь знаем, какие заказы контролируемые
                Dictionary<String, HrmPeriodOrderControl> controlled_orders = card.AllocParameters.OrderControls
                    .Where(x => x.TypeControl != IntecoAG.ERM.FM.Order.FmCOrderTypeControl.NO_ORDERED)
                    .ToDictionary(x => x.Order.Code);

                List<SimplexLimitation> limits = new List<SimplexLimitation>();
                // начинаем идти по подразделениям чтобы сразу формировать ограничения
                foreach(HrmMatrixColumn col in card.ProvisionMatrix.Columns) {
                    String dep_code = col.Department.BuhCode;
                    SimplexLimitation limit = new SimplexLimitation();
                    limit.coefficients = new Dictionary<int, double>();
                    // в этом подразделении перебираем все контролируемые ячейки
                    foreach (HrmMatrixCell cell in col.Cells
                        .Where(x => controlled_orders.ContainsKey(x.Row.Order.Code))) {
                        String ord_code = cell.Row.Order.Code;
                        // вытаскиваем значения из ячейки, чтобы не морочиться потом с приведением типов
                        double cell_plan = (double)cell.PlanMoney;
                        double cell_const = (double)cell.MoneyNoReserve;
                        double cell_reserve = (double)cell.SourceProvision;
                        // добавляем значение в словарь планов по заказу
                        if (ordersPlan.ContainsKey(ord_code))
                            ordersPlan[ord_code] += cell_plan-cell_const;
                        else ordersPlan.Add(ord_code,cell_plan-cell_const);
                        // связываем индекс переменной с реальной ячейкой
                        realControlledCells.Add(numberOfVariables, cell);
                        // добавляем резерв ячейки в словарь текущих значений ( понадобится для начального приближения)
                        current_values.Add(numberOfVariables, cell_reserve);
                        // коэффициент в ограничении при контролируемой переменной = 1
                        limit.coefficients.Add(numberOfVariables, 1);
                        // добавляем переменную в список переменных в конкретном заказе
                        if (!variablesInOrder.ContainsKey(ord_code))
                            variablesInOrder.Add(ord_code,new Dictionary<int,double>());
                        variablesInOrder[ord_code].Add(numberOfVariables, 0);
                        numberOfVariables++;
                    }
                    // сумма резерва в подразделении должна остаться неизменной
                    limit.freeMember = (double)col.Cells.Sum(x => x.SourceProvision);
                    // проверяем, есть ли неконтролируемые заказы в подразделении, строим список неконтролируемых ячеек
                    try {
                        double reserve = (double)col.Cells
                            .Where(x => !controlled_orders.ContainsKey(x.Row.Order.Code)).Sum(x => x.SourceProvision);
                        realDepsWithUncontrolledOrders.Add(numberOfVariables, col);

                    }
                        // если ничего не нашли - ну и не надо
                    catch (ArgumentNullException) { }



                }


            }

            // возвращает значение целевой функции при заданном векторе переменных
            public double funcValue(double[] vars) {
                double result = 0;
                double cells_result = 0;
                double orders_result = 0;
                foreach (int index in cellsPlans.Keys) {
                    double x = vars[index] - cellsPlans[index];
                    x *= x;
                    cells_result+=x;
                }
                foreach (String code in variablesInOrder.Keys) {
                    double x = 0;
                    foreach (int index in variablesInOrder[code].Keys)
                        x += vars[index];
                    x -= ordersPlan[code];
                    x *= x;
                    orders_result += x;
                }
                result = cells_result * 2 * cellsCoefficient + orders_result * 2 * ordersCoefficient;
                return result;
            }

            // считает частную производную от целевой функции по заданной переменной
            public double PartialDerivate(int index, double[] variables) {
                double result = 0;
                // если это не неконтролируемая ячейка
                if (realControlledCells.ContainsKey(index)) {
                    result += cellsCoefficient * 2 * (variables[index] - cellsPlans[index]);
                    double x = 0;
                    string code = realControlledCells[index].Row.Order.Code;
                    foreach (int key in variablesInOrder[code].Keys)
                        x += variables[key];
                    x -= ordersPlan[code];
                    x *= 2 * ordersCoefficient;
                }
                return result;
            }

            // подсчет всех частных производных по целевой функции, работает быстрее так как учитывает специфику
            // данной функции
            public double[] getArrayOfPartialDerivates(double[] variables) {
                double[] result = new double[variables.Count()];
                // это поячеечная составляющая
                for (int i = 0; i < variables.Count(); i++)
                    if (realControlledCells.ContainsKey(i))
                        result[i] += (variables[i] - cellsPlans[i]) * 2 * cellsCoefficient;
                // а это составляющая по заказу
                foreach (string code in variablesInOrder.Keys) {
                    double x = 0;
                    foreach (int key in variablesInOrder[code].Keys)
                        x += variables[key];
                    x -= ordersPlan[code];
                    x *= 2 * ordersCoefficient;
                    foreach (int key in variablesInOrder[code].Keys)
                        result[key] += x;
                }
                return result;
            }

            

        }

    }
}
