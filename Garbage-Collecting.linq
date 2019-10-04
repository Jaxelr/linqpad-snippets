<Query Kind="Program" />

void Main()
{
	GC.Collect();
	GC.WaitForPendingFinalizers();
	GC.Collect();

	var sample = new Example() { MyId = 1, Implem = "Text" };
	sample = null;
		
	GC.Collect();
	GC.WaitForPendingFinalizers();	
	GC.Collect();	
}

class Example 
{ 
	public int MyId { get; set; }
	public string Implem { get; set; }

	~Example()
	{
	
	}
}