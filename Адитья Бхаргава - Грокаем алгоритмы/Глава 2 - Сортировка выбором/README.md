# Глава 2 - Сортировка выбором
При использовании связанного списка элементы могут размещаться где угодно в памяти. Элементы в массиве располагаются в памяти строго по порядку.

Суть алгоритма - пройти по списку и найти самый большой элемент, поместить его в новый список.То же самое происходит со следующим по количеству воспроизведений исполнителем. Продолжая действовать так, мы получаем отсортированный список. Сложность данного алгоритма - **О(n & 2)**. 

Альтернативная реализация алгоритма предполагает не помещать элементы в новый список, а менять их местами с первым элементом. Таким образом с каждым проходом неотсортированная часть массива уменьшается.

```C#
public class Program
{
	public static void Main()
	{
		var list = new int[]{ 7, 0, -4, 3, 1, -2, 5 };
		Console.WriteLine("list: " + string.Join(" ", list));
		var sortedList = SelectionSort(list);
		Console.WriteLine("sortedList: " + string.Join(" ", sortedList));
	}
	
	public static int[] SelectionSort(int[] list)
	{
		var startIndex = 0;
		
		while (startIndex < list.Length)
		{			
			var smallest = list[startIndex];
			var smallest_index = startIndex;

			for (int i=startIndex; i< list.Length; i++)
			{
				if (list[i] < smallest)
				{
					smallest = list[i];
					smallest_index = i;
				}
			}
			
			var temp = list[startIndex];
			list[startIndex] = list[smallest_index];
			list[smallest_index] = temp;
			
			startIndex++;
		}
		
		return list;
	}
}
```