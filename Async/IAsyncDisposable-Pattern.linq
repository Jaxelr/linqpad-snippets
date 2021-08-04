<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
	var example = new Example();
	
	await using (example)
	{
		example.PrintDisposeState();
		Console.WriteLine("Inside Scope");
	}

	Console.WriteLine("Outside Scope");
	example.PrintDisposeState();
}

//Taken from this article -> https://itnext.io/how-to-properly-use-iasyncdisposable-in-c-8-3c7ec6dcc9fe
public class Example : IDisposable, IAsyncDisposable
{
	private bool _disposed;

	public void PrintDisposeState() => Console.WriteLine(_disposed);
	
	private Stream _memoryStream = new MemoryStream();

	public void Dispose()
	{
		_memoryStream.Dispose();
		_disposed = true;
		GC.SuppressFinalize(this);
	}

	async ValueTask IAsyncDisposable.DisposeAsync()
	{
		await _memoryStream.DisposeAsync();
		_disposed = true;
		GC.SuppressFinalize(this);
	}
}
