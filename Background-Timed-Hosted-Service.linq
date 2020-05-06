<Query Kind="Program">
  <NuGetReference>Microsoft.Extensions.Hosting.Abstractions</NuGetReference>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Microsoft.Extensions.Hosting</Namespace>
</Query>

void Main()
{
	
}

public class BackgroundTimedHostedService : IHostedService
{
	private Timer timer;
	
	public Task StartAsync(CancellationToken cancellationToken)
	{
		timer = new Timer(HelloWorld, null, 0, 1000);
		return Task.CompletedTask;
	}

	private void HelloWorld(object state)
	{
		"Hello World".Dump();
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		//New Timer does not have a stop.
		timer?.Change(Timeout.Infinite, 0);
		return Task.CompletedTask;
	}
}