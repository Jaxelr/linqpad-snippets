<Query Kind="Program">
  <NuGetReference>EnyimMemcached</NuGetReference>
  <Namespace>Enyim.Caching</Namespace>
  <Namespace>Enyim.Caching.Configuration</Namespace>
</Query>

void Main()
{
	//The memcached docker image must be online.
	//
	//docker run --name fancy_image_pants -p 11211:11211 -d memcached memcached -m 64
	MemcachedClientConfiguration config = new MemcachedClientConfiguration();
	
	config.AddServer("127.0.0.1", 11211);
	
	MemcachedClient client = new MemcachedClient(config);
	
	string key = "Chicago";

	var p = new Parent() { Name = "Jaxel", ParentId = 2};
	
	var res = client.ExecuteStore(Enyim.Caching.Memcached.StoreMode.Set, key, p);
	
	res.Dump();
	
	var result = client.ExecuteTryGet(key, out object tmp);

	result.Dump();

	if (result.Success)
	{
		if (tmp is Parent)
		{
			tmp.GetType().Dump();
			tmp.Dump();
		}
	}
	
	var t = client.ExecuteRemove(key);

	if (t.Success)
	{
		t.Dump("Removed");
	}
}

// Define other methods and classes here
[Serializable]
public class Parent
{ 
	public int ParentId { get; set; }
	public string Name { get; set; }
	
}