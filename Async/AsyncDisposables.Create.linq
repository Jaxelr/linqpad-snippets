<Query Kind="Program">
  <NuGetReference>System.Reactive</NuGetReference>
  <Namespace>System.Reactive.Disposables</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

/// <summary>Scope code that executes at the end of the scope</summary>
async Task Main()
{
	Sync();
	await Async();
}

public void Sync()
{
	/* Sync Disposable */
	Console.WriteLine("Starting Sync...");
	using var scope = Disposable.Create(() => Console.WriteLine("Done Second!")); /* executes last */
	Console.WriteLine("Working Sync..."); /* This executes first on the scope */
	using var _ = Disposable.Create(() => Done("First!")); /* executes next */
}

public async ValueTask Async()
{
	/* Async Disposable */
	Console.WriteLine("Starting Async...");
	await using var _ = AsyncDisposable.Create(() => DoneAsync("Done")); /* executes last */
	Console.WriteLine("Working..."); /* This executes first on the scope */
}

public void Done(string value) =>  Console.WriteLine($"Done {value}");
public async ValueTask DoneAsync(string value) => await Task.Run (() => Console.WriteLine(value));


#nullable enable
/// <summary>
/// Helper class for creating an asynchronous scope.
/// A scope is simply a using block that calls an async method
/// at the end of the block by returning an <see cref="IAsyncDisposable"/>.
/// This is the same concept as
/// the <see cref="Disposable.Create"/> method.
/// </summary>
public static class AsyncDisposable
{
	/// <summary>
	/// Creates an <see cref="IAsyncDisposable"/> that calls
	/// the specified method asynchronously at the end
	/// of the scope upon disposal.
	/// </summary>
	/// <param name="onDispose">The method to call at the end of the scope.</param>
	/// <returns>An <see cref="IAsyncDisposable"/> that represents the scope.</returns>
	public static IAsyncDisposable Create(Func<ValueTask> onDispose)
	{
		return new AsyncScope(onDispose);
	}

	class AsyncScope : IAsyncDisposable
	{
		Func<ValueTask>? _onDispose;

		public AsyncScope(Func<ValueTask> onDispose)
		{
			_onDispose = onDispose;
		}

		public ValueTask DisposeAsync()
		{
			return Interlocked.Exchange(ref _onDispose, null)?.Invoke() ?? ValueTask.CompletedTask;
		}
	}
}
