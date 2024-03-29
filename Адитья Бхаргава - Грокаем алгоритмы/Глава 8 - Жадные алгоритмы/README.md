# Глава 8 - Жадные алгоритмы
Жадные алгоритмы на каждом шаге выбираеют локально-оптимальное  решение, а в итоге мы получаем глобально-оптимальное решение. Иногда жадная стратегия не дает оптимального решения. Но результат не так уж далек от оптимума. __"Достаточно хорошего"__ решения должно хватить.

Иногда идеальное - враг хорошего. В некоторых случаях достаточно алгоритма, способного решить задачу достаточно хорошо. И в таких областях жадные алгоритмы работают просто отлично, потому что они просто реализуются, а полученное решение обычно близко к оптимуму.

Например, требуется перевезти в грузовике коробки разного размера, максимально эффективно использовав пространство. Жадный алгоритм предлагает на каждом этапе выбирать для погрузки __самую большую коробку__ и переходить к __следующей__, пока в грузовике есть место. Решение не является оптимальным.

Жадные алгоритмы хороши не только тем, что они обычно легко формулируются, но и тем, что простота обычно оборачивается быстротой выполнения.

## NP-полные задачи
Некоторые задачи прославились сложностью своего решения. Задача о коммивояжере и задача о покрытии множества - два классических примера. Многие эксперты считают, что написать быстрый алгоритм для решения таких задач невозможно. Лучше всего воспользоваться приближенным алгоритмом.

Не существует простого способа определить, является ли задача, с которой вы работаете, NP-полной. Несколько характерных признаков:
1. Ваш алгоритм быстро работает при малом количестве элементов, но сильно замедляется при увеличении их числа.
2. Формулировка "все комбинации Х" часто указывает на NP-полноту задачи.
3. Вам приходится вычислять все возможные варианты Х, потому что задачу невозможно разбить на меньшие подзадачи. Такая задача может оказаться NP-полной.
4. Если в задаче встречается некоторая последовательность (например, последовательность городов, как в задаче о коммивояжере) и задаче не имеет простого решения, она может оказаться NP-полной.
5. Если в задаче имеется некоторое множество (напрмиер, множество радиостанций) и задаче не имеет простого решения, она может оказаться NP-полной.
6. Можно ли переформулировать задачу в условиях задачи покрытия множества или задачи о коммивояжере? В таком случае ваша задача определенно является NP-полной.