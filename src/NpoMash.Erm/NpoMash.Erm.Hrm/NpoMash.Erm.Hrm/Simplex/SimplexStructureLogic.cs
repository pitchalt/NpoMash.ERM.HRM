﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NpoMash.Erm.Hrm;
using NpoMash.Erm.Hrm.Salary;

namespace NpoMash.Erm.Hrm.Simplex {
    public class SimplexStructureLogic {

        public static Decimal[] Maximize(SimplexTab tab) {
            int guiding_column = tab.ColumnWithMinDelta();
            while (tab.delta[guiding_column] < 0) {
                int guiding_row = tab.FindGuidingRow(guiding_column);
                tab.ConvertToNextTab(guiding_row, guiding_column);
                guiding_column = tab.ColumnWithMinDelta();
            }
            Decimal[] result = new Decimal[tab.maxIndexOfUnfictiveVar + 1];
            for (int i = 0 ; i < tab.numberOfRows ; i++)
                result[tab.basis[i]] = tab.bearingPlan[i];
            return result;
        }

        public static Decimal[] Minimize(SimplexTab tab) {
            int guiding_column = tab.ColumnWithMaxDelta();
            while (tab.delta[guiding_column] > 0) {
                int guiding_row = tab.FindGuidingRow(guiding_column);
                tab.ConvertToNextTab(guiding_row, guiding_column);
                guiding_column = tab.ColumnWithMaxDelta();
            }
            Decimal[] result = new Decimal[tab.maxIndexOfUnfictiveVar + 1];
            for (int i = 0 ; i < tab.numberOfRows ; i++)
                result[tab.basis[i]] = tab.bearingPlan[i];
            return result;
        }

        // функция для определения точки, лежащей между двумя заданными, через параметр лямбда
        public static Decimal[] DeterimnePointWithLambda(Decimal[] vect1, Decimal[] vect2, Decimal lambda) {
            if (vect1.Count() != vect2.Count())
                throw new Exception("Vectors has different count of variables");
            if (lambda < 0 || lambda > 1)
                throw new Exception("Lamda must be between 0 and 1, but was " + lambda.ToString());

            Decimal[] result = new Decimal[vect1.Count()];
            for (int i = 0 ; i < result.Count() ; i++)
                result[i] = vect1[i] + lambda * (vect2[i] - vect1[i]);
            return result;
        }

        // дихотомический поиск лямбды, при которой значение целевой функции минимально лямбда между 0 и 1
        // вернет точку при заданной лямбде
        public static Decimal[] DichotomicalSearchOfLambda(ReserveSimplexBringingStructure structure, Decimal[] vect1, Decimal[] vect2, Decimal eps) {
            Decimal left_border = 0;
            Decimal right_border = 1;
            Decimal delta = eps / 3;
            // до тех пор пока размер отрезка не будет меньше заданной погрешности
            while (right_border - left_border > eps) {
                Decimal middle = (right_border + left_border) / 2;
                Decimal left_x = middle - delta;
                Decimal right_x = middle + delta;
                // если f(x) слева от центра отрезка > f(x) справа от центра
                if (structure.funcValue(DeterimnePointWithLambda(vect1, vect2, left_x)) >
                    structure.funcValue(DeterimnePointWithLambda(vect1, vect2, right_x)))
                    // то сдвигаем левый край
                    left_border = left_x;
                // иначе правый
                else right_border = right_x;
            }
            // сразу возвращаем точку при найденной лямбде
            Decimal lambda = (right_border + left_border) / 2;
            return DeterimnePointWithLambda(vect1, vect2, lambda);
        }

        public static Decimal Norma(Decimal[] vect1, Decimal[] vect2) {
            Decimal result = 0;
            for (int i = 0 ; i < vect1.Count() ; i++)
                result += (vect1[i] - vect2[i])*(vect1[i]-vect2[i]);
            return Math.Round((decimal)Math.Sqrt((double)result),2);
        }

