<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
	using var Source = new CancellationTokenSource(TimeSpan.FromSeconds(2));
	
	try 
	{
		await DoSomethingAsync(Source.Token);
	}
	catch (OperationCanceledException cancel)
	{
		cancel.Message.Dump();
	}

	Console.WriteLine("Completed...");
}

async Task DoSomethingAsync(CancellationToken token)
{
	int counter = 0;
	while (true)
	{
		token.ThrowIfCancellationRequested();
		Thread.Sleep(1000);
		counter++;
		await Task.Run (() => Console.WriteLine($"Placeholder...{counter}"));
	}
}