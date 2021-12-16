<Query Kind="Program">
  <NuGetReference>TypeMerger</NuGetReference>
  <Namespace>TypeMerger</Namespace>
</Query>

using merger = TypeMerger.TypeMerger;

void Main()
{
	var o1 = new Object1() { Id = 200 };
	var o2 = new Object2() {Name = "Hello world"};
	
	var res = merger.Merge(o1, o2);
	
	res.Dump();
}

public class Object1
{ 
	public int Id { get; set; }
}

public class Object2
{ 
	public string Name { get; set; }
}