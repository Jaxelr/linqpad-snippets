<Query Kind="Program">
  <NuGetReference>StackExchange.Redis</NuGetReference>
  <Namespace>StackExchange.Redis</Namespace>
</Query>

void Main()
{
	//Use docker to get this environment.
	//docker run -d --name redisDev -p 6379:6379 redis
	ConnectionMultiplexer _redis = ConnectionMultiplexer.Connect("localhost");
	
	IDatabase _cache = _redis.GetDatabase();

	string key = "ultimateKey:00";
	var myPoco = new myPoco() { one = 1, two = "two"};
	
	_cache.StringSet(key, Serialize(myPoco));
	
	var cachedResponse = _cache.StringGet(key);
		
	var realResult = Deserialize<myPoco>(cachedResponse);
	
	realResult.Dump();
	
	_cache.KeyDelete(key);
}

// Define other methods and classes here
[Serializable]
class myPoco
{ 
	public int one { get; set; }
	public string two { get; set; }
}


/// <summary>
/// Serialize the object into an array of bytes using the binary formatter
/// </summary>
/// <param name="o"></param>
/// <returns></returns>
private static byte[] Serialize(object o)
{
	byte[] objectDataAsStream = null;

	if (o != null)
	{
		var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
		using (var memoryStream = new MemoryStream())
		{
			binaryFormatter.Serialize(memoryStream, o);
			objectDataAsStream = memoryStream.ToArray();
		}
	}

	return objectDataAsStream;
}

/// <summary>
/// Deserialize the array of bytes into a poco using the binary formatter
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="stream"></param>
/// <returns></returns>
private static T Deserialize<T>(byte[] stream)
{
	var result = default(T);

	if (stream != null)
	{
		var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
		using (var memoryStream = new MemoryStream(stream))
		{
			result = (T)binaryFormatter.Deserialize(memoryStream);
		}
	}

	return result;
}