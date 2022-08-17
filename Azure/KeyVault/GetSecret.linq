<Query Kind="Expression">
  <NuGetReference>Azure.Identity</NuGetReference>
  <NuGetReference>Azure.Security.KeyVault.Secrets</NuGetReference>
  <Namespace>Azure.Identity</Namespace>
  <Namespace>Azure.Security.KeyVault.Secrets</Namespace>
</Query>

void Main()
{
	var client = new SecretClient(new Uri("https://myvault.vault.azure.net"), new DefaultAzureCredential());

	var secret = client.GetSecret("secret-value");

	secret.Value.Dump();
}