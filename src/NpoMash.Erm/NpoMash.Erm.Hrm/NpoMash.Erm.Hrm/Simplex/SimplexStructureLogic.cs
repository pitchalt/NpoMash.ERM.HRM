using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NpoMash.Erm.Hrm.Simplex {
    class SimplexStructureLogic {
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
    }
}
