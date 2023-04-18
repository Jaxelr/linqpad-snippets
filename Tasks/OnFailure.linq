<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
	await Invoke().OnFailure(error => error.Message.Dump());
}

public async Task Invoke()
{
	await Task.Delay(1);
	throw new Exception("Im Exceptioned");
}

//Taken from this post: https://steven-giesel.com/blogPost/d38e70b4-6f36-41ff-8011-b0b0d1f54f6e
public static class TaskExtensions
{
	public static async Task OnFailure(this Task task, Action<Exception> errorHandler)
	{
		try
		{
			await task.ConfigureAwait(false);
		}
		catch (Exception ex)
		{
			errorHandler(ex);
		}
	}
}