<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

//Quick lambda to retry execution n amount of times and backoff
async Task Main()
{
	Retry(3, TimeSpan.FromSeconds(2), () =>
	{
	});
	
	await RetryAsync(3, TimeSpan.FromSeconds(2), () =>
	{
		//We cant return void for async
		return Task.FromResult(0);
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

public static async Task RetryAsync(int times, TimeSpan delay, Func<Task> func)
{
	int retries = 0;
	int backoff = 1;

	while (true)
	{
		try
		{
			retries++;
			await func();
			break;
		}
		catch when (retries < times)
		{
			await Task.Delay(delay * backoff);
			backoff += backoff;
		}
	}
}