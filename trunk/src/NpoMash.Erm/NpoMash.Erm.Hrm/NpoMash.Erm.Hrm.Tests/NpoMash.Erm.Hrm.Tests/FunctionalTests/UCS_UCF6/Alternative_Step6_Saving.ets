#DropDB NpoMash.Erm.Hrm.Test
#Application NpoMash.Erm.Hrm.Tests.Win
*Action Добавить тестовые данные
*Action Обновить

*Action Навигация(Интеграция с А1.Списание затрат.Параметры расчета)
;*CompareScreenshot

*Action Создать
;*CompareScreenshot

*Action Утвердить
;*CompareScreenshot

*Action Закрыть
;*CompareScreenshot

*Action Обновить
;*CompareScreenshot

*Action Навигация(Интеграция с А1.Списание затрат.Параметры расчета)
;*CompareScreenshot

*ProcessRecord 
 Статус контроля = Утвержден список контроллируемых заказов
;*CompareScreenshot

*Action Сохранить