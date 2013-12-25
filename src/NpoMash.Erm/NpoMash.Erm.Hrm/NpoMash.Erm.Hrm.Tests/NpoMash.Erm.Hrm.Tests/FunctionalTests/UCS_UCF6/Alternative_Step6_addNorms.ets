#DropDB NpoMash.Erm.Hrm.Test
#Application NpoMash.Erm.Hrm.Tests.Win
*Action Добавить тестовые данные
*Action Обновить
*Action Навигация(Интеграция с А1.Списание затрат.Параметры расчета)
*Action Создать
*Action Утвердить
*Action Закрыть
*Action Обновить

*Action Навигация(Интеграция с А1.Списание затрат.Параметры расчета)
;*CompareScreenshot

*ProcessRecord 
 Статус контроля = Утвержден список контроллируемых заказов
;*CompareScreenshot

*Action Контроль заказа.Создать
;*CompareScreenshot

*ExecuteEditorAction Заказ
*Action Создать
*FillForm
 Код = 1111
 Норма КБ = 200
 Норма ОЗМ = 2000
*Action Сохранить и закрыть
;*CompareScreenshot

*Action Сохранить
;*CompareScreenshot

*Action Закрыть