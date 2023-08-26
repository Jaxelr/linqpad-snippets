<Query Kind="Program" />

void Main()
{
	//Requirement: retrieve a list of pocos that the value of Value2 is on the collection of Value1s:
	var listOfSamples = new List<Sample>();

	listOfSamples.Add(new Sample(1, 3));
	listOfSamples.Add(new Sample(2, 4));
	listOfSamples.Add(new Sample(3, 5));
	listOfSamples.Add(new Sample(4, 6));

	//Grab the value1s
	var value1 = listOfSamples.Select(x => x.Value1);
	//Bring anybody whos not on Value1
	var not = listOfSamples.Where(o => value1.Any(y => y == o.Value2));

	//3,4 overlaps between Value1 & Value2
	not.Dump("Result");
}

// You can define other methods, fields, classes and namespaces here
public class Sample
{ 
	public Sample(int value1, int value2)
	{
		Value1 = value1;
		Value2 = value2;
	}
	
	public int Value1 { get; set; }
	public int Value2 { get; set; }
}