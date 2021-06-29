<Query Kind="Program" />

void Main()
{
	int state = 0;
	
	var factory = new FlyweightFact();
	
	var fly1 = factory.GetFly("Key1");
	fly1.Operation(++state);
	
	var fly2 = factory.GetFly("Key2");
	fly2.Operation(++state);
	
	var fly3 = factory.GetFly("Key3");
	fly3.Operation(++state);
	
	var hiddenFly = new HiddenFlyweight();
	hiddenFly.Operation(++state);
}

public class FlyweightFact
{ 
	private Dictionary<string, Flyweight> flies = new();
	
	public FlyweightFact()
	{
		flies.Add("Key1", new ImplFlyweight());
		flies.Add("Key2", new ImplFlyweight());
		flies.Add("Key3", new ImplFlyweight());
	}

	public Flyweight GetFly(string key) => flies[key];
	
}

public abstract class Flyweight
{ 
	public abstract void Operation(int state);
}

public class ImplFlyweight : Flyweight
{
	public override void Operation(int state) =>
		$"Implemented Flyweight: {state}".Dump();
}

public class HiddenFlyweight : Flyweight
{
	public override void Operation(int state) =>
		$"Hidden Flyweight: {state}".Dump();
}

