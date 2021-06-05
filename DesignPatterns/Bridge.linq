<Query Kind="Program" />

void Main()
{
	var abstraction = new Abstraction();
	
	abstraction.Implementor = new ConcreteImplementor();
	
	abstraction.Operation();
}

abstract class Implementor
{ 
	public abstract void Operation();
}

class ConcreteImplementor : Implementor
{
	public override void Operation()
	{
		"Concrete operation done.".Dump("Implementor");
	}
}

class Abstraction
{ 
	protected Implementor implementor;

	public Implementor Implementor { set { implementor = value; } }
	
	public virtual void Operation()
	{
		implementor.Operation();
	}
}

