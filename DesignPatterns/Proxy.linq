<Query Kind="Program" />

void Main()
{
	var proxy = new Proxy();
	
	proxy.Request();
}

public class Proxy : Original
{ 
	private ImplOriginal original = new();

	public override void Request()
	{
		original?.Request();
	}
}

public abstract class Original
{ 
	public abstract void Request();
}

public class ImplOriginal : Original
{
	public override void Request()
	{
		$"Invoked original implementation".Dump("Original Implementation");
	}
}