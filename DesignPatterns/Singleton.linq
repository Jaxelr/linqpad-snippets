<Query Kind="Program" />

void Main()
{
	var singleton = Singleton.Get;

	singleton.Dump();
	singleton.GetCounter().Dump();
	
	singleton.Set("Stringy");

	singleton.Dump();
	singleton.GetCounter().Dump();

	singleton = Singleton.Get;	
	
	singleton.Set(12);
	
	singleton.Dump();
	singleton.GetCounter().Dump();

}

// You can define other methods, fields, classes and namespaces here
public sealed class Singleton
{
	private static int counter = 0;

	public string Field1 { get; private set; }
	public int Field2 { get; private set; }

	private static readonly Singleton instance = new Singleton();

	private Singleton()
	{
		//initialized
		counter++;
	}

	public static Singleton Get => instance;

	public void Set(string Field1)
	{
		this.Field1 = Field1;
	}

	public void Set(int Field2)
	{
		this.Field2 = Field2;
	}

	public int GetCounter() => counter;
}