<Query Kind="Program" />

void Main()
{
	var p1 = new Implementation("Key");
	
	var p2 = p1.Clone() as Prototype;
	
	p2.Dump("Proto");
}

// You can define other methods, fields, classes and namespaces here
public abstract class Prototype
{
	public Prototype(string Id)
	{
		this.Id = Id;
	}
	
	public string Id { get; set; }
	
	public abstract Prototype Clone();
}

public class Implementation : Prototype
{
	public Implementation(string Id) : base(Id)
	{
	}
	
	public override Prototype Clone()
	{
		return (Prototype)this.MemberwiseClone();
	}
}