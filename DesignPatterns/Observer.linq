<Query Kind="Program" />

void Main()
{
	var topic = new ImplTopic();
	
	topic.Attach(new ImplObserver(topic, "One"));
	topic.Attach(new ImplObserver(topic, "Two"));
	topic.Attach(new ImplObserver(topic, "Three"));
	
	topic.State = "ABC";
	topic.Notify();
}

public abstract class Topic
{
	private Observers observers = new();
			
	public void Attach(Observer observer) => observers.Add(observer);
	
	public void Detach(Observer observer) => observers.Remove(observer);
	
	public void Notify() => observers.ForEach(o => o.Update());
}

public class ImplTopic : Topic
{ 
	private string state;

	public string State
	{
		get => state;
		set => this.state = value;
	}
}

public abstract class Observer
{ 
	public abstract void Update();
}

public class Observers : List<Observer>
{
}

public class ImplObserver : Observer
{
	private string name;
	private string state;
	private ImplTopic topic;
	
	public ImplObserver(ImplTopic topic, string name) => 
		(this.topic, this.name) = (topic, name);
	
	public override void Update()
	{
		state = topic.State;
		$"Observer {name}'s new state is {state}".Dump("Impl Observer");
	}
	
	public ImplTopic Topic 
	{
		get => topic;
		set => topic = value;
	}
}