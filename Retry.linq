<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

//Quick lambda to retry execution n amount of times and backoff
void Main()
{
	Retry(3, TimeSpan.FromSeconds(2), () =>
	{
		
		
	});
}

// You can define other methods, fields, classes and namespaces here
public static void Retry(int times, TimeSpan delay, Action action)
{
	int retries = 0;
	int backoff = 1;

	while (true)
	{
		try 
		{
			retries++;
			action();
			break;
		}
		catch when (retries < times)
		{
			
			Task.Delay(delay*backoff).Wait();
			backoff+=backoff;
		}
		
	}
}