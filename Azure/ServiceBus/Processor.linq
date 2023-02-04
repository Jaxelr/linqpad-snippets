<Query Kind="Program">
  <NuGetReference>Azure.Messaging.ServiceBus</NuGetReference>
  <Namespace>Azure.Messaging.ServiceBus</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
	try
	{
		var client = new ServiceBusClient("{KeyGoesHere}");
		await using var processor = client?.CreateProcessor("{Topic}", "{Subscription}", new ServiceBusProcessorOptions()
		{
			ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete,
			AutoCompleteMessages = false,
			MaxConcurrentCalls = 1
		});

		processor.ProcessMessageAsync += ProcessMessageAsync;
		processor.ProcessErrorAsync += ProcessError;

		await processor.StartProcessingAsync();
		
		Console.Read(); //Do this to keep listening 
	}
	catch (Exception ex)
	{
		ex.Dump();		
	}
}

private async Task ProcessMessageAsync(ProcessMessageEventArgs args)
{
	string message = args.Message.Body.ToString();
}

private async Task ProcessError(ProcessErrorEventArgs args)
{
	args.Exception.Message.Dump();
}
