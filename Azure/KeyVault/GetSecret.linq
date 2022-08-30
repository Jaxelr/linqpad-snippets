<Query Kind="Expression">
  <NuGetReference>Azure.Identity</NuGetReference>
  <NuGetReference>Azure.Security.KeyVault.Secrets</NuGetReference>
  <Namespace>Azure.Identity</Namespace>
  <Namespace>Azure.Security.KeyVault.Secrets</Namespace>
</Query>

void Main()
{
	//You should use exponential backoff because of throttling of AKV.
	var options = new SecretClientOptions()
	{
		Retry =
							{
								Delay= TimeSpan.FromSeconds(1),
								MaxDelay = TimeSpan.FromSeconds(8),
								MaxRetries = 5,
								Mode = RetryMode.Exponential
							}
	};

	var client = new SecretClient(new Uri("https://myvault.vault.azure.net"), new DefaultAzureCredential(), options);

	var secret = client.GetSecret("secret-value");

	secret.Value.Dump();
}