<Query Kind="Program" />

void Main()
{
	Random random = new Random();
	
	//long
	long longValue = random.NextInt64();
	longValue.Dump();
	longValue.GetBytes().Dump();
	
	//short
	var shortValue = random.Next(short.MaxValue);
	shortValue.Dump();
	shortValue.GetBytes().Dump();

	//int 
	var value = random.Next(int.MaxValue);
	value.Dump();
	value.GetBytes().Dump();
}

public static class IntegerExtensions
{
	public static byte[] GetBytes(this long x)
	{
		return BitConverter.GetBytes(x);
	}

	public static byte[] GetBytes(this int x)
	{
		return BitConverter.GetBytes(x);
	}

	public static byte[] GetBytes(this short x)
	{
		return BitConverter.GetBytes(x);
	}
}
