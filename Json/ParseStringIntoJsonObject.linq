<Query Kind="Program">
  <NuGetReference>System.Text.Json</NuGetReference>
  <Namespace>System.Text.Json.Nodes</Namespace>
</Query>

//Parsing json string to get subset
void Main()
{
	string json =
	@"
	{
		""root"": {
			""data"" : [
				{
					""id"": 1,
					""value"": ""Value""
				},
				{
					""id"": 2,
					""value"": ""Value2""
				}
			]
		}
	}";
	
	
	var obj = JsonNode.Parse(json)["root"]["data"];
	
	obj.Dump();
}

// You can define other methods, fields, classes and namespaces here