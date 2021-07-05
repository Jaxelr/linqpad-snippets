<Query Kind="Program" />

void Main()
{
	var abs = new Subclass();
	
	abs.TemplateMethod();
}

public abstract class Abstraction
{
	public abstract void Operation1();
	public abstract void Operation2();

	public void TemplateMethod()
	{
		Operation1();	
		Operation2();
	}
}

public class Subclass : Abstraction
{
	public override void Operation1() => "Sub class operation 1".Dump("Concrete");
	public override void Operation2() => "Sub class operation 2".Dump("Concrete");
}

