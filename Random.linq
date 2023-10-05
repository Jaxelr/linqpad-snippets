<Query Kind="Program">
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

void Main()
{
	//This is not Thread safe.
	(int x, int y) = (0, 10);
	var random = new Random();
	
	//Get Random number within range
	random.Next(x, y)
	.Dump();
	
	//Range of max - min int values (2147483647)
	random.Next()
	.Dump();
	
	var threadRandom = new TSRandom();
	
	int bv = threadRandom.Next();
	
	bv.Dump();
	
}

//ThreadSafe random
public class TSRandom
{
	[ThreadStatic]
	private static Random? _local;

	public int Next()
	{
		if (_local == null)
		{
			byte[] buffer = new byte[4];
			
			using var rng = RandomNumberGenerator.Create();			
			rng.GetBytes(buffer);

			_local = new Random(BitConverter.ToInt32(buffer, 0));
		}

		return _local.Next();
	}
}