<Query Kind="Program" />

void Main()
{
	var o = new Originator();
	o.State = "On";
	
	var s = new Storage();
	s.Memento = o.CreateMemento();
	
	o.State = "Off";
	
	o.SetMemento(s.Memento);
}

public class Memento
{ 
	private readonly string state;
	
	public Memento(string state) => this.state = state;
	
	public string State { get => state; }
}

public class Storage
{ 
	private Memento memento;
	
	public Memento Memento
	{
		set => memento = value; 
		get => memento;
	}
}

public class Originator
{ 
	private string state;

	public string State
	{
		get => state; 
		set
		{
			state = value;
			$"Assigning state...{state}".Dump("Originator State");
		}
	}

	public Memento CreateMemento() => new Memento(state);
	
	public void SetMemento(Memento memento)
	{
		"Restoring state...".Dump("Originator Set Memento");
		
		State = memento.State;
	}
}