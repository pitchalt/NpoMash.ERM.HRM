#DropDB NpoMash.Erm.Hrm.Test
#Application NpoMash.Erm.Hrm.Tests.Win
*Action Добавить тестовые данные
*Action Обновить

*Action Навигация(Интеграция с А1.Списание затрат.Параметры расчета)
;*CompareScreenshot

*Action Создать параметры расчета
;*CompareScreenshot

*Action Утвердить параметры расчета
;*CompareScreenshot

*Action Закрыть
;*CompareScreenshot

*ProcessRecord 
 Статус контроля = ListOfOrderAccepted
;*CompareScreenshot

*Action Закрыть
;*CompareScreenshot