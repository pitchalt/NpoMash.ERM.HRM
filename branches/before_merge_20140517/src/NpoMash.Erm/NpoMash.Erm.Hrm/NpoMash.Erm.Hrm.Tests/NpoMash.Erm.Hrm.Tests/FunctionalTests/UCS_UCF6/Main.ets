#DropDB NpoMash.Erm.Hrm.Test
#Application NpoMash.Erm.Hrm.TestsWin
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

*ProcessRecord 
 Статус контроля = ListOfOrderAccepted
;*CompareScreenshot

*Action Утвердить