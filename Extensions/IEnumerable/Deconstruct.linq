<Query Kind="Expression" />

//Splits an array into a list of arrays.
public static IEnumerable<T[]> Deconstruct<T>(this T[] array, int size)
{
	for (int i = 0; i < (float)array.Length / size; i++)
	{
		yield return array.Skip(i * size).Take(size).ToArray();
	}
}