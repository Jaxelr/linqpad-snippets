<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Configuration.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Design.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.DirectoryServices.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.EnterpriseServices.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.Services.dll</Reference>
  <NuGetReference>EnyimMemcachedCore</NuGetReference>
  <NuGetReference>Microsoft.Extensions.DependencyInjection</NuGetReference>
  <NuGetReference>Microsoft.Extensions.Logging</NuGetReference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>Enyim.Caching</Namespace>
  <Namespace>Enyim.Caching.Memcached</Namespace>
  <Namespace>Enyim.Caching.Memcached.Transcoders</Namespace>
  <Namespace>Microsoft.Extensions.Configuration</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
</Query>

void Main()
{
	//The memcached docker image must be online.
	//
	//docker run --name fancy_image_pants -p 11211:11211 -d memcached memcached -m 64
	MemcachedClient _client;
	
	IServiceCollection services = new ServiceCollection();
	var configuration = new ConfigurationBuilder().Build();
	services.AddSingleton<IConfiguration>(configuration);
	services.AddEnyimMemcached(options =>
	{
		options.AddServer("127.0.0.1", 11211);
	});
	services.AddLogging();
	//services.AddSingleton<ITranscoder, BinaryFormatterTranscoder>();
	IServiceProvider serviceProvider = services.BuildServiceProvider();
	_client = serviceProvider.GetService<IMemcachedClient>() as MemcachedClient;

	var p = new Parent() { Name = "Jaxel", ParentId = 1};
	
	var mr = _client.ExecuteStore(Enyim.Caching.Memcached.StoreMode.Set, "Keyyy", p);
	
	var r = _client.GetAsync<Parent>("Keyyy");

	r.Result.Dump();
	
	
	if (r.Result.Success)
	{
		var parent = r.Result.Value;
		parent.Dump();
	}
	
	var t = _client.ExecuteRemove("Keyyy");

	if (t.Success)
	{
		t.Dump("Removed");		
	}
}

// Define other methods and classes here
public class Parent
{ 
	public int ParentId { get; set; }
	public string Name { get; set; }
	
}