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
using IntecoAG.ERM.HRM.Organization;
using IntecoAG.ERM.FM.Order;

namespace NpoMash.Erm.Hrm.Salary.ProvisionMatrixBringingStructure {

    public static class ProvBringLogic {

        public static ProvMat CreateProvBringStructure(HrmSalaryTaskProvisionMatrixReduction card){
            ProvMat result = new ProvMat();
            HrmMatrix source_mat = card.ProvisionMatrix;
            // словарь контролируемых зказов
            Dictionary<String, HrmPeriodOrderControl> controlled_orders = card.AllocParameters.OrderControls
                .Where(x => x.TypeControl == FmCOrderTypeControl.FOT || x.TypeControl == FmCOrderTypeControl.TRUDEMK_FOT).
                ToDictionary(x => x.Order.Code);
            foreach (HrmMatrixColumn source_column in source_mat.Columns) {
                String dep_code = source_column.Department.BuhCode;
                ProvDep current_dep = new ProvDep();
                result.deps.Add(dep_code, current_dep);
                current_dep.code = dep_code;
                foreach (HrmMatrixCell source_cell in source_column.Cells) {
                    ProvOrd current_ord = null;
                    String ord_code = source_cell.Row.Order.Code;
                    if (result.ords.ContainsKey(ord_code)) {
                        current_ord = result.ords[ord_code];
                    }
                    else {
                        current_ord = new ProvOrd();
                        result.ords.Add(ord_code, current_ord);
                        current_ord.code = ord_code;
                        current_ord.isControlled = controlled_orders.ContainsKey(ord_code);
                    }
                    if (current_ord.isControlled)
                        current_dep.numberOfControlledOrders += 1;
                    else
                        current_dep.numberOfUncontrolledOrders += 1;
                    // создаем ячейку
                    ProvCell current_cell = new ProvCell();
                    // добавляем ее в текущий заказ
                    current_ord.cells.Add(current_cell);
                    current_cell.ord = current_ord;
                    // ... и подразделение
                    current_dep.cells.Add(current_cell);
                    current_cell.dep = current_dep;
                    // план по ячейке
                    current_cell.plan = source_cell.PlanMoney;
                    // увеличиваем план по текущему заказу в целом
                    current_ord.ordPlan += source_cell.PlanMoney;
                    // реальная база
                    current_cell.realBase = source_cell.MoneyNoReserve;
                    // изначально все запихиваем в нераспределенный резерв
                    current_dep.undistributedReserve += source_cell.SourceProvision;
                    // а это количество резерва в подразделении, с которым будем сверяться
                    current_dep.reserveOfDep += source_cell.SourceProvision;
                    // ссылка на реальную ячейку для быстрой записи результатов после выполнения алгоритма
                    current_cell.refToRealCell = source_cell;
                    result.cells.Add(current_cell);
                }
            }
            return result;
        }

        // а эта процедура возвращает резерв в реальную матрицу
        public static void LoadProvBringResultInTask(ProvMat mat){
            foreach (ProvCell cell in mat.cells)
                cell.refToRealCell.NewProvision = cell.reserve;
        }

        // процедура распределения остатка резерва между неконтролируемыми заказами
        public static void BringUncontrolledReserveInDep(ProvDep dep) {
            // делим остаток резерва тупо поровну между неконтролируемыми заказами в подразделении
            int uncontrolled = dep.numberOfUncontrolledOrders;
                foreach (ProvCell cell in dep.cells.Where(x => !x.ord.isControlled)) {
                    Decimal diff = dep.undistributedReserve / uncontrolled;
                    Math.Truncate(diff);
                    cell.reserve += diff;
                    dep.undistributedReserve -= diff;
                    uncontrolled--;
                }
        }

        public static void BringEasyDeps(ProvMat mat) {
            // приводим подразделения, в которых есть контролируемые и неконтролируемые заказы
            IEnumerable<ProvDep> easyDeps = mat.deps.Values.Where(x => !x.isAlreadyBringed && x.numberOfUncontrolledOrders > 0 && x.numberOfControlledOrders > 0)
                .Where(x => x.cells
                    // где оставшегося неконтролируемого резерва хватает для  распределения в недогруженные контролируемые ячейки
                    .Where(y => y.ord.isControlled && y.planFactDifference > 0)
                    .Sum(y => y.planFactDifference) < x.undistributedReserve);
            foreach (ProvDep dep in easyDeps) {
                foreach (ProvCell cell in dep.cells.Where(x => x.ord.isControlled && x.planFactDifference > 0)) {
                    decimal diff = cell.planFactDifference;
                    cell.reserve += diff;
                    dep.undistributedReserve -= diff;
                }
                BringUncontrolledReserveInDep(dep);
                dep.isAlreadyBringed = true;
            }
        }

