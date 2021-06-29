<Query Kind="Program" />

void Main()
{
	var context = new Context();
	
	var list = new Expressions();
	
	list.Add(new TerminalExpression());
	list.Add(new NonTerminalExpression());
	list.Add(new TerminalExpression());
	list.Add(new NonTerminalExpression());

	list.ForEach(x => x.Interpret(context));
}

public class Context
{ 
}

public class Expressions : List<AbstractExpression>
{ 
}

public abstract class AbstractExpression
{ 
	public abstract void Interpret(Context context);
}

public class TerminalExpression : AbstractExpression
{
	public override void Interpret(Context context) =>
		"Invoked Terminal Interpret".Dump("Terminal Expression");
}


public class NonTerminalExpression : AbstractExpression
{
	public override void Interpret(Context context) =>
		"Invoked NonTerminal Interpret".Dump("Non Terminal Expression");
}

