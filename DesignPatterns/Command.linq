<Query Kind="Program" />

void Main()
{
	var receiver = new Receiver();
	var command = new ImplCommand(receiver);
	var invoker = new Invoker();
	
	invoker.SetCommand(command);
	invoker.ExecuteCommand();	
}

public abstract class Command
{ 
	protected Receiver receiver;
	
	public Command(Receiver receiver) => this.receiver = receiver;
	
	public abstract void Execute();
}

public class ImplCommand : Command
{
	public ImplCommand(Receiver receiver) : base(receiver)
	{
	}
	
	public override void Execute() => receiver.Action();
}

public class Receiver
{ 
	public void Action() => "Invoked Receiver.Action()".Dump("Receiver");
}

public class Invoker
{ 
	Command command;
	
	public void SetCommand(Command command) => this.command = command;
	
	public void ExecuteCommand() => command.Execute();
}