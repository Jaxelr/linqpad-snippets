<Query Kind="Program" />

void Main()
{
	string request = "abcde";
	
	var response = Key.Create<string>(request);
	
	response.Dump();
}

// Define other methods and classes here
public class Key
{
	public static string FieldSeparator = ":";

	public static string Create<T>(string field) => Create(typeof(T), field);

	public static string Create<T>(params string[] fields) => Create(typeof(T), fields);

	public static string Create(Type type, params string[] fields)
	{
		if (type == null)
		{
			throw new ArgumentNullException($"Argument {nameof(type)} cannot be null");
		}

		if (fields == null)
		{
			throw new ArgumentNullException($"Argument {nameof(fields)} cannot be null");
		}

		string urn = type.Name;

		foreach (string field in fields)
		{
			urn += FieldSeparator;
			urn += field;
		}

		return urn;
	}

	public static string Create(Type type, string field)
	{
		if (type == null)
		{
			throw new ArgumentNullException($"Argument {nameof(type)} cannot be null");
		}

		if (field == null)
		{
			throw new ArgumentNullException($"Argument {nameof(field)} cannot be null");
		}

		return $"{type.Name}{FieldSeparator}{field}";
	}
}
