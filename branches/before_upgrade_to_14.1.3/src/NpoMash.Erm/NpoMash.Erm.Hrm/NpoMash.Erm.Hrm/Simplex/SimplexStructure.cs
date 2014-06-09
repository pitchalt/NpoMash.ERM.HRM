using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NpoMash.Erm.Hrm;
using NpoMash.Erm.Hrm.Salary;

namespace NpoMash.Erm.Hrm.Simplex {

    public struct SimplexLimitation {
        // свободный член (чему равно уравнение)
        public Decimal freeMember;
        // коэффициенты уравнения, ключ - индекс переменной (минимально возможный - 0)
        public Dictionary<int, Decimal> coefficients;
    }

    public class SimplexTab {
        // список коэффициентов целевой функции
        public Decimal[] target;
        // сама таблица
        public Decimal[,] tab;
        // опорный план
        public Decimal[] bearingPlan;
        // номера базисных векторов
        public Decimal[] delta;
        public int[] basis;
        public int numberOfColumns;
        public int numberOfRows;
        public int maxIndexOfUnfictiveVar;



        public SimplexTab(List<SimplexLimitation> list_of_limitations, Dictionary<int, Decimal> target_coefficients) {
            // максимальный индекс переменной, имеющейся в системе ограничений
            int max_variable_number = list_of_limitations.Max(x => x.coefficients.Keys.Max());
            // фиктивных переменных пока нет
            maxIndexOfUnfictiveVar = max_variable_number;
            int max_variable_number_with_fictive = max_variable_number;
            // определяем, сколько имеется ограничений
            int number_of_limitations = list_of_limitations.Count;
            numberOfRows = number_of_limitations;
            bearingPlan = new Decimal[number_of_limitations];
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
            target = new Decimal[numberOfColumns];
            delta = new Decimal[numberOfColumns];

            // создаем и заполняем саму таблицу
            tab = new Decimal[number_of_limitations, numberOfColumns];
            SimplexLimitation[] limitations_array = list_of_limitations.ToArray();
            for (int j = 0 ; j < numberOfColumns ; j++) {
                if (target_coefficients.ContainsKey(j))
                    target[j] = target_coefficients[j];
                else target[j] = 0;
                // заполняем строку таблицы коэффициентами
                for (int i = 0 ; i < numberOfRows ; i++) {
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
            for (int j = 0 ; j < numberOfColumns ; j++) {
                Decimal sum = 0;
                for (int i = 0 ; i < numberOfRows ; i++) {
                    sum += tab[i, j] * target[basis[i]];
                }
                sum -= target[j];
                delta[j] = sum;
            }
        }
        // используется как опорный столбец при максимизации
        public int ColumnWithMinDelta() {
            int index = numberOfColumns - 1;
            Decimal min_delt = delta[index];
            for (int i = 0 ; i < numberOfColumns - 1 ; i++) {
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
            Decimal max_delt = delta[index];
            for (int i = 0 ; i < numberOfColumns - 1 ; i++) {
                if (delta[i] > max_delt) {
                    max_delt = delta[i];
                    index = i;
                }
            }
            return index;
        }

        public int FindGuidingRow(int guiding_column) {
            bool founded = false;
            Decimal min = 0;
            int guiding_row = 0;
            for (int i = 0 ; i < numberOfRows ; i++) {
                if (tab[i, guiding_column] <= 0) continue;
                Decimal x = bearingPlan[i] / tab[i, guiding_column];
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
            Decimal guiding_elem = tab[guiding_row, guiding_column];
            // делим опорную строку на разрешающий элемент
            for (int i = 0 ; i < numberOfColumns ; i++)
                tab[guiding_row, i] /= guiding_elem;
            // во избежание погрешностей вручную устанавливаем разрешающий элемент = 1 в новой таблице
            tab[guiding_row, guiding_column] = 1;
            bearingPlan[guiding_row] /= guiding_elem;
            // теперь опорная строка входит в базис
            basis[guiding_row] = guiding_column;
            // делаем из опорного столбца единичный
            for (int i = 0 ; i < numberOfRows ; i++) {
                // опорную строку уже преобразовывали, поэтму пропускаем
                if (i == guiding_row) continue;
                Decimal row_multiplier = tab[i, guiding_column];
                // если множитель этой строки нулевой, то не будем толочь воду в ступе и сэкономим время
                if (row_multiplier == 0) continue;
                for (int j = 0 ; j < numberOfColumns ; j++) {
                    // если это опрный столбец - то коэффициент напрямую обнуляем во избежание погрешностей
                    if (j == guiding_column) tab[i, j] = 0;
                    else tab[i, j] -= tab[guiding_row, j] * row_multiplier;
                }
                bearingPlan[i] -= bearingPlan[guiding_row] * row_multiplier;
            }
            // обновляем последнюю строку симплекс-таблицы по этой же формуле 
            // треугольника, так как это быстрее чем перемножать вектора
            Decimal row_mul = delta[guiding_column];
            for (int i = 0 ; i < numberOfColumns ; i++) {
                if (i == guiding_column) delta[i] = 0;
                else delta[i] -= tab[guiding_row, i] * row_mul;
            }


        }

        // замена коэффициентов целевой функции на коэффициенты из массива и пересчет дельт
        public void ReplaceTargetFuction(Decimal[] coefficents) {
            // заменяем коэффициенты при целевой функции
            for (int i = 0 ; i < coefficents.Count() ; i++) {
                target[i] = coefficents[i];
            }
            // вручную пересчитали дельту
            CountDelta();
        }


    }

    public class ReserveSimplexBringingStructure {
        // сама симплекс-таблица, в которой будет происходить оптимизация линеаризированной целевой функции
        public SimplexTab table;
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
        public Dictionary<int, Decimal> cellsPlans;
        // текущая точка
        public Dictionary<int, Decimal> current_values;
        // список переменных в заказе, доступ по коду
        public Dictionary<String, Dictionary<int, Decimal>> variablesInOrder;
        // план по заказу минус постоянная часть в ячейках по заказу
        public Dictionary<String, Decimal> ordersPlan;
        // весь резерв по подразделению (величина, которая не должна измениться)
        public Dictionary<String, Decimal> departmentReserve;
        // ограничения для таблицы, будут удобны и потом
        public Dictionary<String, SimplexLimitation> simpLimits;
        // относительный коэффициент плана заказа
        public Dictionary<String, Decimal> orderPlanCoef;

        public ReserveSimplexBringingStructure(HrmSalaryTaskProvisionMatrixReduction card, int cell_coef, int order_coef) {
            cellsCoefficient = cell_coef;
            ordersCoefficient = order_coef;
            realControlledCells = new Dictionary<int, HrmMatrixCell>();
            realDepsWithUncontrolledOrders = new Dictionary<int, HrmMatrixColumn>();
            cellsPlans = new Dictionary<int, Decimal>();
            variablesInOrder = new Dictionary<string, Dictionary<int, Decimal>>();
            departmentReserve = new Dictionary<string, Decimal>();
            ordersPlan = new Dictionary<string, Decimal>();
            current_values = new Dictionary<int, Decimal>();
            simpLimits = new Dictionary<string, SimplexLimitation>();
            orderPlanCoef = new Dictionary<string, Decimal>();
            // теперь знаем, какие заказы контролируемые
            Dictionary<String, HrmAllocParameterOrderControl> controlled_orders = card.AllocParameters.OrderControls
                .Where(x => x.TypeControl != IntecoAG.ERM.FM.Order.FmCOrderTypeControl.NO_ORDERED)
                .ToDictionary(x => x.Order.Code);

            List<SimplexLimitation> limits = new List<SimplexLimitation>();
            // начинаем идти по подразделениям чтобы сразу формировать ограничения
            foreach (HrmMatrixColumn col in card.ProvisionMatrix.Columns) {
                String dep_code = col.Department.BuhCode;
                SimplexLimitation limit = new SimplexLimitation();
                limit.coefficients = new Dictionary<int, Decimal>();
                // в этом подразделении перебираем все контролируемые ячейки
                foreach (HrmMatrixCell cell in col.Cells
                    .Where(x => controlled_orders.ContainsKey(x.Row.Order.Code))) {
                    String ord_code = cell.Row.Order.Code;
                    // вытаскиваем значения из ячейки, чтобы не морочиться потом с приведением типов
                    Decimal cell_plan = cell.PlanMoney;
                    Decimal cell_const = cell.MoneyNoReserve;
                    Decimal cell_reserve = cell.SourceProvision;
                    // добавляем значение в словарь планов по заказу
                    if (!ordersPlan.ContainsKey(ord_code)) {
                        ordersPlan.Add(ord_code, 0);
                        orderPlanCoef.Add(ord_code, 0);
                    }
                    ordersPlan[ord_code] += cell_plan - cell_const;
                    orderPlanCoef[ord_code] += cell_plan;
                    // связываем индекс переменной с реальной ячейкой
                    realControlledCells.Add(numberOfVariables, cell);
                    // связываем индекс переменной и план минус постоянная составляющая
                    cellsPlans.Add(numberOfVariables, cell_plan - cell_const);
                    // добавляем резерв ячейки в словарь текущих значений ( понадобится для начального приближения)
                    current_values.Add(numberOfVariables, cell_reserve);
                    // коэффициент в ограничении при контролируемой переменной = 1
                    limit.coefficients.Add(numberOfVariables, 1);
                    // добавляем переменную в список переменных в конкретном заказе
                    if (!variablesInOrder.ContainsKey(ord_code))
                        variablesInOrder.Add(ord_code, new Dictionary<int, Decimal>());
                    variablesInOrder[ord_code].Add(numberOfVariables, 0);
                    numberOfVariables++;
                }
                // сумма резерва в подразделении должна остаться неизменной
                limit.freeMember = col.Cells.Sum(x => x.SourceProvision);
                // проверяем, есть ли неконтролируемые заказы в подразделении, строим список неконтролируемых ячеек
                //try {
                Decimal reserve = 0;
                bool contains_uncontrolled_cells = false;
                foreach (HrmMatrixCell cell in col.Cells)
                    if (!controlled_orders.ContainsKey(cell.Row.Order.Code)) {
                        reserve += cell.SourceProvision;
                        contains_uncontrolled_cells = true;
                    }
                if (contains_uncontrolled_cells) {
                    // связываем резерв из неконтролируемых ячеек с соответствующей колонкой реальной матрицы
                    realDepsWithUncontrolledOrders.Add(numberOfVariables, col);
                    // добавляем в словарь текущих значений значение резерва по всем неконтролируемым ячейкам в данном подразделении
                    current_values.Add(numberOfVariables, reserve);
                    // добавляем соответствующий коэффициент = 1 в ограничение
                    limit.coefficients.Add(numberOfVariables, 1);
                    // число переменных увеличилось
                    numberOfVariables++;
                }

                Decimal source_reserve_in_dep = col.Cells.Sum(x => x.SourceProvision);
                if (limit.freeMember != source_reserve_in_dep)
                    throw new Exception("The reserve in dep " + col.Department.BuhCode +
                            " is " + source_reserve_in_dep + " but in limit was " + limit.freeMember);
                limits.Add(limit);
                simpLimits.Add(dep_code, limit);
            }

            // находим частные производные от текущего значения распределения и создаем симплекс-таблицу
            Decimal[] current_values_array = new Decimal[numberOfVariables];
            for (int i = 0 ; i<numberOfVariables ; i++)
                if (current_values.ContainsKey(i))
                    current_values_array[i]=current_values[i];
            Decimal[] array_of_derivates = getArrayOfPartialDerivates(current_values_array);
            Dictionary<int, Decimal> current_coefficietns = new Dictionary<int, Decimal>();
            for (int i = 0 ; i<numberOfVariables ; i++)
                current_coefficietns.Add(i, array_of_derivates[i]);
            table = new SimplexTab(limits, current_coefficietns);
        }

        // возвращает значение целевой функции при заданном векторе переменных
        public Decimal funcValue(Decimal[] vars) {
            Decimal result = 0;
            Decimal cells_result = 0;
            Decimal orders_result = 0;
            foreach (int index in cellsPlans.Keys) {
                Decimal x = vars[index] - cellsPlans[index];
                x *= x;
                cells_result+=x;
            }
            foreach (String code in variablesInOrder.Keys) {
                Decimal x = 0;
                foreach (int index in variablesInOrder[code].Keys)
                    x += vars[index];
                x -= ordersPlan[code];
                x *= x;
                x /= (orderPlanCoef[code] == 0 ? 1 : orderPlanCoef[code]);
                orders_result += x;
            }
            result = cells_result * 2 * cellsCoefficient + orders_result * 2 * ordersCoefficient;
            return result;
        }

        // считает частную производную от целевой функции по заданной переменной
        public Decimal PartialDerivate(int index, Decimal[] variables) {
            Decimal result = 0;
            // если это не неконтролируемая ячейка
            if (realControlledCells.ContainsKey(index)) {
                result += cellsCoefficient * 2 * (variables[index] - cellsPlans[index]);
                Decimal x = 0;
                string code = realControlledCells[index].Row.Order.Code;
                foreach (int key in variablesInOrder[code].Keys)
                    x += variables[key];
                x -= ordersPlan[code];
                x *= 2 * ordersCoefficient / orderPlanCoef[code];
            }
            return result;
        }

        // подсчет всех частных производных по целевой функции, работает быстрее так как учитывает специфику
        // данной функции
        public Decimal[] getArrayOfPartialDerivates(Decimal[] variables) {
            Decimal[] result = new Decimal[variables.Count()];
            // это поячеечная составляющая
            for (int i = 0 ; i < variables.Count() ; i++)
                if (realControlledCells.ContainsKey(i))
                    result[i] += (variables[i] - cellsPlans[i]) * 2 * cellsCoefficient;
            // а это составляющая по заказу
            foreach (string code in variablesInOrder.Keys) {
                Decimal x = 0;
                foreach (int key in variablesInOrder[code].Keys)
                    x += variables[key];
                x -= ordersPlan[code];
                x *= 2 * (ordersCoefficient / (orderPlanCoef[code] == 0 ? 1 : orderPlanCoef[code]));
                foreach (int key in variablesInOrder[code].Keys)
                    result[key] += x;
            }
            return result;
        }

        public Decimal[] GetArrayOfCurrentValues() {
            Decimal[] result = new Decimal[numberOfVariables];
            for (int i = 0 ; i < numberOfVariables ; i++)
                if (current_values.ContainsKey(i))
                    result[i] = current_values[i];
            return result;
        }



    }


}
