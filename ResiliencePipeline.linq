<Query Kind="Program">
  <NuGetReference>Microsoft.Extensions.Resilience</NuGetReference>
  <NuGetReference>Microsoft.Extensions.DependencyInjection</NuGetReference>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>Polly</Namespace>
  <Namespace>Polly.Registry</Namespace>
  <Namespace>Polly.Retry</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{

	var services = new ServiceCollection();

	services.AddResiliencePipeline("retries", builder =>
	{
		builder.AddRetry(new RetryStrategyOptions
		{
			MaxRetryAttempts = 3,
			ShouldHandle = new PredicateBuilder()
				.Handle<Exception>()
		});
	});

	var provider = services.BuildServiceProvider();
	var pipelineProvider = provider.GetRequiredService<ResiliencePipelineProvider<string>>();
	var pipeline = pipelineProvider.GetPipeline("retries");

	int attempt = 0;

	await pipeline.ExecuteAsync(async ct =>
	{
		attempt++;
		Console.WriteLine($"Attempt {attempt}");

		if (attempt < 3)
		{
			throw new Exception("Transient failure");
		}

		Console.WriteLine("Succeeded");
	});
}
