<Query Kind="Program" />

/* yield returns the control to the caller until further executions are sent */
/* textbook scenario is IEnumerable<T> processing (Lazy loading) */

void Main()
{
	List<int> numbersList = new List<int> {
		 1, 2, 3, 4, 5
	  };

	foreach (int i in RunningTotal())
	{
		Console.WriteLine(i);
	}

	IEnumerable<int> RunningTotal()
	{
		int runningTotal = 0;
		foreach (int i in numbersList)
		{
			runningTotal += i;
			yield return (runningTotal);
		}
	}
}

