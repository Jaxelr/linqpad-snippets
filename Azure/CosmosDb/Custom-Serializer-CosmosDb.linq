<Query Kind="Program">
  <NuGetReference>Microsoft.Azure.Cosmos</NuGetReference>
  <NuGetReference>System.Text.Json</NuGetReference>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Text.Json.Serialization</Namespace>
  <Namespace>Microsoft.Azure.Cosmos</Namespace>
  <Namespace>Azure.Core.Serialization</Namespace>
</Query>

// All cosmos db documents must have an id property at the root
// but, if we want to map from the property string Id into the id
// cosmos property, we must use a custom serialization to avoid
// casing sensitivity issues.
// One thing to note is that this happens only with System.Text.Json
// since json property name doesnt work
async Task Main()
{
	var Options = new JsonSerializerOptions()
	{
		PropertyNameCaseInsensitive = true,
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		Converters = {
			new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
	}
	};

	var cosmosSerializer = new CosmosSystemTextJsonSerializer(Options);

	var client = new CosmosClient(MyExtensions.CosmosDbUrl, MyExtensions.CosmosDbKey, new CosmosClientOptions()
	{
		Serializer = cosmosSerializer
	});

	var db = await client.CreateDatabaseIfNotExistsAsync("SampleDb");
	var container = await db.Database.CreateContainerIfNotExistsAsync("SampleContainer", "/Child/AttributeId");

	var poco = new Poco() { Id = "1", Value = "Test", Child = new Child() { AttributeId = "id" } };

	var res = await container.Container.UpsertItemAsync(poco);

	res.Dump();
}

public class Poco
{
	public string Id { get; set; }
	public string Value { get; set; }
	public Child Child { get; set; }
}

public class Child
{
	public string AttributeId { get; set; }
}

public class CosmosSystemTextJsonSerializer : CosmosSerializer
{
	private readonly JsonObjectSerializer systemTextJsonSerializer;

	public CosmosSystemTextJsonSerializer(JsonSerializerOptions jsonSerializerOptions)
	{
		this.systemTextJsonSerializer = new JsonObjectSerializer(jsonSerializerOptions);
	}

	public override T FromStream<T>(Stream stream)
	{
		using (stream)
		{
			if (stream.CanSeek
			  && stream.Length == 0)
			{
				return default;
			}

			if (typeof(Stream).IsAssignableFrom(typeof(T)))
			{
				return (T)(object)stream;
			}

			return (T)this.systemTextJsonSerializer.Deserialize(stream, typeof(T), default);
		}
	}

	public override Stream ToStream<T>(T input)
	{
		MemoryStream streamPayload = new MemoryStream();
		this.systemTextJsonSerializer.Serialize(streamPayload, input, typeof(T), default);
		streamPayload.Position = 0;
		return streamPayload;
	}
}
