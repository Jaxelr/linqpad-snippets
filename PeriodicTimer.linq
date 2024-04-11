<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
	//A Simpler way of using a timer instead for recurrent periods of time.
	PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(2));
	
	int counter = 1;

	while (await timer.WaitForNextTickAsync())
	{
		$"Ding {counter} times...".Dump();
		counter++;

		if (counter >= 6)
		{
			break;			
		}
	}
}
