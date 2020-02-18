<Query Kind="Program">
  <NuGetReference>IdentityModel</NuGetReference>
  <Namespace>IdentityModel.Client</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
</Query>

void Main()
{
	var res = GetToken()
			  .GetAwaiter()
			  .GetResult();

	res.Dump();
}

// Define other methods, classes and namespaces here
internal async Task<JObject> GetToken()
{
	string address = "<<AddressGoesHere>>";
	// discover endpoints from metadata
	var client = new HttpClient();

	var disco = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest()
	{
		Address = address,
		Policy = new DiscoveryPolicy() { RequireHttps = false }
	}).ConfigureAwait(false);

	if (disco.IsError)
	{
		throw new Exception($"Cannot contact Discovery Document, reason: {disco.Error}");
	}

	// request token
	var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
	{
		Address = disco.TokenEndpoint,
		ClientId = "<ClientIdGoesHere>",
		ClientSecret = "<ClientSecretGoesHere>",

		Scope = "<ScopeGoesHere>"
	}).ConfigureAwait(false);

	if (tokenResponse.IsError)
	{
		throw new Exception($"Error retrieving token {tokenResponse.Error}");
	}
	
	return tokenResponse.Json;
}