<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
	int value = -1;
	value = await GetNumberValue("1").Fallback(0);
	value.Dump();
	value = await GetNumberValue("Uno").Fallback(0);
	value.Dump();
}

public async Task<int> GetNumberValue(string input)
{
	if (int.TryParse(input, out int result))
	{
		return result;
	}
	
	throw new InvalidCastException("Not a Number");
}

public static class TasksExtensions
{
	public static async Task<TResult> Fallback<TResult>(this Task<TResult> task, TResult fallbackValue)
	{
		try
		{
			return await task.ConfigureAwait(false);
		}
		catch
		{
			return fallbackValue;
		}
	}
}