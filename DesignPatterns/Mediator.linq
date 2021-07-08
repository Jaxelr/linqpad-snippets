<Query Kind="Program" />

void Main()
{
	var mediator = new ImplMediator();
	
	var targeta = new TargetA(mediator);
	var targetb = new TargetB(mediator);
	
	mediator.TargetA = targeta;
	mediator.TargetB = targetb;
	
	targeta.Send("Hello! How are you?");
	targetb.Send("I'm fine, thank you");
}

public abstract class Mediator
{ 
	public abstract void Send(string message, Target target);
}

public abstract class Target
{ 
	protected Mediator mediator;

	public Target(Mediator mediator) => this.mediator = mediator;
}

public class TargetA : Target
{ 
	public TargetA(Mediator mediator) : base(mediator)
	{
	}
	
	public void Send(string message) => mediator.Send(message, this);
	
	public void Notify(string message) => $"Target A gets message: {message}".Dump("TargetA");
}

public class TargetB : Target
{
	public TargetB(Mediator mediator) : base(mediator)
	{
	}

	public void Send(string message) => mediator.Send(message, this);

	public void Notify(string message) => $"Target B gets message: {message}".Dump("TargetB");
}

public class ImplMediator : Mediator
{
	TargetA targeta;
	TargetB targetb;
	
	public TargetA TargetA 
	{ 
	 set => targeta = value;
	}

	public TargetB TargetB
	{
		set => targetb = value;
	}

	public override void Send(string message, Target target)
	{
		switch (target)
		{
			case TargetA targeta:
				targeta.Notify(message);
				break;
			case TargetB targetb:
				targetb.Notify(message);
				break;
		}	
	}
}