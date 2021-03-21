<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{
	//This method awaits all Tasks and blocks the thread.
	WaitAll();
	
	//This method awaits only the first Task finished.
	Task.Run (() => WhenAny());
	
	//This method awaits all tasks but it doesnt block the thread.
	Task.Run (() => WhenAll());
}

// Synchronously Wait all tasks in parallel.
public void WaitAll()
{
	var TaskOne = CallMe("One");
	var TaskTwo = CallMe("Two");
	var TaskThree = CallMe("Three");

	Task.WaitAll(TaskOne, TaskTwo, TaskThree);

	TaskOne.Result.Dump("Should be 1");
	TaskTwo.Result.Dump("Should be 2");
	TaskThree.Result.Dump("Should be 3");
}

// Asynchronously (returns Task) Wait all tasks in parallel.
public async Task WhenAll()
{
	var TaskOne = CallMe("One");
	var TaskTwo = CallMe("Two");
	var TaskThree = CallMe("Three");
	
	var awaited = await Task.WhenAll(TaskOne, TaskTwo, TaskThree);

	TaskOne.Result.Dump("Should be 1");
	TaskTwo.Result.Dump("Should be 2");
	TaskThree.Result.Dump("Should be 3");
}

// Asyncrhornously await any of the task to finish in parallel.
public async Task WhenAny()
{
	var TaskOne = CallMe("One");
	var TaskTwo = CallMe("Two");
	var TaskThree = CallMe("Three");

	var hey = await Task.WhenAny(TaskOne, TaskTwo, TaskThree);

	hey.Result.Dump("Should be 1");
}

public async Task<int> CallMe(string aNumber)
{
	("Method Was Called.").Dump();	
	
	return await Task.Factory.StartNew(() =>
	{
		if (aNumber == "One")
		{
			Thread.Sleep(1000);
			return 1;
		}
		if (aNumber == "Two")
		{
			Thread.Sleep(2000);
			return 2;
		}
		if (aNumber == "Three")
		{
			Thread.Sleep(3000);
			return 3;
		}
			
		return 0; 
	});
}