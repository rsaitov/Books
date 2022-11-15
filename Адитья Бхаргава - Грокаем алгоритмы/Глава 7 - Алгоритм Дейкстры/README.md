# Глава 7 - Алгоритм Дейкстры
Поиск в ширину находит путь с минимальным количеством сегментов. Если нужно найти самый быстрый путь - поможет __алгоритм Дейкстры__.

Каждому ребру графа назначается время перемещения в минутах. Алгоритм Дейкстры используется для поиска пути от начальной точки к конечной за кратчайшее возможное время.

Алгоритм состоит из 4х шагов:
1. Найти узел с наименьше стоимостью (то есть узел, до которого можно добраться за минимальное время).
2. Проверить, существует ли более дешевый путь к соседям этого узла, и если существует, обновить их стоимости.
3. Повторять, пока это не будет сделано для всех услов графа.
4. Вычислить итоговый путь.

С каждым ребром графа связано число, называемое __весом__.
Граф с весами называется __взвешенным графом__. Граф без весов называется __невзвешенным графом__.

Для вычисления кратчайшего путь в невзвешенном графе используется __поиск в ширину__. Кратчайшие пути во взвешенном графе вычисляются по алгоритму Дейкстры.

Алгоритм Дейкстры работает только с __направленными ациклическими графами__, которые нередко обозначаются сокращением DAG (Directed Acyclic Graph).

Использование алгоритма Дейкстры с графом, содержащим ребра с отрицательным весом, невозможно. При наличии отрицательных весов используйте алгоритм Беллмана-Форда.

```C#
using System;
using System.Collections.Generic;
					
public class Program
{
	public static void Main()
	{
		var graph = new Dictionary<string, Dictionary<string, int>> 
		{
			{"start", new Dictionary<string, int> {
					{"a", 6}, 
					{"b", 2}
				}
			},
			{ "a", new Dictionary<string, int> {
					{"fin", 1}
				}
			},
			{ "b", new Dictionary<string, int> {
					{"a", 3},
					{"fin", 5}
				}
			},
			{ "fin", new Dictionary<string, int>() }
		};
		
		var costs = new Dictionary<string, int>{
			{"a", 6},
			{"b", 2},
			{"fin", Int32.MaxValue}
		};
		
		var parents = new Dictionary<string, string>{
			{"a", "start"},
			{"b", "start"},
			{"fin", null}
		};
		
		var processed = new HashSet<string>();
		
		var node = FindLowestCostNode(costs, processed);
		while (node != null)
		{
			var cost = costs[node];
			var neighbors = graph[node];
			foreach(var n in neighbors)
			{
				var new_cost = cost + neighbors[n.Key];
				if (costs[n.Key] > new_cost)
				{
					costs[n.Key] = new_cost;
					parents[n.Key] = node;
				}
			}
			processed.Add(node);
			node = FindLowestCostNode(costs, processed);
		}
		
		Console.WriteLine("The lowest price for \"fin\" is: " + costs["fin"]);
	}
	
	private static string FindLowestCostNode(Dictionary<string, int> costs, HashSet<string> processed)
	{
		string lowestNode = null;
		int lowestCost = Int32.MaxValue;
		
		foreach(var node in costs)
		{
			if (processed.Contains(node.Key))
				continue;
			
			var cost = node.Value;		
			if (cost < lowestCost)
			{
				lowestNode = node.Key;
				lowestCost = cost;				
			}
		}
		
		return lowestNode;
	}
}
```