        public static void BringVeryEasyDeps(ProvMat mat) {
            // приводим подразделения, в которых только неконтролируемые заказы (кто знает, а вдруг они есть?)
            IEnumerable<ProvDep> veryEasyDeps = mat.deps.Values.Where(x => !x.isAlreadyBringed && x.numberOfControlledOrders == 0 && x.numberOfUncontrolledOrders > 0);
            foreach (ProvDep dep in veryEasyDeps) {
                BringUncontrolledReserveInDep(dep);
                dep.isAlreadyBringed = true;
            }
        }

        public static void BringDifficultDeps(ProvMat mat) {
            // подразделения, в которых не хватает резерва для контролируемых заказов
            IEnumerable<ProvDep> less_deps = mat.deps.Values.Where(x => !x.isAlreadyBringed && x.numberOfControlledOrders > 0)
                .Where(x => x.cells
                    .Where(y => y.ord.isControlled && y.planFactDifference >= 0)
                    .Sum(y => y.planFactDifference) > x.undistributedReserve);
            // полностью контролируемые подразделения, в которых имеется избыток резерва
            IEnumerable<ProvDep> overloaded_deps = mat.deps.Values.Where(x => !x.isAlreadyBringed && x.numberOfUncontrolledOrders == 0)
                .Where(x => x.cells
                    .Where(y => y.ord.isControlled && y.planFactDifference >= 0)
                    .Sum(y => y.planFactDifference) <= x.undistributedReserve);
            // объединяем эти два списка
            IEnumerable<ProvDep> work_deps = less_deps.Concat(overloaded_deps);
            // приводим каждое подразделение из этого списка
            foreach (ProvDep dep in work_deps) {
                // берем коллекцию контролируемых ячеек этого подразделения
                IEnumerable<ProvCell> all_cells = dep.cells.Where(x => x.ord.isControlled);
                // мучаем подразделение пока не распределили весь резерв
                while (dep.undistributedReserve > 0) {
                    // это наибольшее отклонение между планом и фактом (может быть и отрицательным)
                    Decimal max_diff = all_cells.Max(x => x.planFactDifference);
                    
                    // а это отклонение перед наибольшим по величине
                    Decimal prev_max_diff = 0;
                    // если не найдем такого - во всех ячейках уже одинаковое отклонение
                    bool prev_max_diff_founded = false;
                    // формируем список ячеек, с которыми работаем на данной итерации (с наибольшим отклонением)
                    List<ProvCell> work_cells = new List<ProvCell>();
                    foreach (ProvCell cell in all_cells) {
                        if (cell.planFactDifference == max_diff) {
                            work_cells.Add(cell);
                        }
                        else {
                            if (cell.planFactDifference > prev_max_diff || prev_max_diff_founded == false) {
                                prev_max_diff = cell.planFactDifference;
                                // отмечаем, что второе по величине отклонение нашлось
                                prev_max_diff_founded = true;
                            }
                        }
                    }
                    // если переходим через 0, то пока что используем 0 как второе по величине отклонение
                    // (а надо ли это вообще делать??)
                    if (max_diff > 0 && prev_max_diff < 0)
                        prev_max_diff = 0;

                    Decimal current_diff = max_diff - prev_max_diff;

                    // между скольки ячейками будем распределять резерв на этой итерации
                    int number_of_workcells = work_cells.Count;
                    // если нет второго по величине отклонения или в текущие "рабочие" ячейки уместится весь оставшийся резерв
                    if (!prev_max_diff_founded || number_of_workcells * current_diff > dep.undistributedReserve) {
                        // то распихиваем остаток резерва поровну
                        foreach (ProvCell working_cell in work_cells) {
                            Decimal diff = dep.undistributedReserve / number_of_workcells;
                            Math.Truncate(diff);
                            working_cell.reserve += diff;
                            dep.undistributedReserve -= diff;
                            number_of_workcells--;
                        }
                    }
                    // иначе набиваем резервом "рабочие" ячейки, используя разницу между двумя наибольшими отклонениями
                    else {
                        foreach (ProvCell working_cell in work_cells) {
                            working_cell.reserve += current_diff;
                            dep.undistributedReserve -= current_diff;
                        }
                    }
                }
                // приведение подразделения завершено
                dep.isAlreadyBringed = true;
            }
        }

