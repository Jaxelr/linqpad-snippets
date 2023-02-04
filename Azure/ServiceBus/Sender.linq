<Query Kind="Program">
  <NuGetReference>Azure.Messaging.ServiceBus</NuGetReference>
  <Namespace>Azure.Messaging.ServiceBus</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
	try
	{
		const int size = 5000;

		var client = new ServiceBusClient("{KeyGoesHere}");

		while (true)
		{
			var tasks = new Task[size];
			for (int i = 0; i < size; i++)
			{
				tasks[i] = Submit();
			}
			
			await Task.WhenAll(tasks);
		}
		
		async Task Submit()
		{
			var sender = client?.CreateSender("{Topic}");
			var sample = new Sample(1, "Sample");

			var message = new ServiceBusMessage(BinaryData.FromObjectAsJson(sample));
			await sender?.SendMessageAsync(message)!;
			
			await sender.CloseAsync();
			Thread.Sleep(100);
		}
	}
	catch (Exception ex)
	{
		ex.Dump();		
	}
}

public record Sample(int Id, string Name)
{ 
	public int Id { get; init; }
	public string? Name { get; init; }
} 