<Query Kind="Program" />

void Main()
{
	var context = new Context(new ImplAStrategy());
	context.ContextInterface();

	context = new Context(new ImplBStrategy());
	context.ContextInterface();
}

public abstract class Strategy
{ 
	public abstract void Interface();
}

public class ImplAStrategy : Strategy
{
	public override void Interface() =>
		"Invoke Interface() A".Dump("Implementation Strategy A");
}

public class ImplBStrategy : Strategy
{
	public override void Interface() => 
		"Invoke Interface() B".Dump("Implementation Strategy B");
}

public class Context
{ 
	private readonly Strategy strategy;
	
	public Context(Strategy strategy) => this.strategy = strategy;
	
	public void ContextInterface() => strategy.Interface();
}