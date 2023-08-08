<Query Kind="Program" />

//This is a sample usage of a Semaphore class to control access to lockeable resources by N entities at the same time, while allowing read access to M entities.
void Main()
{
	SemaphoreUsage.Execute();
}

public static class SemaphoreUsage
{
	public static readonly Semaphore semaphore = new(2, 4);
	public static void Execute()
	{
		for (var cnt = 0; cnt < 6; cnt++)
		{
			Thread thread = new(DoWork)
			{
				Name = "Thread " + cnt
			};
			thread.Start();
		}
	}
	private static void DoWork()
	{
		try
		{
			Console.WriteLine($"Thread {Thread.CurrentThread.Name} waits the lock");
			semaphore.WaitOne();
			Console.WriteLine($"Thread {Thread.CurrentThread.Name} enters critical section");
			Thread.Sleep(500);
			Console.WriteLine($"Thread {Thread.CurrentThread.Name} exits critical section");
		}
		finally
		{
			Console.WriteLine($"Thread {Thread.CurrentThread.Name} releases the lock");
			semaphore.Release();
		}
	}
}