using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NpoMash.Erm.Hrm;
using NpoMash.Erm.Hrm.Salary;

namespace NpoMash.Erm.Hrm.Simplex {
    public class SimplexStructureLogic {

        public static double[] Maximize(SimplexTab tab) {
            int guiding_column = tab.ColumnWithMinDelta();
            while (tab.delta[guiding_column] < 0) {
                int guiding_row = tab.FindGuidingRow(guiding_column);
                tab.ConvertToNextTab(guiding_row, guiding_column);
                guiding_column = tab.ColumnWithMinDelta();
            }
            double[] result = new double[tab.maxIndexOfUnfictiveVar + 1];
            for (int i = 0; i < tab.numberOfRows; i++)
                result[tab.basis[i]] = tab.bearingPlan[i];
            return result;
        }

        public static double[] Minimize(SimplexTab tab) {
            int guiding_column = tab.ColumnWithMaxDelta();
            while (tab.delta[guiding_column] > 0) {
                int guiding_row = tab.FindGuidingRow(guiding_column);
                tab.ConvertToNextTab(guiding_row, guiding_column);
                guiding_column = tab.ColumnWithMaxDelta();
            }
            double[] result = new double[tab.maxIndexOfUnfictiveVar + 1];
            for (int i = 0; i < tab.numberOfRows; i++)
                result[tab.basis[i]] = tab.bearingPlan[i];
            return result;
        }

        // функция для определения точки, лежащей между двумя заданными, через параметр лямбда
        public static double[] DeterimnePointWithLambda(double[] vect1, double[] vect2, double lambda) {
            if (vect1.Count() != vect2.Count())
                throw new Exception("Vectors has different count of variables");
            if (lambda < 0 || lambda > 1)
                throw new Exception("Lamda must be between 0 and 1, but was " + lambda.ToString());

            double[] result = new double[vect1.Count()];
            for (int i = 0; i < result.Count(); i++)
                result[i] = vect1[i] + lambda * (vect2[i] - vect1[i]);
            return result;
        }

        // дихотомический поиск лямбды, при которой значение целевой функции минимально лямбда между 0 и 1
        // вернет точку при заданной лямбде
        public static double[] DichotomicalSearchOfLambda(ReserveSimplexBringingStructure structure,double[] vect1, double[] vect2,double eps){
            double left_border = 0;
            double right_border = 1;
            double delta = eps/2;
            // до тех пор пока размер отрезка не будет меньше заданной погрешности
            while (right_border - left_border > eps) {
                double middle = (right_border - left_border)/2;
                double left_x = middle - delta;
                double right_x = middle + delta;
                // если f(x) слева от центра отрезка > f(x) справа от центра
                if (structure.funcValue(DeterimnePointWithLambda(vect1, vect2, left_x)) >
                    structure.funcValue(DeterimnePointWithLambda(vect1, vect2, right_x)))
                    // то сдвигаем левый край
                    left_border = left_x;
                    // иначе правый
                else right_border = right_x;
            }
            // сразу возвращаем точку при найденной лямбде
            return DeterimnePointWithLambda(vect1,vect2,(right_border - left_border)/2);
        }

        public static double Norma(double[] vect1, double[] vect2) {
            double result = 0;
            for (int i = 0; i < vect1.Count(); i++)
                result += Math.Pow(vect1[i] - vect2[i], 2);
            return result;
        }

        // процедура, которая выполняет округление и распределяет отклонения резерва в подразделениях
        public void RoundResults(ReserveSimplexBringingStructure structure) {


        }

        // процедура, которая осуществляет перенос результатов назад в реальную матрицу
        // при этом заодно распределяя резерв между неконтролируемыми заказами
        public void ReturnResultsInRealMatrix(ReserveSimplexBringingStructure structure) {

        }

        // главная процедура, которая выполнит все приведение с заданной точностью
        public void MainAlgorithm(HrmSalaryTaskProvisionMatrixReduction card, int cell_c, int ord_c, double lambda_eps,double main_eps){
            ReserveSimplexBringingStructure structure = new ReserveSimplexBringingStructure(card, cell_c, ord_c);
            double[] previous_distribution = structure.GetArrayOfCurrentValues();
            double[] current_bearing_plan = Minimize(structure.table);
            double[] current_distribution = DichotomicalSearchOfLambda(structure, previous_distribution, current_bearing_plan, lambda_eps);
            do {
                previous_distribution = current_distribution;
                structure.table.ReplaceTargetFuction(structure.getArrayOfPartialDerivates(previous_distribution));
                current_bearing_plan = Minimize(structure.table);
                current_distribution = DichotomicalSearchOfLambda(structure, previous_distribution, current_bearing_plan, lambda_eps);
            } while (Norma(previous_distribution, current_distribution) > main_eps);
            RoundResults(structure);
            ReturnResultsInRealMatrix(structure);
        }



    }
}
