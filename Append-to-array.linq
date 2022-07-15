<Query Kind="Program" />

void Main()
{
	int[] array = { 1, 2, 3, 4 };
	int item = 5;

	int[] result = array.Append(item);
	String.Join(",", result).Dump();
	 
	result = array.AppendConcat(item);
	String.Join(",", result).Dump();

	result = array.AppendCopyTo(item);
	String.Join(",", result).Dump();
	
	result = array.AppendToList(item);
	String.Join(",", result).Dump();
	
	//For benchmark information on executing these scenarios please see:
	//https://github.com/Jaxelr/Benchmarks/tree/master/AppendBenchmark 
}

static class Extensions
{
	public static T[] Append<T>(this T[] array, T item)
	{
		if (array == null)
		{
			return new T[] { item };
		}

		var result = new T[array.Length + 1];
		for (int i = 0; i < array.Length; i++)
		{
			result[i] = array[i];
		}

		result[array.Length] = item;
		return result;
	}

	public static T[] AppendCopyTo<T>(this T[] array, T item)
	{
		if (array == null)
		{
			return new T[] { item };
		}
		var result = new T[array.Length + 1];
		array.CopyTo(result, 0);
		result[array.Length] = item;
		return result;
	}

	public static T[] AppendConcat<T>(this T[] array, T item)
	{
		if (array == null)
		{
			return new T[] { item };
		}
		return array.Concat(new T[] { item }).ToArray();
	}

	public static T[] AppendToList<T>(this T[] array, T item)
	{
		var list = new List<T>(array);
		list.Add(item);

		return list.ToArray();
	}
}