        // процедура, которая выполняет округление и распределяет отклонения резерва в подразделениях
        public static Decimal[] RoundResults(ReserveSimplexBringingStructure structure, Decimal[] distribution) {
            for (int i = 0 ; i < distribution.Count() ; i++)
                distribution[i] = Math.Round(distribution[i],2);
            foreach (SimplexLimitation limit in structure.simpLimits.Values) {
                Decimal current_value = 0;
                Decimal[] current_derivates = structure.getArrayOfPartialDerivates(distribution);
                Dictionary<int, Decimal> current_dervs = new Dictionary<int, Decimal>();
                foreach (int key in limit.coefficients.Keys) {
                    current_value += distribution[key];
                    current_dervs.Add(key, current_derivates[key]);
                }
                Decimal difference = current_value - limit.freeMember;
                // если появился недостаток
                if (difference < 0) {
                    // то свалили его в ячейку с наименьшим значением частной производной
                    distribution[current_dervs.Keys
                        .First(x => current_dervs[x]==current_dervs.Values.Min())] -= difference;
                }
                // если появился излишек
                if (difference > 0)
                    // то пока не снимем его весь ( а в одной ячейке может быть меньше чем весь излишек)
                    while (difference > 0) {
                        // снимаем его с ячейки, где наибольшая частная производная и еще что-то осталось
                        int index = current_dervs.Keys
                            .Where(x => distribution[x] > 0)
                            .First(x => current_dervs[x] == current_dervs
                                .Where(y => distribution[y.Key]>0)
                                .Max(y => y.Value));
                        Decimal take = Math.Min(difference, distribution[index]);
                        distribution[index] -= take;
                        difference -= take;
                    }
            }
            // вернули полученное округленное распределение
            return distribution;
        }

        // процедура, которая осуществляет перенос результатов назад в реальную матрицу
        // при этом заодно распределяя резерв между неконтролируемыми заказами
        public static void ReturnResultsInRealMatrix(HrmSalaryTaskProvisionMatrixReduction card, ReserveSimplexBringingStructure structure, Decimal[] distribution) {
            foreach (int key in structure.realControlledCells.Keys) {
                structure.realControlledCells[key].NewProvision = distribution[key];
            }
            // те заказы что не содержатся в этом словаре - неконтролируемые
            Dictionary<String, HrmAllocParameterOrderControl> controlled_orders = card.AllocParameters.OrderControls
                .Where(x => x.TypeControl != IntecoAG.ERM.FM.Order.FmCOrderTypeControl.NO_ORDERED)
                .ToDictionary(x => x.Order.Code);
            // распределяем резерв между неконтролируемыми заказами в пределах подразделения
            foreach (int key in structure.realDepsWithUncontrolledOrders.Keys) {
                Decimal res = distribution[key];
                List<HrmMatrixCell> work_cells = structure.realDepsWithUncontrolledOrders[key]
                    .Cells.Where(x => !controlled_orders.ContainsKey(x.Row.Order.Code)).ToList();
                int count_of_cells = work_cells.Count();
                foreach (HrmMatrixCell cell in work_cells) {
                    Decimal take = Math.Round(res / count_of_cells,2);
                    cell.NewProvision = take;
                    count_of_cells--;
                    res -= take;
                }
            }
        }

        // главная процедура, которая выполнит все приведение с заданной точностью
        public static void MainAlgorithm(HrmSalaryTaskProvisionMatrixReduction card, int cell_c, int ord_c, Decimal lambda_eps, Decimal main_eps) {
            ReserveSimplexBringingStructure structure = new ReserveSimplexBringingStructure(card, cell_c, ord_c);
            // осуществляем многократный расчет симплекс-методом,
            // по сути, метод Франка-Вульфа
            Decimal[] previous_distribution = structure.GetArrayOfCurrentValues();
            Decimal target_function_value = structure.funcValue(previous_distribution);
            Decimal previous_function_value = target_function_value;
            Decimal[] current_bearing_plan = Minimize(structure.table);
            Decimal[] current_distribution = DichotomicalSearchOfLambda(structure, previous_distribution, current_bearing_plan, 10 / Norma(previous_distribution, current_bearing_plan));
            // до тех пор, пока разница между предыдущим и новым вектором не станет меньше заданной погрешности
            Decimal result_norm = 0;
            Decimal difference = 0;
            do {
                previous_function_value = target_function_value;
                previous_distribution = current_distribution;
                structure.table.ReplaceTargetFuction(structure.getArrayOfPartialDerivates(previous_distribution));
                current_bearing_plan = Minimize(structure.table);
                current_distribution = DichotomicalSearchOfLambda(structure, previous_distribution, current_bearing_plan, 10 / Norma(previous_distribution, current_bearing_plan));
                result_norm = Norma(previous_distribution, current_distribution);
                target_function_value = structure.funcValue(current_distribution);
                difference = previous_function_value - target_function_value;
                if (difference < 0)
                    throw new Exception("Error in minimization algorithm! New value was greater than previous");
            } while (result_norm > main_eps && difference > 100);
            // приводим полученные результаты к целым
            current_distribution = RoundResults(structure, current_distribution);
            target_function_value = structure.funcValue(current_distribution);
            // грузим их в матрицу резерва
            ReturnResultsInRealMatrix(card, structure, current_distribution);
            return;
        }





    }
}
