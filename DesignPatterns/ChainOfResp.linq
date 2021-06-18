<Query Kind="Program" />

void Main()
{
	var h1 = new Impl1Handler();
	var h2 = new Impl2Handler();
	var h3 = new Impl3Handler();

	h1.SetSuccessor(h2);
	h2.SetSuccessor(h3);

	int[] requests = { 2, 5, 14, 22, 18, 3, 27, 20 };

	foreach (int request in requests)

	{
		h1.HandleRequest(request);
	}
}

public abstract class Handler
{ 
	protected Handler successor;
	
	public void SetSuccessor(Handler successor)
	{
		this.successor = successor;
	}
	
	public abstract void HandleRequest(int request);
}

public class Impl1Handler : Handler
{
	public override void HandleRequest(int request)
	{
		if (request >= 0 && request < 10)
			$"{request} Handled by request handler {GetType().Name}".Dump();
		
		successor?.HandleRequest(request);
	}
}

public class Impl2Handler : Handler
{
	public override void HandleRequest(int request)
	{
		if (request >= 10 && request < 20)
			$"{request} Handled by request handler {GetType().Name}".Dump();

		successor?.HandleRequest(request);
	}
}

public class Impl3Handler : Handler
{
	public override void HandleRequest(int request)
	{
		if (request >= 20 && request < 30)
			$"{request} Handled by request handler {GetType().Name}".Dump();

		successor?.HandleRequest(request);
	}
}

