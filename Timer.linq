<Query Kind="Program" />

void Main()
{
	//This timer will run forever, but the thread will be kept in front
	var timer = new System.Threading.Timer((e) => Create(), null, TimeSpan.Zero, TimeSpan.FromSeconds(2));
	Thread.Sleep(Timeout.Infinite);
}
// Main-Thread scenario
// This is all called from main.
public static class Sample
{ 
	public static DateTime timeout { get; set; } = DateTime.UtcNow;
}
public void Create()
{
	Sample.timeout = DateTime.UtcNow;
	Sample.timeout.Dump();
}

// Non-Main-Thread
// This is all called outside the main thread, but below the Linqpad class
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