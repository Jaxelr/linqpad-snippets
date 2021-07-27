<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Runtime.CompilerServices</Namespace>
</Query>

void Main()
{
	var list = new List<Task<int>>();

	list.Add(Task.FromResult(1));
	list.Add(Task.FromResult(2));
	list.Add(Task.FromResult(3));
	
	var t = list.ParallelEnumerateAsync();	
	
	var result = t.Where(l => l == 2);
	
	result.Dump();

}

public static class LinqExtensions
{
	public static async IAsyncEnumerable<T> ParallelEnumerateAsync<T>(
		this IEnumerable<Task<T>> tasks)
	{
		var remaining = new List<Task<T>>(tasks);

		while (remaining.Count != 0)
		{
			var task = await Task.WhenAny(remaining);
			remaining.Remove(task);
			yield return (await task);
		}
	}
	
	public static IAsyncEnumerable<T> Where<T>(this IAsyncEnumerable<T> source, Func<T, bool> predicate)
	{
		if (source is null) throw new ArgumentNullException(nameof(source));
		if (predicate is null) throw new ArgumentNullException(nameof(predicate));

		return Core();

		async IAsyncEnumerable<T> Core([EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
			{
				if (predicate(item))
				{
					yield return item;
				}
			}
		}
	}

	public static IAsyncEnumerable<T> WhereAwait<T>(this IAsyncEnumerable<T> source, Func<T, ValueTask<bool>> predicate)
	{
		if (source is null) throw new ArgumentNullException(nameof(source));
		if (predicate is null) throw new ArgumentNullException(nameof(predicate));

		return Core();

		async IAsyncEnumerable<T> Core([EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
			{
				if (await predicate(item).ConfigureAwait(false))
				{
					yield return item;
				}
			}
		}
	}

	public static IAsyncEnumerable<T> WhereAwaitWithCancellation<T>(this IAsyncEnumerable<T> source, Func<T, CancellationToken, ValueTask<bool>> predicate)
	{
		if (source is null) throw new ArgumentNullException(nameof(source));
		if (predicate is null) throw new ArgumentNullException(nameof(predicate));

		return Core();

		async IAsyncEnumerable<T> Core([EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
			{
				if (await predicate(item, cancellationToken).ConfigureAwait(false))
				{
					yield return item;
				}
			}
		}
	}
}

