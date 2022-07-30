<Query Kind="Program">
  <Namespace>System.Runtime.CompilerServices</Namespace>
</Query>

/* Taken from this article
 https://khalidabuhakmeh.com/create-dotnet-objects-without-calling-the-constructor */
void Main()
{
	//You can create a .NET object without calling the constructor (?!)
	var o = RuntimeHelpers.GetUninitializedObject(typeof(CustomPoco));

	if (o is CustomPoco poco )
	{
		Console.WriteLine($"Hello, {poco.GetName()}");
		Console.WriteLine($"Hello, {poco.Value ?? "(null)"}");
	}
}

public class CustomPoco
{
	public string Value { get; } = "Another World";

	public CustomPoco(string name)
	{
		this.Value = name;
	}

	public string GetName()
	{
		return Value ?? "World";
	}
}