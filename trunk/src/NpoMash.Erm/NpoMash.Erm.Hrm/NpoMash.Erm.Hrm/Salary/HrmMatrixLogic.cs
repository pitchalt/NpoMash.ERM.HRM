using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
//
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
//
using IntecoAG.Erm.HRM.Organization;
using IntecoAG.Erm.FM.Order;

namespace NpoMash.Erm.Hrm.Salary {


    public class HrmMatrixLogic : BaseObject {

        static public HrmMatrixAllocPlan setTestData(IObjectSpace os,HrmPeriod current_period ) {
            var random = new Random();
            var order_list = os.GetObjects<fmCOrder>();
            var order_count = order_list.Count();

            //Ячейки
            var cells = new List<HrmMatrixCell>();
            for (int i = 0; i <= order_count * 10; i++) {
                var new_cell = os.CreateObject<HrmMatrixCell>();
                new_cell.Sum = random.Next(10000, 99999);
                new_cell.Time = Convert.ToInt16(random.Next(100, 999)); 
                cells.Add(new_cell);
            }

        //Подразделений и колонок матрицы
            var columns = new List<HrmMatrixColumn>();
            for (int i = 0; i < 10; i++) {
                var new_department = os.CreateObject<Department>();
                var new_column = os.CreateObject<HrmMatrixColumn>();
                new_department.Code = Convert.ToString(random.Next(10000, 99999));
                int type_group = random.Next(1, 2);
                if (type_group == 1) { new_department.GroupDep = DEPARTMENT_GROUP_DEP.KB; }
                if (type_group == 2) { new_department.GroupDep = DEPARTMENT_GROUP_DEP.OZM; }
                new_column.Sum=random.Next(10000, 99999);
                new_column.Department = new_department;
                columns.Add(new_column);
            }

            //Теперь хочу строк 
            var rows = new List<HrmMatrixRow>();
            foreach (var order in order_list) {
                var new_row = os.CreateObject<HrmMatrixRow>();
                new_row.Sum = random.Next(10000, 99999);
                new_row.Order = order;
                rows.Add(new_row);
            }
            
            int a = 0;
            foreach (var c in columns) {
                c.Cells.Add(cells[a]);
                a++;
            }

            foreach (var r in rows) {
                r.Cells.Add(cells[a]);
                a++;
            }
        //Создаем плановую матрицу
            var plan_matrix=os.CreateObject<HrmMatrixAllocPlan>();
            plan_matrix.Type = HRM_MATRIX_TYPE.Matrix;
            plan_matrix.TypeMatrix = HRM_MATRIX_TYPE_MATRIX.Planned;
            plan_matrix.GroupDep = HRM_MATRIX_GROUP_DEP.KB;
            plan_matrix.Status = HRM_MATRIX_STATUS.Opened;
            plan_matrix.IterationNumber = HRM_MATRIX_ITERATION_NUMBER.Five;
            plan_matrix.Variant = HRM_MATRIX_VARIANT.ProportionsMethod;

            foreach (var c in columns) {
                plan_matrix.Columns.Add(c);
            }

            foreach (var r in rows) {
                plan_matrix.Rows.Add(r);
            }

            current_period.Matrixs.Add(plan_matrix);

            return plan_matrix;
        }

        public HrmMatrixLogic(Session session): base(session) { }
        public override void AfterConstruction() {base.AfterConstruction();}
    }
}
