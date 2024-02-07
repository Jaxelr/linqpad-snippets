<Query Kind="Expression" />

//Foreach for all
public static async Task ForEach<T>(this IEnumerable<T> items, Func<T, Task> action)
{
	foreach (var item in items)
		await action(item);
}
