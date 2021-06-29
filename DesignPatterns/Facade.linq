<Query Kind="Program" />

void Main()
{
	var facade = new Facade();
	
	facade.Method();
}

public class Facade
{ 
	private Subsystem1 subsystem1;	
	private Subsystem2 subsystem2;
	
	public Facade()
	{
		subsystem1 = new Subsystem1();
		subsystem2 = new Subsystem2();
	}

	public void Method()
	{
		"Invoked facade".Dump("Facade");
		
		subsystem1.Method1();
		subsystem2.Method2();
	}
}

public class Subsystem1
{
	public void Method1() => "Invoked subsystem1".Dump("Subsystem1");
}

public class Subsystem2
{ 
	public void Method2() => "Invoked subsystem2".Dump("Subsystem2");
}
