<Query Kind="Program">
  <NuGetReference>System.Text.Json</NuGetReference>
  <Namespace>System.Text.Json.Nodes</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Text.Json.Serialization</Namespace>
</Query>

//Parsing json string to get dictionary structure
void Main()
{
	string json =
	@"
	{
		""data"" : {
				""One"" : {
					""id"": 1,
					""value"": ""Value""
				},
				""Two"" : {
					""id"": 2,
					""value"": ""Value2""
				}
		}
	}";
	
	
	
	var datum = JsonSerializer.Deserialize<Data>(json);
	
	datum.Dump();
}

// You can define other methods, fields, classes and namespaces here
public class Data
{
	[JsonPropertyName("data")]
	public Dictionary<string, Poco> pocos { get; set; }
}

public class Poco
{ 
	[JsonPropertyName("id")]
	public int Id { get; set; }
	[JsonPropertyName("value")]
	public string Value { get; set; }
	
}