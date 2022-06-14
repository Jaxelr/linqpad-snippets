<Query Kind="Program" />

void Main()
{
	//Ctrl + Shift + F5 to cancel all threads
	//Run on another thread the timer delegate
}

// You can define other methods, fields, classes and namespaces here
static Timer timer = new Timer(GetPoco, null, 0, Timeout.Infinite);
static Poco poco = new Poco();

public static void GetPoco(object state)
{
	Thread.Sleep(1000);

	lock (poco)
	{
		poco = new Poco();
	}
	
	poco.Dump();
	timer.Change(1000, Timeout.Infinite);
}

public class Poco
{ 
	public int Id { get; set; }
	public string Value { get; set; }
	public DateTime CurrentTime { get; set; } = DateTime.Now;
}