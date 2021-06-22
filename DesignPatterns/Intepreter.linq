<Query Kind="Program" />

void Main()
{
	var context = new Context();
	
	var list = new List<AbstractExpression>();
	
	list.Add(new TerminalExpression());
	list.Add(new NonTerminalExpression());
	list.Add(new TerminalExpression());
	list.Add(new NonTerminalExpression());

	foreach (var exp in list)
	{
		exp.Interpret(context);
	}
}

public class Context
{ 
}

public abstract class AbstractExpression
{ 
	public abstract void Interpret(Context context);
}

public class TerminalExpression : AbstractExpression
{
	public override void Interpret(Context context)
	{
		"Invoked Terminal Interpret".Dump("Terminal Expression");
	}
}


public class NonTerminalExpression : AbstractExpression
{
	public override void Interpret(Context context)
	{
		"Invoked NonTerminal Interpret".Dump("Non Terminal Expression");
	}
}

