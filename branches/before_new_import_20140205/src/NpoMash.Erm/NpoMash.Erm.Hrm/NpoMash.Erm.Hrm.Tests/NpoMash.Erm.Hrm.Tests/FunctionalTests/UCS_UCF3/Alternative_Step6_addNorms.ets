#DropDB NpoMash.Erm.Hrm.Test
#Application NpoMash.Erm.Hrm.Tests.Win
*Action Добавить тестовые данные
*Action Обновить

*Action Навигация(Интеграция с А1.Списание затрат.Параметры расчета)
;*CompareScreenshot

*Action Создать
;*CompareScreenshot

*Action Контроль заказа
;*CompareScreenshot

*Action Контроль заказа.Создать
;*CompareScreenshot

*FillForm
 Заказ = 90139
;*CompareScreenshot

*Action Сохранить
;*CompareScreenshot