# Packman-like-Project
Что из тестового задания сделано:
1) Генерация лабиринта. Лабиринт генерируется квадратный, размер стороны и стен можно задать в ScriptableObject файле MazeSettings. Задание настройки сделано с помощью CustomEditor.
При снижении размера лабиринта снижается допустимое максимальное кол-во стен в нём, которое можно задать настройкой.
2) Спавн персонажа. Настройки спавна можно задать в файле PlayerSettings
3) Спавн гороха. Аналогично настройки в PeaseSettings. Для спавна реализован паттерн Object pool. Т.к. мы используем Ecs решил пул записать не как абстрактный класс, а как компонент PeasePoolObjectsComponent.
В настройках есть флаг, который отключает или включает спавн гороха раз в несколько секунд. Задание настройки сделано с помощью CustomEditor.
4) Подбор гороха, когда герой наступает на клетку где горох есть. Реализовано с помощью статуса (простой enum). Каждая клетка находится в одном из статусов (на ней игрок, на ней горох, она пустая).
Исчезновение гороха происходит при смене статуса клетки.

Что  не сделано:
1) Пункт MVP+ со спавном врагов и смертью героя
2) Камера. Камера статична, для тестового запуска проще смотреть через Editor.
3) Нет звуков.

Что можно улучшить:
1) Сделать некие хелперы для некоторых часто используемых эвентов (например CellStatusChangeEventComponent). 
Что-то вроде статичного метода, который можно вызвать откуда угодно и чтобы он отправлял сообщение об этом эвенте. 
Это снизит дублирование кода.
2) В теории сделать спавн клеток и гороха как новые entity, все данные заполнять в компонент (CellComponent, PeaseComponent). 
В данный момент создаётся только entity для спавнера и у него хранятся все данные об объектах, которые он заспавнил (MazeComponent, PeasePoolObjectsComponent).
3) Вынести настройки в Json/БД/Любой другой внешний ресурс, чтобы можно было не билдить постоянно проект заново при изменении настроек
(для тестового проекта считаю, что слишком затратно по времени и того не стоит).
4) В данный момент система управления считывает одно нажатие. Т.е. при каждом сдвиге на одну клетку нужно заново нажать клавишу.
Если использовать настройку "зажатия", то игрок начинает летать с невообразимой скоростью. Возможно сделать зажатие, но поставить паузу (доработать таймером систему ввода).
5) Сделать плавное перемещение по клеткам. Пока это просто сдвиг из центра одной клетки в центр другой.
