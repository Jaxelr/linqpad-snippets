<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
	using var Source = new CancellationTokenSource();
	using var Source2 = new CancellationTokenSource(TimeSpan.FromSeconds(1));
	using var Combined = CancellationTokenSource.CreateLinkedTokenSource(Source.Token, Source2.Token);
	
	try 
	{
		await DoSomethingAsync(Combined.Token);
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