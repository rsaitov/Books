# Глава 6 - Поиск в ширину

Поиск в ширину позволяет найти кратчайшее расстояние между двумя объектами.

Граф моделирует набор связей. Каждый граф состоит из узлов и ребер. Узел может быть напрямую соединен с несколькими другими узлами. Эти узлы называются __соседями__.

Алгоритм поиск в ширину отвечает на 2 вопроса:
1. Существует ли путь от узла А к узлу Б;
2. Как выглядит кратчайший путь от узла А к узлу Б.

Сначала проверяются связи 1го уровня, затем 2го итп. Для решения этой задачи может потребоваться использование структуры __очередь__. Очередь относится к категории структу данных FIFO (First In First Out). Стек принадлежит к структурам LIFO (Last In First Out).

## Реализация графа

Отношения в графе выражаются с помощью __хеш-таблиц__.

```C#
using System;
using System.Collections.Generic;

/// <summary>
/// поиска узла, отвечающего условиям		
/// </summary>
public class Program
{
	public static void Main()
	{
		var graph = new Dictionary<string, string[]>();
		graph["me"] = new string[] {"Alice", "Dob", "Clare"};
		graph["Alice"] = new string[] { "Daniel", "Ernest", "me" }; //цикличная ссылка me-Alice
		graph["Dob"] = new string[] { "Ernest", "Forrest" };
		graph["Clare"] = new string[] {};
		graph["Daniel"] = new string[] {};
		graph["Ernest"] = new string[] {};
		graph["Forrest"] = new string[] {};
		
		var seller = GetSeller(graph);
		
		if (seller == null)
			Console.WriteLine("There is no seller");
		else
			Console.WriteLine("The seller is: " + seller);
	}
	
    /// <summary>
    /// Метод поиска узла, отвечающего признаку
    /// </summary>
	private static string GetSeller(Dictionary<string, string[]> graph)
	{
		var checkQueue = new Queue<string>();
		var checkedItems = new HashSet<string>();
		
		checkQueue.Enqueue("me");		
		
		while (checkQueue.Count > 0)
		{			
			var checkItem = checkQueue.Dequeue();
			if (checkedItems.Contains(checkItem))
				continue;
				
			checkedItems.Add(checkItem);
			
			if (IsSeller(checkItem))
				return checkItem;
			
			foreach (var childItem in graph[checkItem])
				checkQueue.Enqueue(childItem);			
		}
		
		return null;
	}
	
    /// <summary>
    /// Проверка произвольного признака
    /// </summary>
	private static bool IsSeller(string checkItem) 
	{ 
		return checkItem[0] == 'B';				
	}
}
```

## Время выполнения

Поиск в ширину выполняется за время О (количество людей + количество ребёр), что обычно записывается в форме O(V+E) (V - количество вершин, E - количество ребер).

## Деревья

Деревьями называются разновидности графов, у которых нет ребер, указывающих в обратном направлении.