<Query Kind="Program" />

void Main()
{
	var destination = new Adapter();
	
	destination.Request();
}

// You can define other methods, fields, classes and namespaces here
public class Destination
{ 
	public virtual void Request()
	{
		"Call request".Dump("Destination");
	}
}

public class Adapter : Destination
{
	private Source source = new Source();
	
	public override void Request()
	{
		base.Request();
		
		source.SourceRequest();
	}
}

public class Source
{
	public void SourceRequest()
	{
		"Call Request".Dump("Source");
	}
}