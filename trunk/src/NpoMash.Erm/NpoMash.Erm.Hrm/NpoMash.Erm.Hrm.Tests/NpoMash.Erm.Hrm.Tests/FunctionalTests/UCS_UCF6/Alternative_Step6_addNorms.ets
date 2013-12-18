#DropDB NpoMash.Erm.Hrm.Test
#Application NpoMash.Erm.Hrm.Tests.Win
*Action Добавить тестовые данные
*Action Обновить

*Action Навигация(Интеграция с А1.Списание затрат.Параметры расчета)
*Action Создать параметры расчета
*Action Утвердить параметры расчета
*Action Закрыть
*ProcessRecord 
 Статус контроля = ListOfOrderAccepted
*Action Контроль заказа
*Action Контроль заказа.Создать
*FillForm
 Заказ = 53514
*Action Сохранить