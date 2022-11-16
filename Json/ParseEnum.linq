<Query Kind="Program">
  <NuGetReference>System.Text.Json</NuGetReference>
  <Namespace>System.Text.Json.Nodes</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Text.Json.Serialization</Namespace>
  <Namespace>System.Runtime.Serialization</Namespace>
</Query>

void Main()
{
	string json = @"{""status"": ""Bad""}";

	var result = JsonSerializer.Deserialize<Poco>(json, DefaultJsonSerializerOptions.Options);
	
	result.Dump();
}

public class Poco
{ 
	public Status status { get; set; }
}

public enum Status
{
	Good,
	Bad
}


public static class DefaultJsonSerializerOptions
{
	public static readonly JsonSerializerOptions Options = new()
	{
		PropertyNameCaseInsensitive = true,
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		Converters = {
				new JsonStringEnumConverter()
			}
	};
}
