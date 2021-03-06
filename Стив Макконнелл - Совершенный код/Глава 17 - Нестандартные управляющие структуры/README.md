## Глава 17 - Нестандартные управляющие структуры
### Множественные возвраты из метода
- используйте return, если это повышает читабельность;
- упрощайте сложную обработку ошибок с помощью сторожевых операторов (досрочных return и exit);
- минимизируйте число возвратов из каждого метода.
### Рекурсия
При рекурсии метод решает небольшую задачу, разбивает задачу на меньшие порции и вызывает сам себя для решения каждой из этих порций. Применяйте рекурсию выборочно, так как для разного размера задач он может как давать элегантное и простое решение, так и исключительно запутанные.

Советы по использованию рекурсии:
- убедитесь, что рекурсия остановится;
- предотвращайте бесконечную рекурсию с помощью счетчиков безопасности;
- ограничьте рекурсию одним методом;
- не используйте рекурсию для факториалов и чисел Фибоначчи. Гораздо понятнее итеративный вариант;
- рассмотрите другие варианты, прежде чем применять рекурсию.
### Оператор goto
Код без операторов goto - более качественный. С goto - трудно форматировать, усложняет анализ кода и уменьшает возможность оптимизации. Нарушает принцип, что нормальный ход алгоритма должен быть строго сверху вниз.
Стратегии преобразования кода с оператором goto:
- переписать с помощью вложенных операторов if. Глубоко вложенные if-else;
- переписать с использованием статусной переменной. Добавляет дополнительные проверки. Позволяет избежать глубоко вложенных структур if-else. Легче для понимания, потому что лучше моделирует способ человеческого мышления.
- переписать с помощью try-finally. Доступен не во всех языках.
Иногда операторы goto - лучший способ облегчить чтение и сопровождение кода. Таких случаев очень немного. Используйте goto только как последнее средство.
