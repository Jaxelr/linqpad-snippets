<Query Kind="Program">
  <NuGetReference>System.Text.Json</NuGetReference>
  <Namespace>System.Text.Json.Nodes</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Text.Json.Serialization</Namespace>
  <Namespace>System.Runtime.Serialization</Namespace>
  <Namespace>System.Text.Json.Serialization.Metadata</Namespace>
</Query>

void Main()
{
	var poco = new Poco() { Id = 1 };
	
	var result = JsonSerializer.Serialize<Poco>(poco, DefaultJsonSerializerOptions.Options);
	
	result.Dump();
}

public class Poco
{ 
	internal int Id { get; set; }
	private string Value { get; set; } = "Uno";
}



public static class DefaultJsonSerializerOptions
{
	public static readonly JsonSerializerOptions Options = new()
	{
		TypeInfoResolver = new DefaultJsonTypeInfoResolver()
		{
			Modifiers = { AddInternalPropertiesModifier }
		}
	};

	private static void AddInternalPropertiesModifier(JsonTypeInfo jsonTypeInfo)
	{
		if (jsonTypeInfo.Kind != JsonTypeInfoKind.Object)
			return;

		foreach (PropertyInfo property in jsonTypeInfo.Type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic))
		{
			JsonPropertyInfo jsonPropertyInfo = jsonTypeInfo.CreateJsonPropertyInfo(property.PropertyType, property.Name);
			jsonPropertyInfo.Get = property.GetValue;
			jsonPropertyInfo.Set = property.SetValue;

			jsonTypeInfo.Properties.Add(jsonPropertyInfo);
		}
	}
}

