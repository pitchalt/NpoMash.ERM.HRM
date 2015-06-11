using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoMash.Erm.Hrm.Optimization
{
    class Program
    {
        static void Main(string[] args)
        {
            // Тесты 4-6
            //FunctionWithSingleVar my_func = new FunctionWithSingleVar();
            //SingleVarFunctionElem elem = new ReserveMatrixCriteriaElemCells(new Variable(),-1,(double)3);
            //my_func.SetElement(elem);
            //DichotomousSearch ds = new DichotomousSearch((double)0.01,my_func,4,7);
            //ds.Optimize();
            //Console.WriteLine(ds.CurrentState.ToString());
            //Console.WriteLine(ds.Iterations.ToString());

            // Тест 1
            //LinearFunction crit = new LinearFunction();
            //Variable v1 = new Variable();
            //Variable v2 = new Variable();
            //Variable v3 = new Variable();
            //Variable v4 = new Variable();
            //Variable v5 = new Variable();
            //Variable v6 = new Variable();
            //crit.AddElement(new SimpleFunctionElem(v1,-9));
            //crit.AddElement(new SimpleFunctionElem(v2, -10));
            //crit.AddElement(new SimpleFunctionElem(v3, -16));
            //List<Equality> restrictions = new List<Equality>();
            //Equality eq1 = new Equality();
            //eq1.EqualTo = 360;
            //eq1.AddElement(new SimpleFunctionElem(v1, 18));
            //eq1.AddElement(new SimpleFunctionElem(v2, 15));
            //eq1.AddElement(new SimpleFunctionElem(v3, 12));
            //eq1.AddElement(new SimpleFunctionElem(v4, 1));
            //Equality eq2 = new Equality();
            //eq2.EqualTo = 192;
            //eq2.AddElement(new SimpleFunctionElem(v1, 6));
            //eq2.AddElement(new SimpleFunctionElem(v2, 4));
            //eq2.AddElement(new SimpleFunctionElem(v3, 8));
            //eq2.AddElement(new SimpleFunctionElem(v5, 1));
            //Equality eq3 = new Equality();
            //eq3.EqualTo = 180;
            //eq3.AddElement(new SimpleFunctionElem(v1, 5));
            //eq3.AddElement(new SimpleFunctionElem(v2, 3));
            //eq3.AddElement(new SimpleFunctionElem(v3, 3));
            //eq3.AddElement(new SimpleFunctionElem(v6, 1));
            //restrictions.Add(eq1);
            //restrictions.Add(eq2);
            //restrictions.Add(eq3);

            //SimplexMethod simplex = new SimplexMethod(1, crit, restrictions);
            //simplex.Optimize();
            //Console.WriteLine("X1= " + simplex.CurrentState[v1].ToString());
            //Console.WriteLine("X2= " + simplex.CurrentState[v2].ToString());
            //Console.WriteLine("X3= " + simplex.CurrentState[v3].ToString());
            //Console.WriteLine("X4= " + simplex.CurrentState[v4].ToString());
            //Console.WriteLine("X5= " + simplex.CurrentState[v5].ToString());
            //Console.WriteLine("X6= " + simplex.CurrentState[v6].ToString());

            // Тест 2
            //LinearFunction crit = new LinearFunction();
            //Variable v1 = new Variable();
            //Variable v2 = new Variable();
            //Variable v3 = new Variable();
            //Variable v4 = new Variable();
            //Variable v5 = new Variable();
            //Variable v6 = new Variable();
            //crit.AddElement(new SimpleFunctionElem(v1, -2));
            //crit.AddElement(new SimpleFunctionElem(v2, 3));
            //crit.AddElement(new SimpleFunctionElem(v3, -6));
            //crit.AddElement(new SimpleFunctionElem(v4, -1));
            //List<Equality> restrictions = new List<Equality>();
            //Equality eq1 = new Equality();
            //eq1.EqualTo = 24;
            //eq1.AddElement(new SimpleFunctionElem(v1, 2));
            //eq1.AddElement(new SimpleFunctionElem(v2, 1));
            //eq1.AddElement(new SimpleFunctionElem(v3, -2));
            //eq1.AddElement(new SimpleFunctionElem(v4, 1));
            //Equality eq2 = new Equality();
            //eq2.EqualTo = 22;
            //eq2.AddElement(new SimpleFunctionElem(v1, 1));
            //eq2.AddElement(new SimpleFunctionElem(v2, 2));
            //eq2.AddElement(new SimpleFunctionElem(v3, 4));
            //eq2.AddElement(new SimpleFunctionElem(v5, 1));
            //Equality eq3 = new Equality();
            //eq3.EqualTo = 10;
            //eq3.AddElement(new SimpleFunctionElem(v1, 1));
            //eq3.AddElement(new SimpleFunctionElem(v2, -1));
            //eq3.AddElement(new SimpleFunctionElem(v3, 2));
            //eq3.AddElement(new SimpleFunctionElem(v6, -1));
            //restrictions.Add(eq1);
            //restrictions.Add(eq2);
            //restrictions.Add(eq3);

            //SimplexMethod simplex = new SimplexMethod(1, crit, restrictions);
            //simplex.Optimize();
            //Console.WriteLine("X1= " + simplex.CurrentState[v1].ToString());
            //Console.WriteLine("X2= " + simplex.CurrentState[v2].ToString());
            //Console.WriteLine("X3= " + simplex.CurrentState[v3].ToString());
            //Console.WriteLine("X4= " + simplex.CurrentState[v4].ToString());
            //Console.WriteLine("X5= " + simplex.CurrentState[v5].ToString());
            //Console.WriteLine("X6= " + simplex.CurrentState[v6].ToString());

            // Тест 3
            //LinearFunction crit = new LinearFunction();
            //Variable v1 = new Variable();
            //Variable v2 = new Variable();
            //Variable v3 = new Variable();
            //Variable v4 = new Variable();
            //crit.AddElement(new SimpleFunctionElem(v1, -1));
            //crit.AddElement(new SimpleFunctionElem(v2, -1));
            //List<Equality> restrictions = new List<Equality>();
            //Equality eq1 = new Equality();
            //eq1.EqualTo = 3;
            //eq1.AddElement(new SimpleFunctionElem(v1, -2));
            //eq1.AddElement(new SimpleFunctionElem(v2, 1));
            //eq1.AddElement(new SimpleFunctionElem(v3, 1));
            //Equality eq2 = new Equality();
            //eq2.EqualTo = 1;
            //eq2.AddElement(new SimpleFunctionElem(v1, -1));
            //eq2.AddElement(new SimpleFunctionElem(v2, 1));
            //eq2.AddElement(new SimpleFunctionElem(v4, 1));
            //restrictions.Add(eq1);
            //restrictions.Add(eq2);

            //SimplexMethod simplex = new SimplexMethod(1, crit, restrictions);
            //simplex.Optimize();

            //Console.WriteLine(simplex.CurrentState[v1].ToString());
            //Console.WriteLine(simplex.CurrentState[v2].ToString());
            //Console.WriteLine(simplex.CurrentState[v3].ToString());
            //Console.WriteLine(simplex.CurrentState[v4].ToString());

            // тест 7
            Variable v1 = new Variable();
            Variable v2 = new Variable();
            Variable v3 = new Variable();
            Variable v4 = new Variable();

            FunctionWithSingleVarElements crit = new FunctionWithSingleVarElements();
            crit.AddElement(new ReserveMatrixCriteriaElemCells(v1, 1, 5));
            crit.AddElement(new ReserveMatrixCriteriaElemCells(v2, 1, 3));

            List<Equality> restrictions = new List<Equality>();
            Equality eq1 = new Equality();
            eq1.EqualTo = 12;
            eq1.AddElement(new SimpleFunctionElem(v1, 1));
            eq1.AddElement(new SimpleFunctionElem(v2, 1));
            eq1.AddElement(new SimpleFunctionElem(v3, 1));
            Equality eq2 = new Equality();
            eq2.EqualTo = 25;
            eq2.AddElement(new SimpleFunctionElem(v1, 3));
            eq2.AddElement(new SimpleFunctionElem(v2, 2));
            eq2.AddElement(new SimpleFunctionElem(v4, 1));
            restrictions.Add(eq1);
            restrictions.Add(eq2);
            ValuesVector start_point = new ValuesVector();
            start_point.Add(v1, 0);
            start_point.Add(v2, 0);
            start_point.Add(v3, 12);
            start_point.Add(v4, 25);
            FrankWulfMethod fwm = new FrankWulfMethod(0.0001, crit, restrictions, start_point);
            ValuesVector result = fwm.Optimize();
            Console.WriteLine(result[v1].ToString());
            Console.WriteLine(result[v2].ToString());
            Console.WriteLine(result[v3].ToString());
            Console.WriteLine(result[v4].ToString());
            Console.WriteLine("Iters: " + fwm.Iterations.ToString());
            TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - fwm.StartTime.Ticks);
            Console.WriteLine("Time passed: " + ts.Seconds.ToString() + " seconds " + ts.Milliseconds.ToString() + " millisecons");
            Console.WriteLine(crit.Calculate(result).ToString());
            // тест 8
            //Variable v1 = new Variable();
            //Variable v2 = new Variable();
            //Variable v3 = new Variable();
            //Variable v4 = new Variable();

            //FunctionWithSingleVarElements crit = new FunctionWithSingleVarElements();
            //crit.AddElement(new ReserveMatrixCriteriaElemCells(v1, 1, 2));
            //crit.AddElement(new ReserveMatrixCriteriaElemCells(v2, 1, 5));

            //List<Equality> restrictions = new List<Equality>();
            //Equality eq1 = new Equality();
            //eq1.EqualTo = 5;
            //eq1.AddElement(new SimpleFunctionElem(v1, 1));
            //eq1.AddElement(new SimpleFunctionElem(v2, 1));
            //eq1.AddElement(new SimpleFunctionElem(v3, 1));
            //Equality eq2 = new Equality();
            //eq2.EqualTo = 8;
            //eq2.AddElement(new SimpleFunctionElem(v1, 2));
            //eq2.AddElement(new SimpleFunctionElem(v2, 1));
            //eq2.AddElement(new SimpleFunctionElem(v4, 1));
            //restrictions.Add(eq1);
            //restrictions.Add(eq2);
            //ValuesVector start_point = new ValuesVector();
            //start_point.Add(v1, 0);
            //start_point.Add(v2, 0);
            //start_point.Add(v3, 5);
            //start_point.Add(v4, 8);
            //FrankWulfMethod fwm = new FrankWulfMethod((double)0.01, crit, restrictions, start_point);
            //fwm.Optimize();
            //Console.WriteLine(fwm.CurrentState[v1].ToString());
            //Console.WriteLine(fwm.CurrentState[v2].ToString());
            //Console.WriteLine(fwm.CurrentState[v3].ToString());
            //Console.WriteLine(fwm.CurrentState[v4].ToString());

            Console.ReadKey();
        }
    }
}
