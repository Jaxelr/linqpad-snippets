<Query Kind="Statements">
  <NuGetReference>Microsoft.Extensions.Hosting.Abstractions</NuGetReference>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Microsoft.Extensions.Hosting</Namespace>
</Query>

public class BackgroundTimedService : BackgroundService
{
	protected override async Task ExecuteAsync(CancellationToken stoppingToken) => await HelloWorld(stoppingToken);
	
	private async Task HelloWorld(CancellationToken token)
	{
		while (!token.IsCancellationRequested)
		{
			"Hello World".Dump();	
			
			await Task.Delay(TimeSpan.FromSeconds(1));
		}
		
		
	}
}