        // это базовый алгоритм, чтобы не вызывать по одной эти три процедуры
        public static void baseAlgorithm(ProvMat mat){
            BringVeryEasyDeps(mat);
            BringEasyDeps(mat);
            BringDifficultDeps(mat);
        }

        // здесь выполяется основной алгоритм
        public static void mainAlgorithm(ProvMat mat) {
            // выполнили первоначальное распределение
            baseAlgorithm(mat);
            // ищем наиболее отклонившийся заказ
            
            ProvOrd work_order = theMostDeviatedOrd(mat);
            ProvOrd check_point_order = null;
            bool check_point_is_made = false;
            decimal d = 0;
            while (work_order != null) {
                // проверяем, является ли он полностью контролируемым, если да то...
                if (isFullyControlled(work_order)) {
                    // если была сделана контрольная точка и значение целевой функции ухудшилось...
                    if (check_point_is_made && mat.CountTargetFunctionValue() > mat.checkPointTargetFunctionValue ||
                         check_point_order == work_order) {
                        // увеличиваем допустимое отклонение на 3 процента
                        d += 3;
                        // если допускаем отклонение еще меньше 100%
                        if (d < 100) {
                            // то откатываемся к контрольной точке
                            mat.RevertToCheckPoint();
                            work_order = check_point_order;
                        }
                            // иначе считаем заказ контрольной точки успешно приведенным
                        else {
                            check_point_order.isFinallyBringed = true;
                            d = 0;
                            // делаем новую контрольную точку
                            mat.MakeCheckPoint();
                            check_point_order = work_order;
                            check_point_is_made = true;
                        }
                    }
                        // если не было контрольной точки, или произошло улучшение
                    else {
                        if(check_point_is_made)
                            check_point_order.isFinallyBringed = true;
                        d = 0;
                        // делаем новую контрольную точку
                        mat.MakeCheckPoint();
                        check_point_order = work_order;
                        check_point_is_made = true;
                    }
                    // выполняем процедуру распределения виртуальной базы в полностью контролируемом заказе
                    distributeVBInFullyControlledOrder(work_order,d);
                }
                // если же заказ полностью контролируемым не является, то...
                else {
                    // выполняем процедуру распределения виртуальной базы в неполностью контролируемом заказе
                }
                // выплняем базовый алгоритм
                baseAlgorithm(mat);
                // ищем следующий наиболее отклоняющийся заказ
                work_order = theMostDeviatedOrd(mat);
            }
        }

        // проверка, является ли данный заказ полностью контролируемым
        public static bool isFullyControlled(ProvOrd ord) {
            bool result = false;
            // факт превышает план, следует смотреть можно ли что-то выпихнуть в неконтролируемые заказы
            if (ord.ordDeviation > 0) {
                result = ord.cells.Where(x => x.dep.numberOfUncontrolledOrders > 0 && x.reserve > 0).Count() > 0;
            }
            // факт меньше плана, смотрим можно ли что-то добавить из неконтролируемых заказов
            if (ord.ordDeviation < 0) {
                result = ord.cells.Where(x => x.dep.numberOfUncontrolledOrders > 0
                    && Math.Min(x.constFact, x.dep.cells.Where(y => !y.ord.isControlled).Sum(y => y.reserve)) > 0)
                    .Count() > 0;
            }
            return result;
        }

        // поиск наиболее отклоняющегося неприведенного заказа
        public static ProvOrd theMostDeviatedOrd(ProvMat mat) {
            ProvOrd result = null;
            try {
                // ищем среди контролируемых заказов, не отмеченных как приведенные, заказ с наибольшим по модулю отклонением
                Decimal deviation = mat.ords.Values.Where(x => !x.isFinallyBringed && !x.isControlled).Max(x => Math.Abs(x.ordDeviation));
                result = mat.ords.Values.FirstOrDefault(x => x.ordDeviation == deviation);
            }
                // если вдруг при операции максимума список был пуст то поймали исключениел и вернем ничто
            catch (InvalidOperationException) { }
            return result;
        }

        // распределение виртуальной базы для полностью контролируемого заказа
        public static void distributeVBInFullyControlledOrder(ProvOrd order, Decimal d) {
            if (d > 100 || d < 0) throw new InvalidOperationException("D must be between 0 and 100, but was " + d.ToString());


        }

        // распределение виртуальной базы для неполностью контролируемого заказа
        public static void distributeVBInNotFullyControlledOrder(ProvOrd order) {


        }

    }

}
