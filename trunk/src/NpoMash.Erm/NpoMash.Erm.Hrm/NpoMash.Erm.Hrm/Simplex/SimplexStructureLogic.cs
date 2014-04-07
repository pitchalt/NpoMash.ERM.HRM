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
        public double[] DeterimnePointWithLambda(double[] vect1, double[] vect2, double lambda) {
            if (vect1.Count() != vect2.Count())
                throw new Exception("Vectors has different count of variables");
            if (lambda < 0 || lambda > 1)
                throw new Exception("Lamda must be between 0 and 1, but was " + lambda.ToString());

            double[] result = new double[vect1.Count()];
            for (int i = 0; i < result.Count(); i++)
                result[i] = vect1[i] + lambda * (vect2[i] - vect1[i]);
            return result;
        }

    }
}
