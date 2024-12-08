# Greenhouse
Проект "Теплица" по курсу технологии программирования.

## Замечания по поводу работы программы
При запуске программа выдаст окошко Проводника Windows, нужно будет выбрать текстовый файл с планом выращивания. Он находится в корневой папке проекта с названием "Plan.txt".

Чтобы система назначила сенсорам начальные параметры, нужно поставить сесоры с самого начала, когда прошло "0d 0h", иначе сенсоры начнут отсчёт с нуля, что приведёт к нереалистичной логике программы.

## Управление графическими элементами, добавляемыми с помощью кнопок
* Левая кнопка мыши (зажать и держать) - перетаскивание элемента.
* Колёсико мыши - повернуть элемент на 90 градусов влево.
* Правая кнопка мыши - удалить элемент.

P.S. Из-за специфики работы используемых классов, текст, размещённый на графических элементах, мешает управлять ими, поэтому может понадобиться навестись на край графического элемента, чтобы управлять им.
