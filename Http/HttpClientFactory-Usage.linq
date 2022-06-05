<Query Kind="Program">
  <NuGetReference>Microsoft.Extensions.DependencyInjection</NuGetReference>
  <NuGetReference>Microsoft.Extensions.Http</NuGetReference>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>System.Net.Http</Namespace>
</Query>

/// <summary> Following the guidance from this docs 
/// https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
/// </summary>
void Main()
{
	const string NamedClient = "Test";
	int max = 1000;
	
	//Client Factory construction depends on MS DI
	var serviceProvider = new ServiceCollection();

	serviceProvider
		.AddHttpClient(NamedClient, x => 
		{
			x.DefaultRequestHeaders.Add("Poke", DateTime.Now.ToLongTimeString());
		})
		.ConfigurePrimaryHttpMessageHandler(() =>
		{
			return new HttpClientHandler()
			{
				AllowAutoRedirect = false,
			};
		})
		.SetHandlerLifetime(TimeSpan.FromMinutes(1));
	
	var builder = serviceProvider.BuildServiceProvider();
	
	var httpClientFactory = builder.GetService<IHttpClientFactory>();

	for(int retries = 0; retries < max; retries++)
	{
		Thread.Sleep(TimeSpan.FromSeconds(1));
		
		var client = httpClientFactory.CreateClient(NamedClient);
		
		client.DefaultRequestHeaders.Where(x => x.Key == "Poke").Dump("Header");
		
		var response = client.GetAsync("https://www.google.com");
		
		response.Status.Dump("StatusCode");
	}
}

// You can define other methods, fields, classes and namespaces here

