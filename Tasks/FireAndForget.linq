<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{
	Invoke().FireAndForget(error => error.Message.Dump());
}

public async Task Invoke()
{
	await Task.Delay(1);
	("I'm Fired and Forgotten").Dump();
}

//Taken from this post: https://steven-giesel.com/blogPost/d38e70b4-6f36-41ff-8011-b0b0d1f54f6e
public static class TaskExtensions
{
	public static void FireAndForget(this Task task, Action<Exception> errorHandler = null)
	{
		task.ContinueWith(t => 
		{
		   if (t.IsFaulted && errorHandler is not null && t.Exception is not null) 
		   	   errorHandler(t.Exception);
		}, TaskContinuationOptions.OnlyOnFaulted);
	}

}