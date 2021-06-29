<Query Kind="Program" />

void Main()
{
	var impl1 = RootFactory.GetRoot("Impl1");
	impl1.Dump();
	
	impl1 = RootFactory.GetRoot("Impl1", 10);
	impl1.Dump();

	var impl2 = RootFactory.GetRoot("Impl2");
	impl2.Dump();

	impl2 = RootFactory.GetRoot("Impl2", 10);
	impl2.Dump();
}

public abstract class Root
{ 
	public abstract string Type { get; }
	public abstract int Value { get; set; }	
}

public class Implementation1 : Root
{
	private string _type;
	private int _value;

	public Implementation1(int Value) : base() => _value = Value;

	public Implementation1() => _type = nameof(Implementation1);
	
	public override string Type { get => _type; }

	public override int Value
	{
		get => _value; 
		set => _value = value; 
	}
}

public class Implementation2 : Root
{
	private string _type;
	private int _value;

	public Implementation2(int Value) : base() => _value = Value;
	
	public Implementation2() => _type = nameof(Implementation2);

	public override string Type { get => _type; }

	public override int Value
	{
		get => _value; 
		set => _value = value; 
	}
}

public class RootFactory
{
	public static Root GetRoot(string type, int? value = null)
	{
		if (type == "Impl1")
		{
			if (value.HasValue)
				return new Implementation1(value.Value);
			
			return new Implementation1();
		}

		if (type == "Impl2")
		{
			if (value.HasValue)
				return new Implementation2(value.Value);

			return new Implementation2();
		}
		
		return null;
	}
}