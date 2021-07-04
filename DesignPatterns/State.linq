<Query Kind="Program" />

void Main()
{
	var context = new Context(new Impl1State());
	
	context.Request();
	context.Request();
}

public abstract class State
{ 
	public abstract void Handle(Context context);
}

public class Impl1State : State
{
	public override void Handle(Context context) =>  context.State = new Impl2State();
}

public class Impl2State : State
{
	public override void Handle(Context context) =>  context.State = new Impl1State();
}

public class Context
{ 
	private State state;
	
	public Context(State state) => this.state = state;

	public State State
	{
		get => state;
		set
		{
			state = value;
			$"State: {state.GetType().Name}".Dump("Context");
		}
	}
	
	public void Request()
	{
		state.Handle(this);
	}
}