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

*Action Код оплаты
;*CompareScreenshot

*Action Код оплаты.Создать
;*CompareScreenshot

*ExecuteEditorAction Код оплаты
*Action Создать
*FillForm
 Код = 1111
*Action Сохранить и закрыть
;*CompareScreenshot

*Action Сохранить
;*CompareScreenshot